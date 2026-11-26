using System;
using System.Reflection;
using System.Threading;
using NUnit.Framework;
using static ImmediateReflection.TypeAccessor;

namespace ImmediateReflection.Tests;

/// <summary>
/// Tests related to <see cref="ImmediateTypeCache"/>.
/// </summary>
[TestFixture]
internal sealed class ImmediateTypeCacheTests : ImmediateReflectionTestsBase
{
    [Test]
    public static void GetOrCreate()
    {
        ImmediateTypeCache cache = new(typeof(TestObject));

        ImmediateType type = cache.GetOrCreate(DefaultFlags);
        Assert.AreSame(type, cache.GetOrCreate(DefaultFlags));

        // Other flags create another entry
        const BindingFlags otherFlags = BindingFlags.NonPublic | BindingFlags.Instance;
        ImmediateType other = cache.GetOrCreate(otherFlags);
        Assert.AreNotSame(type, other);
        Assert.AreSame(other, cache.GetOrCreate(otherFlags));
    }

    // Type that lets a test pause the creation of the ImmediateType (done while the cache lock is held)
    private sealed class BlockingType : TypeDelegator
    {
        private readonly ManualResetEvent _entered;
        private readonly ManualResetEvent _release;

        public BlockingType(Type type, ManualResetEvent entered, ManualResetEvent release)
            : base(type)
        {
            _entered = entered;
            _release = release;
        }

        /// <inheritdoc />
        public override Type? BaseType
        {
            get
            {
                _entered.Set();
                _release.WaitOne();
                return base.BaseType;
            }
        }
    }

    [Test]
    public static void GetOrCreateConcurrent()
    {
        using ManualResetEvent creationEntered = new(false);
        using ManualResetEvent releaseCreation = new(false);
        ImmediateTypeCache cache = new(new BlockingType(typeof(TestObject), creationEntered, releaseCreation));

        // First thread creates the type and blocks during the creation, so it holds the cache lock
        ImmediateType? fromFirstThread = null;
        Thread firstThread = new(() => fromFirstThread = cache.GetOrCreate(DefaultFlags));
        firstThread.Start();
        Assert.IsTrue(creationEntered.WaitOne(TimeSpan.FromSeconds(10d)));

        // Second thread misses the lock free lookup then blocks on the cache lock
        ImmediateType? fromSecondThread = null;
        Thread secondThread = new(() => fromSecondThread = cache.GetOrCreate(DefaultFlags));
        secondThread.Start();
        Assert.IsTrue(
            SpinWait.SpinUntil(
                () => (secondThread.ThreadState & ThreadState.WaitSleepJoin) != 0,
                TimeSpan.FromSeconds(10d)));

        // Let the first thread finish, the second one must find its type once it gets the lock
        releaseCreation.Set();
        Assert.IsTrue(firstThread.Join(TimeSpan.FromSeconds(10d)));
        Assert.IsTrue(secondThread.Join(TimeSpan.FromSeconds(10d)));

        Assert.IsNotNull(fromFirstThread);
        Assert.AreSame(fromFirstThread, fromSecondThread);
    }
}

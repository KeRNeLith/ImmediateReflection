using System;
using System.Threading;
using NUnit.Framework;

namespace ImmediateReflection.Tests;

/// <summary>
/// Tests related to <see cref="MemoryCache{TKey,TValue}"/>.
/// </summary>
[TestFixture]
internal sealed class MemoryCacheTests : ImmediateReflectionTestsBase
{
    [Test]
    public static void GetOrCreate()
    {
        MemoryCache<string, object> cache = new();
        int factoryCalls = 0;

        object first = cache.GetOrCreate(
            "key",
            _ =>
            {
                ++factoryCalls;
                return new object();
            });
        object second = cache.GetOrCreate(
            "key",
            _ =>
            {
                ++factoryCalls;
                return new object();
            });

        Assert.AreSame(first, second);
        Assert.AreEqual(1, factoryCalls);
    }

    [Test]
    public static void GetOrCreateConcurrent()
    {
        MemoryCache<string, object> cache = new();
        using ManualResetEvent factoryEntered = new(initialState: false);
        using ManualResetEvent releaseFactory = new(initialState: false);
        int factoryCalls = 0;

        // First thread creates the value and blocks in the factory, so it holds the cache lock
        object? fromFirstThread = null;
        Thread firstThread = new(() =>
            fromFirstThread = cache.GetOrCreate(
                "key",
                _ =>
                {
                    Interlocked.Increment(ref factoryCalls);
                    // ReSharper disable AccessToDisposedClosure, Justification: ManualResetEvent is disposed after the threads are joined
                    factoryEntered.Set();
                    releaseFactory.WaitOne();
                    // ReSharper restore AccessToDisposedClosure
                    return new object();
                }));
        firstThread.Start();
        Assert.IsTrue(factoryEntered.WaitOne(TimeSpan.FromSeconds(10d)));

        // Second thread misses the lock free lookup then blocks on the cache lock
        object? fromSecondThread = null;
        Thread secondThread = new(() =>
            fromSecondThread = cache.GetOrCreate(
                "key",
                _ =>
                {
                    Interlocked.Increment(ref factoryCalls);
                    return new object();
                }));
        secondThread.Start();
        Assert.IsTrue(
            SpinWait.SpinUntil(
                () => (secondThread.ThreadState & ThreadState.WaitSleepJoin) != 0,
                TimeSpan.FromSeconds(10d)));

        // Let the first thread finish, the second one must find its value once it gets the lock
        releaseFactory.Set();
        Assert.IsTrue(firstThread.Join(TimeSpan.FromSeconds(10d)));
        Assert.IsTrue(secondThread.Join(TimeSpan.FromSeconds(10d)));

        Assert.IsNotNull(fromFirstThread);
        Assert.AreSame(fromFirstThread, fromSecondThread);
        Assert.AreEqual(1, factoryCalls);
    }
}
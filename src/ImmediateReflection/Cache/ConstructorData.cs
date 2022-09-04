using System.Diagnostics;
using JetBrains.Annotations;

namespace ImmediateReflection
{
    /// <summary>
    /// Data stored in a constructor cache.
    /// </summary>
    /// <typeparam name="TConstructorDelegate">Constructor delegate type.</typeparam>
    internal sealed class ConstructorData<TConstructorDelegate>
    {
        /// <summary>
        /// Indicates if there is a constructor.
        /// </summary>
        public bool HasConstructor { get; }

        /// <summary>
        /// Constructor delegate.
        /// </summary>
        [NotNull]
        public TConstructorDelegate Constructor { get; }

        public ConstructorData([NotNull] TConstructorDelegate constructor, bool hasConstructor)
        {
            Debug.Assert(constructor != null);

            HasConstructor = hasConstructor;
            Constructor = constructor;
        }
    }
}
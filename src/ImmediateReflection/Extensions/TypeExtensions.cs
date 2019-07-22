#if SUPPORTS_CACHING
using System;
using System.Reflection;
using JetBrains.Annotations;

namespace ImmediateReflection
{
    /// <summary>
    /// Extensions to work with <see cref="Type"/>.
    /// </summary>
    [PublicAPI]
    public static class TypeExtensions
    {
        /// <summary>
        /// Creates an instance of this <paramref name="type"/> with that type's default constructor.
        /// </summary>
        /// <param name="type"><see cref="Type"/> to instantiate.</param>
        /// <returns>A reference to the newly created object.</returns>
        /// <exception cref="ArgumentException"><see cref="Type"/> a RuntimeType or is an open generic type (that is, the ContainsGenericParameters property returns true).</exception>
        /// <exception cref="AmbiguousMatchException"><see cref="Type"/> has several constructors defining "params" parameter only.</exception>
        /// <exception cref="MissingMethodException">No matching public constructor was found.</exception>
        /// <exception cref="TargetInvocationException">The constructor being called throws an exception.</exception>
        [PublicAPI]
        [Pure]
        [NotNull]
        [ContractAnnotation("type:null => halt")]
        public static object New(
#if SUPPORTS_EXTENSIONS
            [NotNull] this Type type)
#else
            [NotNull] Type type)
#endif
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            return CachesHandler.Instance.GetDefaultConstructor(type).Constructor();
        }

        /// <summary>
        /// Tries to create an instance of this <paramref name="type"/> with that type's default constructor.
        /// </summary>
        /// <remarks>This method will not throw if instantiation failed.</remarks>
        /// <param name="type"><see cref="Type"/> to instantiate.</param>
        /// <param name="newInstance">A reference to the newly created object, otherwise null.</param>
        /// <param name="exception">Caught exception if the instantiation failed, otherwise null.</param>
        /// <returns>True if the new instance was successfully created, false otherwise.</returns>
        [PublicAPI]
        [Pure]
        [ContractAnnotation("=> true, newInstance:notnull, exception:null;=> false, newInstance:null, exception:notnull")]
        public static bool TryNew(
#if SUPPORTS_EXTENSIONS
            [NotNull] this Type type,
#else
            [NotNull] Type type,
#endif
            out object newInstance, 
            out Exception exception)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            try
            {
                exception = null;
                newInstance = CachesHandler.Instance.GetDefaultConstructor(type).Constructor();
                return true;
            }
            catch (Exception ex)
            {
                newInstance = null;
                exception = ex;
                return false;
            }
        }
    }
}
#endif
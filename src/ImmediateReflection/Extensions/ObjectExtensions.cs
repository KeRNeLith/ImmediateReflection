#if SUPPORTS_CACHING
using System;
#if SUPPORTS_AGGRESSIVE_INLINING
using System.Runtime.CompilerServices;
#endif
using JetBrains.Annotations;

namespace ImmediateReflection
{
    /// <summary>
    /// Extensions to work with object instance.
    /// </summary>
    [PublicAPI]
    public static class ObjectExtensions
    {
        /// <summary>
        /// Checks if this <paramref name="instance"/> can be copied by a copy constructor.
        /// </summary>
        /// <param name="instance">Object to check if its <see cref="Type"/> has a copy constructor.</param>
        /// <returns>True if the <paramref name="instance"/> can be copied, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="instance"/> is null.</exception>
        [PublicAPI]
        [ContractAnnotation("instance:null => halt")]
        public static bool HasCopyConstructor<T>(
#if SUPPORTS_EXTENSIONS
            [NotNull] this T instance)
#else
            [NotNull] T instance)
#endif
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));
            return CachesHandler.Instance.GetCopyConstructor(instance.GetType()).HasConstructor;
        }

        /// <summary>
        /// Creates a copy instance of this <paramref name="instance"/> with its copy constructor.
        /// </summary>
        /// <param name="instance">Object to copy.</param>
        /// <returns>A reference to the newly created object.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="instance"/> is null.</exception>
        /// <exception cref="MissingMethodException">
        /// No matching public copy constructor was found,
        /// or constructor exists but was not considered as copy constructor.
        /// </exception>
        [PublicAPI]
        [NotNull]
        [ContractAnnotation("instance:null => halt")]
        public static T Copy<T>(
#if SUPPORTS_EXTENSIONS
            [NotNull] this T instance)
#else
            [NotNull] T instance)
#endif
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));
            return (T)CachesHandler.Instance.GetCopyConstructor(instance.GetType()).Constructor(instance);
        }

        /// <summary>
        /// Tries to create a copy instance of this <paramref name="newInstance"/> with its copy constructor.
        /// </summary>
        /// <remarks>This method will not throw if instantiation failed.</remarks>
        /// <param name="instance">Object to copy.</param>
        /// <param name="newInstance">A reference to the newly created object, otherwise null.</param>
        /// <param name="exception">Caught exception if the instantiation failed, otherwise null.</param>
        /// <returns>True if the new instance was successfully created, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="instance"/> is null.</exception>
        [PublicAPI]
        [ContractAnnotation("=> true, newInstance:notnull, exception:null;=> false, newInstance:null, exception:notnull")]
        public static bool TryCopy<T>(
#if SUPPORTS_EXTENSIONS
            [NotNull] this T instance,
#else
            [NotNull] T instance,
#endif
            out T newInstance,
            out Exception exception)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            try
            {
                exception = null;
                newInstance = (T)CachesHandler.Instance.GetCopyConstructor(instance.GetType()).Constructor(instance);
                return true;
            }
            catch (Exception ex)
            {
                newInstance = default(T);
                exception = ex;
                return false;
            }
        }
    }
}
#endif
using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace ImmediateReflection;

/// <summary>
/// Extensions to work with object instance.
/// </summary>
[PublicAPI]
public static class ObjectExtensions
{
    /// <summary>
    /// Checks if this <paramref name="instance"/> can be copied by a copy constructor.
    /// </summary>
    /// <typeparam name="T">Instance type.</typeparam>
    /// <param name="instance">Object to check if its <see cref="T:System.Type"/> has a copy constructor.</param>
    /// <returns>True if the <paramref name="instance"/> can be copied, false otherwise.</returns>
    /// <exception cref="T:System.ArgumentNullException">If the given <paramref name="instance"/> is null.</exception>
    [PublicAPI]
    [ContractAnnotation("instance:null => halt")]
    public static bool HasCopyConstructor<T>(this T instance)
    {
        if (instance is null)
            throw new ArgumentNullException(nameof(instance));
        if (instance is Type)
            return true;
        return CachesHandler.Instance.GetCopyConstructor(instance.GetType()).HasConstructor;
    }

    /// <summary>
    /// Creates a copy instance of this <paramref name="instance"/> with its copy constructor.
    /// </summary>
    /// <typeparam name="T">Instance type.</typeparam>
    /// <param name="instance">Object to copy.</param>
    /// <returns>A reference to the newly created object.</returns>
    /// <exception cref="T:System.ArgumentNullException">If the given <paramref name="instance"/> is null.</exception>
    /// <exception cref="T:System.MissingMethodException">
    /// No matching public copy constructor was found,
    /// or constructor exists but was not considered as copy constructor.
    /// </exception>
    [PublicAPI]
    [ContractAnnotation("instance:null => null;instance:notnull => notnull")]
    public static T? Copy<T>(this T? instance)
        where T : class
    {
        if (instance is null)
            return null;
        if (instance is Type)
            return instance;
        return (T?)CachesHandler.Instance.GetCopyConstructor(instance.GetType()).Constructor(instance);
    }

    /// <summary>
    /// Creates a copy instance of this <paramref name="instance"/> with its copy constructor.
    /// </summary>
    /// <typeparam name="T">Instance type.</typeparam>
    /// <param name="instance">Object to copy.</param>
    /// <returns>A reference to the newly created object.</returns>
    /// <exception cref="T:System.ArgumentNullException">If the given <paramref name="instance"/> is null.</exception>
    /// <exception cref="T:System.MissingMethodException">
    /// No matching public copy constructor was found,
    /// or constructor exists but was not considered as copy constructor.
    /// </exception>
    [PublicAPI]
    [ContractAnnotation("instance:null => null;instance:notnull => notnull")]
    public static T Copy<T>(this T instance)
        where T : struct
    {
        return (T)CachesHandler.Instance.GetCopyConstructor(instance.GetType()).Constructor(instance);
    }

    /// <summary>
    /// Tries to create a copy instance of this <paramref name="newInstance"/> with its copy constructor.
    /// </summary>
    /// <typeparam name="T">Instance type.</typeparam>
    /// <remarks>This method will not throw if instantiation failed.</remarks>
    /// <param name="instance">Object to copy.</param>
    /// <param name="newInstance">A reference to the newly created object, otherwise null.</param>
    /// <param name="exception">Caught exception if the instantiation failed, otherwise null.</param>
    /// <returns>True if the new instance was successfully created, false otherwise.</returns>
    /// <exception cref="T:System.ArgumentNullException">If the given <paramref name="instance"/> is null.</exception>
    [PublicAPI]
    [ContractAnnotation("instance:null => true, newInstance:null, exception:null;"
                        + "instance:notnull => true, newInstance:notnull, exception:null;"
                        + "instance:null => false, newInstance:null, exception:notnull;"
                        + "instance:notnull => false, newInstance:null, exception:notnull")]
    public static bool TryCopy<T>(
        this T? instance,
        out T? newInstance,
        [NotNullWhen(false)] out Exception? exception)
    {
        try
        {
            exception = null;
            newInstance = Copy(instance);
            return true;
        }
        catch (Exception ex)
        {
            newInstance = default;
            exception = ex;
            return false;
        }
    }
}
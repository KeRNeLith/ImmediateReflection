using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
#if SUPPORTS_AGGRESSIVE_INLINING
using System.Runtime.CompilerServices;
#endif
using System.Runtime.Serialization;
using System.Security.Permissions;
using JetBrains.Annotations;
using static ImmediateReflection.Utils.FieldHelpers;

namespace ImmediateReflection
{
    /// <summary>
    /// Represents type declarations (class types, interface types, array types, value types, enumeration types,
    /// type parameters, generic type definitions, and open or closed constructed generic types) in a faster way.
    /// </summary>
    [PublicAPI]
    [Serializable]
    public sealed class ImmediateType 
        : ImmediateMember
        , IEquatable<ImmediateType>
        , ISerializable
    {
        private BindingFlags _flags;

        /// <summary>
        /// Gets the wrapped <see cref="T:System.Type"/>.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public Type Type { get; }

        /// <summary>
        /// Gets the base <see cref="T:System.Type"/> of this <see cref="T:System.Type"/>.
        /// If this is an interface or has no base class null is returned.
        /// <see cref="T:System.Object"/> is the only <see cref="T:System.Type"/> that does not have a base class.
        /// </summary>
        [PublicAPI]
        [CanBeNull]
        public Type BaseType { get; }

        /// <summary>
        /// Gets the <see cref="T:System.Type"/> owning this <see cref="T:System.Type"/> (declaring it).
        /// </summary>
        [PublicAPI]
        [CanBeNull]
        public Type DeclaringType { get; }

        /// <summary>
        /// Gets the fully qualified name of the <see cref="T:System.Type"/>, including its namespace but not its assembly.
        /// </summary>
        /// <remarks>Fallback on the type name if full name is null.</remarks>
        [PublicAPI]
        [NotNull]
        public string FullName { get; }

        /// <summary>
        /// Gets all the members of this <see cref="T:System.Type"/>.
        /// </summary>
        [PublicAPI]
        [NotNull, ItemNotNull]
        public IEnumerable<ImmediateMember> Members
        {
            get
            {
                foreach (ImmediateField field in Fields)
                    yield return field;
                foreach (ImmediateProperty property in Properties)
                    yield return property;
            }
        }

        [NotNull]
        private readonly Lazy<ImmediateFields> _fields;

        /// <summary>
        /// Gets all the fields of this <see cref="T:System.Type"/>.
        /// </summary>
        [PublicAPI]
        [NotNull, ItemNotNull]
        public ImmediateFields Fields => _fields.Value;

        /// <summary>
        /// Gets all the properties of this <see cref="T:System.Type"/>.
        /// </summary>
        [PublicAPI]
        [NotNull, ItemNotNull]
        public ImmediateProperties Properties { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="type"><see cref="T:System.Type"/> to wrap.</param>
        /// <param name="flags">Flags that must be taken into account to get members.</param>
        internal ImmediateType([NotNull] Type type, BindingFlags flags = TypeAccessor.DefaultFlags)
            : base(type)
        {
            _flags = flags;
            Type = type;
            BaseType = type.BaseType;
            DeclaringType = type.DeclaringType;
            FullName = type.FullName ?? Name;

            // Default constructor
            ConstructorData<DefaultConstructorDelegate> defaultCtorData = CachesHandler.Instance.GetDefaultConstructor(Type);
            _constructor = defaultCtorData.Constructor;
            HasDefaultConstructor = defaultCtorData.HasConstructor;

            // Copy constructor
            ConstructorData<CopyConstructorDelegate> copyCtorData = CachesHandler.Instance.GetCopyConstructor(Type);
            _copyConstructor = copyCtorData.Constructor;
            HasCopyConstructor = copyCtorData.HasConstructor;

            if (type.IsEnum)
            {
                _fields = new Lazy<ImmediateFields>(() => new ImmediateFields(type.GetFields()));

                Properties = new ImmediateProperties(Enumerable.Empty<PropertyInfo>());
            }
            else
            {
                _fields = new Lazy<ImmediateFields>(() => new ImmediateFields(IgnoreBackingFields(type.GetFields(_flags))));

                Properties = new ImmediateProperties(type.GetProperties(_flags));
            }
        }

        /// <summary>
        /// Gets all the members of this <see cref="T:System.Type"/>.
        /// </summary>
        /// <returns>All <see cref="ImmediateMember"/>.</returns>
        [PublicAPI]
        [Pure]
        [NotNull, ItemNotNull]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public IEnumerable<ImmediateMember> GetMembers() => Members;

        /// <summary>
        /// Gets the <see cref="ImmediateMember"/> corresponding to the given <paramref name="memberName"/>.
        /// </summary>
        /// <param name="memberName">Member name.</param>
        /// <returns>Found <see cref="ImmediateMember"/>, otherwise null.</returns>
        /// <exception cref="T:System.ArgumentNullException">If the given <paramref name="memberName"/> is null.</exception>
        [PublicAPI]
        [CanBeNull]
        public ImmediateMember this[[NotNull] string memberName]
        {
            get
            {
                ImmediateField field = Fields[memberName];
                if (field is null)
                    return Properties[memberName];
                return field;
            }
        }

        /// <summary>
        /// Gets the <see cref="ImmediateMember"/> corresponding to the given <paramref name="memberName"/>.
        /// </summary>
        /// <param name="memberName">Member name.</param>
        /// <returns>Found <see cref="ImmediateMember"/>, otherwise null.</returns>
        /// <exception cref="T:System.ArgumentNullException">If the given <paramref name="memberName"/> is null.</exception>
        [PublicAPI]
        [Pure]
        [CanBeNull]
        [ContractAnnotation("memberName:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public ImmediateMember GetMember([NotNull] string memberName) => this[memberName];

        /// <summary>
        /// Gets all the fields of this <see cref="T:System.Type"/>.
        /// </summary>
        /// <returns>All <see cref="ImmediateField"/>.</returns>
        [PublicAPI]
        [Pure]
        [NotNull, ItemNotNull]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public IEnumerable<ImmediateField> GetFields() => Fields;

        /// <summary>
        /// Gets the <see cref="ImmediateField"/> corresponding to the given <paramref name="fieldName"/>.
        /// </summary>
        /// <param name="fieldName">Property name.</param>
        /// <returns>Found <see cref="ImmediateProperty"/>, otherwise null.</returns>
        /// <exception cref="T:System.ArgumentNullException">If the given <paramref name="fieldName"/> is null.</exception>
        [PublicAPI]
        [Pure]
        [CanBeNull]
        [ContractAnnotation("fieldName:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public ImmediateField GetField([NotNull] string fieldName) => Fields[fieldName];

        /// <summary>
        /// Gets all the properties of this <see cref="T:System.Type"/>.
        /// </summary>
        /// <returns>All <see cref="ImmediateProperty"/>.</returns>
        [PublicAPI]
        [Pure]
        [NotNull, ItemNotNull]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public IEnumerable<ImmediateProperty> GetProperties() => Properties;

        /// <summary>
        /// Gets the <see cref="ImmediateProperty"/> corresponding to the given <paramref name="propertyName"/>.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <returns>Found <see cref="ImmediateProperty"/>, otherwise null.</returns>
        /// <exception cref="T:System.ArgumentNullException">If the given <paramref name="propertyName"/> is null.</exception>
        [PublicAPI]
        [Pure]
        [CanBeNull]
        [ContractAnnotation("propertyName:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public ImmediateProperty GetProperty([NotNull] string propertyName) => Properties[propertyName];

        #region New/TryNew

        [NotNull]
        private readonly DefaultConstructorDelegate _constructor;

        /// <summary>
        /// Indicates if this <see cref="T:System.Type"/> has a default constructor.
        /// </summary>
        [PublicAPI]
        public bool HasDefaultConstructor { get; }

        /// <summary>
        /// Creates an instance of this <see cref="T:System.Type"/> with that type's default constructor.
        /// </summary>
        /// <returns>A reference to the newly created object.</returns>
        /// <exception cref="T:System.ArgumentException"><see cref="T:System.Type"/> is a RuntimeType or is an open generic type (that is, the ContainsGenericParameters property returns true).</exception>
        /// <exception cref="T:System.MissingMethodException">No matching public constructor was found.</exception>
        /// <exception cref="T:System.Reflection.AmbiguousMatchException"><see cref="T:System.Type"/> has several constructors defining "params" parameter only.</exception>
        [PublicAPI]
        [Pure]
        [NotNull]
        public object New()
        {
            return _constructor();
        }

        /// <summary>
        /// Tries to create an instance of this <see cref="T:System.Type"/> with that type's default constructor.
        /// </summary>
        /// <remarks>This method will not throw if instantiation failed.</remarks>
        /// <param name="newInstance">A reference to the newly created object, otherwise null.</param>
        /// <param name="exception">Caught exception if the instantiation failed, otherwise null.</param>
        /// <returns>True if the new instance was successfully created, false otherwise.</returns>
        [PublicAPI]
        [Pure]
        [ContractAnnotation("=> true, newInstance:notnull, exception:null;=> false, newInstance:null, exception:notnull")]
        public bool TryNew(out object newInstance, out Exception exception)
        {
            try
            {
                exception = null;
                newInstance = _constructor();
                return true;
            }
            catch (Exception ex)
            {
                newInstance = null;
                exception = ex;
                return false;
            }
        }

        /// <summary>
        /// Creates an instance of this <see cref="T:System.Type"/> using the constructor that best matches the specified parameters.
        /// </summary>
        /// <remarks>Tries to use the <see cref="New()"/> if no parameter provided, otherwise fallback on <see cref="M:System.Activator.CreateInstance(System.Type,object[])"/>.</remarks>
        /// <param name="args">
        /// An array of arguments that match in number, order, and type the parameters of the constructor to invoke.
        /// If <paramref name="args"/> is an empty array or null, the constructor that takes no parameters (the default constructor) is invoked.
        /// </param>
        /// <returns>A reference to the newly created object.</returns>
        /// <exception cref="T:System.ArgumentException"><see cref="T:System.Type"/> is a RuntimeType or is an open generic type (that is, the ContainsGenericParameters property returns true).</exception>
        /// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism.</exception>
        /// <exception cref="T:System.MissingMethodException">No matching public constructor was found.</exception>
        /// <exception cref="T:System.NotSupportedException">
        /// If the <see cref="T:System.Type"/> cannot be a TypeBuilder.
        /// -or- Creation of <see cref="T:System.TypedReference"/>, ArgIterator, <see cref="T:System.Void"/>, and <see cref="T:System.RuntimeArgumentHandle"/> types, or arrays of those types, is not supported.
        /// -or- The assembly that contains type is a dynamic assembly that was created with Save.
        /// -or- The constructor that best matches args has varargs arguments.
        /// </exception>
        /// <exception cref="T:System.TypeLoadException">If the <see cref="T:System.Type"/> is not a valid type.</exception>
        /// <exception cref="T:System.Reflection.TargetInvocationException">The constructor being called throws an exception.</exception>
        [PublicAPI]
        [Pure]
        [NotNull]
        public object New([CanBeNull, ItemCanBeNull] params object[] args)
        {
            if (args is null || args.Length == 0)
                return New();
            return Activator.CreateInstance(Type, args);
        }

        /// <summary>
        /// Tries to create an instance of this <see cref="T:System.Type"/> with the best matching constructor.
        /// </summary>
        /// <remarks>
        /// This method will not throw if instantiation failed.
        /// Tries to use the <see cref="New()"/> if no parameter provided, otherwise fallback on <see cref="M:System.Activator.CreateInstance(System.Type,object[])"/>.
        /// </remarks>
        /// <param name="newInstance">A reference to the newly created object, otherwise null.</param>
        /// <param name="exception">Caught exception if the instantiation failed, otherwise null.</param>
        /// <param name="args">
        /// An array of arguments that match in number, order, and type the parameters of the constructor to invoke.
        /// If <paramref name="args"/> is an empty array or null, the constructor that takes no parameters (the default constructor) is invoked.
        /// </param>
        /// <returns>True if the new instance was successfully created, false otherwise.</returns>
        [PublicAPI]
        [Pure]
        [ContractAnnotation("=> true, newInstance:notnull, exception:null;=> false, newInstance:null, exception:notnull")]
        public bool TryNew(out object newInstance, out Exception exception, [CanBeNull, ItemCanBeNull] params object[] args)
        {
            try
            {
                exception = null;
                newInstance = New(args);
                return true;
            }
            catch (Exception ex)
            {
                newInstance = null;
                exception = ex;
                return false;
            }
        }

        #endregion

        #region Copy/TryCopy

        [NotNull]
        private readonly CopyConstructorDelegate _copyConstructor;

        /// <summary>
        /// Indicates if this <see cref="T:System.Type"/> has a copy constructor.
        /// </summary>
        [PublicAPI]
        public bool HasCopyConstructor { get; }

        /// <summary>
        /// Creates a copy instance of <paramref name="other"/> with this <see cref="T:System.Type"/>'s copy constructor.
        /// </summary>
        /// <param name="other">Object to copy.</param>
        /// <returns>A reference to the newly created object.</returns>
        /// <exception cref="T:System.ArgumentException">
        /// <see cref="T:System.Type"/> is a RuntimeType or is an open generic type (that is, the ContainsGenericParameters property returns true),
        /// or if the <paramref name="other"/> instance is not exactly an instance of <see cref="T:System.Type"/>.
        /// </exception>
        /// <exception cref="T:System.MissingMethodException">
        /// No matching public copy constructor was found,
        /// or constructor exists but was not considered as copy constructor.
        /// </exception>
        [PublicAPI]
        [Pure]
        [ContractAnnotation("other:null => null; other:notnull => notnull")]
        public object Copy([CanBeNull] object other)
        {
            if (other is null)
                return null;
            return _copyConstructor(other);
        }

        /// <summary>
        /// Tries to create a copy instance of <paramref name="other"/> with this <see cref="T:System.Type"/>'s copy constructor.
        /// </summary>
        /// <remarks>This method will not throw if instantiation failed.</remarks>
        /// <param name="other">Object to copy.</param>
        /// <param name="newInstance">A reference to the newly created object, otherwise null.</param>
        /// <param name="exception">Caught exception if the instantiation failed, otherwise null.</param>
        /// <returns>True if the new instance was successfully created, false otherwise.</returns>
        [PublicAPI]
        [Pure]
        [ContractAnnotation("other:null => true, newInstance:null, exception:null;"
                            + "other:notnull => true, newInstance:notnull, exception:null;"
                            + "other:null => false, newInstance:null, exception:notnull;"
                            + "other:notnull => false, newInstance:null, exception:notnull")]
        public bool TryCopy([CanBeNull] object other, out object newInstance, out Exception exception)
        {
            try
            {
                exception = null;
                newInstance = Copy(other);
                return true;
            }
            catch (Exception ex)
            {
                newInstance = null;
                exception = ex;
                return false;
            }
        }

        #endregion

        #region Equality / IEquatable<T>

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return Equals(obj as ImmediateType);
        }

        /// <inheritdoc />
        public bool Equals(ImmediateType other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return Type == other.Type;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Type.GetHashCode();
        }

        #endregion

        #region ISerializable

        private ImmediateType(SerializationInfo info, StreamingContext context)
            : this((Type)info.GetValue("Type", typeof(Type)), (BindingFlags)info.GetValue("Flags", typeof(BindingFlags)))
        {
        }

        /// <inheritdoc />
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Flags", _flags);
            info.AddValue("Type", Type);
        }

        #endregion

        /// <inheritdoc />
        public override string ToString()
        {
            return Type.ToString();
        }
    }
}
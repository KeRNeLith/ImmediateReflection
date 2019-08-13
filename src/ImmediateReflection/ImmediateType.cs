using System;
using System.Collections.Generic;
#if SUPPORTS_SYSTEM_CORE
using System.Linq;
#else
using static ImmediateReflection.Utils.EnumerableUtils;
#endif
using System.Reflection;
using static ImmediateReflection.Utils.FieldHelpers;
#if SUPPORTS_AGGRESSIVE_INLINING
using System.Runtime.CompilerServices;
#endif
using JetBrains.Annotations;

namespace ImmediateReflection
{
    /// <summary>
    /// Represents type declarations: class types, interface types, array types, value types, enumeration types,
    /// type parameters, generic type definitions, and open or closed constructed generic types in a faster way.
    /// </summary>
    public sealed class ImmediateType : ImmediateMember, IEquatable<ImmediateType>
    {
        /// <summary>
        /// Gets the wrapped <see cref="System.Type"/>.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public Type Type { get; }

        /// <summary>
        /// Gets the fully qualified name of the <see cref="System.Type"/>, including its namespace but not its assembly.
        /// </summary>
        /// <remarks>Fallback on the type name if full name is null.</remarks>
        [PublicAPI]
        [NotNull]
        public string FullName { get; }

        /// <summary>
        /// Gets all the members of this <see cref="System.Type"/>.
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

#if SUPPORTS_LAZY
        [NotNull]
        private readonly Lazy<ImmediateFields> _fields;
#endif

        /// <summary>
        /// Gets all the fields of this <see cref="System.Type"/>.
        /// </summary>
        [PublicAPI]
        [NotNull, ItemNotNull]
#if SUPPORTS_LAZY
        public ImmediateFields Fields => _fields.Value;
#else
        public ImmediateFields Fields { get; }
#endif

        /// <summary>
        /// Gets all the properties of this <see cref="System.Type"/>.
        /// </summary>
        [PublicAPI]
        [NotNull, ItemNotNull]
        public ImmediateProperties Properties { get; }

        [NotNull]
        private readonly DefaultConstructorDelegate _constructor;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="type"><see cref="System.Type"/> to wrap.</param>
        /// <param name="flags">Flags that must be taken into account to get members.</param>
        internal ImmediateType([NotNull] Type type, BindingFlags flags = TypeAccessor.DefaultFlags)
            : base(type)
        {
            Type = type;
            FullName = type.FullName ?? Name;

            // Default constructor
#if SUPPORTS_CACHING
            DefaultConstructorData data = CachesHandler.Instance.GetDefaultConstructor(Type);
            _constructor = data.Constructor;
            HasDefaultConstructor = data.HasDefault;
#else
            _constructor = DelegatesFactory.CreateDefaultConstructor(Type, out bool hasConstructor);
            HasDefaultConstructor = hasConstructor;
#endif

            if (type.IsEnum)
            {
#if SUPPORTS_LAZY
                _fields = new Lazy<ImmediateFields>(GetImmediateFieldsForEnum);
#else
                Fields = GetImmediateFieldsForEnum();
#endif

#if SUPPORTS_SYSTEM_CORE
                Properties = new ImmediateProperties(Enumerable.Empty<PropertyInfo>());
#else
                Properties = new ImmediateProperties(Empty<PropertyInfo>());
#endif
            }
            else
            {
#if SUPPORTS_LAZY
                _fields = new Lazy<ImmediateFields>(() => new ImmediateFields(IgnoreBackingFields(type.GetFields(flags))));
#else
                Fields = new ImmediateFields(IgnoreBackingFields(type.GetFields(flags)));
#endif

                Properties = new ImmediateProperties(type.GetProperties(flags));
            }

            #region Local function

            ImmediateFields GetImmediateFieldsForEnum()
            {
                FieldInfo[] enumFields = type.GetFields();
#if SUPPORTS_SYSTEM_CORE
                FieldInfo enumValue = enumFields.First(field => !field.IsStatic);               // Current enum value field (not static)
                IEnumerable<FieldInfo> enumValues = enumFields.Where(field => field.IsStatic);  // Enum values (static)
#else
                FieldInfo enumValue = First(enumFields, field => !field.IsStatic);               // Current enum value field (not static)
                IEnumerable<FieldInfo> enumValues = Where(enumFields, field => field.IsStatic);  // Enum values (static)
#endif
                return new ImmediateFields(type, enumValue, enumValues);
            }

            #endregion
        }

        /// <summary>
        /// Gets all the members of this <see cref="System.Type"/>.
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
        /// <exception cref="ArgumentNullException">If the given <paramref name="memberName"/> is null.</exception>
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
        /// <exception cref="ArgumentNullException">If the given <paramref name="memberName"/> is null.</exception>
        [PublicAPI]
        [Pure]
        [CanBeNull]
        [ContractAnnotation("memberName:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public ImmediateMember GetMember([NotNull] string memberName) => this[memberName];

        /// <summary>
        /// Gets all the fields of this <see cref="System.Type"/>.
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
        /// <exception cref="ArgumentNullException">If the given <paramref name="fieldName"/> is null.</exception>
        [PublicAPI]
        [Pure]
        [CanBeNull]
        [ContractAnnotation("fieldName:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public ImmediateField GetField([NotNull] string fieldName) => Fields[fieldName];

        /// <summary>
        /// Gets all the properties of this <see cref="System.Type"/>.
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
        /// <exception cref="ArgumentNullException">If the given <paramref name="propertyName"/> is null.</exception>
        [PublicAPI]
        [Pure]
        [CanBeNull]
        [ContractAnnotation("propertyName:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public ImmediateProperty GetProperty([NotNull] string propertyName) => Properties[propertyName];

        /// <summary>
        /// Indicates if this <see cref="Type"/> has a default constructor.
        /// </summary>
        [PublicAPI]
        public bool HasDefaultConstructor { get; }

        /// <summary>
        /// Creates an instance of this <see cref="Type"/> with that type's default constructor.
        /// </summary>
        /// <returns>A reference to the newly created object.</returns>
        /// <exception cref="ArgumentException"><see cref="Type"/> a RuntimeType or is an open generic type (that is, the ContainsGenericParameters property returns true).</exception>
        /// <exception cref="AmbiguousMatchException"><see cref="Type"/> has several constructors defining "params" parameter only.</exception>
        /// <exception cref="MissingMethodException">No matching public constructor was found.</exception>
        /// <exception cref="TargetInvocationException">The constructor being called throws an exception.</exception>
        [PublicAPI]
        [Pure]
        [NotNull]
        public object New()
        {
            return _constructor();
        }

        /// <summary>
        /// Tries to create an instance of this <see cref="Type"/> with that type's default constructor.
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
        /// Creates an instance of this <see cref="Type"/> using the constructor that best matches the specified parameters.
        /// </summary>
        /// <remarks>Tries to use the <see cref="New()"/> if no parameter provided, otherwise fallback on <see cref="Activator.CreateInstance(System.Type,object[])"/>.</remarks>
        /// <param name="args">
        /// An array of arguments that match in number, order, and type the parameters of the constructor to invoke.
        /// If <paramref name="args"/> is an empty array or null, the constructor that takes no parameters (the default constructor) is invoked.
        /// </param>
        /// <returns>A reference to the newly created object.</returns>
        /// <exception cref="ArgumentException"><see cref="Type"/> a RuntimeType or is an open generic type (that is, the ContainsGenericParameters property returns true).</exception>
        /// <exception cref="MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism.</exception>
        /// <exception cref="MissingMethodException">No matching public constructor was found.</exception>
        /// <exception cref="NotSupportedException">
        /// If the <see cref="Type"/> cannot be a TypeBuilder.
        /// -or- Creation of <see cref="TypedReference"/>, ArgIterator, <see cref="Void"/>, and <see cref="RuntimeArgumentHandle"/> types, or arrays of those types, is not supported.
        /// -or- The assembly that contains type is a dynamic assembly that was created with Save.
        /// -or- The constructor that best matches args has varargs arguments.
        /// </exception>
        /// <exception cref="TargetInvocationException">The constructor being called throws an exception.</exception>
        /// <exception cref="TypeLoadException">If the <see cref="Type"/> is not a valid type.</exception>
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
        /// Tries to create an instance of this <see cref="Type"/> with the best matching constructor.
        /// </summary>
        /// <remarks>
        /// This method will not throw if instantiation failed.
        /// Tries to use the <see cref="New()"/> if no parameter provided, otherwise fallback on <see cref="Activator.CreateInstance(System.Type,object[])"/>.
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

        /// <inheritdoc />
        public override string ToString()
        {
            return Type.ToString();
        }
    }
}
using System.Collections;
using System.Reflection;
using JetBrains.Annotations;

namespace ImmediateReflection
{
    /// <summary>
    /// Equality comparer to use <see cref="MemberInfo"/> as keys of hash based collections.
    /// </summary>
    /// <remarks>
    /// <see cref="MemberInfo.GetHashCode"/> does not take <see cref="MemberInfo.ReflectedType"/> into account.
    /// As a consequence, an inherited member retrieved from many derived types always produces the same hash code
    /// while being a different key for each type (<see cref="MemberInfo"/> equality does take
    /// <see cref="MemberInfo.ReflectedType"/> into account). Using such keys in a hash based collection leads to
    /// massive collisions, and so to a quadratic cost when caching members of types sharing a base class.
    /// This comparer keeps the default equality but mixes <see cref="MemberInfo.ReflectedType"/> into the hash code.
    /// </remarks>
    internal sealed class MemberInfoEqualityComparer : IEqualityComparer
    {
        /// <summary>
        /// Comparer instance.
        /// </summary>
        [NotNull]
        public static readonly MemberInfoEqualityComparer Instance = new MemberInfoEqualityComparer();

        private MemberInfoEqualityComparer()
        {
        }

        /// <summary>
        /// Computes a hash code for the given <paramref name="member"/> that takes
        /// <see cref="MemberInfo.ReflectedType"/> into account.
        /// </summary>
        /// <param name="member">A <see cref="MemberInfo"/>.</param>
        /// <returns>The hash code.</returns>
        [Pure]
        public static int GetMemberHashCode([NotNull] MemberInfo member)
        {
            unchecked
            {
                return (member.GetHashCode() * 397) ^ (member.ReflectedType?.GetHashCode() ?? 0);
            }
        }

        /// <inheritdoc />
        public new bool Equals(object x, object y)
        {
            return object.Equals(x, y);
        }

        /// <inheritdoc />
        public int GetHashCode(object obj)
        {
            if (obj is MemberInfo member)
                return GetMemberHashCode(member);
            return obj.GetHashCode();
        }
    }
}

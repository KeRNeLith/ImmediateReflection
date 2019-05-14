using System;
using System.Reflection;
using JetBrains.Annotations;

namespace ImmediateReflection
{
    /// <summary>
    /// Represents a member of a <see cref="Type"/> and provides access to its metadata in a faster way.
    /// </summary>
    public abstract class ImmediateMember
    {
        /// <summary>
        /// Gets the name of the current member.
        /// </summary>
        [NotNull]
        public string Name { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="member"><see cref="MemberInfo"/> to wrap.</param>
        /// <exception cref="ArgumentNullException">If the <paramref name="member"/> is null.</exception>
        protected ImmediateMember([NotNull] MemberInfo member)
        {
            if (member is null)
                throw new ArgumentNullException(nameof(member));

            Name = member.Name;
        }
    }
}
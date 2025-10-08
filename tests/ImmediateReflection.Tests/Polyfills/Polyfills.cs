#if NETFRAMEWORK || !NET7_0_OR_GREATER
namespace System.Runtime.CompilerServices
{
    [AttributeUsage(
        AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface |
        AttributeTargets.Delegate | AttributeTargets.Enum | AttributeTargets.Field |
        AttributeTargets.Property | AttributeTargets.Event | AttributeTargets.Method |
        AttributeTargets.Parameter | AttributeTargets.ReturnValue | AttributeTargets.GenericParameter)]
    internal sealed class NullableContextAttribute : Attribute
    {
        public readonly byte Flag;
        public NullableContextAttribute(byte flag) => Flag = flag;
    }
}
#endif
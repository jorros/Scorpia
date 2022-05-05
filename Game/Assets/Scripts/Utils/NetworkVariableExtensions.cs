using Unity.Collections;
using Unity.Netcode;

namespace Utils
{
    public static class NetworkVariableExtensions
    {
        public static string ValueAsString(
            this NetworkVariable<ForceNetworkSerializeByMemcpy<FixedString64Bytes>> variable)
        {
            return variable.Value.Value.Value;
        }
    }
}
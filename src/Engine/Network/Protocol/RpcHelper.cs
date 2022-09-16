using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Scorpia.Engine.Helper;

namespace Scorpia.Engine.Network.Protocol;

internal static class RpcHelper
{
    public static Dictionary<int, MethodBase> GetClientRpcs(this Type type)
    {
        return RetrieveRpcs(type, typeof(ClientRpcAttribute));
    }
    
    public static Dictionary<int, MethodBase> GetServerRpcs(this Type type)
    {
        return RetrieveRpcs(type, typeof(ServerRpcAttribute));
    }
    
    private static Dictionary<int, MethodBase> RetrieveRpcs(Type type, Type attribute)
    {
        var rpcs = new Dictionary<int, MethodBase>();

        foreach (var method in type.GetRuntimeMethods()
                     .Where(x => Attribute.IsDefined(x, attribute)))
        {
            if (method.GetParameters().Length > 2)
            {
                throw new EngineException($"Malformed RPC method: {method.Name} on {type.Name}");
            }

            rpcs.Add(method.Name.GetDeterministicHashCode(), method);
        }

        return rpcs;
    }
}
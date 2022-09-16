using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Scorpia.Engine.Helper;

namespace Scorpia.Engine.Network.Protocol;

public static class NetworkedHelper
{
    public static Dictionary<int, FieldInfo> GetNetworkedFields(this Type type)
    {
        var vars = new Dictionary<int, FieldInfo>();

        foreach (var field in type.GetRuntimeFields().Where(t =>
                     t.FieldType.IsGenericType && t.FieldType.GetGenericTypeDefinition() == typeof(NetworkedVar<>)))
        {
            vars.Add(field.Name.GetDeterministicHashCode(), field);
        }

        return vars;
    }
    
    public static Dictionary<int, FieldInfo> GetNetworkedLists(this Type type)
    {
        var vars = new Dictionary<int, FieldInfo>();

        foreach (var field in type.GetRuntimeFields().Where(t =>
                     t.FieldType.IsGenericType && t.FieldType.GetGenericTypeDefinition() == typeof(NetworkedList<>)))
        {
            vars.Add(field.Name.GetDeterministicHashCode(), field);
        }

        return vars;
    }
}
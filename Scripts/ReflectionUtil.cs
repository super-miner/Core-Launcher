using System;
using System.Collections.Generic;
using System.Reflection;

namespace CoreLauncher.Scripts; 

public static class ReflectionUtil {
    public static IEnumerable<Type> GetTypesWithAttribute(Type attributeType, Assembly assembly = null) {
        assembly = assembly ?? Assembly.GetCallingAssembly();
        
        foreach(Type type in assembly.GetTypes()) {
            if (type.GetCustomAttributes(attributeType, true).Length > 0) {
                yield return type;
            }
        }
    }
}
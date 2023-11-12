using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CoreLauncher.Scripts.Systems; 

public static class ReflectionUtil {
    public static IEnumerable<(Type, T)> GetTypesWithAttribute<T>(Assembly assembly = null) {
        assembly = assembly ?? Assembly.GetCallingAssembly();
        
        foreach(Type type in assembly.GetTypes()) {
            object[] attributes = type.GetCustomAttributes(typeof(T), true);
            if (attributes.Length > 0) {
                yield return (type, (T) attributes[0]);
            }
        }
    }
    
    public static IEnumerable<Type> GetDerivedTypes<T>(Assembly assembly = null)
    {
        assembly = assembly ?? Assembly.GetCallingAssembly();
        
        Type baseType = typeof(T);
        foreach (Type type in assembly.GetTypes()) {
            if (type != baseType && baseType.IsAssignableFrom(type)) {
                yield return type;
            }
        }
    }
}
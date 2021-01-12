using Snow.Attributes;
using System;
using System.Linq;
using System.Reflection;

namespace Snow.Core
{
    /// <summary>
    /// Creates instances of a specified type.
    /// </summary>
    internal static class SnowActivator
    {
        /// <summary>
        /// Returns a new instance of the Type, using
        /// the method marked as [CreateInstance] if
        /// present. Otherwise, the default constructor
        /// must be available.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        internal static object CreateInstance(Type t)
        {
            var methodInfo = t.GetRuntimeMethods().FirstOrDefault(mi =>
            {
                if (mi.IsStatic)
                    return mi.CustomAttributes.Any(ca => ca.AttributeType == typeof(CreateInstanceAttribute));
                return false;
            });

            return methodInfo != null ? methodInfo.Invoke(null, null) : Activator.CreateInstance(t);
        }
    }
}

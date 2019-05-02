using Snow.Exceptions;
using System;
using System.Collections.Generic;

namespace Snow.Core
{
    /// <summary>
    /// Stores components ready to be injected.
    /// </summary>
    internal static class Container
    {
        private static readonly IDictionary<Type, object?> Dependencies = new Dictionary<Type, object?>();

        /// <summary>
        /// Register a component with App-scope: A single instance
        /// will be used for all subsequent injections.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        public static void Register<T>(T instance) where T : class => Dependencies.Add(instance.GetType(), instance);

        /// <summary>
        /// Register a component with Request-scope: A new instance
        /// will be created for each subsequent injection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        public static void RegisterRequestScoped<T>(T instance) where T : class => Dependencies.Add(instance.GetType(), null);

        /// <summary>
        /// Retrieves an instance of a component to
        /// inject it (creating a new instance if the
        /// component is Request-scoped).
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static object Retrieve(Type t)
        {
            if (Dependencies.ContainsKey(t))
                return Dependencies[t] ?? SnowReflection.InstanceRequestScoped(t);
            throw new NoSuitableTypeFound(t?.FullName);
        }
    }
}

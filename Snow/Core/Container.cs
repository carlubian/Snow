using Snow.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Snow.Core
{
    /// <summary>
    /// Stores components ready to be injected.
    /// </summary>
    internal static class Container
    {
        internal static readonly IDictionary<Type, object?> Dependencies = new Dictionary<Type, object?>();
        /// <summary>
        /// A list of all components that could be instanced at any point.
        /// </summary>
        internal static IList<Type> AllComponents = Enumerable.Empty<Type>().ToList();

        /// <summary>
        /// Stores types alias equivalence.
        /// </summary>
        internal static IDictionary<Type, Type> Aliases = new Dictionary<Type, Type>();

        /// <summary>
        /// Register a component with App-scope: A single instance
        /// will be used for all subsequent injections.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        public static void Register<T>(Type type, T instance) where T : class => Dependencies.Add(type, instance);

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
            throw new NoSuitableTypeFound(t?.FullName ?? "Null type received.");
        }
    }
}

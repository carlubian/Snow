using DotNet.Misc.Extensions.Linq;
using Snow.Attributes;
using Snow.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Snow.Core
{
    /// <summary>
    /// Handles discovery and registration of components
    /// across the entire assembly source.
    /// </summary>
    internal static class SnowReflection
    {
        /// <summary>
        /// Finds all classes marked as [Component] in the
        /// assembly, and stores them in the Container.
        /// </summary>
        /// <param name="runnable">Runnable class</param>
        internal static void InstanceComponents(ISnowRunnable runnable)
        {
            var type = runnable.GetType();
            var assembly = type?.Assembly;
            if (assembly is null)
                return;

            assembly.GetTypes()
                .Where(t => t.CustomAttributes.Any(ca => typeof(ComponentAttribute).IsAssignableFrom(ca.AttributeType)))
                .ForEach(t =>
                {
                    if (t.CustomAttributes.Any(ca => ca.AttributeType == typeof(RequestScopeAttribute)))
                    {
                        Container.RegisterRequestScoped<object>(Activator.CreateInstance(t));
                    }
                    else
                    {
                        var instance = SnowActivator.CreateInstance(t);
                        if (instance == null)
                            throw new NoSuitableConstructorFound(t.FullName);
                        Container.Register(instance);
                    }
                });
        }

        /// <summary>
        /// Finds all components in the assembly, and provides
        /// each occurrence of [Autowired] with an instance.
        /// </summary>
        /// <param name="runnable">Runnable class</param>
        internal static void InjectDependencies(ISnowRunnable runnable)
        {
            var type = runnable.GetType();
            ReadyInstance(type, runnable);

            var assembly = type?.Assembly;
            if (assembly is null)
                return;

            ((IEnumerable<Type>)assembly.GetTypes())
                .Where(t => t.CustomAttributes.Any(ca => typeof(ComponentAttribute).IsAssignableFrom(ca.AttributeType)))
                .ForEach(t => ReadyInstance(t, Container.Retrieve(t)));
        }

        /// <summary>
        /// Handles instantiation of Request-scoped components.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        internal static object InstanceRequestScoped(Type t)
        {
            var instance = SnowActivator.CreateInstance(t);
            if (instance == null)
                throw new NoSuitableConstructorFound(t?.FullName);

            ReadyInstance(t, instance);
            return instance;
        }

        /// <summary>
        /// Handles component injection for an individual
        /// component class.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="instance"></param>
        private static void ReadyInstance(Type t, object instance)
        {
            t.GetRuntimeFields()
                .Where(f => f.CustomAttributes.Any(ca => ca.AttributeType == typeof(AutowiredAttribute)))
                .ForEach(f =>
                {
                    if (f is null)
                        return;
                    
                    f.SetValue(instance, Container.Retrieve(f.FieldType));
                });
        }
    }
}

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

            Container.AllComponents = assembly.GetTypes()
                                .Where(t => t.CustomAttributes.Any(ca => typeof(ComponentAttribute).IsAssignableFrom(ca.AttributeType)))
                                .ToList();

            foreach (var component in Container.AllComponents)
            {
                InstanceComponent(component);
            }
        }

        private static void InstanceComponent(Type component)
        {
            // The component may have been initialized by the
            // autowiring constructor on one of the previous components.
            if (Container.Dependencies.Keys.Contains(component))
                return;

            // Locate the autowiring constructor, if present
            var constructor = component.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .FirstOrDefault(c => c.CustomAttributes.Any(ca => typeof(AutowiredAttribute).IsAssignableFrom(ca.AttributeType)));

            // Missing constructor: Create default instance and store
            if (constructor is null)
            {
                var instance = SnowActivator.CreateInstance(component);
                if (instance == null)
                    throw new NoSuitableConstructorFound(component.FullName);
                Container.Register(instance);
                return;
            }

            // Found constructor: Locate and inject dependencies
            var parameters = constructor.GetParameters();
            var paramObjects = new List<object>();

            foreach (var parameter in parameters)
            {
                // Parameter must be a component
                if (parameter.ParameterType.CustomAttributes.Any(ca => typeof(ComponentAttribute).IsAssignableFrom(ca.AttributeType)))
                {
                    // Check if the type is already instanced
                    if (!Container.Dependencies.Keys.Contains(parameter.ParameterType))
                    {
                        // Instance the type before injecting it
                        InstanceComponent(parameter.ParameterType); // TODO Check for cyclic dependencies
                    }
                    
                    paramObjects.Add(Container.Retrieve(parameter.ParameterType));
                }
                else
                    throw new AutowiringConstructorException($@"Parameter {parameter.ParameterType} is not a component.
All parameters in an Autowired Constructor must be components.");
            }

            // Invoke constructor and register component
            var newInstance = constructor.Invoke(paramObjects.ToArray());
            Container.Register(newInstance);
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

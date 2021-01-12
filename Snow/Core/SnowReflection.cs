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

            // Also check referenced assemblies.
            foreach (var assemblyName in assembly.GetReferencedAssemblies())
            {
                var dependency = Assembly.Load(assemblyName);

                Container.AllComponents = Container.AllComponents.Concat(
                    dependency.GetTypes()
                                .Where(t => t.CustomAttributes.Any(ca => typeof(ComponentAttribute).IsAssignableFrom(ca.AttributeType)))
                ).ToList();
            }

            foreach (var component in Container.AllComponents)
            {
                InstanceComponent(component);
            }
        }

        private static void InstanceComponent(Type component)
        {
            // Check if type is an alias
            if (Container.Aliases.ContainsKey(component))
                component = Container.Aliases[component];

            // The component may have been initialized by the
            // autowiring constructor on one of the previous components.
            if (Container.Dependencies.Keys.Contains(component))
                return;

            // Locate the autowiring constructor, if present
            var constructor = LocateAutowiredConstructor(component);

            // Missing constructor: Create default instance and store
            if (constructor is null)
            {
                var instance = SnowActivator.CreateInstance(component);
                if (instance == null)
                    throw new NoSuitableConstructorFound(component.FullName);
                Container.Register(component, instance);

                // Look for type aliases
                var defAliases = component.CustomAttributes.Where(ca => typeof(TypeAliasAttribute).IsAssignableFrom(ca.AttributeType))
                    .SelectMany(ca => ca.ConstructorArguments)
                    .Select(ca => ca.Value)
                    .Cast<Type>();

                foreach (var alias in defAliases)
                {
                    // The component must be assignable to alias type
                    if (!alias.IsAssignableFrom(component))
                        throw new InvalidTypeAliasException($"{component} cannot have {alias} as a Type Alias.");

                    Container.Register(alias, instance);
                    Container.Aliases.Add(alias, component);
                }

                return;
            }

            // Found constructor: Locate and inject dependencies
            var parameters = constructor.GetParameters();
            var paramObjects = new List<object>();

            foreach (var parameter in parameters)
            {
                var parameterType = parameter.ParameterType;
                // Check if parameter is an alias
                if (Container.Aliases.ContainsKey(parameterType))
                    parameterType = Container.Aliases[parameterType];

                // Parameter must be a component
                if (parameterType.CustomAttributes.Any(ca => typeof(ComponentAttribute).IsAssignableFrom(ca.AttributeType)))
                {
                    // Check if the type is already instanced
                    if (!Container.Dependencies.Keys.Contains(parameterType))
                    {
                        // Instance the type before injecting it
                        InstanceComponent(parameterType);
                    }
                    
                    paramObjects.Add(Container.Retrieve(parameterType));
                }
                else
                    throw new AutowiringConstructorException($@"Parameter {parameterType} is not a component.
All parameters in an Autowired Constructor must be components.");
            }

            // Invoke constructor and register component
            var newInstance = constructor.Invoke(paramObjects.ToArray());
            Container.Register(component, newInstance);

            // Look for type aliases
            var aliases = component.CustomAttributes.Where(ca => typeof(TypeAliasAttribute).IsAssignableFrom(ca.AttributeType));
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

            // Also fill components in dependent assemblies.
            foreach (var assemblyName in assembly.GetReferencedAssemblies())
            {
                var dependency = Assembly.Load(assemblyName);

                ((IEnumerable<Type>)dependency.GetTypes())
                    .Where(t => t.CustomAttributes.Any(ca => typeof(ComponentAttribute).IsAssignableFrom(ca.AttributeType)))
                    .ForEach(t => ReadyInstance(t, Container.Retrieve(t)));
            }
        }

        /// <summary>
        /// Handles instantiation of Request-scoped components.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        internal static object InstanceRequestScoped(Type component)
        {
            // Check if type is an alias
            if (Container.Aliases.ContainsKey(component))
                component = Container.Aliases[component];

            // Locate the autowiring constructor, if present
            var constructor = LocateAutowiredConstructor(component);

            // Missing constructor: Create default instance and return
            if (constructor is null)
            {
                var instance = SnowActivator.CreateInstance(component);
                if (instance == null)
                    throw new NoSuitableConstructorFound(component.FullName);

                ReadyInstance(component, instance);
                return instance;
            }

            // Found constructor: Locate and inject dependencies
            var parameters = constructor.GetParameters();
            var paramObjects = new List<object>();

            foreach (var parameter in parameters)
            {
                // Parameter must be a component
                if (parameter.ParameterType.CustomAttributes.Any(ca => typeof(ComponentAttribute).IsAssignableFrom(ca.AttributeType)))
                {
                    // At this point, all dependencies have been registered
                    // during the initial launch pass.
                    paramObjects.Add(Container.Retrieve(parameter.ParameterType));
                }
                else
                    throw new AutowiringConstructorException($@"Parameter {parameter.ParameterType} is not a component.
All parameters in an Autowired Constructor must be components.");
            }

            // Invoke constructor and register component
            var newInstance = constructor.Invoke(paramObjects.ToArray());

            ReadyInstance(component, newInstance);
            return newInstance;
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

        /// <summary>
        /// Returns the ConstructorInfo of the first autowired
        /// constructor present in the specified component
        /// type, if present.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        private static ConstructorInfo LocateAutowiredConstructor(Type component)
        {
            return component.GetConstructors(BindingFlags.Public 
                                            | BindingFlags.NonPublic 
                                            | BindingFlags.Instance)
                            .FirstOrDefault(c => c.CustomAttributes
                                .Any(ca => typeof(AutowireConstructorAttribute).IsAssignableFrom(ca.AttributeType)));
        }
    }
}

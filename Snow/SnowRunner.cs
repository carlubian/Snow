using Snow.Core;
using System;

[assembly: CLSCompliant(true)]
namespace Snow
{
    /// <summary>
    /// Entry Point for Snow apps.
    /// </summary>
    public static class SnowRunner
    {
        /// <summary>
        /// Runs a Snow app with optional parameters.
        /// </summary>
        /// <param name="runnable">Snow runnable class</param>
        /// <param name="args">Parameters</param>
        public static void Run(ISnowRunnable runnable, string[]? args = null)
        {
            SnowReflection.InstanceComponents(runnable);
            SnowReflection.InjectDependencies(runnable);
            runnable.Run(args);
        }
    }
}

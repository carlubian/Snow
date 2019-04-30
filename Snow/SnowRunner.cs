using Snow.Core;

namespace Snow
{
    /// <summary>
    /// Entry Point for Snow apps.
    /// </summary>
    public static class SnowRunner
    {
        /// <summary>
        /// Runs a Snow app with the specified parameters.
        /// </summary>
        /// <param name="runnable">Snow runnable class</param>
        /// <param name="args">Parameters</param>
        public static void Run(ISnowRunnable runnable, string[] args)
        {
            SnowReflection.InstanceComponents(runnable);
            SnowReflection.InjectDependencies(runnable);
            runnable.Run(args);
        }

        /// <summary>
        /// Runs a Snow app.
        /// </summary>
        /// <param name="runnable">Snow runnable class</param>
        public static void Run(ISnowRunnable runnable)
        {
            Run(runnable, null);
        }
    }
}

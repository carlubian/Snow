namespace Snow
{
    /// <summary>
    /// Indicates that this class can be run
    /// by the Snow Runner.
    /// </summary>
    public interface ISnowRunnable
    {
        void Run(string[]? args);
    }
}

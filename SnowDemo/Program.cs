using Snow;
using Snow.Attributes;
using SnowDemo.Services;
using System;

namespace SnowDemo
{
    // Make the EntryPoint class implement ISnowRunnable
    class Program : ISnowRunnable
    {
        // Invoke Snow in the Main method:
        static void Main(string[] _)
        {
            SnowRunner.Run(new Program());
        }

        // Inject a component
        [Autowired]
        private readonly WrapService? Service;

        // Implemented from ISnowRunnable. Logic goes here.
        public void Run(string[]? args)
        {
            // Use the injected component
            Console.WriteLine(Service?.GetText());
            Console.WriteLine(Service?.GetNumber());
            Console.WriteLine(Service?.Calculate(8));
            Console.WriteLine(Service?.NumberProperty());
        }
    }
}

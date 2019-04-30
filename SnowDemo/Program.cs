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
        static void Main(string[] args)
        {
            SnowRunner.Run(new Program());
            // Or this way:
            //SnowRunner.Run(new Program(), args);
        }

        // Inject a component
        [Autowired]
        private TextService Text;

        // Implemented from ISnowRunnable. Logic goes here.
        public void Run(string[] args)
        {
            // Use the injected component
            Console.WriteLine(Text.GetText());
        }
    }
}

using System;

namespace Snow.Exceptions
{
    /// <summary>
    /// Snow couldn't instantiate this component,
    /// because no suitable constructor was found.
    /// </summary>
    public class NoSuitableConstructorFound : Exception
    {
        public NoSuitableConstructorFound()
        {
        }

        public NoSuitableConstructorFound(string message)
          : base(message)
        {
        }
    }
}

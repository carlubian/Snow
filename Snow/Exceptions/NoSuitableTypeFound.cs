using System;

namespace Snow.Exceptions
{
    /// <summary>
    /// Snow couldn't inject a component into this
    /// request, because no suitalbe component was
    /// registered.
    /// </summary>
    public class NoSuitableTypeFound : Exception
    {
        public NoSuitableTypeFound()
        {
        }

        public NoSuitableTypeFound(string message)
          : base(message)
        {
        }
    }
}

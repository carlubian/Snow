using System;

namespace Snow.Exceptions
{
    /// <summary>
    /// An error happened while processing a component
    /// with an Autowired Constructor.
    /// </summary>
    class AutowiringConstructorException : Exception
    {
        public AutowiringConstructorException() : base()
        {
        }

        public AutowiringConstructorException(string message)
            : base(message)
        {
        }
    }
}

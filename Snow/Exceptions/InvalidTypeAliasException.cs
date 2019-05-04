using System;

namespace Snow.Exceptions
{
    /// <summary>
    /// Thrown by Snow when an invalid type is specified
    /// as alias for a component.
    /// </summary>
    public class InvalidTypeAliasException : Exception
    {
        public InvalidTypeAliasException() : base()
        {
        }

        public InvalidTypeAliasException(string message) : base(message)
        {
        }
    }
}

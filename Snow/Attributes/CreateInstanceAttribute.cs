using System;

namespace Snow.Attributes
{
    /// <summary>
    /// This method will be used by Snow when
    /// instancing the component for injection.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor)]
    public class CreateInstanceAttribute : Attribute
    {
    }
}

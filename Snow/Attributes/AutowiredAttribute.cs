using System;

namespace Snow.Attributes
{
    /// <summary>
    /// The value of this property is a component
    /// that will be provided by Snow at runtime.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class AutowiredAttribute : Attribute
    {
    }
}

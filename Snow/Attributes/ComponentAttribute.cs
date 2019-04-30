using System;

namespace Snow.Attributes
{
    /// <summary>
    /// This class is a component that can
    /// be instanced and injected by Snow.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ComponentAttribute : Attribute
    {
    }
}

using System;

namespace Snow.Attributes
{
    /// <summary>
    /// This is an Autowired Constructor whose parameters
    /// will be resolved by Snow at runtime. Note that all
    /// parameters must be component classes.
    /// </summary>
    public class AutowireConstructorAttribute : Attribute
    {
    }
}

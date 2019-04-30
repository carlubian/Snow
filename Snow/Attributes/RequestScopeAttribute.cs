using System;

namespace Snow.Attributes
{
    /// <summary>
    /// This component must be instanced each time
    /// it is injected, instead of sharing a single
    /// instance across the app.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class RequestScopeAttribute : Attribute
    {
    }
}

using System;

namespace Snow.Attributes
{
    /// <summary>
    /// Snow will store a copy of this component associated
    /// with the specified type, as well as its own type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TypeAliasAttribute : Attribute
    {
        public Type Alias { get; }

        public TypeAliasAttribute(Type alias)
        {
            Alias = alias;
        }
    }
}

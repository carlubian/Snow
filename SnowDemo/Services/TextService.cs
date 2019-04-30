using Snow.Attributes;

namespace SnowDemo.Services
{
    [Component]
    internal class TextService
    {
        internal string GetText() => "Hello from Component";
    }
}

using Snow.Attributes;

namespace SnowDemo.Services
{
    [Service]
    internal class TextService
    {
        internal string GetText() => "Hello from Component";
    }
}

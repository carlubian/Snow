using Snow.Attributes;
using SnowDemo.Interfaces;

namespace SnowDemo.Services
{
    [Service]
    internal class TextService
    {
        private INumberService NumberService;

        [AutowireConstructor]
        internal TextService(INumberService NumberService)
        {
            this.NumberService = NumberService;
        }

        internal string GetText() => "Hello from Component";

        internal int GetNumber() => NumberService.Invoke();
    }
}

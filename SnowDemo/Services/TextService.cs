using Snow.Attributes;

namespace SnowDemo.Services
{
    [Service]
    internal class TextService
    {
        private NumberService NumberService;

        [Autowired]
        internal TextService(NumberService NumberService)
        {
            this.NumberService = NumberService;
        }

        internal string GetText() => "Hello from Component";

        internal int GetNumber() => NumberService.Invoke();
    }
}

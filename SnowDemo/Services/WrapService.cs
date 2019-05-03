using Snow.Attributes;

namespace SnowDemo.Services
{
    [Service]
    internal class WrapService
    {
        private readonly TextService TextService;

        [Autowired]
        internal WrapService(TextService TextService)
        {
            this.TextService = TextService;
        }

        internal string GetText() => TextService.GetText();

        internal int GetNumber() => TextService.GetNumber();
    }
}

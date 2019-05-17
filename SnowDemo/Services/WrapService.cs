using Snow.Attributes;
using SnowDemo.Interfaces;

namespace SnowDemo.Services
{
    [Service]
    internal class WrapService
    {
        private readonly TextService TextService;
        private readonly CalculatorService CalculatorService;
        [Autowired]
        private INumberService NumberService;

        [AutowireConstructor]
        internal WrapService(TextService TextService, CalculatorService CalculatorService)
        {
            this.TextService = TextService;
            this.CalculatorService = CalculatorService;
        }

        internal string GetText() => TextService.GetText();

        internal int GetNumber() => TextService.GetNumber();

        internal int Calculate(int n) => CalculatorService.TimesTwo(n);

        internal int NumberProperty() => NumberService.Invoke();
    }
}

using Snow.Attributes;

namespace SnowDemo.Services
{
    [Service]
    [RequestScope]
    internal class CalculatorService
    {
        [Autowired]
        private NumberService NumberService;

        internal int TimesTwo(int n) => n * NumberService.Invoke();
    }
}

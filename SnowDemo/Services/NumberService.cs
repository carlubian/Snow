using Snow.Attributes;
using SnowDemo.Interfaces;

namespace SnowDemo.Services
{
    [Service]
    [RequestScope]
    [TypeAlias(typeof(INumberService))]
    public class NumberService : INumberService
    {
        public int Invoke() => 2;
    }
}

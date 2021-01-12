using Snow.Attributes;
using System;

namespace SnowDemo.Core
{
    [Service]
    public class LibraryService
    {
        [Autowired]
        private InternalService Internal;

        public string GetMessage() => $"Message from library, version {Internal.GetVersion()}";
    }
}

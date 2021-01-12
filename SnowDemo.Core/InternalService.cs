using Snow.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowDemo.Core
{
    [Service]
    internal class InternalService
    {
        internal int GetVersion() => 12;
    }
}

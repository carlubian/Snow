using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.AspNetCore
{
    /// <summary>
    /// Temporary fix to allow SignalR Hubs to request
    /// a service collection and use Snow components.
    /// 
    /// SignalR Hubs are managed by the ASP.Net Core DI
    /// framework, and have a transient lifecycle.
    /// </summary>
    public static class HubServices
    {
        /// <summary>
        /// Retrieve a type from the Snow container. Ugly hack,
        /// but that's how it works for now.
        /// </summary>
        public static T Retrieve<T>()
        {
            var type = typeof(T);

            return (T)Core.Container.Retrieve(type);
        }
    }
}

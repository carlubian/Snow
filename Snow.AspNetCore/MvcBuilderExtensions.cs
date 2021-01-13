using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.AspNetCore
{
    public static class MvcBuilderExtensions
    {
        /// <summary>
        /// Adds support for Snow-instanced controllers, allowing controllers to
        /// act as a component and request component injections. Note: All
        /// controllers must be marked with a [ComponentAttribute] in order to
        /// work at all.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMvcBuilder AddSnow(this IMvcBuilder builder)
        {
            builder.Services.Replace(ServiceDescriptor.Transient<IControllerActivator, SnowControllerActivator>());

            return builder;
        }
    }
}

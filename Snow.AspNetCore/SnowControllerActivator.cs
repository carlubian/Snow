using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;

namespace Snow.AspNetCore
{
    public class SnowControllerActivator : IControllerActivator
    {
        public object Create(ControllerContext context)
        {
            var controllerType = context.ActionDescriptor.ControllerTypeInfo.AsType();

            return Core.Container.Retrieve(controllerType);
        }

        public void Release(ControllerContext context, object controller)
        {
            // Controllers are not disposed
        }
    }
}

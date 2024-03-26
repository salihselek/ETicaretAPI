using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.CustomAttributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Reflection;

namespace ETicaretAPI.API.Filters
{
    public class RolePermissionFilter : IAsyncActionFilter
    {

        readonly IUserService _userService;

        public RolePermissionFilter(IUserService userService)
        {
            this._userService = userService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var name = context.HttpContext.User.Identity?.Name;
            if (!string.IsNullOrEmpty(name) && name != "slhs1")
            {
                var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
                var attribute = descriptor.MethodInfo.GetCustomAttribute(typeof(AuthorizeDefinationAttribute)) as AuthorizeDefinationAttribute;

                var httpAttributte = descriptor.MethodInfo.GetCustomAttribute(typeof(HttpMethodAttribute)) as HttpMethodAttribute;

                var code = $"{(httpAttributte != null ? httpAttributte.HttpMethods.First() : HttpMethods.Get)}.{attribute.ActionType}.{attribute.Defination.Replace(" ", "")}";

                var hasRole = await _userService.HasRolePermissionToEndpointAsync(name, code);
                if (!hasRole)
                    context.Result = new UnauthorizedResult();
                else
                    await next();
            }
            else
                await next();
        }
    }
}

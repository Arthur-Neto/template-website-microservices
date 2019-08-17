using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Template.Api.Filters
{
    public class CheckInvalidIdOnRouteFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.TryGetValue("id", out object value))
            {
                if ((int)value <= 0)
                {
                    context.Result = new BadRequestResult();
                }
            }
        }
    }
}

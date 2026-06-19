using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FashionShop.Web.Filters
{
    public class AdminAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var role = context.HttpContext.Session.GetString("UserRole");

            if (role != "Admin")
            {
                context.Result = new RedirectToActionResult(
                    "Login",
                    "Account",
                    new { area = "" }
                );
            }

            base.OnActionExecuting(context);
        }
    }
}
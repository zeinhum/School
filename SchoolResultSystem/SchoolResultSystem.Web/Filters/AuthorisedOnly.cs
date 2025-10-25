using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SchoolResultSystem.Web.Filters
{
    public class AuthorizeUser : ActionFilterAttribute
    {
        private readonly string _requiredRole;

        public AuthorizeUser(string requiredRole = "")
        {
            _requiredRole = requiredRole;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            var userRole = session.GetString("UserRole");

            // 🔒 If no session
            if (string.IsNullOrEmpty(userRole))
            {
                context.Result = new RedirectToActionResult("Index", "Home", new { area = "" });
                return;
            }

            // 🔒 If specific role required
            if (!string.IsNullOrEmpty(_requiredRole) && userRole != _requiredRole)
            {
                context.Result = new RedirectToActionResult("Index", "Home", new { area = "" });
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}

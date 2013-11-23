using InjectCC.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using InjectCC.Web.Controllers;
using System.Web.Routing;

namespace InjectCC.Web.Filters
{
    public class ValidateUserAttribute : ActionFilterAttribute
    {
        private UserRepository _users;
        private string _action;
        private string _controller;

        /// <param name="action">The action to redirect to on failed validation.</param>
        /// <param name="controller">The controller to redirect to on failed validation.</param>
        public ValidateUserAttribute(string action, string controller)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            if (controller == null)
                throw new ArgumentNullException("controller");

            _users = new UserRepository();
            _action = action;
            _controller = controller;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext == null)
                throw new ArgumentNullException("filterContext");

            // A profile must exist for the logged-in user. If not, log them out.
            var authenticated = filterContext.HttpContext.User.Identity.IsAuthenticated;
            var userExists = _users.GetById(WebSecurity.CurrentUserId) != null;
            if (authenticated && !userExists)
            {
                WebSecurity.Logout();
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    { "action", _action },
                    { "controller", _controller }
                });
            }
        }
    }
}
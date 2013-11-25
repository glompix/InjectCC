using System;
using System.Web.Mvc;

namespace InjectCC.Web.Controllers
{
    public class InjectCcController : Controller
    {
        private const string _tdErrorKey = "__Error";

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (TempData[_tdErrorKey] != null)
            {
                var errorMessage = (string)TempData[_tdErrorKey];
                ModelState.AddModelError(errorMessage);
            }
            base.OnActionExecuting(filterContext);
        }

        public RedirectToRouteResult RedirectErrorToAction(string errorMessage, string actionName, string controllerName)
        {
            TempData[_tdErrorKey] = errorMessage;
            return RedirectToAction(actionName, controllerName);
        }

        public RedirectToRouteResult RedirectErrorToAction(string errorMessage, string actionName, string controllerName, object routeValues)
        {
            TempData[_tdErrorKey] = errorMessage;
            return RedirectToAction(actionName, controllerName, routeValues);
        }
    }

    public static class Extensions
    {
        public static void AddModelError(this ModelStateDictionary modelState, string errorMessage)
        {
            modelState.AddModelError(string.Empty, errorMessage);
        }
    }
}
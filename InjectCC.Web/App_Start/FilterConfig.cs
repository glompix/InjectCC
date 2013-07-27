using System.Web;
using System.Web.Mvc;
using InjectCC.Web.Filters;

namespace InjectCC.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new InitializeSimpleMembershipAttribute());
        }
    }
}
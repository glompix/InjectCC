using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace InjectCC.Web
{
    public class BundleConfig
    {
        private static string[] _jqueryValidation = new string[] {
            
        };

        private static string[] _jqueryUI = new string[] {
            "~/Scripts/jquery-ui-{version}.js"
        };

        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/common").Include(
                "~/Scripts/jquery-1.9.1.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/underscore.js",
                "~/Scripts/Models/injectcc.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/account").Include(
                "~/Scripts/jquery.validate.js",
                "~/Scripts/jquery.validate.unobtrusive.js",
                "~/Scripts/bootstrap.validate.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/medication").Include(
                "~/Scripts/raphael.js",
                "~/Scripts/jquery.validate.js",
                "~/Scripts/jquery.validate.unobtrusive.js",
                "~/Scripts/bootstrap.validate.js",
                "~/Scripts/Modules/sitepicker.js",
                "~/Scripts/Models/injectcc.medication.js",
                "~/Scripts/jquery-ui-1.10.1.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/injection").Include(
                "~/Scripts/raphael.js",
                "~/Scripts/jquery.validate.js",
                "~/Scripts/jquery.validate.unobtrusive.js",
                "~/Scripts/bootstrap.validate.js",
                "~/Scripts/bootstrap-datepicker.js",
                "~/Scripts/Modules/sitepicker.js",
                "~/Scripts/Models/injectcc.injection.js"
            ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/bootstrap-responsive.css",
                "~/Content/font-awesome.css",
                "~/Content/Site.css"
            ));

            bundles.Add(new StyleBundle("~/Content/injection").Include(
                "~/Content/bootstrap-datepicker.css"));
        }
    }
}
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace InjectCC.Web
{
    public class BundleConfig
    {
        private static string[] _jqueryValidation = new string[] {
            "~/Scripts/jquery.unobtrusive*",
            "~/Scripts/jquery.validate*",
            "~/Scripts/bootstrap.validate*"
        };

        private static string[] _jqueryUI = new string[] {
            "~/Scripts/jquery-ui-{version}.js"
        };

        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/common").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/bootstrap*",
                "~/Scripts/modernizr-*",
                "~/Scripts/underscore.js",
                "~/Scripts/backbone.js",
                "~/Scripts/Models/injectcc.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/account").Include(
                _jqueryUI.Union(_jqueryValidation).ToArray()
            ));

            bundles.Add(new ScriptBundle("~/bundles/medication").Include(
                "~/Scripts/raphael*",
                "~/Scripts/Modules/sitepicker.js",
                "~/Scripts/Models/injectcc.medication.js",
                "~/Scripts/jquery-ui-{version}.js",
                "~/Scripts/jquery.validate.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/injection").Include(
                "~/Scripts/raphael*",
                "~/Scripts/bootstrap-datepicker*",
                "~/Scripts/Modules/sitepicker.js",
                "~/Scripts/Models/injectcc.injection.js"
            ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/bootstrap-responsive.css",
                "~/Content/font-awesome.css",
                "~/Content/site.css"
            ));

            bundles.Add(new StyleBundle("~/Content/injection").Include(
                "~/Content/bootstrap-datepicker*"));
        }
    }
}
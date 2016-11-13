using System.Web;
using System.Web.Optimization;

namespace SchedBot
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/plugins").Include(
                "~/Scripts/js.cookie.min.js",
                "~/Scripts/jquery.slimscroll.min.js",
                "~/Scripts/jquery.blockui.min.js",
                "~/Scripts/jquery.uniform.min.js",
                "~/Scripts/bootstrap-switch.min.js",
                "~/Scripts/bootstrap-hover-dropdown.min.js",
                "~/Scripts/moment.min.js",
                "~/Scripts/fullcalendar.js",
                "~/Scripts/jquery-ui.min.js",
                "~/Scripts/calendar.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/globalscripts").Include(
                "~/Scripts/app.min.js",
                "~/Scripts/ui-blockui.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/layoutscripts").Include(
                "~/Scripts/layout.min.js",
                "~/Scripts/demo.min.js",
                "~/Scripts/quick-sidebar.min.js"
                ));


            bundles.Add(new StyleBundle("~/Content/css", "http://fonts.googleapis.com/css?family=Open+Sans:400,300,600,700&subset=all").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/font-awesome.css",
                      "~/Content/simple-line-icons.css",
                      "~/Content/uniform.default.css",
                      "~/Content/bootstrap-switch.css",
                      "~/Content/components.css",
                      "~/Content/plugins.css",
                      "~/Content/Site.css",
                      "~/Content/fullcalendar.css"));

            bundles.Add(new StyleBundle ("~/Content/Login/css").Include(
                 "~/Content/login.css",
                 "~/Content/select2.css",
                 "~/Content/select2-bootstrap.css"
                ));

            bundles.Add(new StyleBundle("~/Content/Layout/css").Include(
                "~/Content/layout.min.css",
                "~/Content/grey.min.css",
                "~/Content/custom.min.css"));

        }

        

       
    }
}

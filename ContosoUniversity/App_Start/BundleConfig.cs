using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Optimization;

namespace ContosoUniversity.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(
                new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.js")
            );
            bundles.Add(
                new ScriptBundle("~/bundles/jqueryui").Include("~/Scripts/jquery-ui-{version}.js")
            );

            bundles.Add(new StyleBundle("~/Content1/css").Include("~/Content/site.css"));
            bundles.Add(
                new StyleBundle("~/Content1/themes/base/css").Include(
                    "~/Content/themes/base/jquery.ui.core.css",
                    "~/Content/themes/base/jquery.ui.resizable.css",
                    "~/Content/themes/base/jquery.ui.selectable.css",
                    "~/Content/themes/base/jquery.ui.accordion.css",
                    "~/Content/themes/base/jquery.ui.autocomplete.css",
                    "~/Content/themes/base/jquery.ui.theme.css"
                )
            );
        }
    }
}

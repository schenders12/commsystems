using System.Web;
using System.Web.Optimization;

namespace systems
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            // Bootstrap
            bundles.Add(new ScriptBundle("~/bundles/bootStrap").Include(
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/bootstrap-image-gallery.min.js"));

            // AOS/Assoc
            bundles.Add(new ScriptBundle("~/bundles/aosassoc").Include(
                        "~/Scripts/appjs/aosassoc.view.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/animatedcollapse").Include(
                        "~/Scripts/appjs/animatedcollapse.js"
            ));

            // Modules
            bundles.Add(new ScriptBundle("~/bundles/manageModules").Include(
                        "~/Scripts/appjs/managemodules.view.js"));

            // Pages
            bundles.Add(new ScriptBundle("~/bundles/managePages").Include(
                        "~/Scripts/appjs/managepages.view.js"));

            bundles.Add(new ScriptBundle("~/bundles/addPage").Include(
                        "~/Scripts/appjs/addpage.view.js"));

            bundles.Add(new ScriptBundle("~/bundles/editPage").Include(
                        "~/Scripts/appjs/editpage.view.js"));

            // Catalog (TBD)
            bundles.Add(new ScriptBundle("~/bundles/catalogModule").Include(
                        "~/Scripts/appjs/catalog.view.js"));

            // File bundle (also need Bootstrap bundle)
            bundles.Add(new ScriptBundle("~/bundles/manageFiles").Include(
                        "~/Scripts/FileUpload/load-image.min.js",
                        "~/Scripts/FileUpload/canvas-to-blob.min.js",
                        "~/Scripts/FileUpload/tmpl.min.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/bootstrap-image-gallery.min.js",
                        "~/Scripts/FileUpload/jquery.fileupload.js",
                        "~/Scripts/FileUpload/jquery.iframe-transport.js",
                        "~/Scripts/FileUpload/jquery.fileupload-ip.js",
                        "~/Scripts/FileUpload/jquery.fileupload-ui.js",
                        "~/Scripts/FileUpload/locale.js"));
                       // "~/Scripts/FileUpload/main.js"));

            // Styles
            bundles.Add(new StyleBundle("~/Content/validation").Include("~/Content/FPIMValidate.css"));

            bundles.Add(new StyleBundle("~/Content/fpimUI").Include("~/Content/FPIMUI.css"));

            bundles.Add(new StyleBundle("~/Content/manageModules").Include("~/Content/manageModules.css"));

            bundles.Add(new StyleBundle("~/Content/managePages").Include("~/Content/managePages.css"));

            bundles.Add(new StyleBundle("~/Content/addPage").Include("~/Content/addPage.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

            // File Upload
            bundles.Add(new StyleBundle("~/Content/fileUpload").Include(
                        "~/Scripts/FileUpload/jquery.fileupload-ui.css"));

            // Bootstrap
            bundles.Add(new StyleBundle("~/Content/bootStrap").Include(
                        "~/Content/bootStrap.min.css",
                        "~/Content/bootStrap-responsive.css",
                        "~/Content/bootstrap-image-gallery.min.css"));

            // debug mode
           // BundleTable.EnableOptimizations = true;



        }
    }
}
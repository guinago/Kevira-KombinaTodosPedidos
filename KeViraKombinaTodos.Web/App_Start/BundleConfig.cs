﻿using System.Collections.Generic;
using System.Web;
using System.Web.Optimization;

namespace KeViraKombinaTodos.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;

            var bundle = new ScriptBundle("~/bundles/jqueryval") { Orderer = new AsIsBundleOrderer() };

            bundle
                    .Include("~/Scripts/jquery.validate-vsdoc.js")
                    .Include("~/Scripts/jquery.validate.js")
                    .Include("~/Scripts/jquery.validate.unobtrusive.js")
                    .Include("~/Scripts/globalize.js")
                    .Include("~/Scripts/cldr.js").IncludeDirectory("~/Scripts/cldr/", "~/Scripts/globalize/")
                    .Include("~/Scripts/jquery.validate.globalize.js")
                .Include("~/Scripts/methods_pt.js");
            bundles.Add(bundle);

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                    "~/Scripts/jquery.datetimepicker.js",
                    "~/Scripts/jquery-{version}.js",
                    "~/Scripts/toastr.js",
                    "~/Scripts/toastr.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/funcoesjs").Include(
                        "~/Scripts/funcoes.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            // Fontawesome
            string fontawesome = "https://kit.fontawesome.com/7d14bda40a.js";
            bundles.Add(new ScriptBundle("~/bundles/fontwesome", fontawesome));

            bundles.Add(new ScriptBundle("~/bundles/popper").Include(
                            "~/Scripts/popper.js"));

            bundles.Add(new ScriptBundle("~/bundles/stellar").Include(
                            "~/Scripts/stellar.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryplugins").Include(
                            "~/Scripts/jquery.form.js"));


            bundles.Add(new ScriptBundle("~/bundles/mail").Include(
                            "~/Scripts/mail-script.js",
                            "~/Scripts/contact.js"));

            bundles.Add(new ScriptBundle("~/bundles/theme").Include(
                            "~/Scripts/theme.js"));

            bundles.Add(new ScriptBundle("~/bundles/inputmask").Include(
                        "~/Scripts/inputmask/inputmask.js",
                        "~/Scripts/inputmask/jquery.inputmask.js",
                        "~/Scripts/inputmask/inputmask.extensions.js",
                        "~/Scripts/inputmask/inputmask.date.extensions.js",
                        "~/Scripts/inputmask/inputmask.numeric.extensions.js"));

        }
    }
    public class AsIsBundleOrderer : IBundleOrderer
    {
        public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            return files;
        }
    }
}

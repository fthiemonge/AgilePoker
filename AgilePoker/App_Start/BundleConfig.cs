using System.Web.Optimization;

namespace AgilePoker
{
    public class BundleConfig
    {
        #region Public Static Methods

        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));
        }

        #endregion
    }
}
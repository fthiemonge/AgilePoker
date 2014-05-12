using System.Web.Mvc;
using System.Web.Routing;

namespace AgilePoker
{
    public class RouteConfig
    {
        #region Public Static Methods

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new
                    {
                        controller = "Home",
                        action = "Index",
                        id = UrlParameter.Optional
                    }
                );
        }

        #endregion
    }
}
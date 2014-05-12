using System.Web.Mvc;

namespace AgilePoker
{
    public class FilterConfig
    {
        #region Public Static Methods

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        #endregion
    }
}
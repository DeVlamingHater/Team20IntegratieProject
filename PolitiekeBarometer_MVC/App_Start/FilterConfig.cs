using PolitiekeBarometer_MVC.Controllers.ActionFilter;
using System.Web;
using System.Web.Mvc;

namespace PolitiekeBarometer_MVC
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new DeelplatformFilter());
        }
    }
}

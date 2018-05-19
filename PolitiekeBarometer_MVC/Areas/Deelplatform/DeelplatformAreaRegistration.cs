using System.Web.Mvc;

namespace PolitiekeBarometer_MVC.Areas.Deelplatform
{
    public class DeelplatformAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Deelplatform";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Deelplatform_default",
                "Deelplatform/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
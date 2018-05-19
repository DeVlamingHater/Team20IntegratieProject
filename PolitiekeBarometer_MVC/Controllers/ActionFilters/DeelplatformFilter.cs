using BL.Interfaces;
using BL.Managers;
using Domain.Platformen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using System.Web.Routing;

namespace PolitiekeBarometer_MVC.Controllers.ActionFilter
{
    public class DeelplatformFilter : ActionFilterAttribute
    {
        public override object TypeId => base.TypeId;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            IPlatformManager platformManager = new PlatformManager();
            Uri url = HttpContext.Current.Request.Url;
            filterContext.RouteData.Values.Remove("deelplatform");
            string deelplatform = GetDeelPlatform(url);
            if (platformManager.getDeelPlatform(deelplatform) == null && deelplatform != "pb")
            {
                //Redirect naar pagina met overzicht alle deelplatformen
                filterContext.Result = new RedirectResult("/pb/Home/Index");
            }else if(deelplatform == "pb" && url.AbsolutePath != "/pb/Home/Index")
            {
                filterContext.Result = new RedirectResult("/pb/Home/Index");
            }
            else
            {
                //Deelplatform bestaat, geef deelplatformnaam mee
                filterContext.RouteData.Values.Add("deelplatform", deelplatform);
                base.OnActionExecuting(filterContext);
            }
        }

        private static string GetDeelPlatform(Uri url)
        {
            string[] segments = url.AbsolutePath.Split('/');
            string deelplatform = segments[1];
            return deelplatform;
        }
    }
}

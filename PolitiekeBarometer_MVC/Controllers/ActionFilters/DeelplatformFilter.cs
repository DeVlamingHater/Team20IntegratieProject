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
            string[] segments = url.AbsolutePath.Split('/');
            string deelplatformURL = segments[1];
            string action = "";
            Deelplatform deelplatform = platformManager.getDeelplatformByNaam(deelplatformURL);
            if (segments.Length > 2)
            {
                action = segments[2];
            }
            if (deelplatformURL == "pb")
            {
                if (url.AbsolutePath != "/pb/Home/Index")
                {
                    filterContext.Result = new RedirectResult("/pb/Home/Index");
                }
            }
            else
            {
                if (deelplatform == null)
                {
                    filterContext.Result = new RedirectResult("/pb/Home/Index");
                }
                else
                {
                    if (action == "" || action == "Home")
                    {
                        filterContext.Result = new RedirectResult("/pb/Home/Index");
                    }
                    else
                    {
                        filterContext.RouteData.Values.Add("deelplatform", deelplatformURL);

                        base.OnActionExecuting(filterContext);
                    }
                }
            }
        }
    }
}

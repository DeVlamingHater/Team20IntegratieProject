using BL.Interfaces;
using BL.Managers;
using Domain.Platformen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PolitiekeBarometer_MVC.Controllers
{
    public class BaseController : Controller
    {
        public static string deelplatformURL { get; set; }

        public Deelplatform Deelplatform { get; set; }
        public BaseController()
        {
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var segments = HttpContext.Request.Url.Segments;
            if (!(segments.Length <= 1))
            {
                BaseController.deelplatformURL = HttpContext.Request.Url.Segments[1].Trim('/');
                IPlatformManager platformManager = new PlatformManager();
                this.Deelplatform = platformManager.getDeelplatformByNaam(deelplatformURL);
                ViewBag.Deelplatform = deelplatformURL;
                IElementManager elementManager = new ElementManager();
                if (Deelplatform != null)
                {
                    ViewBag.Elementen = elementManager.getAllElementen(Deelplatform);
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
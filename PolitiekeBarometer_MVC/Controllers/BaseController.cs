using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PolitiekeBarometer_MVC.Controllers
{
    public class BaseController : Controller
    {
        public string deelplatformURL { get; set; }

        public BaseController()
        {
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            deelplatformURL = HttpContext.Request.Url.Segments[1];
            base.OnActionExecuting(filterContext);
        }
    }
}
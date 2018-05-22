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
            if (this.RouteData!=null)
            {
                deelplatformURL = (string)this.RouteData.Values["deelplatform"];
            }
        }
    }
}
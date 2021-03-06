﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PolitiekeBarometer_MVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "Default",
                url: "{deelplatform}/{controller}/{action}/{id}",
                defaults: new {deelplatform="pb", controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}

using BL.Managers;
using Domain;
using Domain.Dashboards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Elementen;
using System.Data.SqlClient;
using Newtonsoft.Json;
using BL.Interfaces;
using Microsoft.AspNet.Identity;
using System.Text;
using PolitiekeBarometer_MVC.Controllers.ActionFilter;
using System.Web.Http.Filters;
using Domain.Platformen;

namespace PolitiekeBarometer_MVC.Controllers
{
    [DeelplatformFilter]
    public class HomeController : Controller
    {
        
        [DeelplatformFilter]
        public ActionResult Index(string deelplatform)
        {
            IPlatformManager platformManager = new PlatformManager();

            List<Deelplatform> deelplatformen = platformManager.getAllDeeplatformen();
            
            return View(deelplatformen);
        }
    }
}
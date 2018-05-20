using BL.Interfaces;
using BL.Managers;
using Domain.Dashboards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Domain;
using Domain.Elementen;
using Domain.Platformen;
using Newtonsoft.Json;
using PolitiekeBarometer_MVC.Controllers.ActionFilter;
using System.Text;

namespace PolitiekeBarometer_MVC.Controllers
{
    public class DashboardController : BaseController
    {
        public ActionResult ItemPartial(int ItemId)
        {
            IDashboardManager dashboardManager = new DashboardManager();
            Item item = dashboardManager.getItem(ItemId);
            return View();
        }

        public ActionResult Index()
        {
            IDashboardManager dashboardManager = new DashboardManager();
            string email = System.Web.HttpContext.Current.User.Identity.GetUserName();
           Dashboard dashboard = dashboardManager.getDashboard(email);
            return View(dashboard);
        }
        public ActionResult CreateZone()
        {
            IDashboardManager mgr = new DashboardManager();
            string email = System.Web.HttpContext.Current.User.Identity.GetUserName();
            Dashboard dashboard = mgr.getDashboard(email);
            Zone zone = mgr.addZone(dashboard);
            //GEBRUIKER NOG JUISTE MANIER VINDEN

            Index();
            return RedirectToAction("Index");
        }
    }
}
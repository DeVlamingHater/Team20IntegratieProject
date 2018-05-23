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
        public ActionResult Index()
        {
            IElementManager elementManager = new ElementManager();
            IDashboardManager dashboardManager = new DashboardManager();
            string email = System.Web.HttpContext.Current.User.Identity.GetUserName();
            Dashboard dashboard = dashboardManager.getDashboard(email);
            ViewBag.Suggestions = elementManager.getAllElementen();
            return View(dashboard);
        }
        public ActionResult Test()
        {
           
            return View();
        }
        public ActionResult ItemPartial(int ItemId)
        {
            IDashboardManager dashboardManager = new DashboardManager();
            Item item = dashboardManager.getItem(ItemId);
            return View("DashboardPartials/ItemPartial", item);
        }
        public ActionResult Suggestions()
        {
            IElementManager elementManager = new ElementManager();
            ViewBag.Suggestions = new MultiSelectList(elementManager.getAllElementen(), "Id", "Naam");
            return View("search/Suggestions", elementManager.getAllElementen());
        }
        public ActionResult CreateItem(FormCollection form)
        {
            string itemtype = form["ItemType"];
            return RedirectToAction("Index");
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

        public ActionResult DeleteZone(int zoneId)
        {
            IDashboardManager mgr = new DashboardManager();
            mgr.deleteZone(zoneId);
            return RedirectToAction("Index");
        }
        public ActionResult saveTabNaam(string naam)
        {
            IDashboardManager mgr = new DashboardManager();
            string email = System.Web.HttpContext.Current.User.Identity.GetUserName();
            Dashboard dashboard = mgr.getDashboard(email);
            Zone zone = new Zone()
            {
                Naam = naam,
                Dashboard = dashboard
            };
            mgr.updateZone(zone);
            return RedirectToAction("Index");
        }
    }
}
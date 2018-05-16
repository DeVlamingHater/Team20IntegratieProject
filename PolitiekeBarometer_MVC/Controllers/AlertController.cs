using BL.Interfaces;
using BL.Managers;
using Domain;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace PolitiekeBarometer_MVC.Controllers
{
    public class AlertController : Controller
    {
        public ActionResult LijstAlerts()
        {
            IDashboardManager dashboardManager = new DashboardManager();
            string username = System.Web.HttpContext.Current.User.Identity.GetUserName();
            dashboardManager.getDashboard(username);
            return View(dashboardManager.getActiveAlerts());
        }

        public ActionResult CreateAlert()
        {
            return View();
        }

        public ActionResult EditAlert(int id)
        {
            IDashboardManager dashboardManager = new DashboardManager();
            Alert alert = dashboardManager.getAlert(id);
            return View(alert);
        }
    }
}
using BL.Interfaces;
using BL.Managers;
using Domain;
using Domain.Dashboards;
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
        public ActionResult MeldingDetail(int id)
        {
            return View();
        }
        public ActionResult MeldingDropDown()
        {
            IDashboardManager dashboardManager = new DashboardManager();
            string username = System.Web.HttpContext.Current.User.Identity.GetUserName();
            Dashboard dashboard = dashboardManager.getDashboard(username);
            List<Melding> meldingen = dashboardManager.getActiveMeldingen(dashboard).ToList();

            Melding melding1 = new Melding()
            {
                IsActive = true,
                IsPositive = true,
                MeldingDateTime = DateTime.Now.AddHours(-1),
                Message = "Deze Alert is een positieve test alert",
                Titel = "Postieve Test Melding"
            };
            Melding melding2 = new Melding()
            {
                IsActive = true,
                IsPositive = false,
                MeldingDateTime = DateTime.Now.AddHours(-2),
                Message = "Deze Alert is een Negatieve test alert",
                Titel = "Negatieve Test Melding"
            };
            meldingen.Add(melding1);
            meldingen.Add(melding2);
            return PartialView(meldingen);
        }
    }
}
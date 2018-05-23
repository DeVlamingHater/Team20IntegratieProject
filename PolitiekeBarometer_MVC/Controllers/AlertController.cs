using BL.Interfaces;
using BL.Managers;
using Domain;
using Domain.Dashboards;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PolitiekeBarometer_MVC.Controllers
{
    public class AlertController : BaseController
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
            IElementManager elementManager = new ElementManager();
            ViewBag.Suggestions = elementManager.getAllElementen();
            return View();
        }
        [HttpPost]
        public ActionResult CreateAlert(FormCollection form)
        {
            IElementManager elementManager = new ElementManager();
            IDashboardManager dashboardManager = new DashboardManager();
            string username = System.Web.HttpContext.Current.User.Identity.GetUserName();
            Dashboard dashboard= dashboardManager.getDashboard(username);

            Element element = elementManager.getElementByNaam(form["element"]);

            Element vergelijkingselement = elementManager.getElementByNaam(form["vergelijking"]);

            var waarde = int.Parse(form["waarde"]);
            DataType bewerking = DataType.TOTAAL;
            switch (form["bewerking"])
            {
                case "totaal":
                    bewerking = DataType.TOTAAL;
                    break;
                case "percentage":
                    bewerking = DataType.TREND;
                    break;
                default:
                    break;
            }

            var operatorS = "";

            switch (form["operator"])
            {
                case "stijging":
                    operatorS = ">";
                    break;
                case "daling":
                    operatorS = "<";
                    break;
                default:
                    break;
            }
            bool emailMelding = parseCheckbox(form["emailMelding"]);
            bool browserMelding = parseCheckbox(form["browserMelding"]);
            bool applicatieMelding = parseCheckbox(form["applicatieMelding"]);

            List<Domain.Dashboards.Filter> filters = new List<Domain.Dashboards.Filter>();

            Domain.Dashboards.Filter filter = parseFilter(form["Age"]);
            if (filter != null)
            {
                filters.Add(filter);
            }
            filter = parseFilter(form["Sentiment"]);
            if (filter != null)
            {
                filters.Add(filter);
            }
            filter = parseFilter(form["Retweet"]);
            if (filter != null)
            {
                filters.Add(filter);
            }
            DataConfig dataConfig = new DataConfig()
            {
                Element = element,
                Vergelijking = vergelijkingselement,
                Filters = filters
            };

            Alert alert = new Alert()
            {
                DataConfig = dataConfig,
                DataType = bewerking,
                Operator = operatorS,
                ApplicatieMelding = applicatieMelding,
                BrowserMelding = browserMelding,
                EmailMelding = emailMelding,
                Dashboard = dashboard,
                Meldingen = new List<Melding>(),
                Waarde = waarde,
                Status = AlertStatus.ACTIEF
            };
            dashboardManager.createAlert(alert);

            return RedirectToAction("LijstAlerts");
        }
        private Domain.Dashboards.Filter parseFilter(string filterS)
        {
            Domain.Dashboards.Filter filter = null;
            if (filterS != "geen")
            {
                bool isPositive;
                if (filterS == "Positief")
                {
                    isPositive = true;
                }
                else
                {
                    isPositive = false;
                }
                filter = new Domain.Dashboards.Filter()
                {
                    Type = FilterType.AGE,
                    IsPositive = isPositive
                };
            }
            return filter;
        }
        private bool parseCheckbox(string checkbox)
        {
            if (checkbox == "true")
            {
                return true;
            }
            return false;
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
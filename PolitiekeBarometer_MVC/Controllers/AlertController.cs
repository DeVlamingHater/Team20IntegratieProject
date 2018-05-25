using BL.Interfaces;
using BL.Managers;
using Domain;
using Domain.Dashboards;
using Domain.Platformen;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PolitiekeBarometer_MVC.Controllers
{
    public class AlertController : BaseController
    {
        #region Alert
        public ActionResult LijstAlerts()
        {
            //Ophalen van alle alerts van een gebruiker
            IDashboardManager dashboardManager = new DashboardManager();
            string username = System.Web.HttpContext.Current.User.Identity.GetUserName();
            Dashboard dashboard = dashboardManager.getDashboard(username, Deelplatform.Naam);
            return View(dashboardManager.getAllDashboardAlerts(dashboard));
        }

        public ActionResult CreateAlert()
        {
            //Tonen van de UI voor createAlert, en elementen voor de zoekbalken
            IElementManager elementManager = new ElementManager();
            ViewBag.Suggestions = elementManager.getAllElementen(Deelplatform);
            Alert alert = new Alert();
            return View(alert);
        }
        [HttpPost]
        public ActionResult CreateAlert(FormCollection form,Alert alert)
        {
            //Form parsen naar een alert
            UnitOfWorkManager unitOfWorkManager = new UnitOfWorkManager();
            IDashboardManager dashboardManager = new DashboardManager(unitOfWorkManager);

            alert = ParseFormToAlert(form, unitOfWorkManager);
            //Alert opslaan
            dashboardManager.createAlert(alert);

            //Terug naar lijst gaan
            return RedirectToAction("LijstAlerts");
        }
        private Alert ParseFormToAlert(FormCollection form, UnitOfWorkManager unitOfWorkManager)
        {
            IElementManager elementManager = new ElementManager(unitOfWorkManager);
            IDashboardManager dashboardManager = new DashboardManager(unitOfWorkManager);
            string username = System.Web.HttpContext.Current.User.Identity.GetUserName();
            Dashboard dashboard = dashboardManager.getDashboard(username, Deelplatform.Naam);

            //Element & vergelijkingselement ophalen
            Element element = elementManager.getElementByNaam(form["element"], Deelplatform);

            Element vergelijkingselement = elementManager.getElementByNaam(form["vergelijking"], Deelplatform);

            //OverschrijdingsWaarde Parsen
            var waarde = int.Parse(form["waarde"]);

            //DefaultWaarde voor Datatype en Datatype Parsen
            DataType bewerking = DataType.TOTAAL;
            switch (form["bewerking"])
            {
                case "totaal":
                    bewerking = DataType.TOTAAL;
                    break;
                case "percentage":
                    bewerking = DataType.PERCENTAGE;
                    break;
                default:
                    break;
            }

            //Operator Parsen
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

            //Meldingen Parsen
            bool emailMelding = parseCheckbox(form["emailMelding"]);
            bool browserMelding = parseCheckbox(form["browserMelding"]);
            bool applicatieMelding = parseCheckbox(form["applicatieMelding"]);

            //Filters Parsen
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

            //Dataconfig 
            DataConfig dataConfig = new DataConfig()
            {
                Element = element,
                Vergelijking = vergelijkingselement,
                Filters = filters
            };

            //Alert configureren
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
            return alert;
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
                else if(filterS =="Negatief")
                {
                    isPositive = false;
                }
                else
                {
                    return null;
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
            IElementManager elementManager = new ElementManager();
            ViewBag.Suggestions = elementManager.getAllElementen(Deelplatform);
            Alert alert = dashboardManager.getAlert(id);
            foreach (Domain.Dashboards.Filter filter in alert.DataConfig.Filters)
            {
                switch (filter.Type)
                {
                    case FilterType.AGE:
                        ViewBag.Age = filter.IsPositive;
                        break;
                    case FilterType.SENTIMENT:
                        ViewBag.Sentiment = filter.IsPositive;
                        break;
                    case FilterType.RETWEET:
                        ViewBag.Retweet = filter.IsPositive;
                        break;
                    case FilterType.PERSONALITEIT:
                        ViewBag.Personaliteit = filter.IsPositive;
                        break;
                    case FilterType.OPLEIDING:
                        ViewBag.Opleiding = filter.IsPositive;
                        break;
                    case FilterType.GESLACHT:
                        ViewBag.Geslacht= filter.IsPositive;
                        break;
                    default:
                        break;
                }
            }
            return View(alert);
        }

        [HttpPost]
        public ActionResult EditAlert(FormCollection form)
        {
            UnitOfWorkManager unitOfWorkManager = new UnitOfWorkManager();
            IDashboardManager dashboardManager = new DashboardManager(unitOfWorkManager);
            Alert alert = ParseFormToAlert(form, unitOfWorkManager);

            dashboardManager.updateAlert(alert);
            return RedirectToAction("LijstAlerts");
        }
        #endregion

        #region Melding
        public ActionResult MeldingDetail(int id)
        {
            return View();
        }
        public ActionResult MeldingDropDown()
        {
            //Ophalen Dashboard van de User van het huidige Deelplatform
            IDashboardManager dashboardManager = new DashboardManager();
            string username = System.Web.HttpContext.Current.User.Identity.GetUserName();
            List<Melding> meldingen = new List<Melding>();
            if (Deelplatform != null)
            {   
                //Ophalen Meldingen van het dashboard
                Dashboard dashboard = dashboardManager.getDashboard(username, Deelplatform.Naam);
                meldingen = dashboardManager.getActiveMeldingen(dashboard).ToList();
    
                //TestMeldingen
                #region TestMeldingen
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
                #endregion
            }
            return PartialView(meldingen);
        }
        #endregion
    }
}
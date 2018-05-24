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
            IPlatformManager platformManager = new PlatformManager();
            Deelplatform deelplatform = platformManager.getDeelplatformByNaam(deelplatformURL);

            string email = System.Web.HttpContext.Current.User.Identity.GetUserName();
            Dashboard dashboard = dashboardManager.getDashboard(email, deelplatform);
            ViewBag.Suggestions = elementManager.getAllElementen(Deelplatform);
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
            ViewBag.Suggestions = new MultiSelectList(elementManager.getAllElementen(Deelplatform), "Id", "Naam");
            return View("search/Suggestions", elementManager.getAllElementen(Deelplatform));
        }
        [HttpPost]
        public ActionResult CreateItem(FormCollection form)
        {
            IDashboardManager dashboardManager = new DashboardManager();
            IElementManager elementManager = new ElementManager();
            Grafiek grafiek = new Grafiek() { Dataconfigs = new List<DataConfig>()};
            var itemtype = form["ItemType"];
            var interval = form["Interval"];
            var dataType = form["DataType"];
            var elementForm = form["elementen"];

            var age = form["Age"];
            var sentiment = form["Sentiment"];
            var retweet = form["Retweet"];

            string[] elementNamen;

            if (elementForm != null)
            {
                elementNamen = elementForm.Split(',');
            }
            else
            {
                elementNamen = new string[] { "" };
            }
            List<DataConfig> dataConfigs = new List<DataConfig>();
            if (elementNamen[0] != "")
            {
                foreach (string elementNaam in elementNamen)
                {
                    List<DataConfig> allDataConfigs = new List<DataConfig>();
                    DataConfig baseConfig = new DataConfig()
                    {
                        Element = elementManager.getElementByNaam(elementNaam, Deelplatform),
                        Filters = new List<Domain.Dashboards.Filter>(),
                        Vergelijking = null
                    };

                    dataConfigs.Add(baseConfig);
                }
                dataConfigs = filterConfigs(dataConfigs, FilterType.AGE, age);
                dataConfigs = filterConfigs(dataConfigs, FilterType.RETWEET, retweet);
                dataConfigs = filterConfigs(dataConfigs, FilterType.SENTIMENT, sentiment);
                grafiek.Dataconfigs.AddRange(dataConfigs);
            }
            switch (dataType)
            {
                case "Totaal":
                    grafiek.DataType = DataType.TOTAAL;
                    break;
                case "Trend":
                    grafiek.DataType = DataType.TREND;
                    break;
                default:
                    break;
            }
            switch (interval)
            {
                case "12u":
                    grafiek.Tijdschaal = new TimeSpan(12, 0, 0);
                    grafiek.AantalDataPoints = 12;
                    break;
                case "24u":
                    grafiek.Tijdschaal = new TimeSpan(24, 0, 0);
                    grafiek.AantalDataPoints = 24;
                    break;
                case "7d":
                    grafiek.Tijdschaal = new TimeSpan(7, 0, 0, 0);
                    grafiek.AantalDataPoints = 7;
                    break;
                case "30d":
                    grafiek.Tijdschaal = new TimeSpan(30, 0, 0, 0);
                    grafiek.AantalDataPoints = 30;
                    break;
                default:
                    break;
            }

            dashboardManager.createGrafiek(grafiek);

            return RedirectToAction("Index");
        }
        private List<DataConfig> filterConfigs(List<DataConfig> dataConfigs, FilterType type, string waarde)
        {
            if (waarde == "Geen")
            {
                return dataConfigs;
            }
            Domain.Dashboards.Filter positiveFilter = new Domain.Dashboards.Filter()
            {
                IsPositive = true,
                Type = type
            };
            Domain.Dashboards.Filter negativeFilter = new Domain.Dashboards.Filter()
            {
                IsPositive = false,
                Type = type
            };
            List<DataConfig> allConfigs = new List<DataConfig>();

            foreach (DataConfig dataconfig in dataConfigs)
            {
                DataConfig positivedataConfig = dataconfig;
                positivedataConfig.Filters.Add(positiveFilter);

                DataConfig negativedataConfig = dataconfig;
                negativedataConfig.Filters.Add(positiveFilter);

                if (waarde == "Splits")
                {
                    allConfigs.Add(negativedataConfig);
                    allConfigs.Add(positivedataConfig);
                }
                else if (waarde == "Positief")
                {
                    allConfigs.Add(positivedataConfig);
                }
                else if (waarde == "Negatief")
                {
                    allConfigs.Add(negativedataConfig);
                }
            }

            return allConfigs;
        }

        public ActionResult CreateZone()
        {
            IDashboardManager mgr = new DashboardManager();
            string email = System.Web.HttpContext.Current.User.Identity.GetUserName();
            Dashboard dashboard = mgr.getDashboard(email, Deelplatform);
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
        public ActionResult saveTabNaam(int id, string naam)
        {
            IDashboardManager mgr = new DashboardManager();
            mgr.changeZoneName(id, naam);
            return RedirectToAction("Index");
        }
    }
}
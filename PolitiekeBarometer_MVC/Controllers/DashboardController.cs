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
using PolitiekeBarometer_MVC.Models;

namespace PolitiekeBarometer_MVC.Controllers
{
    public class DashboardController : BaseController
    {
        public ActionResult Index()
        {
            UnitOfWorkManager uowMgr = new UnitOfWorkManager();
            IElementManager elementManager = new ElementManager(uowMgr);
            IDashboardManager dashboardManager = new DashboardManager(uowMgr);

            string email = System.Web.HttpContext.Current.User.Identity.GetUserName();
            Dashboard dashboard = dashboardManager.getDashboard(email, deelplatformURL);
            ViewBag.Suggestions = elementManager.getAllElementen(Deelplatform);
            List<Zone> zones = new List<Zone>();
            zones  = dashboardManager.getZones(dashboard).ToList();
            List<ZoneViewModel> zonesViewModel = ZoneParser.ParseZones(zones, uowMgr);
            
            return View(zonesViewModel);
        }
        public ActionResult Item(Item item)
        {
            return PartialView("DashboardPartials/NewItemPartial", item);
        }
       
        public ActionResult Test()
        {
            return View();
        }
        public ActionResult Grafiek(Grafiek grafiek)
        {
            IDashboardManager dashboardManager = new DashboardManager();
            var grafiekData = dashboardManager.getGraphData(grafiek);
            ViewBag.grafiekData = grafiekData;
            return PartialView("DashboardPartials/NewGrafiekPartial", grafiek);
        }
        [HttpPost]
        public ActionResult CreateItem(FormCollection form)
        {
            UnitOfWorkManager uowMgr = new UnitOfWorkManager();
            IDashboardManager dashboardManager = new DashboardManager(uowMgr);
            IElementManager elementManager = new ElementManager(uowMgr);
            Grafiek grafiek = new Grafiek() { Dataconfigs = new List<DataConfig>() };
            var itemtype = form["ItemType"];
            var interval = form["Interval"];
            var dataType = form["DataType"];
            var elementForm = form["elementen"];
            var zoneId = Int32.Parse(form["Zone"]);
            var age = form["Age"];
            var geslacht = form["Geslacht"];
            var sentiment = form["Sentiment"];
            var retweet = form["Retweet"];
            var personaliteit = form["Personaliteit"];
            var opleiding = form["Opleiding"];

            Zone zone = dashboardManager.getZone(zoneId);
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
                if (dataConfigs.Count == 0)
                {
                    dataConfigs.Add(new DataConfig()
                    {
                        Label="Totaal"
                    });
                }
                dataConfigs = filterConfigs(dataConfigs, FilterType.AGE, age);
                dataConfigs = filterConfigs(dataConfigs, FilterType.GESLACHT, geslacht);
                dataConfigs = filterConfigs(dataConfigs, FilterType.RETWEET, retweet);
                dataConfigs = filterConfigs(dataConfigs, FilterType.SENTIMENT, sentiment);
                dataConfigs = filterConfigs(dataConfigs, FilterType.PERSONALITEIT, personaliteit);
                dataConfigs = filterConfigs(dataConfigs, FilterType.OPLEIDING, opleiding);
                grafiek.Dataconfigs.AddRange(dataConfigs);
            }
            switch (dataType)
            {
                case "Totaal":
                    grafiek.DataType = DataType.TOTAAL;
                    break;
                case "Trend":
                    grafiek.DataType = DataType.PERCENTAGE;
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
            grafiek.Zone = zone;
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
            Dashboard dashboard = mgr.getDashboard(email, Deelplatform.Naam);
            Zone zone = mgr.addZone(dashboard);

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
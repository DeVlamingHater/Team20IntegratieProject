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

            List<ZoneViewModel> zones = ZoneParser.ParseZones(dashboardManager.getZones(dashboard).ToList(), uowMgr);

            return View(zones);
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

        public ActionResult GrafiekPartial()
        {
            //IDashboardManager dashboardManager = new DashboardManager();
            //Grafiek grafiek = dashboardManager.getGrafiek(ItemId);
            //List<Dictionary<string, double>> datasets = dashboardManager.getGraphData(grafiek);
            //GrafiekViewModel grafiekViewModel = new GrafiekViewModel();

            List<string> labels = new List<string>()
            {
                "1","2","3"
            };
            List<double> data = new List<double>()
            {
                10.0,50.0,55.0
            };
            ViewBag.labels = labels;
            ViewBag.data = data;
            GrafiekViewModel grafiekViewModel = new GrafiekViewModel();
            //grafiekViewModel.datasets = datasets;
            //grafiekViewModel.tittel = grafiek.Tittel;
            //grafiekViewModel.GrafiekType =grafiek.GrafiekType;
            return PartialView("DashboardPartials/GrafiekPartial", grafiekViewModel);
        }

        [HttpPost]
        public ActionResult CreateItem(FormCollection form)
        {
            //TODO: controleren
            IDashboardManager dashboardManager = new DashboardManager();
            IElementManager elementManager = new ElementManager();
            Grafiek grafiek = new Grafiek() { Dataconfigs = new List<DataConfig>() };
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
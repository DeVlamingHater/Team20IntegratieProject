using BL.Interfaces;
using BL.Managers;
using DAL.EF;
using Domain;
using Domain.Dashboards;
using Domain.Platformen;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PolitiekeBarometer_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;


namespace PolitiekeBarometer_MVC.Controllers
{

    [Authorize]
    public class AndroidApiController : ApiController
    {

        [HttpGet]
        [Route("api/AndroidApi/Zones")]
        public List<ZoneViewModel> GetZones(string email)
        {
            IDashboardManager dashboardManager = new DashboardManager();
            IElementManager elementManager = new ElementManager();

            //TODO: eventueel meerdere deelplatformen
            IPlatformManager platformManager = new PlatformManager();
            Deelplatform deelplatform = platformManager.getDeelplatformByNaam("Politiek");

            Dashboard dashboard = dashboardManager.getDashboard(email, deelplatform);

            List<Zone> lijstZones = dashboardManager.getZones(dashboard).ToList();

            #region testData
            ZoneViewModel testZone = new ZoneViewModel()
            {
                naam = "testZone",
                items = new List<ItemViewModel>()
            };

            List<Dictionary<string, double>> datasets = new List<Dictionary<string, double>>();
            Dictionary<string, double> dataset = new Dictionary<string, double>();
            dataset.Add("Bart", 20.0);
            dataset.Add("Imade", 80.0);
            datasets.Add(dataset);

            GrafiekViewModel testGrafiek = new GrafiekViewModel()
            {
                tittel = "testGrafiek",
                datasets = datasets,
                GrafiekType = GrafiekType.BAR,
                DataType = 1
            };

            testZone.items.Add(testGrafiek);

            List<ZoneViewModel> testzones = new List<ZoneViewModel>();
            testzones.Add(testZone);
            #endregion
            return testzones;
        }

        [HttpGet]
        public List<Dictionary<string, double>> getData(Grafiek grafiek)
        {
            IDashboardManager dashboardManager = new DashboardManager();
            dashboardManager.getGraphData(grafiek);
            return dashboardManager.getGraphData(grafiek);

        }
        [HttpGet]
        [Route("api/AndroidApi/Alerts")]
        public List<Alert> GetAlerts(string email)
        {
            IDashboardManager dashboardManager = new DashboardManager();

            //TODO: eventueel meerdere deelplatformen
            IPlatformManager platformManager = new PlatformManager();
            Deelplatform deelplatform = platformManager.getDeelplatformByNaam("Politiek");

            Dashboard dashboard = dashboardManager.getDashboard(email, deelplatform);

            List<Alert> lijstAlerts = dashboardManager.getDashboardAlerts(dashboard).ToList();

            return lijstAlerts;
        }
        [HttpGet]
        [Route("api/AndroidApi/Meldingen")]
        public List<Melding> GetMeldingen(string email)
        {
            IDashboardManager dashboardManager = new DashboardManager();

            //TODO: eventueel meerdere deelplatformen
            IPlatformManager platformManager = new PlatformManager();
            Deelplatform deelplatform = platformManager.getDeelplatformByNaam("Politiek");

            Dashboard dashboard = dashboardManager.getDashboard(email, deelplatform);
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
            return meldingen;
        }

    }
}
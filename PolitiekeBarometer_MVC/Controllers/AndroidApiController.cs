using BL.Interfaces;
using BL.Managers;
using DAL.EF;
using Domain;
using Domain.Dashboards;
using Domain.Platformen;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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
        public List<Zone> GetZones(string email)
        {
            IDashboardManager dashboardManager = new DashboardManager();
            IElementManager elementManager = new ElementManager();

            Dashboard dashboard = dashboardManager.getDashboard(email);

            List<Zone> lijstZones = dashboardManager.getZones(dashboard).ToList();

            Zone testZone = new Zone()
            {
                Dashboard = dashboard,
                Naam = "TestZone"
            };

            Element testElement = elementManager.getElementByNaam("Bart De Wever");

            DataConfig testDataConfig = new DataConfig()
            {
                DataConfiguratieId = 100,
                Element = testElement

            };
            Grafiek testGrafiek = new Grafiek()
            {
                DataType = DataType.TOTAAL,
                Tijdschaal = new TimeSpan(7, 0, 0, 0),
                Dataconfigs = new List<DataConfig>()
                {
                    testDataConfig
                },
                GrafiekType = GrafiekType.LINE,
                AantalDataPoints = 12
            };

            testZone.Items.Add(testGrafiek);
            lijstZones.Add(testZone);
            return lijstZones;
        }


        [HttpGet]
        [Route("api/AndroidApi/Alerts")]
        public List<Alert> GetAlerts(string email)
        {
            IDashboardManager dashboardManager = new DashboardManager();

            Dashboard dashboard = dashboardManager.getDashboard(email);

            List<Alert> lijstAlerts = dashboardManager.getDashboardAlerts(dashboard).ToList();

            return lijstAlerts;
        }
        [HttpGet]
        [Route("api/AndroidApi/Meldingen")]
        public List<Melding> GetMeldingen(string email)
        {
            IDashboardManager dashboardManager = new DashboardManager();
            Dashboard dashboard = dashboardManager.getDashboard(email);
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
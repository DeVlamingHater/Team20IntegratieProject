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
        public List<Zone> GetZones(string email)
        {
            IDashboardManager dashboardManager = new DashboardManager();

            Dashboard dashboard = dashboardManager.getDashboard(email);

            List<Zone> lijstZones = dashboardManager.getZones(dashboard).ToList();

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

    }
}
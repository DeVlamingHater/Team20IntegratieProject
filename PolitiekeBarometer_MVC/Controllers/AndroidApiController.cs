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
    public String Get()
    {
      String tekst = "hallo";
      return tekst;
    }

        [HttpGet]
        public String Get(string email)
        {
            IPlatformManager platformManager = new PlatformManager();

            Gebruiker gebruiker = platformManager.getGebruikerMetEmail(email);
 
            return gebruiker.Naam;
        }

    }
    [HttpGet]
    public int getDashboar(string email)
    {
      DashboardManager dashboardManager = new DashboardManager();
      Dashboard dashboard = dashboardManager.getDashboard(email);

      return dashboard.DashboardId;
    }
    [HttpGet]
    public List<string> getZones(string email)
    {
      DashboardManager dashboardManager = new DashboardManager();
      Dashboard dashboard = dashboardManager.getDashboard(email);
      IEnumerable<Zone> zones = dashboardManager.getZones(dashboard);
      List<string> namen = new List<string>();
      foreach (Zone zone in zones)
      {
        namen.Add(zone.Naam);
      }
      return namen;
    }
    [HttpGet]
    public List<Item> getItems(string naam)
    {
      DashboardManager dashboardManager = new DashboardManager();
      Zone zone = dashboardManager.getZoneByNaam(naam);
      IEnumerable<Item> items = dashboardManager.getItems(zone.Id);

      List<string> itemsList = new List<string>();
      foreach (Item item in items)
      {
        string itemString = item.Id + ";" + item.Size + ";" + item.X + ";" + item.Y + ";";
        itemsList.Add(itemString);
      }

      return items.ToList();
    }
    [HttpGet]
    public List<Alert> getAlerts(string email)
    {
      DashboardManager dashboardManager = new DashboardManager();
      Dashboard dashboard = dashboardManager.getDashboard(email);
      IEnumerable<Alert> alerts = dashboardManager.getDashboardAlerts(dashboard).Where(a => a.Status == AlertStatus.ACTIEF).Where(a => a.ApplicatieMelding == true);

      List<string> itemsList = new List<string>();
      foreach (Alert alert in alerts)
      {
        string alertString = alert.AlertId + ";" + alert.DataConfig + ";" + alert.DataType + ";" + alert.Interval + ";" + alert.Operator + ";" + alert.Waarde + ";" + alert.Meldingen;
        itemsList.Add(alertString);
      }

      return alerts.ToList();
    }
    [HttpGet]
    public List<Melding> getMeldingen(string email)
    {
      DashboardManager dashboardManager = new DashboardManager();
      Dashboard dashboard = dashboardManager.getDashboard(email);
      IEnumerable<Melding> meldingen = dashboardManager.getActiveMeldingen(dashboard).Where(m => m.Alert.ApplicatieMelding == true);
      return meldingen.ToList();
    }





    //[HttpGet]
    //[Authorize(Roles ="Admin,SuperAdmin",Claims="Platform|1")]
    //public IEnumerable<string> Get()
    //{
    //    var user = this.User as System.Security.Claims.ClaimsPrincipal;
    //    user.Claims.SingleOrDefault();
    //    //var newClaim = new Claim("Platform", "1");
    //    (this.User.Identity as ClaimsIdentity).HasClaim(c => c.Type == "Platform" && c.Value == "1");
    //    identity.
    //    return new string[] { "User1", "User2" };
    //}



  }
}

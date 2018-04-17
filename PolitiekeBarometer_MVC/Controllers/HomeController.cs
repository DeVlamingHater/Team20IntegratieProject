using BL.Managers;
using Domain.Dashboards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PolitiekeBarometer_MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
          this._DashboardPartial(1); //zorgen voor juiste gebruikerId
          return View();
        }

        public ActionResult NewTab()
        {
            return View();
        }

        public ActionResult Element()
        {
            return View();
        }
   
      private DashboardManager mgr = new DashboardManager();

      public ActionResult _DashboardPartial(int gebruikerId)
      {
        Dashboard dashboard = mgr.getDashboard(1); //aanpassen naar gebruikerId
        IEnumerable<Zone> zones = mgr.getZones(dashboard);
        return View(zones);
      }
      public ActionResult GetZone(int zoneId)
      {
        Zone zone = mgr.getZone(zoneId);
        return View(zone);
      }
      public ActionResult AddZone()
    {
      Zone zone= mgr.addZone();
      //GEBRUIKER NOG JUISTE MANIER VINDEN
      this._DashboardPartial(1);
      return View();
    }
    }
  }
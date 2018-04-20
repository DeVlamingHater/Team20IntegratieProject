
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
      this.Dashboard(1); //zorgen voor juiste gebruikerId
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

    #region Dashboard
 DashboardManager mgr = new DashboardManager();
    public ActionResult Dashboard(int gebruikerId)
    {
      Dashboard dashboard = mgr.getDashboard(gebruikerId); //aanpassen naar gebruikerId
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
      Zone zone = mgr.addZone();
      //GEBRUIKER NOG JUISTE MANIER VINDEN
      this.Dashboard(1);
      return RedirectToAction("Index");
      return View();
    }
    /*public ActionResult changeZone(int zoneid, Zone zone)
    {
      if (ModelState.IsValid)
      {
        mgr.changeZone(zone);
      }
    }*/
    #endregion
  }
}
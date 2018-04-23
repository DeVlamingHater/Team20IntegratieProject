
using BL.Managers;
using Domain;
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
      return View();
    }

    public ActionResult NewTab()
    {
      return View();
    }

    #region Dashboard
    static int actieveZone;
    DashboardManager mgr = new DashboardManager();
    public ActionResult Dashboard()
    {
      Dashboard dashboard = mgr.getDashboard(1); //aanpassen naar gebruikerId
      IEnumerable<Zone> zones = mgr.getZones(dashboard);
       if (actieveZone == 0)
      {
        actieveZone = zones.First().Id;
      }
      return View(zones);
      
    }
    public ActionResult _ItemsPartial()
    {
     
      IEnumerable<Item> items = mgr.getItems(actieveZone);
      return PartialView(items);
    }
    public ActionResult setActiveZone(int zoneId)
    {
      actieveZone = mgr.getZone(zoneId).Id;
      return RedirectToAction("Dashboard");
      //return RedirectToAction("_ItemsPartial");
      return View();
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
      this.Dashboard();
      return RedirectToAction("Dashboard");
      return View();
    }
    public ActionResult DeleteZone(int zoneId)
    {
      mgr.deleteZone(zoneId);
      return RedirectToAction("Dashboard");
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

    #region Element
    ElementManager Emgr = new ElementManager();
    public ActionResult Organisatie(int id)
    {
      Element element = Emgr.getElementById(id);
      return View(element);
    }
    public ActionResult Persoon(int id)
    {
      Element element = Emgr.getElementById(id);
      return View(element);
    }
    public ActionResult Thema(int id)
    {
      Element element = Emgr.getElementById(id);
      return View(element);
    }
    #endregion
  }
}

using BL.Managers;
using Domain;
using Domain.Dashboards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain;

namespace PolitiekeBarometer_MVC.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
  _PersonenDropDown();
      _OrganisatieDropDown();
      _ThemaDropDown();
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
        
        public ActionResult Grafiek()
        {
            ElementManager mgr = new ElementManager();
            List<Element> elementen = new List<Element>();
            elementen = mgr.getTrendingElementen();
            return View(elementen.ToList());
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
    //public ActionResult fillDropdowns()
    //{
    //  List<Organisatie> organisaties = Emgr.getAllOrganisaties();

    //  List<Thema> themas = Emgr.getAllThemas();

    //  return View();
    //}
    public ActionResult _PersonenDropDown()
    {
      List<Element> elementen = Emgr.getTrendingElementen(3).Where(e => e.GetType().Equals(typeof(Persoon))).ToList();
      List<Persoon> personen = new List<Domain.Persoon>();

      foreach (Element element in elementen)
      {
        personen.Add((Persoon)element);
      }
      return PartialView(personen);
    }

    public ActionResult _ThemaDropDown()
    {
      List<Element> elementen = Emgr.getTrendingElementen(3).Where(e => e.GetType().Equals(typeof(Thema))).ToList();
      List<Thema> themas = new List<Domain.Thema>();
      foreach (Element element in elementen)
      {
        themas.Add((Thema)element);
      }
      return PartialView(themas);
    }

    public ActionResult _OrganisatieDropDown()
    {
      List<Element> elementen = Emgr.getTrendingElementen(3).Where(e => e.GetType().Equals(typeof(Organisatie))).ToList();
      List<Organisatie> organisaties = new List<Domain.Organisatie>();
      foreach (Element element in elementen)
      {
        organisaties.Add((Organisatie)element);
      }
      return PartialView(organisaties);
    }

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
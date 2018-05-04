using BL.Managers;
using Domain;
using Domain.Dashboards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Elementen;
using System.Data.SqlClient;
using Newtonsoft.Json;

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

    public ActionResult Test()
    {
      ElementManager mgr = new ElementManager();
      List<string> namen = new List<string>();
      List<int> ids = new List<int>();
      foreach (Element element in mgr.getTrendingElementen())
      {
        namen.Add(element.Naam);
        ids.Add(element.Id);
      }
      Json(ViewBag.Namen = namen);
      ViewBag.Lengte = namen.Count();
      ViewBag.Ids = ids;
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

    public ActionResult _BarGrafiekPartial(int index)
    {
      ElementManager mgr = new ElementManager();
      List<Element> elementen = new List<Element>();
      elementen = mgr.getTrendingElementen();
      List<string> namen = new List<string>();
      List<double> trends = new List<double>();
      foreach (Element element in mgr.getTrendingElementen())
      {
        namen.Add(element.Naam);
        trends.Add(element.Trend);
      }
      Json(ViewBag.Namen = namen);
      Json(ViewBag.Trending = trends);
      ViewBag.Index = index;
      return PartialView(elementen.ToList());
    }

    public ActionResult _LijnGrafiekPartial(int index)
    {
      ElementManager mgr = new ElementManager();
      List<Element> elementen = new List<Element>();
      //elementen = mgr.getTrendingElementen();
      //List<string> namen = new List<string>();
      //List<double> trends = new List<double>();
      //foreach (Element element in mgr.getTrendingElementen())
      //{
      //    namen.Add(element.Naam);
      //    trends.Add(element.Trend);
      //}
      //Json(ViewBag.Namen = namen);
      //Json(ViewBag.Trending = trends);

      
      Element testElement = mgr.getElementByNaam("Bart De Wever");
      DashboardManager dashboardManager = new DashboardManager();
      DataConfig testDataConfig = new DataConfig()
      {
        DataConfiguratieId = 100,
        Element =
             testElement

      };
      Grafiek testGrafiek = new Grafiek()
      {
        DataType = Domain.DataType.TOTAAL,

        Tijdschaal = new TimeSpan(1, 0, 0, 0),
        Dataconfigs = new List<DataConfig>()
                {
                    testDataConfig
                }
      };
      string dataString = dashboardManager.getGraphData(testGrafiek);
     // Dictionary<string, int> data = JsonConvert.DeserializeObject<Dictionary<string, int>>(dataString);

      Json(ViewBag.Namen = data.Keys);
      Json(ViewBag.Trending = data.Values);
      ViewBag.Index = index;
      return PartialView(elementen.ToList());

    }
    public ActionResult _TaartGrafiekPartial(int index)
    {
      ElementManager mgr = new ElementManager();
      List<Element> elementen = new List<Element>();
      elementen = mgr.getTrendingElementen();
      List<string> namen = new List<string>();
      List<double> trends = new List<double>();
      foreach (Element element in mgr.getTrendingElementen())
      {
        namen.Add(element.Naam);
        trends.Add(element.Trend);
      }
      Json(ViewBag.Namen = namen);
      Json(ViewBag.Trending = trends);
      ViewBag.Index = index;
      return PartialView(elementen.ToList());
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
    //public ActionResult _ItemsPartial()
    //{

    //  IEnumerable<Item> items = mgr.getItems(actieveZone);
    //  return PartialView(items);
    //}
    public ActionResult _ItemsPartial(int zoneId)
    {

      IEnumerable<Item> items = mgr.getItems(zoneId);
      return PartialView(items);
    }
    public ActionResult setActiveZone(int zoneId)
    {
      actieveZone = mgr.getZone(zoneId).Id;
      _ItemsPartial(actieveZone);
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
    public ActionResult _SearchPartial(string searchstring)
    {
      if (searchstring is null)
      {
        List<Element> leeg = new List<Element>();
        return PartialView(leeg);
      }
      List<Element> elementen = Emgr.getAllElementen().Where(e => e.Naam.ToLower().Contains(searchstring.ToLower())).ToList();
      List<Persoon> personen = Emgr.getAllPersonen().Where(e => e.Organisatie.Naam.ToLower().Contains(searchstring.ToLower())).ToList();
      elementen.AddRange(personen);
      List<Thema> themas = Emgr.getAllThemas();
      for (int i = 0; i < themas.Count(); i++)
      {
        Thema thema = themas.ElementAt(i);
        List<Keyword> keywords = thema.Keywords;
        if (keywords is null)
        {
          break;
        }
        else
        {
          for (int j = 0; j <= keywords.Count(); j++)
          {
            Keyword keyword = keywords.ElementAt(j);
            if (keyword.KeywordNaam.ToLower().Contains(searchstring.ToLower()))
            {
              elementen.Add(thema);
              break;
            }
          }
        }

      }

      return PartialView(elementen);
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
    public ActionResult setImage(string twitter)
    {

      string twitter1 = twitter.Replace("@", "");

      string url = "https://twitter.com/" + twitter1 + "/profile_image?size=original";
      return Redirect(url);


    }
    public ActionResult setTwitter(string twitter)
    {
      string twitter1 = twitter.Replace("@", "");
      string url = "https://twitter.com/" + twitter1;
      return Redirect(url);
    }
    public ActionResult setOrganisatie(Organisatie organisatie)
    {
      return View(organisatie.Naam);


      //string twitter = organisatie.Naam; //moet twitter worden;
      ////string twitter1 = twitter.Remove(0, 1);
      //string url = "https://twitter.com/" + twitter + "/profile_image?size=original";
      //return View(twitter);
    }
    public ActionResult Thema(int id)
    {
      Element element = Emgr.getElementById(id);
      return View(element);
    }
    #endregion
  }

}
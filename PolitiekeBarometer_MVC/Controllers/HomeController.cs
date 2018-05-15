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
using BL.Interfaces;
using Microsoft.AspNet.Identity;
using System.Text;

namespace PolitiekeBarometer_MVC.Controllers
{
  public class HomeController : Controller
  {


        public ActionResult Index()
        {
            _PersonenDropDown();
            _OrganisatieDropDown();
            _ThemaDropDown();
            ElementManager elementManager = new ElementManager();
            List<Element> elementenTrending = elementManager.getTrendingElementen(3);
            StringBuilder labels = new StringBuilder();
            StringBuilder data = new StringBuilder();
            int i = 0;

            foreach (Element element in elementenTrending)
            {
                labels.Append(element.Naam).Append(".");
                data.Append(element.Trend).Append(".");
            }
            labels.Remove(labels.Length - 1, 1);
            data.Remove(data.Length - 1, 1);
            ViewBag.Labels = labels;
            ViewBag.Data = data;
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

    public JsonResult getGrafiekData(string grafiekType, int zone, string datum, string dataType)
    {
      saveGrafiek(grafiekType, zone, datum, dataType);
      return Json(new
      {
        result = "OK"
      });
    }

    public ActionResult saveGrafiek(string grafiekType, int zone, string datum, string dataType)
    {
      //nog implementeren
      int datapoints = 1;
      if (grafiekType == "line")
      {
        datapoints = 15;
      }
      ElementManager mgr = new ElementManager();
      Element testElement = mgr.getElementByNaam("Bart De Wever");
      DashboardManager dashboardManager = new DashboardManager();
      GrafiekType grafiektype = (GrafiekType)Enum.Parse(typeof(GrafiekType), grafiekType.ToUpper());
      DataType datatype = (DataType)Enum.Parse(typeof(DataType), dataType.ToUpper());
      DataConfig testDataConfig = new DataConfig()
      {
        Element =
             testElement
      };
      Grafiek testGrafiek = new Grafiek()
      {
        Zone = dashboardManager.getZone(zone),
        GrafiekType = grafiektype,
        DataType = datatype,
        Tijdschaal = new TimeSpan(7, 0, 0),
        Dataconfigs = new List<DataConfig>()
                {
                    testDataConfig
                },
        AantalDataPoints = datapoints
      };
      dashboardManager.addGrafiek(testGrafiek);
      return View();
    }

    public ActionResult Element()
    {
      return View();
    }

    public ActionResult NewTab()
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

    public ActionResult Alerts()
    {
      return View();
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

      Grafiek testGrafiek = new Grafiek()
      {
        DataType = DataType.TOTAAL,


        Tijdschaal = new TimeSpan(30, 0, 0, 0),
        Dataconfigs = new List<DataConfig>()
        {
        },
        AantalDataPoints = 30
      };

      string dataString = dashboardManager.getGraphData(testGrafiek);


      Dictionary<string, string> dataconfigs = JsonConvert.DeserializeObject<Dictionary<string, string>>(dataString);

      Dictionary<DateTime, double> data = JsonConvert.DeserializeObject<Dictionary<DateTime, double>>(dataconfigs.First().Value);


      List<string> dates = new List<string>();
      foreach (DateTime item in data.Keys)
      {
        dates.Add(item.ToString("d MMM yyyy "));
      }
      Json(ViewBag.Namen = dates);
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
    public ActionResult bewerkGrafiek()
    {
      // nog implementeren
      return View();
    }
    public ActionResult maakAllert()
    {
      //nog implementeren
      return View();
    }


    static int actieveZone;
    public ActionResult Dashboard()
    {
      IDashboardManager mgr = new DashboardManager();
      string email = System.Web.HttpContext.Current.User.Identity.GetUserName();
      Dashboard dashboard = mgr.getDashboard(email); //aanpassen naar gebruikerId
      List<Zone> zones = dashboard.Zones;
      //if (actieveZone == 0)
      //{
      //    actieveZone = zones.First().Id;
      //}
      return View(zones);
    }
    //public ActionResult _ItemsPartial()
    //{


        //  IEnumerable<Item> items = mgr.getItems(actieveZone);
        //  return PartialView(items);
        //}
        public ActionResult _ItemPartial(int grafiekType,int index,string labels,string data)

        {
            ViewBag.GrafiekType = grafiekType;
            ViewBag.GrafiekIndex = index;
            ViewBag.Labels = labels;
            ViewBag.Data = data;
            return PartialView();
        }
        public ActionResult setActiveZone(int zoneId)
        {
            IDashboardManager mgr = new DashboardManager();
            actieveZone = mgr.getZone(zoneId).Id;
            //_ItemsPartial(actieveZone);
            return RedirectToAction("Dashboard");
            //return RedirectToAction("_ItemsPartial");
            return View();
        }
        public ActionResult GetZone(int zoneId)
        {
            IDashboardManager mgr = new DashboardManager();
            Zone zone = mgr.getZone(zoneId);
            return View(zone);
        }
        public ActionResult AddZone()
        {
            IDashboardManager mgr = new DashboardManager();
            string email = System.Web.HttpContext.Current.User.Identity.GetUserName();
            Dashboard dashboard = mgr.getDashboard(email);
            Zone zone = mgr.addZone(dashboard);
            //GEBRUIKER NOG JUISTE MANIER VINDEN

            this.Dashboard();
            return RedirectToAction("Dashboard");
            return View();
        }
        public ActionResult DeleteZone(int zoneId)
        {
            IDashboardManager mgr = new DashboardManager();
            mgr.deleteZone(zoneId);
            return RedirectToAction("Dashboard");
            return View();
        }

    public ActionResult getZonesJson()
    {
      IDashboardManager mgr = new DashboardManager();
      string email = System.Web.HttpContext.Current.User.Identity.GetUserName();
      Dashboard dashboard = mgr.getDashboard(email);
      List<Zone> zones = dashboard.Zones;
      List<string> lijst = new List<string>();
      foreach (Zone zone in zones)
      {
        lijst.Add(zone.Naam);
      }
      return Json(
          lijst
         );
    }
    public ActionResult dashboardGrafiek(string zoneNaam)
    {
      IDashboardManager mgr = new DashboardManager();
      Zone zone = mgr.getZoneByNaam(zoneNaam);
      return Json("ok");
    }

    /*public ActionResult changeZone(int zoneid, Zone zone)
    {
      if (ModelState.IsValid)
      {
        mgr.changeZone(zone);
      }
    }*/

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
    public ActionResult _SearchPartial(string searchstring = "test")
    {
      if (searchstring is null)
      {
        List<Element> leeg = new List<Element>();
        return PartialView(leeg);
      }
      List<Element> elementen = Emgr.getAllElementen().Where(e => e.Naam.ToLower().Contains(searchstring.ToLower())).ToList();
      elementen = elementen.OrderBy(o => o.Trend).ToList();
      List<Persoon> personen = Emgr.getAllPersonen().Where(e => e.Organisatie.Naam.ToLower().Contains(searchstring.ToLower())).ToList();
      personen = personen.OrderBy(o => o.Trend).ToList();
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
      if (elementen.Count() > 5)
      {
        elementen = elementen.GetRange(0, 5);
      }

      return PartialView(elementen);
    }


    public ActionResult Search(string searchstring = "test")
    {
      if (searchstring == "test")
      {
        List<Element> leeg = new List<Element>();
        ViewBag.Lijst = leeg;
        return View();
      }
      List<Element> elementen = Emgr.getAllElementen().Where(e => e.Naam.ToLower().Contains(searchstring.ToLower())).ToList();
      elementen = elementen.OrderBy(o => o.Trend).ToList();
      List<Persoon> personen = Emgr.getAllPersonen().Where(e => e.Organisatie.Naam.ToLower().Contains(searchstring.ToLower())).ToList();
      personen = personen.OrderBy(o => o.Trend).ToList();
      elementen.AddRange(personen);
      List<Thema> themas = Emgr.getAllThemas();
      for (int i = 0; i < themas.Count(); i++)
      {
        Thema thema = themas.ElementAt(i);
        List<Keyword> keywords = thema.Keywords;
        if (keywords is null) { }


      }
      if (elementen.Count() > 5)
      {
        elementen = elementen.GetRange(0, 5);
      }
      List<string> lijst = new List<string>();
      foreach (Element element in elementen)
      {
        lijst.Add(element.Naam);
      }
      ViewBag.Lijst = lijst;
      return Json(
           lijst
          );
    }
    public ActionResult getElementType(string naam)
    {
      Element element = Emgr.getElementByNaam(naam);
      if (element.GetType().Equals(typeof(Persoon)))
      {
        return Json("Persoon");
      }
      else if (element.GetType().Equals(typeof(Organisatie)))
      {
        return Json("Organisatie");
      }
      else if (element.GetType().Equals(typeof(Thema)))
      {
        return Json("Thema");
      }
      else
      {
        return Json("");
      }
    }

    [HttpPost]
    public ActionResult getElementen()
    {
      IElementManager elementManager = new ElementManager();

      List<Element> elementen = elementManager.getAllElementen();
      return Json(elementen);
    }

        public ActionResult Organisatie(int id)
        {
            Element element = Emgr.getElementById(id);
            ViewBag.Labels = null;
            ViewBag.Data = null;
            return View(element);
        }
        public ActionResult Persoon(int id,string naam)
        {
            ElementManager mgr = new ElementManager();
            Element testElement = mgr.getElementByNaam(naam);
            DashboardManager dashboardManager = new DashboardManager();

            Grafiek testGrafiek = new Grafiek()
            {
                DataType = DataType.TOTAAL,


                Tijdschaal = new TimeSpan(30, 0, 0, 0),
                Dataconfigs = new List<DataConfig>()
                {
                },
                AantalDataPoints = 30
            };

            string dataString = dashboardManager.getGraphData(testGrafiek);


            Dictionary<string, string> dataconfigs = JsonConvert.DeserializeObject<Dictionary<string, string>>(dataString);

            Dictionary<DateTime, double> data = JsonConvert.DeserializeObject<Dictionary<DateTime, double>>(dataconfigs.First().Value);


            List<string> dates = new List<string>();
            foreach (DateTime item in data.Keys)
            {
                dates.Add(item.ToString("d MMM yyyy "));
            }
            Element element = Emgr.getElementById(id);
            ViewBag.LabelsLijn = dates;
            ViewBag.DataLijn = data;
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
    public ActionResult Thema(string naam)
    {
      Element element = Emgr.getElementByNaam(naam);
      ViewBag.Labels = null;
      ViewBag.Data = null;
      return View(element);
    }
    #endregion
  }

}
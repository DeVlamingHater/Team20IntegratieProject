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
            //BarGrafiek
            ElementManager elementManager = new ElementManager();
            List<Element> elementenTrending = elementManager.getTrendingElementen(3);
            StringBuilder labelsBar1 = new StringBuilder();
            StringBuilder dataBar1 = new StringBuilder();
            foreach (Element element in elementenTrending)
            {
                labelsBar1.Append(element.Naam).Append(".");
                dataBar1.Append(element.Trend).Append(".");
            }
            labelsBar1.Remove(labelsBar1.Length - 1, 1);
            dataBar1.Remove(dataBar1.Length - 1, 1);
            ViewBag.LabelsBar1 = labelsBar1;
            ViewBag.DataBar1 = dataBar1;
            //LijnGrafiek1
            string naamLijnGrafiek1 = elementenTrending.First().Naam;
            string[] arrayLijnGrafiek1 = getGraphData(naamLijnGrafiek1, "totaal");
            ViewBag.LabelsLijn1 = arrayLijnGrafiek1[0];
            ViewBag.DataLijn1 = arrayLijnGrafiek1[1];
            elementenTrending.Remove(elementenTrending.First());

            /*
            //LijnGrafiek2
            string naamLijnGrafiek2 = elementenTrending.First().Naam;
            string[] arrayLijnGrafiek2 = getGraphData(naamLijnGrafiek2, "totaal");
            ViewBag.LabelsLijn2 = arrayLijnGrafiek2[0];
            ViewBag.DataLijn2 = arrayLijnGrafiek2[1];
            elementenTrending.Remove(elementenTrending.First());

            //LijnGrafiek3
            string naamLijnGrafiek3 = elementenTrending.First().Naam;
            string[] arrayLijnGrafiek3 = getGraphData(naamLijnGrafiek3, "totaal");
            ViewBag.LabelsLijn3 = arrayLijnGrafiek3[0];
            ViewBag.DataLijn3 = arrayLijnGrafiek3[1];

            TaartGrafiek1
            string naamTaartGrafiek1 = elementManager.getTrendingElementen(1)[0].Naam;
            string[] arrayTaartGrafiek1 = getGraphData(naamTaartGrafiek1, "sentiment");
            ViewBag.LabelsTaart1 = arrayTaartGrafiek1[0];
            ViewBag.DataTaart1 = arrayTaartGrafiek1[1];

            //TaartGrafiek2
            string naamTaartGrafiek2 = elementManager.getTrendingElementen(1)[1].Naam;
            string[] arrayTaartGrafiek2 = getGraphData(naamTaartGrafiek2, "sentiment");
            ViewBag.LabelsTaart2 = arrayTaartGrafiek2[0];
            ViewBag.DataTaart2 = arrayTaartGrafiek2[1];

            //TaartGrafiek3
            string naamTaartGrafiek3 = elementManager.getTrendingElementen(1)[2].Naam;
            string[] arrayTaartGrafiek3 = getGraphData(naamTaartGrafiek3, "sentiment");
            ViewBag.LabelsTaart3 = arrayTaartGrafiek3[0];
            ViewBag.DataTaart3 = arrayTaartGrafiek3[1];
            */
            return View();
        }
        public string[] getGraphData(string naam, string dataType1)
        {
            ElementManager elementManager = new ElementManager();
            Element persoon = elementManager.getElementByNaam(naam);
            DashboardManager dashboardManager = new DashboardManager();
            Enum.TryParse(dataType1.ToUpper(), out DataType mijnDatatype);
            DataConfig testDataConfig = new DataConfig()
            {
                Element =
             persoon
            };
            Grafiek testGrafiek = new Grafiek()
            {
                DataType = mijnDatatype,


                Tijdschaal = new TimeSpan(30, 0, 0, 0),
                Dataconfigs = new List<DataConfig>()
                {
                     testDataConfig
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
            StringBuilder labels = new StringBuilder();
            StringBuilder dataStringbuilder = new StringBuilder();
            for (int i = 0; i < dates.Count(); i++)
            {
                labels.Append(dates.ElementAt(i)).Append(".");
                dataStringbuilder.Append(data.Values.ElementAt(i)).Append(".");
            }
            labels.Remove(labels.Length - 1, 1);
            dataStringbuilder.Remove(dataStringbuilder.Length - 1, 1);
            string[] gegevens = new string[2];
            gegevens[0] = labels.ToString();
            gegevens[1] = dataStringbuilder.ToString();
            return gegevens;
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
      //  actieveZone = zones.First().Id;
     //}
      return View(zones);
    }
    public ActionResult _ItemsPartial(int zoneId)
    {

      IDashboardManager mgr = new DashboardManager();
      IEnumerable<Item> items = mgr.getItems(zoneId);
      return PartialView(items);
    }
    public ActionResult _ItemPartial(int grafiekType, int index, string labels, string data, string page)

    {
      ViewBag.GrafiekType = grafiekType;
      ViewBag.GrafiekIndex = index;
      ViewBag.Labels = labels;
      ViewBag.Data = data;
      ViewBag.grafiekButtons = "grafiekButtons"+index;
      ViewBag.alert = "alert" + index;
      ViewBag.dash = "dash" + index;
      ViewBag.edit = "edit" + index;
      ViewBag.delete = "delete" + index;
      ViewBag.page = page;
      //ViewBag.startDatum = labels.First();
      //ViewBag.eindDatum = labels.Last();
      //filter ook nog
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
    public ActionResult dashboardGrafiek(string zoneNaam, int grafiekIndex)
    {
      // IDashboardManager mgr = new DashboardManager();
      // Zone zone = mgr.getZoneByNaam(zoneNaam);
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

        public ActionResult Organisatie(string naam)
        {
            ElementManager elementManager = new ElementManager();
            Element element = Emgr.getElementByNaam(naam);
            string[] arrayLijnGrafiek = getGraphData(naam, "totaal");
            ViewBag.LabelsLijn = arrayLijnGrafiek[0];
            ViewBag.DataLijn = arrayLijnGrafiek[1];
            string[] arrayBarGrafiek = getGraphData(naam, "trend");
            ViewBag.LabelsBar = arrayBarGrafiek[0];
            ViewBag.DataBar = arrayBarGrafiek[1];
            //string[] arrayTaartGrafiek = getGraphData(naam, "sentiment");
            //ViewBag.LabelsTaart = arrayTaartGrafiek[0];
            //ViewBag.DataTaart = arrayTaartGrafiek[1];
            return View(element);
        }
        public ActionResult Persoon(string naam)
        {
            ElementManager elementManager = new ElementManager();
            Element element = Emgr.getElementByNaam(naam);
            string[] arrayLijnGrafiek = getGraphData(naam, "totaal");
            ViewBag.LabelsLijn = arrayLijnGrafiek[0];
            ViewBag.DataLijn = arrayLijnGrafiek[1];
            string[] arrayBarGrafiek = getGraphData(naam, "trend");
            ViewBag.LabelsBar = arrayBarGrafiek[0];
            ViewBag.DataBar = arrayBarGrafiek[1];
            //string[] arrayTaartGrafiek = getGraphData(naam, "sentiment");
            //ViewBag.LabelsTaart = arrayTaartGrafiek[0];
            //ViewBag.DataTaart = arrayTaartGrafiek[1];
            return View(element);
        }
        public ActionResult setImage(string twitter)
    {
      if(twitter != null)
      {
          string twitter1 = twitter.Replace("@", "");
        string url = "https://twitter.com/" + twitter1 + "/profile_image?size=original";
        return Redirect(url);
      }
      return View();

    }
    public ActionResult setTwitter(string twitter)
    {
      if (twitter != null)
      {
        string twitter1 = twitter.Replace("@", "");
        string url = "https://twitter.com/" + twitter1;
        return Redirect(url);
      }
      return View();
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
            ElementManager elementManager = new ElementManager();
            Element element = Emgr.getElementByNaam(naam);
            string[] arrayLijnGrafiek = getGraphData(naam, "totaal");
            ViewBag.LabelsLijn = arrayLijnGrafiek[0];
            ViewBag.DataLijn = arrayLijnGrafiek[1];
            string[] arrayBarGrafiek = getGraphData(naam, "trend");
            ViewBag.LabelsBar = arrayBarGrafiek[0];
            ViewBag.DataBar = arrayBarGrafiek[1];
            //string[] arrayTaartGrafiek = getGraphData(naam, "sentiment");
            //ViewBag.LabelsTaart = arrayTaartGrafiek[0];
            //ViewBag.DataTaart = arrayTaartGrafiek[1];
            return View(element);
    }
    public ActionResult addWeeklyReview(int id)
    {
      Element element = Emgr.getElementById(id);
      //weeklyReview.add(element);
      return Json(new
      {
        result = "OK"
      });
    }
    public ActionResult removeWeeklyReview(int id)
    {
      Element element = Emgr.getElementById(id);
      //weeklyReview.remove(element);
      return Json(new
      {
        result = "OK"
      });
    }
    public ActionResult addAlert(int id, int percentage, string soort, string radio, Boolean browser, Boolean mail, Boolean app)
    {
      // bij grafiek is id = grafiekindex
      // bij element is id = elementid
      // addAlert()
      return Json(new
      {
        result = "OK"
      });
    }
    public ActionResult editGrafiek(int grafiekIndex)
    {
      // edit grafiek
      return Json(new
      {
        result = "OK"
      });
    }
    public ActionResult deleteGrafiek(int grafiekIndex)
    {
      // edit grafiek
      return Json(new
      {
        result = "OK"
      });
    }

    #endregion
  }

}
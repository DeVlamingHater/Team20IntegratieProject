using BL.Interfaces;
using BL.Managers;
using Domain;
using Domain.Dashboards;
using Domain.Elementen;
using Domain.Platformen;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using PolitiekeBarometer_MVC.Controllers.ActionFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace PolitiekeBarometer_MVC.Controllers
{
    public class DeelplatformController : BaseController
    {
        public ActionResult Index(string deelplatform)
        {
            //TODO: verplaats naar DeelplatformController
            //Dashboard van het juiste deelplatform ophalen aan de hand van de string in de url
            IPlatformManager platformManager = new PlatformManager();
            DeelplatformDashboard dpd = platformManager.getDeelplatformDashboard(deelplatformURL);
            List<Item> items = new List<Item>();
            items.AddRange(dpd.Items);
            if (items.Count==0)
            {
                items.AddRange(TestItems.GetTestItems(deelplatformURL));
            }
            return View(items);
        }
        public ActionResult Test()
        {
            ElementManager mgr = new ElementManager();
            List<string> namen = new List<string>();
            List<int> ids = new List<int>();
            foreach (Element element in mgr.getTrendingElementen(1, Deelplatform))
            {
                namen.Add(element.Naam);
                ids.Add(element.Id);
            }
            Json(ViewBag.Namen = namen);
            ViewBag.Lengte = namen.Count();
            ViewBag.Ids = ids;
            return View();
        }

        public ActionResult Element()
        {
            return View();
        }

        public ActionResult Alerts()
        {
            return View();
        }

        /*public ActionResult changeZone(int zoneid, Zone zone)
        {
          if (ModelState.IsValid)
          {
            mgr.changeZone(zone);
          }
        }*/

        #region Element
        public ActionResult _PersonenDropDown()
        {
            IElementManager elementManager = new ElementManager();
            List<Element> elementen = elementManager.getTrendingElementen(3, Deelplatform).Where(e => e.GetType().Equals(typeof(Persoon))).ToList();
            List<Persoon> personen = new List<Domain.Persoon>();

            foreach (Element element in elementen)
            {
                personen.Add((Persoon)element);
            }
            return PartialView(personen);
        }
        public ActionResult _OrganisatieDropDown()
        {
            IElementManager elementManager = new ElementManager();
            List<Element> elementen = elementManager.getTrendingElementen(3, Deelplatform).Where(e => e.GetType().Equals(typeof(Organisatie))).ToList();
            List<Organisatie> organisaties = new List<Domain.Organisatie>();
            foreach (Element element in elementen)
            {
                organisaties.Add((Organisatie)element);
            }
            return PartialView(organisaties);
        }


        public ActionResult _ThemaDropDown()
        {
            IElementManager elementManager = new ElementManager();
            List<Element> elementen = elementManager.getTrendingElementen(3, Deelplatform).Where(e => e.GetType().Equals(typeof(Thema))).ToList();
            List<Thema> themas = new List<Domain.Thema>();
            foreach (Element element in elementen)
            {
                themas.Add((Thema)element);
            }
            return PartialView(themas);
        }
        public ActionResult _SearchPartial(string searchstring = "test")
        {
            IElementManager elementManager = new ElementManager();
            if (searchstring is null)
            {
                List<Element> leeg = new List<Element>();
                return PartialView(leeg);
            }
            List<Element> elementen = elementManager.getAllElementen(Deelplatform).Where(e => e.Naam.ToLower().Contains(searchstring.ToLower())).ToList();
            elementen = elementen.OrderBy(o => o.Trend).ToList();
            List<Persoon> personen = elementManager.getAllPersonen(Deelplatform).Where(e => e.Organisatie.Naam.ToLower().Contains(searchstring.ToLower())).ToList();
            personen = personen.OrderBy(o => o.Trend).ToList();
            elementen.AddRange(personen);
            List<Thema> themas = elementManager.getAllThemas(Deelplatform);
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
            IElementManager elementManager = new ElementManager();
            if (searchstring == "test")
            {
                List<Element> leeg = new List<Element>();
                ViewBag.Lijst = leeg;
                return View();
            }
            List<Element> elementen = elementManager.getAllElementen(Deelplatform).Where(e => e.Naam.ToLower().Contains(searchstring.ToLower())).ToList();
            elementen = elementen.OrderBy(o => o.Trend).ToList();
            List<Persoon> personen = elementManager.getAllPersonen(Deelplatform).Where(e => e.Organisatie.Naam.ToLower().Contains(searchstring.ToLower())).ToList();
            personen = personen.OrderBy(o => o.Trend).ToList();
            elementen.AddRange(personen);
            List<Thema> themas = elementManager.getAllThemas(Deelplatform);

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
            return Json(lijst);
        }
        public ActionResult getElementType(string naam)
        {
            IElementManager elementManager = new ElementManager();
            Element element = elementManager.getElementByNaam(naam, Deelplatform);
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

            List<Element> elementen = elementManager.getAllElementen(Deelplatform);
            return Json(elementen);
        }

        public ActionResult Organisatie(string naam)
        {
            IElementManager elementManager = new ElementManager();
            Element element = elementManager.getElementByNaam(naam, Deelplatform);
        
            //string[] arrayTaartGrafiek = getGraphData(naam, "sentiment");
            //ViewBag.LabelsTaart = arrayTaartGrafiek[0];
            //ViewBag.DataTaart = arrayTaartGrafiek[1];
            return View(element);
        }
        public ActionResult Persoon(string naam)
        {
            IElementManager elementManager = new ElementManager();
            Element element = elementManager.getElementByNaam(naam, Deelplatform);
          
            //string[] arrayTaartGrafiek = getGraphData(naam, "sentiment");
            //ViewBag.LabelsTaart = arrayTaartGrafiek[0];
            //ViewBag.DataTaart = arrayTaartGrafiek[1];
            return View(element);
        }
        public ActionResult setImage(string twitter)
        {
            if (twitter != null)
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
            IElementManager elementManager = new ElementManager();
            Element element = elementManager.getElementByNaam(naam, Deelplatform);
           
            //string[] arrayTaartGrafiek = getGraphData(naam, "sentiment");
            //ViewBag.LabelsTaart = arrayTaartGrafiek[0];
            //ViewBag.DataTaart = arrayTaartGrafiek[1];
            return View(element);
        }
        public ActionResult addWeeklyReview(int id)
        {
            IElementManager elementManager = new ElementManager();
            Element element = elementManager.getElementById(id);
            //weeklyReview.add(element);
            return Json(new
            {
                result = "OK"
            });
        }
        public ActionResult removeWeeklyReview(int id)
        {
            IElementManager elementManager = new ElementManager();
            Element element = elementManager.getElementById(id);
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

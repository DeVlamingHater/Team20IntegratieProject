using BL.Interfaces;
using BL.Managers;
using Domain;
using Domain.Elementen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PolitiekeBarometer_MVC.Controllers
{
    public class apiController : ApiController
    {
        public List<Element> search(string searchString)
        {
            List<Element> elementen = null;

             IElementManager elementManager = new ElementManager();
            if (searchString is null)
            {
                
            }
            elementen = elementManager.getAllElementen().Where(e => e.Naam.ToLower().Contains(searchString.ToLower())).ToList();
            elementen = elementen.OrderBy(o => o.Trend).ToList();
            List<Persoon> personen = elementManager.getAllPersonen().Where(e => e.Organisatie.Naam.ToLower().Contains(searchString.ToLower())).ToList();
            personen = personen.OrderBy(o => o.Trend).ToList();
            elementen.AddRange(personen);
            List<Thema> themas = elementManager.getAllThemas();
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
                        if (keyword.KeywordNaam.ToLower().Contains(searchString.ToLower()))
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
            return elementen;
        }
    }
}

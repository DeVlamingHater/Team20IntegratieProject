using BL.Interfaces;
using DAL;
using DAL.Repositories_EF;
using Domain;
using Domain.Dashboards;
using Newtonsoft.Json;
using POC_IntegratieProject_framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BL.Managers
{
    public class ElementManager : IElementManager
    {
        IElementRepository elementRepository = new ElementRepository_EF();

        UnitOfWorkManager uowManager;

        public ElementManager(UnitOfWorkManager uowManager)
        {
            this.uowManager = uowManager;
            elementRepository = new ElementRepository_EF(uowManager.UnitOfWork);
        }

        public ElementManager()
        {
            elementRepository = new ElementRepository_EF();
        }

        #region Element
        public List<Element> getAllElementen()
        {
            return elementRepository.getAllElementen().ToList();
        }

        public Element getElementByNaam(string naam)
        {
            Element element = elementRepository.getElementByName(naam);
            return element;
        }

        public void setTrendingElementen()
        {
            UnitOfWorkManager unitOfWorkManager = new UnitOfWorkManager();
            this.uowManager = unitOfWorkManager;
            PostManager postManager = new PostManager(unitOfWorkManager);

            List<Element> elementen = getAllElementen();

            foreach (Element element in elementen)
            {
                element.Trend = postManager.calculateElementTrend(element);
            }
            //TODO: 3Per Type
            elementen.ForEach(e => elementRepository.setElement(e));
        }

        public List<Element> getTopTrending(List<Element> elementen, int amount)
        {
            List<Element> elementenTrending = new List<Element>();

            for (int i = 0; i < amount; i++)
            {
                if (elementen.Count == 0)
                {
                    return elementenTrending;
                }
                double maxTrend = 0.0;

                Element maxElement = elementen.First();
                foreach (Element element in elementen)
                {
                    if (element.Trend > maxTrend)
                    {
                        maxElement = element;
                        maxTrend = maxElement.Trend;
                    }
                }
                elementen.Remove(maxElement);
                elementenTrending.Add(maxElement);
            }
            return elementenTrending;
        }

        public List<Element> getTrendingElementen(int amount = 1)
        {
            List<Element> elementenTrending = elementRepository.getAllElementen().ToList();
            List<Element> elementen = new List<Element>();
            List<Element> personen = elementenTrending.Where(e => e.GetType().Equals(typeof(Persoon))).ToList();
            List<Element> themas = elementenTrending.Where(e => e.GetType().Equals(typeof(Thema))).ToList();
            List<Element> organisaties = elementenTrending.Where(e => e.GetType().Equals(typeof(Organisatie))).ToList();

            elementen.AddRange(getTopTrending(personen, amount));
            elementen.AddRange(getTopTrending(themas, amount));
            elementen.AddRange(getTopTrending(organisaties, amount));

            return elementen;
        }

        public Element getElementById(int id)
        {
            return elementRepository.getElementByID(id);
        }

        public void addElementen(List<Element> elementen)
        {
            elementRepository.addElementen(elementen);
        }
        #endregion

        #region Persoon
        public List<Persoon> getAllPersonen()
        {
            return elementRepository.getAllPersonen().ToList();
        }

        public void addPersonen(List<Persoon> personen)
        {
            personen.ForEach(p => elementRepository.AddPersoon(p));
        }

        public List<Persoon> readJSONPersonen()
        {
            List<Persoon> personen = new List<Persoon>();
            List<PersoonParser> items;

            using (StreamReader r = new StreamReader("politici.json"))
            {
                string json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<List<PersoonParser>>(json);
            }
            foreach (PersoonParser persoon in items)
            {
                Persoon politicus = new Persoon()
                {
                    DateOfBirth = persoon.dateOfBirth,
                    District = persoon.district,
                    Facebook = persoon.facebook,
                    Gender = persoon.gender,
                    Naam = persoon.full_name,
                    Position = persoon.position,
                    Level = persoon.level,
                    Postal_code = persoon.postal_code,
                    Site = persoon.site,
                    Town = persoon.town,
                    Twitter = persoon.twitter,
                };
                Organisatie organisatie = (Organisatie)getElementByNaam(persoon.organisation);
                if (organisatie == null)
                {
                    organisatie = new Organisatie()
                    {
                        Naam = persoon.organisation,
                        Personen = new List<Persoon>()
                        {
                            politicus
                        }
                    };
                    addOrganisatie(organisatie);
                }
                politicus.Organisatie = organisatie;
                personen.Add(politicus);
            }
            return personen;
        }

        public void deleteAllPersonen()
        {
            elementRepository.deleteAllPersonen();
        }

        public void addPersoon(Persoon persoon)
        {
            elementRepository.AddPersoon(persoon);
        }

        public void deletePersoon(Persoon persoon)
        {
            elementRepository.deletePersoon(persoon);
        }
        #endregion

        #region Thema
        public List<Thema> getAllThemas()
        {
            return elementRepository.getAllThemas();
        }
        #endregion

        #region Organisatie
        public void addOrganisatie(Organisatie organisatie)
        {
            elementRepository.addOrganisatie(organisatie);
        }
        #endregion
    }
}

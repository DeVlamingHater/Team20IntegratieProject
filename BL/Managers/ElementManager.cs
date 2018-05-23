using BL.Interfaces;
using DAL;
using DAL.Repositories_EF;
using Domain;
using Domain.Dashboards;
using Domain.Platformen;
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
        #region Constructor
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
        #endregion

        #region Element
        public List<Element> getAllElementen(Deelplatform deelplatform)
        {
            return elementRepository.getAllElementen(deelplatform).ToList();
        }

        public Element getElementByNaam(string naam, Deelplatform deelplatform)
        {
            Element element = elementRepository.getElementByName(naam, deelplatform);
            return element;
        }

        public void setTrendingElementen(Deelplatform deelplatform)
        {
            UnitOfWorkManager unitOfWorkManager = new UnitOfWorkManager();
            this.uowManager = unitOfWorkManager;
            PostManager postManager = new PostManager(unitOfWorkManager);

            List<Element> elementen = getAllElementen(deelplatform);

            foreach (Element element in elementen)
            {
                element.Trend = postManager.calculateElementTrend(element);
            }
            //TODO: 3Per Type
            elementen.ForEach(e => elementRepository.setElement(e));
        }

        public List<Element> getTrendingElementen(int amount , Deelplatform deelplatform)
        {
            List<Element> elementenTrending = elementRepository.getAllElementen(deelplatform).ToList();
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
        public List<Persoon> getAllPersonen(Deelplatform deelplatform)
        {
            return elementRepository.getAllPersonen(deelplatform).ToList();
        }

        public void addPersonen(List<Persoon> personen)
        {
            personen.ForEach(p => elementRepository.AddPersoon(p));
        }

        public List<Persoon> readJSONPolitici()
        {
            IPlatformManager platformManager = new PlatformManager();
            Deelplatform deelplatform = platformManager.getDeelplatformByNaam("Politiek");

            List<Persoon> personen = new List<Persoon>();
            List<PersoonParser> items;

            using (StreamReader r = new StreamReader(AppContext.BaseDirectory + "bin/politici.json"))
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
                    Deelplatform = deelplatform
                };
                Organisatie organisatie = (Organisatie)getElementByNaam(persoon.organisation, deelplatform);
                if (organisatie == null)
                {
                    organisatie = new Organisatie()
                    {
                        Naam = persoon.organisation,
                        Personen = new List<Persoon>()
                        {
                            politicus
                        },
                        Deelplatform = deelplatform
                    };
                    addOrganisatie(organisatie);
                }
                politicus.Organisatie = organisatie;
                personen.Add(politicus);
            }
            return personen;
        }

        public void deleteAllPersonen(Deelplatform deelplatform)
        {
            elementRepository.deleteAllPersonen(deelplatform);
        }

        public void addPersoon(Persoon persoon)
        {
            elementRepository.AddPersoon(persoon);
        }

        public void deletePersoon(Persoon persoon)
        {
            elementRepository.deletePersoon(persoon);
        }

        public void updatePersoon(Persoon persoon)
        {
            elementRepository.updatePersoon(persoon);
        }
        #endregion

        #region Thema
        public List<Thema> getAllThemas(Deelplatform deelplatform)
        {
            return elementRepository.getAllThemas(deelplatform);
        }
        public void addThema(Thema thema)
        {
            elementRepository.addThema(thema);
        }
        public void updateThema(Thema thema)
        {
            elementRepository.updateThema(thema);
        }
        public void deleteThema(Thema thema)
        {
            elementRepository.deleteThema(thema);
        }
        #endregion

        #region Organisatie
        public void addOrganisatie(Organisatie organisatie)
        {
            elementRepository.addOrganisatie(organisatie);
        }

        public List<Organisatie> getAllOrganisaties(Deelplatform deelplatform)
        {
            return elementRepository.getAllOrganisaties(deelplatform);
        }
        public void updateOrganisatie(Organisatie organisatie)
        {
            elementRepository.updateOrganisatie(organisatie);
        }
        public void deleteOrganisatie(Organisatie organisatie)
        {
            elementRepository.deleteOrganisatie(organisatie);
        }
        #endregion

        #region Helper
        private List<Element> getTopTrending(List<Element> elementen, int amount)
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
        #endregion
    }
}

using BL.Interfaces;
using DAL;
using DAL.Repositories_EF;
using Domain;
using System;
using System.Collections.Generic;
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

        public List<Element> getTrendingElementen(int amount = 3)
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

    public IEnumerable<Persoon> getAllPersonen()
    {
      return elementRepository.getAllPersonen();
      }

        public void addElementen(List<Element> elementen)
        {
            elementRepository.addElementen(elementen);
        }

        public void addOrganisatie(Organisatie organisatie)
        {
            elementRepository.addOrganisatie(organisatie);
        }
    }
  }
}

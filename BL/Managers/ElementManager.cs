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

            if (element == null)
            {
                element = new Persoon()
                {
                    Naam = naam
                };
                elementRepository.AddPersoon((Persoon)element);
            }
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

        public List<Element> getTrendingElementen(int amount = 3)
        {
            List<Element> elementenTrending = elementRepository.getAllElementen().ToList();
            List<Element> elementen = new List<Element>();
            for (int i = 0; i < amount; i++)
            {
                double maxTrend = 0.0;
                Element maxElement = elementenTrending.First();
                foreach (Element element in elementenTrending)
                {
                    if (element.Trend > maxTrend)
                    {
                        maxElement = element;
                        maxTrend = maxElement.Trend;
                    }
                }
                elementenTrending.Remove(maxElement);
                elementen.Add(maxElement);
            }
            return elementen;
        }
    }
}

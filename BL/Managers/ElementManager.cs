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

        public List<Element> getTrendingElementen(int amount = 3)
        {
            List<Element> elementenTrending = new List<Element>();
            elementenTrending = elementRepository.getTrendingElementen(amount);
            return null;
        }
       

        public void setTrendingElementen()
        {
            PostManager postManager = new PostManager();

            List<Element> elementen = getAllElementen();

            foreach (Element element in elementen)
            {
                element.Trend = postManager.calculateElementTrend(element);
                elementRepository.setElement(element);
            }
            elementen.Sort();
            int index = 0;
            elementen.ForEach(e => e.TrendingPlaats = index++);
        }

       
    }
}

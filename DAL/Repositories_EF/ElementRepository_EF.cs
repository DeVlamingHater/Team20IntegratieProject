using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using DAL.EF;
using Domain;
using Domain.Elementen;

namespace DAL.Repositories_EF
{
    public class ElementRepository_EF : IElementRepository
    {
        PolitiekeBarometerContext context;

        public ElementRepository_EF()
        {
            context = new PolitiekeBarometerContext();
        }
        public ElementRepository_EF(UnitOfWork unitOfWork)
        {
            context = unitOfWork.Context;
        }
        public void AddPersoon(Persoon persoon)
        {
            context.Personen.Add(persoon);
            context.SaveChanges();
        }

        public IEnumerable<Element> getAllElementen()
        {
            List<Element> elementen = new List<Element>();
            elementen.AddRange(context.Themas);
            elementen.AddRange(context.Organisaties);
            elementen.AddRange(context.Personen);
            return elementen;
        }

        public Element getElementByID(int elementId)
        {
            return null;
        }

        public Element getElementByName(string naam)
        {
            return null;
        }
    }
}

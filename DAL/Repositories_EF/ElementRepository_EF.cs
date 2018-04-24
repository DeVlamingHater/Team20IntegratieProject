using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using DAL.EF;
using Domain;
using Domain.Elementen;
using System.ComponentModel.DataAnnotations;

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

        public void addElementen(List<Element> elementen)
        {
            List<Persoon> personen = (List<Persoon>)elementen.Where(e => e.GetType().Equals(typeof(Persoon)));
            List<Thema> themas = (List<Thema>)elementen.Where(e => e.GetType().Equals(typeof(Thema)));
            List<Organisatie> organisaties = (List<Organisatie>)elementen.Where(e => e.GetType().Equals(typeof(Organisatie)));

            if (personen.Count != 0)
                context.Personen.AddRange(personen);

            if (themas.Count != 0)
                context.Themas.AddRange(themas);

            if (organisaties.Count != 0)
                context.Organisaties.AddRange(organisaties);
        }

        public void addOrganisatie(Organisatie organisatie)
        {
            List<ValidationResult> errors = new List<ValidationResult>();
            if (Validator.TryValidateObject(organisatie, new ValidationContext(organisatie), errors))
            {
                context.Organisaties.Add(organisatie);
                context.SaveChanges();
            }

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

        public void setElement(Element element)
        {
            context.Entry(element).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}

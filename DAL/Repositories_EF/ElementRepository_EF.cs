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
            throw new NotImplementedException();
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
            List<ValidationResult> errors = new List<ValidationResult>();
            List<Persoon> persoons = context.Personen.Where(p => p.Naam == persoon.Naam).ToList();
            if (Validator.TryValidateObject(persoon, new ValidationContext(persoon), errors) && context.Personen.Where(p=>p.Naam == persoon.Naam).Count()==0)
            {
                context.Personen.Add(persoon);
                context.SaveChanges();
            }
        }

        public IEnumerable<Element> getAllElementen()
        {
            List<Element> elementen = new List<Element>();
            elementen.AddRange(context.Themas);
            elementen.AddRange(context.Organisaties);
            elementen.AddRange(context.Personen);
            return elementen;
        }

        public IEnumerable<Persoon> getAllPersonen()
        {
            return context.Personen;
        }
        
        public List<Thema> getAllThemas()
    {
      return context.Themas.ToList();
    }

        public Element getElementByID(int elementId)
        {
            Element element = (Element)context.Personen.FirstOrDefault(p => p.Id.Equals(elementId));
            if (element == null)
            {
                element = (Element)context.Organisaties.FirstOrDefault(p => p.Id.Equals(elementId));
            }
            if (element == null)
            {
                element = (Element)context.Themas.FirstOrDefault(p => p.Id.Equals(elementId));
            }
            return null;
        }

        public Element getElementByName(string naam)
        {
            Element element = (Element)context.Personen.FirstOrDefault(p=>p.Naam.Equals(naam));
            if (element == null)
            {
                element = (Element)context.Organisaties.FirstOrDefault(p => p.Naam.Equals(naam));
            }
            if (element == null)
            {
                element = (Element)context.Themas.FirstOrDefault(p => p.Naam.Equals(naam));
            }
            return element;
        }

        public List<Element> getTrendingElementen(int amount)
        {
            throw new NotImplementedException();
        }

        public void setElement(Element element)
        {
            context.Entry(element).State = EntityState.Modified;
            context.SaveChanges();
        }
    
  }
}
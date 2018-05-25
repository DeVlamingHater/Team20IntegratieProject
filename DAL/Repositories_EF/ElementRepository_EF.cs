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
using Domain.Platformen;

namespace DAL.Repositories_EF
{
    public class ElementRepository_EF : IElementRepository
    {
        #region Constructor
        PolitiekeBarometerContext context;

        public ElementRepository_EF()
        {
            context = new PolitiekeBarometerContext();
        }
        public ElementRepository_EF(UnitOfWork unitOfWork)
        {
            context = unitOfWork.Context;
        }
        #endregion

        #region Elementen
        public void addElementen(List<Element> elementen)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Element> getAllElementen(Deelplatform deelplatform)
        {
            List<Element> elementen = new List<Element>();
            List<Thema> themas = context.Themas.Where(t => t.Deelplatform.Id == deelplatform.Id).ToList();
            if (themas.Count() != 0)
            {
                elementen.AddRange(themas);

            }
            IEnumerable<Organisatie> organisaties = context.Organisaties.Include(o => o.Personen).Where(t => t.Deelplatform.Id == deelplatform.Id);
            if (themas.Count() != 0)
            {
                elementen.AddRange(organisaties);
            }

            List<Persoon> personen = context.Personen.Include(p => p.Organisatie).Where(t => t.Deelplatform.Id == deelplatform.Id).ToList();
            if (personen.Count() != 0)
            {
                personen.Sort(Element.compareByNaam);
                elementen.AddRange(personen);
            }

            return elementen;
        }

        public Element getElementByID(int elementId)
        {
            Element element = (Element)context.Personen.Include(p => p.Organisatie).FirstOrDefault(p => p.Id.Equals(elementId));
            if (element == null)
            {
                element = (Element)context.Organisaties.FirstOrDefault(p => p.Id.Equals(elementId));
            }
            if (element == null)
            {
                element = (Element)context.Themas.FirstOrDefault(p => p.Id.Equals(elementId));
            }
            return element;
        }

        public Element getElementByName(string naam, Deelplatform deelplatform)
        {
            Element element = (Element)context.Personen.Include(p => p.Organisatie).FirstOrDefault(p => p.Naam.Equals(naam) && p.Deelplatform.Id == deelplatform.Id);
            if (element == null)
            {
                element = (Element)context.Organisaties.FirstOrDefault(p => p.Naam.Equals(naam) && p.Deelplatform.Id == deelplatform.Id);
            }
            if (element == null)
            {
                element = (Element)context.Themas.FirstOrDefault(p => p.Naam.Equals(naam) && p.Deelplatform.Id == deelplatform.Id);
            }
            return element;
        }

        public List<Element> getTrendingElementen(int amount, Deelplatform deelplatform)
        {
            List<Element> elementenTrending = new List<Element>();
            List<Element> elementen = getAllElementen(deelplatform).ToList();
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

        public void setElement(Element element)
        {
            context.Entry(element).State = EntityState.Modified;
            context.SaveChanges();
        }
        #endregion

        #region Organisatie
        public void addOrganisatie(Organisatie organisatie)
        {
            context.Organisaties.Add(organisatie);
            context.SaveChanges();
        }

        public List<Organisatie> getAllOrganisaties(Deelplatform deelplatform)
        {
            return context.Organisaties.Where(o => o.Deelplatform.Id == deelplatform.Id).ToList();
        }

        public void updateOrganisatie(Organisatie organisatie)
        {
            context.Entry(organisatie).State = EntityState.Modified;
            context.SaveChanges();
        }
        public void deleteOrganisatie(Organisatie organisatie)
        {
            context.Organisaties.Remove(organisatie);
            context.SaveChanges();
        }
        #endregion

        #region Persoon
        public void AddPersoon(Persoon persoon)
        {
            List<ValidationResult> errors = new List<ValidationResult>();
            List<Persoon> persoons = context.Personen.Where(p => p.Naam == persoon.Naam && p.Deelplatform.Id == persoon.Deelplatform.Id).ToList();
            if (Validator.TryValidateObject(persoon, new ValidationContext(persoon), errors))
            {
                if (context.Personen.Where(p => p.Naam == persoon.Naam).Count() == 0)
                {
                    context.Personen.Add(persoon);
                }
                else
                {
                    setPersoon(persoon);
                }
            }
            context.SaveChanges();
        }

        public IEnumerable<Persoon> getAllPersonen(Deelplatform deelplatform)
        {
            return context.Personen.Where(p => p.Deelplatform.Id == deelplatform.Id).Include(p => p.Organisatie).Include("Organisatie");
        }

        public void setPersoon(Persoon persoon)
        { 
            context.Entry(persoon).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void deleteAllPersonen(Deelplatform deelplatform)
        {
            context.Personen.RemoveRange(context.Personen.Where(p => p.Deelplatform.Id == deelplatform.Id));
            context.SaveChanges();
        }
        public void deletePersoon(Persoon persoon)
        {
            context.Personen.Remove(persoon);
            context.SaveChanges();
        }
        public void updatePersoon(Persoon persoon)
        {
            context.Entry(persoon).State = EntityState.Modified;
            context.SaveChanges();
        }
        #endregion

        #region Thema

        public List<Thema> getAllThemas(Deelplatform deelplatform)
        {
            return context.Themas.Where(t => t.Deelplatform.Id == deelplatform.Id).ToList();
        }

        public void addThema(Thema thema)
        {
            context.Themas.Add(thema);
            context.SaveChanges();
        }

        public void updateThema(Thema thema)
        {
            context.Entry(thema).State = EntityState.Modified;
            context.SaveChanges();
        }
        public void deleteThema(Thema thema)
        {
            context.Themas.Remove(thema);
            context.SaveChanges();
        }

        #endregion
    }
}
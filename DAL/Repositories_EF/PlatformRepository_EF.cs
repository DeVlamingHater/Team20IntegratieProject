using DAL.EF;
using Domain.Dashboards;
using Domain.Platformen;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories_EF
{
    public class PlatformRepository_EF : IPlatformRepository
    {
        PolitiekeBarometerContext context;

        public PlatformRepository_EF()
        {
            context = new PolitiekeBarometerContext();
        }
        public PlatformRepository_EF(UnitOfWork unitOfWork)
        {
            context = unitOfWork.Context;
        }

        #region Platform
        public Platform getPlatform()
        {
            return context.Platformen.First();
        }
        #endregion

        #region Deelplatform
        public Deelplatform getDeelPlatform(string deelplatform)
        {
            return context.Deelplatformen.FirstOrDefault(dp => dp.Naam == deelplatform);
        }

        public IEnumerable<Deelplatform> getAllDeelplatformen()
        {
            return context.Deelplatformen;
        }

        public void createDeelplatform(Deelplatform deelplatform)
        {
            context.Deelplatformen.Add(deelplatform);
            context.SaveChanges();
        }
        public DeelplatformDashboard getDeelplatformDashboard(string deelplatform)
        {
            DeelplatformDashboard deelplatformDashboard = context.DeelplatformDashboards.Include(dpd => dpd.Items).Where(dpd => dpd.Deelplatform.Naam == deelplatform).FirstOrDefault();
            if (deelplatformDashboard == null)
            {
                deelplatformDashboard = new DeelplatformDashboard()
                {
                    Deelplatform = context.Deelplatformen.FirstOrDefault(dp => dp.Naam == deelplatform),
                    Items = new List<Item>()
                };
            }
            return deelplatformDashboard;
        }

        #endregion

        #region Gebruiker
        public void createGebruiker(Gebruiker gebruiker)
        {
            context.Gebruikers.Add(gebruiker);
        }

        public void deleteGebruiker(string id)
        {
            Gebruiker gebruiker = context.Gebruikers.Where(g => g.GebruikerId == id).FirstOrDefault();
            context.Gebruikers.Remove(gebruiker);
            context.SaveChanges();
        }

        public List<Gebruiker> getAllGebruikers()
        {
            return context.Gebruikers.ToList();
        }

        public Gebruiker getGebruiker(string id)
        {
            return context.Gebruikers.First(g => g.GebruikerId == id);
        }

        public Gebruiker getGebruikerMetEmail(string email)
        {
            return context.Gebruikers.First(g => g.Email == email);
        }

        public void saveGebruiker(Gebruiker gebruiker)
        {
            context.Entry(gebruiker).State = EntityState.Modified;
            context.SaveChanges();
        }
        #endregion
    }
}

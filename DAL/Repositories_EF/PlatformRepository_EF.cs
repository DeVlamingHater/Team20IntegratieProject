using DAL.EF;
using Domain.Platformen;
using System;
using System.Collections.Generic;
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

        public void createGebruiker(string id, string name, string email)
        {
            context.Gebruikers.Add(new Gebruiker()
            {
                GebruikerId = id,
                Naam = name,
                Email = email
            });
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
            return context.Gebruikers.First(g=>g.GebruikerId == id);
        }
    }
}

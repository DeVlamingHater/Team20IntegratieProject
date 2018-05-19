using BL.Interfaces;
using DAL;
using DAL.Repositories_EF;
using Domain.Platformen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BL.Managers
{
    public class PlatformManager : IPlatformManager
    {
        IPlatformRepository platformRepository;

        UnitOfWorkManager uowManager;

        public PlatformManager()
        {
            platformRepository = new PlatformRepository_EF();
        }

        public PlatformManager(UnitOfWorkManager uowManager)
        {
            this.uowManager = uowManager;
            platformRepository = new PlatformRepository_EF(uowManager.UnitOfWork);
        }

        #region Gebruiker
        public Gebruiker getGebruiker(string id)
        {
            Gebruiker gebruiker = platformRepository.getGebruiker(id);
            return gebruiker;
        }

        public Gebruiker getGebruikerMetEmail(string email)
        {
            return platformRepository.getGebruikerMetEmail(email);
        }

        public void createGebruiker(string id, string name, string email)
        {
            platformRepository.createGebruiker(id, name, email);
        }

        public void deleteGebruiker(string id)
        {
            platformRepository.deleteGebruiker(id);
        }

        public List<Gebruiker> getAllGebruikers()
        {
            return platformRepository.getAllGebruikers();
        }

        public void updateGebruiker(Gebruiker gebruiker)
        {
            platformRepository.saveGebruiker(gebruiker);
        }


        #endregion

        #region deelplatform
        public Deelplatform getDeelPlatform(string deelplatform)
        {
            return platformRepository.getDeelPlatform(deelplatform);
        }

        public List<Deelplatform> getAllDeeplatformen()
        {
            return platformRepository.getAllDeelplatformen().ToList();
        }
        #endregion
    }
}

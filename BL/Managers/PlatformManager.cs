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
        #region Constructor
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
        #endregion

        #region Platform
        public TimeSpan getHistoriek()
        {
            return platformRepository.getPlatform().Historiek;
        }
        #endregion
      
        #region Deelplatform
        public Deelplatform getDeelplatformByNaam(string deelplatformNaam)
        {
            return platformRepository.getDeelPlatform(deelplatformNaam);
        }

        public List<Deelplatform> getAllDeeplatformen()
        {
            return platformRepository.getAllDeelplatformen().ToList();
        }

        public void createDeelplatform(Deelplatform deelplatform)
        {
            platformRepository.createDeelplatform(deelplatform);
        }
        #endregion

        #region Gebruiker
        public Gebruiker getGebruiker(string id)
        {
            Gebruiker gebruiker = platformRepository.getGebruiker(id);
            return gebruiker;
        }

        public Gebruiker getGebruikerByEmail(string email)
        {
            return platformRepository.getGebruikerMetEmail(email);
        }

        public void createGebruiker(string id, string name, string email)
        {
            Gebruiker gebruiker = new Gebruiker()
            {
                Naam = name,
                GebruikerId = id,
                Email = email
            };
            platformRepository.createGebruiker(gebruiker);
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
    }
}

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
        }

        public PlatformManager(UnitOfWorkManager uowManager)
        {
            this.uowManager = uowManager;
            platformRepository = new PlatformRepository_EF(uowManager.UnitOfWork);
        }

        public void initNonExistingRepo(bool createWithUnitOfWork = false)
        {

            if (platformRepository == null)
            {
                if (createWithUnitOfWork)
                {
                    if (uowManager == null)
                    {
                        uowManager = new UnitOfWorkManager();
                    }
                    platformRepository= new PlatformRepository_EF(uowManager.UnitOfWork);
                }
                else
                {
                    platformRepository = new PlatformRepository_EF();
                }
            }
        }
        #endregion

        #region Platform
        public TimeSpan getHistoriek()
        {
            initNonExistingRepo();

            return platformRepository.getPlatform().Historiek;
        }
        #endregion

        #region Deelplatform
        public DeelplatformDashboard getDeelplatformDashboard(string deelplatform)
        {
            initNonExistingRepo();
            DeelplatformDashboard dpd = platformRepository.getDeelplatformDashboard(deelplatform);
            return dpd;
        }

        public Deelplatform getDeelplatformByNaam(string deelplatformNaam)
        {
            initNonExistingRepo();

            return platformRepository.getDeelPlatform(deelplatformNaam);
        }

        public List<Deelplatform> getAllDeeplatformen()
        {
            initNonExistingRepo();

            return platformRepository.getAllDeelplatformen().ToList();
        }

        public void createDeelplatform(Deelplatform deelplatform)
        {
            initNonExistingRepo();

            platformRepository.createDeelplatform(deelplatform);
        }

        public void deleteDeelplatform(Deelplatform deelplatform)
        {
            initNonExistingRepo();

            platformRepository.deleteDeelplatform(deelplatform);

        }

        public Deelplatform getDeelplatformById(int id)
        {
            initNonExistingRepo();

           return platformRepository.getDeelPlatformById(id);
        }

        #endregion

        #region Gebruiker
        public Gebruiker getGebruiker(string id)
        {
            initNonExistingRepo();

            Gebruiker gebruiker = platformRepository.getGebruiker(id);
            return gebruiker;
        }

        public Gebruiker getGebruikerByEmail(string email)
        {
            initNonExistingRepo();

            return platformRepository.getGebruikerMetEmail(email);
        }

        public void createGebruiker(Gebruiker gebruiker)
        {
            initNonExistingRepo();

            platformRepository.createGebruiker(gebruiker);
        }

        public void deleteGebruiker(string id)
        {
            initNonExistingRepo();

            platformRepository.deleteGebruiker(id);
        }

        public List<Gebruiker> getAllGebruikers()
        {
            initNonExistingRepo();

            return platformRepository.getAllGebruikers();
        }

        public void updateGebruiker(Gebruiker gebruiker)
        {
            initNonExistingRepo();

            platformRepository.saveGebruiker(gebruiker);
        }
        #endregion
    }
}

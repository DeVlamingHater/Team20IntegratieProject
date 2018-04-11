using BL.Interfaces;
using DAL;
using DAL.Repositories_EF;
using Domain.Platformen;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Managers
{
    public class PlatformManager : Interfaces.IPlatformManager
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
        public Gebruiker getGebruiker(int gebruikerId)
    {
      Gebruiker gebruiker = platformRepository.getGebruiker(gebruikerId);
      return gebruiker;
    }
    }
}

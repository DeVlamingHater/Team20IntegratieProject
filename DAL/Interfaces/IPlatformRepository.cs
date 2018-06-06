using Domain.Platformen;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public interface IPlatformRepository
    { 
        #region Platform
        Platform getPlatform();
        DeelplatformDashboard getDeelplatformDashboard(string deelplatform);
        #endregion

        #region Deelplatform
        Deelplatform getDeelPlatform(string deelplatform);
        IEnumerable<Deelplatform> getAllDeelplatformen();
        void createDeelplatform(Deelplatform deelplatform);
        void deleteDeelplatform(Deelplatform deelplatform);
        Deelplatform getDeelPlatformById(int id);

        #endregion

        #region Gebruiker
        void createGebruiker(Gebruiker gebruiker);
        void deleteGebruiker(string id);
        List<Gebruiker> getAllGebruikers();
        Gebruiker getGebruiker(string id);
        Gebruiker getGebruikerMetEmail(string email);
        void saveGebruiker(Gebruiker gebruiker);
        
        #endregion

    }
}

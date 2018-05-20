using Domain.Platformen;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Interfaces
{
    public interface IPlatformManager
    {
        #region Platform
        TimeSpan getHistoriek();
        #endregion

        #region Deelplatform
        Deelplatform getDeelplatformByNaam(string deelplatformNaam);
        List<Deelplatform> getAllDeeplatformen();
        #endregion

        #region Gebruiker
        Gebruiker getGebruiker(string id);
        Gebruiker getGebruikerByEmail(string email);
        void createGebruiker(string id, string name, string email);
        void deleteGebruiker(string id);
        List<Gebruiker> getAllGebruikers();
        void updateGebruiker(Gebruiker gebruiker);
        #endregion
    }
}

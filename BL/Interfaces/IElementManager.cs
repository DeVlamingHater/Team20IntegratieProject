using System;
using System.Collections.Generic;
using System.Text;
using Domain;
using Domain.Dashboards;
using Domain.Platformen;

namespace BL.Interfaces
{
    public interface IElementManager
    {
        #region Element        
        List<Element> getAllElementen(Deelplatform deelplatform);
        Element getElementByNaam(string naam,Deelplatform deelplatform);
        void setTrendingElementen(Deelplatform deelplatform);
        List<Element> getTrendingElementen(int amount, Deelplatform deelplatform);
        Element getElementById(int id);
        void addElementen(List<Element> elementen);
        #endregion

        #region Persoon
        List<Persoon> getAllPersonen(Deelplatform deelplatform);
        void addPersonen(List<Persoon> personen);
        List<Persoon> readJSONPolitici();
        void deleteAllPersonen(Deelplatform deelplatform);
        void addPersoon(Persoon persoon);
        void deletePersoon(Persoon persoon);
        #endregion

        #region Thema
        List<Thema> getAllThemas(Deelplatform deelplatform);
        #endregion

        #region Organisatie
        void addOrganisatie(Organisatie organisatie);
        void addThema(Thema thema);
        void updateThema(Thema thema);
        void deleteThema(Thema thema);
        List<Organisatie> getAllOrganisaties(Deelplatform deelplatform);
        void updateOrganisatie(Organisatie organisatie);
        void deleteOrganisatie(Organisatie organisatie);
        void updatePersoon(Persoon persoon);
        #endregion
    }
}

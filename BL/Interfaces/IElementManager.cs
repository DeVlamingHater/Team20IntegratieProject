using System;
using System.Collections.Generic;
using System.Text;
using Domain;
using Domain.Dashboards;

namespace BL.Interfaces
{
    public interface IElementManager
    {
        #region Element        
        List<Element> getAllElementen();
        Element getElementByNaam(string naam);
        void setTrendingElementen();
        List<Element> getTrendingElementen(int amount);
        Element getElementById(int id);
        void addElementen(List<Element> elementen);
        #endregion

        #region Persoon
        List<Persoon> getAllPersonen();
        void addPersonen(List<Persoon> personen);
        List<Persoon> readJSONPersonen();
        void deleteAllPersonen();
        void addPersoon(Persoon persoon);
        void deletePersoon(Persoon persoon);
        #endregion

        #region Thema
        List<Thema> getAllThemas();
        #endregion

        #region Organisatie
        void addOrganisatie(Organisatie organisatie);
        #endregion
    }
}

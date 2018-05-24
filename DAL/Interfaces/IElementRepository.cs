using Domain;
using Domain.Platformen;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public interface IElementRepository
    {
        Element getElementByID(int elementId);
        Element getElementByName(string naam, Deelplatform deelplatform);
        IEnumerable<Element> getAllElementen(Deelplatform deelplatform);

        void setElement(Element element);

        List<Element> getTrendingElementen(int amount, Deelplatform deelplatform);
        IEnumerable<Persoon> getAllPersonen(Deelplatform deelplatform);

        void AddPersoon(Persoon persoon);
        void addElementen(List<Element> elementen);
        void addOrganisatie(Organisatie organisatie);
        List<Thema> getAllThemas(Deelplatform deelplatform);
        void deleteAllPersonen(Deelplatform deelplatform);
        void deletePersoon(Persoon persoon);
        void addThema(Thema thema);
        void updateThema(Thema thema);
        void deleteThema(Thema thema);
        List<Organisatie> getAllOrganisaties(Deelplatform deelplatform);
        void updateOrganisatie(Organisatie organisatie);
        void deleteOrganisatie(Organisatie organisatie);
        void updatePersoon(Persoon persoon);
    }

}

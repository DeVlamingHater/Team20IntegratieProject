using System;
using System.Collections.Generic;
using System.Text;
using Domain;

namespace BL.Interfaces
{
    public interface IElementManager
    {
        Element getElementByNaam(string naam);

        List<Element> getAllElementen();

        List<Element> getTrendingElementen(int amount);

        void setTrendingElementen();
        void addElementen(List<Element> elementen);
        void addOrganisatie(Organisatie organisatie);
    }
}

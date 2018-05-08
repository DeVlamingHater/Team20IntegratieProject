﻿using System;
using System.Collections.Generic;
using System.Text;
using Domain;
using Domain.Dashboards;

namespace BL.Interfaces
{
    public interface IElementManager
    {
        Element getElementByNaam(string naam);
        Element getElementById(int id);

        List<Element> getAllElementen();

        List<Element> getTrendingElementen(int amount);

        void setTrendingElementen();
        void addElementen(List<Element> elementen);
        void addOrganisatie(Organisatie organisatie);
        void addPersonen(List<Persoon> personen);

        List<Persoon> readJSONPersonen();
        void deleteAllPersonen();
    }
}

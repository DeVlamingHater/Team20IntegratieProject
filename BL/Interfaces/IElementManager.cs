using System;
using System.Collections.Generic;
using System.Text;
using Domain;

namespace BL.Interfaces
{
    public interface IElementManager
    {
        Element getElementByNaam(string naam);
    Element getElementById(int id);

        List<Element> getAllElementen();

        List<Element> getTrendingElementen(int amount);

        void setTrendingElementen();


    }
}

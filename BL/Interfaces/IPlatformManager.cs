using Domain.Platformen;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Interfaces
{
    public interface IPlatformManager
    {
        void createGebruiker(string id, string name, string email);
        Gebruiker getGebruiker(string id);
        void deleteGebruiker(string id);
        List<Gebruiker> getAllGebruikers();
        void saveGebruiker(Gebruiker gebruiker);
    }
}

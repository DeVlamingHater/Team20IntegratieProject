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
        Gebruiker getGebruikerMetEmail(string email);
        void deleteGebruiker(string id);
        List<Gebruiker> getAllGebruikers();
        void updateGebruiker(Gebruiker gebruiker);
    }
}

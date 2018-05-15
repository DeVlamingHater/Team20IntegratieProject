using Domain.Platformen;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Interfaces
{
    public interface IPlatformManager
    {
        void createGebruiker(string id, string name, string email);
        Gebruiker getGebruiker(string email);
        void deleteGebruiker(string id);
    }
}

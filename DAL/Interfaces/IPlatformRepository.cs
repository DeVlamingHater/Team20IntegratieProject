using Domain.Platformen;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public interface IPlatformRepository
    {
        Gebruiker getGebruiker(string email);
        void createGebruiker(string id, string name, string email);
    }
}

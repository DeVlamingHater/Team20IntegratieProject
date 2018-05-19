﻿using Domain.Platformen;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public interface IPlatformRepository
    {
        Gebruiker getGebruiker(string id);
        void createGebruiker(string id, string name, string email);
        void deleteGebruiker(string id);
        List<Gebruiker> getAllGebruikers();
        void saveGebruiker(Gebruiker gebruiker);
        Gebruiker getGebruikerMetEmail(string email);
        Deelplatform getDeelPlatform(string deelplatform);
    }
}

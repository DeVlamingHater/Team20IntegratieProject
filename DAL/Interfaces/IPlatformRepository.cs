using Domain.Platformen;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public interface IPlatformRepository
    {
    Gebruiker getGebruiker(int gebruikerId);
    }
}

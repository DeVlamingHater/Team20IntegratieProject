using Domain.Dashboards;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Platformen
{
    public class Deelplatform
    {
        [Key]
        public int Id { get; set; }

        public string Naam { get; set; }

        public TimeSpan Historiek  { get; set; }

        public List<Gebruiker> Admins { get; set; }

        public string PersoonString { get; set; }

        public string OrganisatieString { get; set; }

        public string ThemaString { get; set; }

        public string PersonenString { get; set; }

        public string OrganisatiesString { get; set; }

        public string ThemasString { get; set; }

        public byte[] BannerImage { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Entity;
using Domain;
using Domain.Dashboards;
using System.Linq;
using Domain.Elementen;
using Domain.Platformen;

namespace DAL.EF
{

    class PolitiekeBarometerInitializer : DropCreateDatabaseAlways<PolitiekeBarometerContext>
    {
        protected override void Seed(PolitiekeBarometerContext context)
        {
            #region Platformen
            Platform platform1 = new Platform()
            {
                Historiek = new TimeSpan(7, 0, 0)
            };
            #endregion

            #region Deelplatformen
            Deelplatform deelplatform1 = new Deelplatform()
            {
                Naam = "Politieke Barometer",
                Historiek = new TimeSpan(7, 0, 0)
            };
            #endregion

            #region Gebruikers

            Gebruiker gebruiker1 = new Gebruiker()
            {
                Email = "sam.claessen@student.kdg.be",
                Naam = "Sam Claessen",
                GebruikerId = "1"
            };

           
            #endregion

            #region Dashboard

            Dashboard dashboard1 = new Dashboard()
            {
                DashboardId = 1,
                Gebruiker = gebruiker1
            };

           
            #endregion

            #region Zone
            Zone zone1 = new Zone()
            {
                Naam = "Zone1",
                Dashboard = dashboard1
            };

            Zone zone2 = new Zone()
            {
                Naam = "Zone2",
                Dashboard = dashboard1
            };

            Zone zone3 = new Zone()
            {
                Naam = "Zone3",
                Dashboard = dashboard1
            };
            #endregion

            #region Item
            Item item1 = new Item()
            {
                Zone = zone1,
                X = 1,
                Y = 1,
                Size = 1
            };
            Item item2 = new Item()
            {
                Zone = zone1,
                X = 1,
                Y = 1,
                Size = 1
            };
            Item item3 = new Item()
            {
                Zone = zone2,
                X = 1,
                Y = 1,
                Size = 1
            };
            Item item4 = new Item()
            {
                Zone = zone2,
                X = 1,
                Y = 1,
                Size = 1
            };
            Item item5 = new Item()
            {
                Zone = zone3,
                X = 1,
                Y = 1,
                Size = 1
            };
            #endregion

            #region Keywords
            Keyword keyword1 = new Keyword()
            {
                KeywordNaam = "moslimouders",
                Themas = new List<Thema>()
            };
            #endregion

            #region Themas
            Thema thema1 = new Thema()
            {
                Naam = "Cultuur",
                Keywords = new List<Keyword>()
            };
            thema1.Keywords.Add(keyword1);
            keyword1.Themas.Add(thema1);
            #endregion

            #region AddToDB

            #region AddPlatform
            
            #region AddPlatformen
            context.Platformen.Add(platform1);
            #endregion

            #region AddDeelplatformen
            context.Deelplatformen.Add(deelplatform1);
            #endregion

            #region AddGebruikers
            context.Gebruikers.Add(gebruiker1);
            #endregion

            #region addDashboards
            context.Dashboards.Add(dashboard1);
            #endregion

            #region AddZones
            context.Zones.Add(zone1);
            context.Zones.Add(zone2);
            context.Zones.Add(zone3);
            #endregion

            #region AddItems
            context.Items.Add(item1);
            context.Items.Add(item2);
            context.Items.Add(item3);
            context.Items.Add(item4);
            context.Items.Add(item5);
            #endregion
            #endregion

            #region AddDashboard

            #endregion
           
            #region AddElementen

            #region AddKeywords
            context.Keywords.Add(keyword1);
            #endregion

            #region AddThemas
            context.Themas.Add(thema1);
            #endregion
            #endregion
            #endregion


            context.SaveChanges();
            base.Seed(context);
        }
    }
}

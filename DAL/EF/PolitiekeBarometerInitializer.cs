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

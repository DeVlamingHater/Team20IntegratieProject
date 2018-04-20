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
  class PolitiekeBarometerInitializer : DropCreateDatabaseIfModelChanges<PolitiekeBarometerContext>
  {
    protected override void Seed(PolitiekeBarometerContext context)
    {
      #region Gebruikers

      Gebruiker gebruiker1 = new Gebruiker()
      {
        Email = "sam.claessen@student.kdg.be",
        Naam = "Sam Claessen",
        GebruikerId = 1,
        Wachtwoord = "wachtwoord"
      };

      Gebruiker gebruiker2 = new Domain.Platformen.Gebruiker
      {
        Email = "thomas.somers@student.kdg.be",
        Naam = "Thomas Somers",
        GebruikerId = 2,
        Wachtwoord = "123"
      };
      #endregion

      #region Dashboard

      Dashboard dashboard1 = new Dashboard()
      {
        DashboardId = 1,
        Gebruiker = gebruiker1
      };

      Dashboard dashboard2 = new Dashboard()
      {
        DashboardId = 2,
        Gebruiker = gebruiker2
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

      #region Organisatie
      Organisatie organisatie1 = new Organisatie()
      {
        Naam = "NVA",
        Personen = new List<Persoon>()
      };
      Organisatie organisatie2 = new Organisatie()
      {
        Naam = "GROEN",
        Personen = new List<Persoon>()
      };
      #endregion{

      #region Personen
      Persoon persoon1 = new Persoon()
      {
        Naam = "Imade Annouri",
        Organisatie = organisatie1
      };

      Persoon persoon2 = new Persoon()
      {
        Naam = "Caroline Bastiaens",
        Organisatie = organisatie2
      };
      Persoon persoon3 = new Persoon()
      {
        Naam = "Vera Celis",
        Organisatie = organisatie2
      };

      organisatie1.Personen.Add(persoon1);
      organisatie2.Personen.Add(persoon2);
      organisatie2.Personen.Add(persoon3);
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

      #region DataConfigs
      DataConfig dataConfig1 = new DataConfig
      {
        DataConfiguratieId = 0,
        DataType = DataType.TOTAAL,
        Elementen = new List<Element>()
      };

      dataConfig1.Elementen.Add(persoon1);

      DataConfig dataConfig2 = new DataConfig
      {
        DataType = DataType.TOTAAL,
        Elementen = new List<Element>()
      };

      dataConfig2.Elementen.Add(organisatie2);

      DataConfig dataConfig3 = new DataConfig
      {
        DataType = DataType.TOTAAL,
        Elementen = new List<Element>()
      };

      dataConfig3.Elementen.Add(thema1);

      DataConfig dataConfig4 = new DataConfig
      {
        DataType = DataType.TREND,
        Elementen = new List<Element>()
      };

      dataConfig4.Elementen.Add(persoon1);

      #endregion

      #region Alerts
      Alert alert1 = new Alert
      {
        ApplicatieMelding = true,
        BrowserMelding = true,
        EmailMelding = true,
        Waarde = 50,
        Operator = "<",
        Status = AlertStatus.ACTIEF,
        DataConfig = dataConfig1,
        Dashboard = dashboard1
      };

      Alert alert2 = new Alert
      {
        ApplicatieMelding = true,
        BrowserMelding = true,
        EmailMelding = true,
        Waarde = 50,
        Operator = "<",
        Status = AlertStatus.ACTIEF,
        DataConfig = dataConfig2,
        Dashboard = dashboard1
      };
      Alert alert3 = new Alert
      {
        ApplicatieMelding = false,
        BrowserMelding = true,
        EmailMelding = false,
        Waarde = 50,
        Operator = "<",
        Status = AlertStatus.ACTIEF,
        DataConfig = dataConfig3,
        Dashboard = dashboard2
      };

      Alert alert4 = new Alert
      {
        ApplicatieMelding = false,
        BrowserMelding = false,
        EmailMelding = true,
        Waarde = -4,
        Operator = ">",
        Status = AlertStatus.ACTIEF,
        DataConfig = dataConfig4,
        Dashboard = dashboard2
      };
      Alert alert5 = new Alert
      {
        ApplicatieMelding = true,
        BrowserMelding = false,
        EmailMelding = false,
        Waarde = 50,
        Operator = "<",
        Status = AlertStatus.INACTIEF,
        DataConfig = dataConfig1,
        Dashboard = dashboard2
      };
      #endregion

      #region AddToDB

      #region AddPlatformen
      #region AddGebruikers
      context.Gebruikers.Add(gebruiker1);
      context.Gebruikers.Add(gebruiker2);
      #endregion
      #region addDashboards
      context.Dashboards.Add(dashboard1);
      context.Dashboards.Add(dashboard2);
      #endregion
      #region AddZones
      context.Zones.Add(zone1);
      context.Zones.Add(zone2);
      context.Zones.Add(zone3);
      #endregion
      #endregion

      #region AddDataConfigs
      context.DataConfigs.Add(dataConfig1);
      context.DataConfigs.Add(dataConfig2);
      context.DataConfigs.Add(dataConfig3);
      context.DataConfigs.Add(dataConfig4);
      #endregion

      #region AddAlerts
      context.Alerts.Add(alert1);
      context.Alerts.Add(alert2);
      context.Alerts.Add(alert3);
      context.Alerts.Add(alert4);
      context.Alerts.Add(alert5);
      #endregion
      #endregion

      #region AddElementen

      #region AddKeywords
      context.Keywords.Add(keyword1);
      #endregion

      #region AddOrganisaties
      context.Organisaties.Add(organisatie1);
      context.Organisaties.Add(organisatie2);
      #endregion

      #region AddPersonen
      context.Personen.Add(persoon1);
      context.Personen.Add(persoon2);
      context.Personen.Add(persoon3);
      #endregion

      #region AddThemas
      context.Themas.Add(thema1);
      #endregion
      #endregion
      context.SaveChanges();
      base.Seed(context);
    }
  }
}

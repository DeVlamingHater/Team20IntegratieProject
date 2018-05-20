using Domain;
using Domain.Dashboards;
using Domain.Platformen;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace BL.Interfaces
{
    public interface IDashboardManager
    {
        #region Dashboard
        Dashboard getDashboard(string gebruikersNaam);
        #endregion

        #region Zone
        IEnumerable<Zone> getZones(Dashboard dashboard);
        Zone getZone(int zoneId);
        void deleteZone(int zoneId);
        Zone addZone(Dashboard dashboard);
        void updateZone(Zone zone);
        #endregion

        #region Item
        IEnumerable<Item> getItems(int actieveZone);
        Item getItem(int itemId);
        #endregion

        #region Grafiek
        string getGraphData(Grafiek grafiek);
        Grafiek createGrafiek(GrafiekType grafiekType, Domain.DataType dataType, int aantalDataPoints, TimeSpan Tijdschaal, int zoneId, List<Filter> filters, List<DataConfig> dataConfigs);
        #endregion

        #region Alert
        Alert getAlert(int alertId);
        Gebruiker getAlertGebruiker(Alert alert);
        List<Alert> getActiveAlerts();
        DataConfig getAlertDataConfig(Alert alert);
        List<Alert> getAllAlerts();
        void sendAlerts();
        void createAlert(Alert testAlert);
        IEnumerable<Alert> getDashboardAlerts(Dashboard testDashboard);
        #endregion

        #region Melding
        IEnumerable<Melding> getActiveMeldingen(Dashboard dashboard);
        Melding createMelding(Alert alert, double waarde);
        #endregion
    }
}

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
        Dashboard getDashboard(string gebruikersNaam, Deelplatform deelplatform);
        #endregion

        #region Zone
        IEnumerable<Zone> getZones(Dashboard dashboard);
        Zone getZone(int zoneId);
        void deleteZone(int zoneId);
        Zone addZone(Dashboard dashboard);
        void changeZoneName(int zoneId, string zoneNaam);
        void updateZone(Zone zone);
        #endregion

        #region Item
        IEnumerable<Item> getItems(int actieveZone);
        Item getItem(int itemId);
        #endregion

        #region Grafiek
        List<Dictionary<string, double>> getGraphData(Grafiek grafiek);
        Grafiek createGrafiek(Grafiek grafiek);
        IEnumerable<Grafiek> getGrafieken(int actieveZone);
        Grafiek getGrafiek(int itemId);

        #endregion

        #region Alert
        Alert getAlert(int alertId);
        Gebruiker getAlertGebruiker(Alert alert);
        List<Alert> getActiveAlerts(Dashboard dashboard);
        List<Alert> getAllDashboardAlerts(Dashboard dashboard);
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

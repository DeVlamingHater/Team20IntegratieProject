using Domain;
using Domain.Dashboards;
using Domain.Platformen;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public interface IDashboardRepository
    {
        #region Dashboard
        Dashboard getDashboard(string gebruikersNaam);
        #endregion

        #region Zone
        IEnumerable<Zone> getDashboardZones(int dashboardId);
        Zone getZone(int zoneId);
        Zone addZone(Zone zone);
        void changeZoneName(int zoneId,string zoneNaam);
        void UpdateZone(Zone zone);
        void deleteZone(int zoneId);
        #endregion

        #region Item
        IEnumerable<Item> getItems(int actieveZone);
        Item getItem(int itemId);
        #endregion

        #region Grafiek
        IEnumerable<Grafiek> getGrafieken(int actieveZone);
        void addGrafiek(Grafiek grafiek);
        #endregion

        #region Alert
        IEnumerable<Alert> getActiveAlerts();
        DataConfig getAlertDataConfig(Alert alert);
        IEnumerable<Alert> getAllAlerts();
        void createAlert(Alert alert);
        IEnumerable<Alert> getDashboardAlerts(Dashboard dashboard);
        Alert getAlert(int id);
        #endregion

        #region Melding
        void addMelding(Melding melding);
        IEnumerable<Melding> getActiveMeldingen(Dashboard dashboard);
        Grafiek getGrafiek(int itemId);
        #endregion
    }
}
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

        IEnumerable<Alert> getActiveAlerts();

        DataConfig getAlertDataConfig(Alert alert);
        IEnumerable<Alert> getAllAlerts();
        IEnumerable<Zone> getZones(int dashboardId);
        Zone getZone(int zoneId);
        Zone addZone(Zone zone);
        Dashboard getDashboard(string gebruikersNaam);
        void UpdateZone(Zone zone);
        void deleteZone(int zoneId);
        IEnumerable<Item> getItems(int actieveZone);
        Platform getPlatform();
    }
}
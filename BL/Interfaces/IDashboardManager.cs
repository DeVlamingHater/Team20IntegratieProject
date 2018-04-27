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
        Gebruiker getAlertGebruiker(Alert alert);
        List<Alert> getActiveAlerts();
        DataConfig getAlertDataConfig(Alert alert);
        List<Alert> getAllAlerts();
        void sendAlerts();
        Dashboard getDashboard(int gebruikerId);
        void changeZone(Zone zone);
        IEnumerable<Item> getItems(int actieveZone);
        IEnumerable<Zone> getZones(Dashboard dashboard);
        Zone getZone(int zoneId);
        void deleteZone(int zoneId);
        Zone addZone();
        TimeSpan getHistoriek();
    }
}

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
        Dashboard getDashboard(string gebruikersNaam);

        IEnumerable<Item> getItems(int actieveZone);

        IEnumerable<Zone> getZones(Dashboard dashboard);
        Zone getZone(int zoneId);
        void deleteZone(int zoneId);
        Zone addZone();
        void changeZone(Zone zone);

        TimeSpan getHistoriek();
        string getGraphData(Grafiek grafiek);

        string getGrafiekData(Grafiek grafiek);
        List<Post> filterPosts(List<Post> posts, List<Filter> filters);
        Grafiek createGrafiek(Domain.DataType dataType, int aantalDataPoints, TimeSpan Tijdschaal, int zoneId, List<Filter> filters, List<DataConfig> dataConfigs);
     
    }
}

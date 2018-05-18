﻿using Domain;
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
        void addAlert(Alert testAlert);
        IEnumerable<Alert> getDashboardAlerts(Dashboard testDashboard);

        IEnumerable<Melding> getActiveMeldingen(Dashboard dashboard);

        Dashboard getDashboard(string email);

        IEnumerable<Item> getItems(int actieveZone);

        IEnumerable<Zone> getZones(Dashboard dashboard);
        Zone getZone(int zoneId);
    Zone getZoneByNaam(string zoneNaam);
        void deleteZone(int zoneId);
        Zone addZone(Dashboard dashboard);
        Alert getAlert(int id);
        void updateZone(Zone zone);

        TimeSpan getHistoriek();
        string getGraphData(Grafiek grafiek);
        Grafiek createGrafiek(GrafiekType grafiekType, Domain.DataType dataType, int aantalDataPoints, TimeSpan Tijdschaal, int zoneId, List<Filter> filters, List<DataConfig> dataConfigs);
        
        Melding createMelding(Alert alert, double waarde);

    }
}

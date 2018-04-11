using Domain;
using Domain.Dashboards;
using Domain.Platformen;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
  public interface DashboardRepository
  {

    IEnumerable<Alert> getActiveAlerts();

    DataConfig getAlertDataConfig(Alert alert);
    IEnumerable<Alert> getAllAlerts();
    Dashboard getDashboard(Gebruiker gebruiker);
    IEnumerable<Zone> getZones(int dashboardId);
    Zone getZone(int zoneId);
  }
}
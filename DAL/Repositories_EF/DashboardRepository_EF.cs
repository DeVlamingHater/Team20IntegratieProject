using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using DAL.EF;
using Domain;
using Domain.Dashboards;
using Domain.Platformen;

namespace DAL.Repositories_EF
{
    public class DashboardRepository_EF : IDashboardRepository
    {
        PolitiekeBarometerContext context;

        public DashboardRepository_EF()
        {
            context = new PolitiekeBarometerContext();
        }

        public DashboardRepository_EF(UnitOfWork unitOfWork)
        {
            context = unitOfWork.Context;
        }

        public IEnumerable<Alert> getAllAlerts()
        {
            return  context.Alerts.Include(a => a.DataConfig).ToList();
        }

        public IEnumerable<Alert> getActiveAlerts()
        {
            return context.Alerts.Include(a => a.DataConfig.Elementen).Where<Alert>(a=>a.Status ==AlertStatus.ACTIEF).ToList<Alert>();
        }

        public DataConfig getAlertDataConfig(Alert alert)
        {
            return context.Alerts.Include(a => a.DataConfig.Elementen).Single<Alert>(a => a.AlertId == alert.AlertId).DataConfig;
        }

    public Dashboard getDashboard(int gebruikerId)
    {
      Dashboard dashboard = context.Dashboards.Include(db=>db.Gebruiker).Single(r => r.Gebruiker.GebruikerId == gebruikerId);
      return dashboard;
    }

    public IEnumerable<Zone> getZones(int dashboardId)
    {
      return context.Zones.Where(r => r.Dashboard.DashboardId == dashboardId).AsEnumerable();
    }

    public Zone getZone(int zoneId)
    {
      throw new NotImplementedException();
    }

    public Zone addZone(Zone zone)
    {
      context.Zones.Add(zone);
      context.SaveChanges();
      return zone;
    }
    public void UpdateZone(Zone zone)
    { 
      // dit staat in supportcenter????:
      // Do nothing! All data lives in memory, everything references the same objects!!
    }
  }
}

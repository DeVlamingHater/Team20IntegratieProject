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
using System.ComponentModel.DataAnnotations;

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
            return context.Alerts.Include(a => a.DataConfig).ToList();
        }

        public IEnumerable<Alert> getActiveAlerts()
        {
            return context.Alerts.Include(a => a.DataConfig.Elementen).Where<Alert>(a => a.Status == AlertStatus.ACTIEF).ToList<Alert>();
        }

        public DataConfig getAlertDataConfig(Alert alert)
        {
            return context.Alerts.Include(a => a.DataConfig.Elementen).Single<Alert>(a => a.AlertId == alert.AlertId).DataConfig;
        }

        public Dashboard getDashboard(int gebruikerId)
        {
            Dashboard dashboard = context.Dashboards.Include(db => db.Gebruiker).Single(r => r.Gebruiker.GebruikerId == gebruikerId);
            return dashboard;
        }

        public IEnumerable<Zone> getZones(int dashboardId)
        {
            return context.Zones.Where(r => r.Dashboard.DashboardId == dashboardId).AsEnumerable();
        }

        public Zone getZone(int zoneId)
        {
            return context.Zones.Find(zoneId);
        }


        public Zone addZone(Zone zone)
        {
            context.Zones.Add(zone);
            context.SaveChanges();
            return zone;
        }
        public void UpdateZone(Zone zone)
        {
            List<ValidationResult> errors = new List<ValidationResult>();
            bool valid = Validator.TryValidateObject(zone, new ValidationContext(zone), errors, true);
            if (valid)
            {
                context.Entry(zone).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
        public void deleteZone(int zoneId)
        {
            Zone zone = getZone(zoneId);
            context.Zones.Remove(zone);
            for ( int i = 0; i < items.Count(); i++ )
      {
        Item item = items.ElementAt(i);
        context.Items.Remove(item);
      };
            context.SaveChanges();
        }

        public IEnumerable<Item> getItems(int actieveZone)
        {
            return context.Items.Where(r => r.Zone.Id == actieveZone).AsEnumerable();
        }

        public Platform getPlatform()
        {
            return context.Platformen.First();
        }
    }
}

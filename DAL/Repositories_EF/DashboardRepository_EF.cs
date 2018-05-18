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
            return context.Alerts.Include(a => a.DataConfig.Element).Where<Alert>(a => a.Status == AlertStatus.ACTIEF).ToList<Alert>();
        }

        public DataConfig getAlertDataConfig(Alert alert)
        {
            return context.Alerts.Include(a => a.DataConfig.Element).Single<Alert>(a => a.AlertId == alert.AlertId).DataConfig;
        }

        public Dashboard getDashboard(string email)
        {
            Dashboard dashboard = null;

            Gebruiker gebruiker = context.Gebruikers.First(g => g.Email == email);
            List<Dashboard> dashboards = context.Dashboards.Include<Dashboard>("Zones").Where(d => d.Gebruiker.Email == gebruiker.Email).ToList();
            if (dashboards.Count != 0)
            {
                dashboard = dashboards.First();

            }
            if (dashboard == null)
            {

                dashboard = new Dashboard()
                {
                    Gebruiker = gebruiker,
                    Zones = new List<Zone>()
                    {
                        new Zone()
                        {
                            Dashboard = dashboard,
                            Items = new List<Item>(),
                            Naam = "Zone 1"
                        }
                    }
                };
                context.Dashboards.Add(dashboard);
                context.SaveChanges();
            }
            return dashboard;

        }


        public IEnumerable<Zone> getZones(int dashboardId)
        {
            return context.Zones.Where(r => r.Dashboard.DashboardId == dashboardId).AsEnumerable();
        }

        public Zone getZone(int zoneId)
        {
            return context.Zones.Include(z => z.Dashboard).First(z => z.Id == zoneId);
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
            IEnumerable<Item> items = getItems(zoneId);
            for (int i = 0; i < items.Count(); i++)
            {
                Item item = items.ElementAt(i);
                context.Items.Remove(item);
            };
            context.SaveChanges();
        }

        public IEnumerable<Item> getItems(int actieveZone)
        {
            return context.Items.Where(r => r.Zone.Id == actieveZone);
        }

        public Platform getPlatform()
        {
            return context.Platformen.First();
        }

        public void addGrafiek(Grafiek grafiek)
        {
            List<ValidationResult> errors = new List<ValidationResult>();
            bool valid = Validator.TryValidateObject(grafiek, new ValidationContext(grafiek), errors, true);
            if (valid)
            {
                grafiek.Filters = new List<Filter>();
                context.Grafieken.Add(grafiek);
                context.SaveChanges();
            }

        }

        public void addMelding(Melding melding)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Melding> getActiveMeldingen(Dashboard dashboard)
        {
            return context.Meldingen.Where(m => m.Alert.Dashboard.DashboardId == dashboard.DashboardId);
        }

        public void addAlert(Alert alert)
        {
            List<ValidationResult> errors = new List<ValidationResult>();
            bool valid = Validator.TryValidateObject(alert, new ValidationContext(alert), errors, true);
            if (valid)
            {
                context.Entry(alert).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public IEnumerable<Alert> getDashboardAlerts(Dashboard dashboard)
        {
            return context.Alerts.Include(a => a.Dashboard).Where(d => d.Dashboard.DashboardId == dashboard.DashboardId);
        }

        public Zone getZoneByNaam(string zoneNaam)
        {
            return context.Zones.Include(z => z.Dashboard).First(z => z.Naam == zoneNaam);
        }

        public Alert getAlert(int id)
        {
            return (Alert)context.Alerts.Where(a => a.AlertId == id);
        }
    }
}

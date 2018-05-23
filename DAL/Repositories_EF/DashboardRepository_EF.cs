﻿using System;
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
        #region Constructor
        PolitiekeBarometerContext context;

        public DashboardRepository_EF()
        {
            context = new PolitiekeBarometerContext();
        }

        public DashboardRepository_EF(UnitOfWork unitOfWork)
        {
            context = unitOfWork.Context;
        }
        #endregion

        #region Dashboard
        public Dashboard getDashboard(string gebruikersNaam)
        {
            Dashboard dashboard = null;

            Gebruiker gebruiker = context.Gebruikers.First(g => g.Email == gebruikersNaam);
            List<Dashboard> dashboards = context.Dashboards.Include<Dashboard>("Zones").Where(d => d.Gebruiker.Email == gebruiker.Email).ToList();
            if (dashboards.Count != 0)
            {
                dashboard = dashboards.First();

            }
            if (dashboard == null)
            {
                //Eerste keer dat een gebruiker dat een gebruiker inlogd heeft deze nog geen dashboard
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
        #endregion

        #region Zone
        public IEnumerable<Zone> getDashboardZones(int dashboardId)
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
        #endregion

        #region Item
        public IEnumerable<Item> getItems(int actieveZone)
        {
            return context.Items.Where(r => r.Zone.Id == actieveZone);
        }

        public Item getItem(int itemId)
        {
            return context.Items.Find(itemId);
        }
        #endregion

        #region Grafiek

        public IEnumerable<Grafiek> getGrafieken(int actieveZone)
        {
            return context.Grafieken.Where(r => r.Zone.Id == actieveZone);
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
        #endregion

        #region Alert
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

        public void createAlert(Alert alert)
        {
            List<ValidationResult> errors = new List<ValidationResult>();
            bool valid = Validator.TryValidateObject(alert, new ValidationContext(alert), errors, true);
            if (valid)
            {
                context.Alerts.Add(alert);
                context.SaveChanges();
            }
        }

        public IEnumerable<Alert> getDashboardAlerts(Dashboard dashboard)
        {
            return context.Alerts.Include(a => a.Dashboard).Where(d => d.Dashboard.DashboardId == dashboard.DashboardId);
        }

        public Alert getAlert(int id)
        {
            return (Alert)context.Alerts.Where(a => a.AlertId == id).FirstOrDefault();
        }
        #endregion

        #region Melding
        public void addMelding(Melding melding)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Melding> getActiveMeldingen(Dashboard dashboard)
        {
            return context.Meldingen.Where(m => m.Alert.Dashboard.DashboardId == dashboard.DashboardId);
        }
        #endregion
    }
}

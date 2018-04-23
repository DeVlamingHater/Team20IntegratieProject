using BL.Interfaces;
using DAL;
using DAL.Repositories_EF;
using Domain;
using Domain.Dashboards;
using Domain.Platformen;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BL.Managers
{
    public class DashboardManager : IDashboardManager
    {
        IDashboardRepository dashboardRepository;
        public UnitOfWorkManager uowManager;
        public DashboardManager()
        {
      dashboardRepository = new DashboardRepository_EF();
        }

        public DashboardManager(UnitOfWorkManager uowManager)
        {
            this.uowManager = uowManager;
            dashboardRepository = new DashboardRepository_EF(uowManager.UnitOfWork);
        }

    public Dashboard getDashboard(int gebruikerId)
    {
      Dashboard dashboard = dashboardRepository.getDashboard(gebruikerId);
      return dashboard;
    }

    public IEnumerable<Item> getItems(int actieveZone)
    {
      return dashboardRepository.getItems(actieveZone);
    }

    public IEnumerable<Zone> getZones(Dashboard dashboard)
    {
      int dashboardId = dashboard.DashboardId;
      return dashboardRepository.getZones(dashboardId);
    }
    public Zone getZone(int zoneId)
    {
      return dashboardRepository.getZone(zoneId);
    }

    public void deleteZone(int zoneId)
    {
      dashboardRepository.deleteZone(zoneId);
    }

    public Zone addZone()
    {
      // GEBRUIKER VAN DASHBOARD VINDEN NIET JUIST
      Dashboard dashboard = this.getDashboard(1);
      IEnumerable<Zone> zones = this.getZones(dashboard);
      Zone zone = new Zone()
      {
        Id = zones.Count() + 1,
        Naam = "NewZone",
        Dashboard = dashboard
      };
      return dashboardRepository.addZone(zone);
    }

    public void changeZone(Zone zone)
    {
      this.Validate(zone);
      dashboardRepository.UpdateZone(zone);
    }

    private void Validate(Zone zone)
    {
      List<ValidationResult> errors = new List<ValidationResult>();
      bool valid = Validator.TryValidateObject(zone, new ValidationContext(zone), errors, validateAllProperties: true);

      if (!valid)
        throw new ValidationException("Zone not valid!");
    }

    public List<Alert> getActiveAlerts()
        {
            initNonExistingRepo(false);
            return dashboardRepository.getActiveAlerts().ToList();
        }

        public DataConfig getAlertDataConfig(Alert alert)
        {
            initNonExistingRepo(false);
            return alert.DataConfig;
        }

        public Gebruiker getAlertGebruiker(Alert alert)
        {
            initNonExistingRepo(false);
            return alert.Dashboard.Gebruiker;
        }

        public List<Alert> getAllAlerts()
        {
            initNonExistingRepo(false);
            return dashboardRepository.getAllAlerts().ToList();
        }

        public void sendAlerts()
        {
            initNonExistingRepo(true);
            PostManager postManager = new PostManager(uowManager);
            List<Alert> activeAlerts = getActiveAlerts();
            foreach (Alert alert in activeAlerts)
            {
                double waarde = postManager.getHuidigeWaarde(alert.DataConfig);
                bool sendMelding = false;
                switch (alert.Operator)
                {
                    case "<":
                        if (waarde < alert.Waarde)
                        {
                            sendMelding = true;
                        }
                        break;
                    case ">":
                        if (waarde > alert.Waarde)
                        {
                            sendMelding = true;
                        }
                        break;
                    default:
                        break;
                }
                if (sendMelding)
                {
                    Console.WriteLine("MELDING!!!");
                }
            }

        }

        public void initNonExistingRepo(bool createWithUnitOfWork = false)
        {
            // De onderstaande logica is enkel uit te voeren als er nog geen repo bestaat. 
            //Als we een repo met UoW willen gebruiken en als er nog geen uowManager bestaat
            // dan maken we de uowManager aan en gebruiken we de context daaruit om de repo aan te maken.
            if (dashboardRepository == null)
            {
                if (createWithUnitOfWork)
                {
                    if (uowManager == null)
                    {
                        uowManager = new UnitOfWorkManager();
                    }
                    dashboardRepository = new DashboardRepository_EF(uowManager.UnitOfWork);
                }
                // Als we niet met UoW willen werken, dan maken we een repo aan als die nog niet bestaat.
                else
                {
                    dashboardRepository = new DashboardRepository_EF();
                }

            }
        }

    void IDashboardManager.Validate(Zone zone)
    {
      throw new NotImplementedException();
    }
  }
}

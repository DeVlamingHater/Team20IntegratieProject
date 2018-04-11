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

namespace DAL.Repositories_EF
{
    public class DashboardRepository_EF : DashboardRepository
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

    public Dashboard getDashboard(Gebruiker gebruiker)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<Zone> getZones(int dashboardId)
    {
      throw new NotImplementedException();
    }

    public Zone getZone(int zoneId)
    {
      throw new NotImplementedException();
    }
  }
}

using BL.Interfaces;
using DAL;
using DAL.Repositories_EF;
using Domain;
using Domain.Platformen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BL.Managers
{
    public class DashboardManager : IDashboardManager
    {
        IDashboardRepository dashboardRepository;
        public DashboardManager()
        {
            dashboardRepository = new DashboardRepository_EF();
        }

        public List<Alert> getActiveAlerts()
        {
            return dashboardRepository.getActiveAlerts().ToList();
        }

        public DataConfig getAlertDataConfig(Alert alert)
        {
            return alert.DataConfig;
        }

        public Gebruiker getAlertGebruiker(Alert alert)
        {
            return alert.Dashboard.Gebruiker;
        }

        public List<Alert> getAllAlerts()
        {
            return dashboardRepository.getAllAlerts().ToList();
        }
    }
}

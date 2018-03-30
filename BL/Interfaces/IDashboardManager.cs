using Domain;
using Domain.Platformen;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Interfaces
{
    public interface IDashboardManager
    {
        Gebruiker getAlertGebruiker(Alert alert);
        List<Alert> getActiveAlerts();
        DataConfig getAlertDataConfig(Alert alert);
        List<Alert> getAllAlerts();
    }
}

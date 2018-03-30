using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public interface IDashboardRepository
    {

        IEnumerable<Alert> getActiveAlerts();

        DataConfig getAlertDataConfig(Alert alert);
        IEnumerable<Alert> getAllAlerts();
    }
}

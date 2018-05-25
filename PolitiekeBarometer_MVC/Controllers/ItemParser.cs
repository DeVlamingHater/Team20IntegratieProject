using BL.Managers;
using Domain.Dashboards;
using PolitiekeBarometer_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PolitiekeBarometer_MVC.Controllers
{
    public class ItemParser
    {
        public static ItemViewModel ParseItem(Item item, UnitOfWorkManager uowmgr )
        {
            if (item.GetType() == typeof(Grafiek))
            {
                Grafiek grafiek = dashboardManager.getGrafiek(item.Id);
                GrafiekViewModel itemViewModel = new GrafiekViewModel()
                {
                    tittel = item.Tittel,
                    DataType = grafiek.DataType,
                    GrafiekType = grafiek.GrafiekType,
                    datasets = new Dictionary<string, Dictionary<string, double>>(),
                    id = itemid,
                    itemId = item.Id
                };
                itemid++;
                Dictionary<string, Dictionary<string, double>> graphData = dashboardManager.getGraphData(grafiek);
                itemViewModel.datasets = (graphData);
                zoneViewModel.items.Add(itemViewModel);
            }
        }
    }
}
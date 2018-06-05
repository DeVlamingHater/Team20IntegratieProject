using BL.Interfaces;
using BL.Managers;
using Domain;
using Domain.Dashboards;
using PolitiekeBarometer_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PolitiekeBarometer_MVC.Controllers
{
    public class ZoneParser
    {
        public static List<ZoneViewModel> ParseZones(List<Zone> zones, UnitOfWorkManager uowmgr)
        {
            int itemid = 1;
            int zoneId = 1;
            List<ZoneViewModel> zonesViewModel = new List<ZoneViewModel>();

            IDashboardManager dashboardManager = new DashboardManager(uowmgr);

            foreach (Zone zone in zones)
            {
                ZoneViewModel zoneViewModel = new ZoneViewModel()
                {
                    Naam = zone.Naam,
                    Id = zone.Id,
                    Items = new List<ItemViewModel>(),
                    plaatsId = zoneId
                };
                zoneId++;
                if (zone.Items != null)
                {
                    List<Item> items = new List<Item>();
                    foreach (Item item in zone.Items)
                    {
                        if (item.GetType() == typeof(Grafiek))
                        {
                            Grafiek grafiek = dashboardManager.getGrafiek(item.Id);
                            GrafiekViewModel itemViewModel = new GrafiekViewModel()
                            {
                                tittel = item.Titel,
                                DataType = grafiek.DataType,
                                GrafiekType = grafiek.GrafiekType,
                                datasets = new Dictionary<string, Dictionary<string, double>>(),
                                id = itemid,
                                itemId = item.Id
                            };
                            itemid++;
                            Dictionary<string, Dictionary<string, double>> graphData = dashboardManager.getGraphData(grafiek);
                            itemViewModel.datasets=(graphData);
                            zoneViewModel.Items.Add(itemViewModel);
                        }
                    }
                }
                zonesViewModel.Add(zoneViewModel);


            }
            #region testData
            ZoneViewModel testZone = new ZoneViewModel()
            {
                Naam = "testZone",
                Items = new List<ItemViewModel>(),
                plaatsId = zoneId
            };
            zoneId++;
            ZoneViewModel legeZone = new ZoneViewModel()
            {
                Naam = "legeZone",
                Items = new List<ItemViewModel>(),
                plaatsId = zoneId
            };
            zoneId++;
            #region testBar
            Dictionary<string, Dictionary<string, double>> datasets = new Dictionary<string, Dictionary<string, double>>();
            Dictionary<string, double> dataset = new Dictionary<string, double>();
            dataset.Add("Bart", 20.0);
            dataset.Add("Imade", 80.0);
            datasets.Add("", dataset);

            GrafiekViewModel testGrafiek = new GrafiekViewModel()
            {
                tittel = "testGrafiek",
                datasets = datasets,
                GrafiekType = GrafiekType.BAR,
                DataType = DataType.TOTAAL,
                id = itemid
            };
            itemid++;
            testZone.Items.Add(testGrafiek);
            #endregion

            #region testPie
            Dictionary<string, Dictionary<string, double>> datasetsPie = new Dictionary<string, Dictionary<string, double>>();
            Dictionary<string, double> datasetPie = new Dictionary<string, double>();
            datasetPie.Add("Bart", 20.0);
            datasetPie.Add("Imade", 80.0);
            datasetsPie.Add("", dataset);

            GrafiekViewModel testGrafiekPie = new GrafiekViewModel()
            {
                tittel = "testGrafiek",
                datasets = datasetsPie,
                GrafiekType = GrafiekType.PIE,
                DataType = DataType.TOTAAL,
                id = itemid
            };
            itemid++;
            testZone.Items.Add(testGrafiekPie);
            #endregion
            #region testLine

            Dictionary<string, Dictionary<string, double>> datasetsLine = new Dictionary<string, Dictionary<string, double>>();
            Dictionary<string, double> datasetLine1 = new Dictionary<string, double>();
            datasetLine1.Add(DateTime.Now.AddDays(-4).ToShortDateString(), 20.0);
            datasetLine1.Add(DateTime.Now.AddDays(-3).ToShortDateString(), 30.0);

            Dictionary<string, double> datasetLine2 = new Dictionary<string, double>();
            datasetLine2.Add(DateTime.Now.AddDays(-4).ToShortDateString(), 50.0);
            datasetLine2.Add(DateTime.Now.AddDays(-3).ToShortDateString(), 40.0);

            datasetsLine.Add("Lijn 1", datasetLine1);
            datasetsLine.Add("Lijn 2", datasetLine2);

            GrafiekViewModel testLine = new GrafiekViewModel()
            {
                tittel = "testLine",
                datasets = datasetsLine,
                GrafiekType = GrafiekType.LINE,
                DataType = DataType.TOTAAL,
                id = itemid
            };
            itemid++;
            testZone.Items.Add(testLine);
            #endregion
            zonesViewModel.Add(testZone);
            zonesViewModel.Add(legeZone);
            #endregion


            return zonesViewModel;
        }
    }
}
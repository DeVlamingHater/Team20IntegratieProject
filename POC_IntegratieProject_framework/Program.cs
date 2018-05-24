using BL.Interfaces;
using BL.Managers;
using DAL;
using Domain;
using Domain.Dashboards;
using Domain.Elementen;
using Domain.Platformen;
using Newtonsoft.Json;
using POC_IntegratieProject_framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using Newtonsoft;
using System.Linq;
using System.Timers;

namespace PolitiekeBarometer_CA
{
    class Program
    {
        private const string Path = "D:\\School\\Academiejaar 2 (2017-2018)\\Integratieproject\\project\\Team20IntegratieProject\\DAL\\politici.json";
        static void Main(string[] args)
        {
            HttpClient client = new HttpClient();
            Console.WriteLine("Politieke Barometer");

            //CurrentSecondsTimer
            //Timer tick = new Timer();
            //tick.Interval = 1000;
            //tick.Enabled = true;
            //tick.Elapsed += new ElapsedEventHandler(sec);

            ////PlayingWithRefreshRate
            //Platform.refreshTimer.Elapsed += new ElapsedEventHandler(refreshData);
            //SetTimer(10);
            //Console.ReadLine();
            //SetTimer(7);
            //Console.ReadLine();

            bool afsluiten = false;

            //addPoliticiJSON();
            Console.WriteLine("Personen geupdate");
            //updateAPIAsync();
            Console.WriteLine("Posts opgehaald");
        }

        private static void sec(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine(e.SignalTime);
        }

        private static void SetTimer(int timeInSeconds)
        {
            Platform.refreshTimer.Enabled = true;
            Platform.refreshTimer.Interval = 0050;
            Platform.refreshTimer.Interval = timeInSeconds * 1000;
            Console.WriteLine("TIME SET TO " + timeInSeconds + " SECONDS");
        }
        private static void refreshData(Object source, ElapsedEventArgs elapsedEventArgs)
        {
            Console.WriteLine("REFRESH DATA");

        }

        private static void refreshData()
        {
            Console.WriteLine("REFRESH DATA");
        }
    }
}


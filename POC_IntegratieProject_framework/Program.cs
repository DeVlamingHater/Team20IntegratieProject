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
            Timer tick = new Timer();
            tick.Interval = 1000;
            tick.Enabled = true;
            tick.Elapsed += new ElapsedEventHandler(sec);

            //PlayingWithRefreshRate
            Platform.refreshTimer.Elapsed += new ElapsedEventHandler(refreshData);
            SetTimer(10);
            Console.ReadLine();
            SetTimer(7);
            Console.ReadLine();
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
        private static void addAlerts()
        {
            IElementManager elementManager = new ElementManager();
            IDashboardManager dashboardManager = new DashboardManager();
            Dashboard testDashboard = dashboardManager.getDashboard("thomas.somers@student.kdg.be");
            Element testElement = elementManager.getElementByNaam("Bart De Wever");
            Alert testAlert = new Alert()
            {
                ApplicatieMelding = true,
                BrowserMelding = true,
                EmailMelding = true,
                Dashboard = testDashboard,
                DataConfig = new DataConfig()
                {
                    Element = testElement
                },
                DataType = DataType.TOTAAL,
                Interval = new TimeSpan(10, 0, 0, 0),
                Status = AlertStatus.ACTIEF,
                Meldingen = new List<Melding>(),
                Operator = ">",
                Waarde = 10.0
            };
            dashboardManager.addAlert(testAlert);
        }

        private static void showMenu()
        {
            Console.WriteLine("=====================");
            Console.WriteLine("MENU");
            Console.WriteLine("1. Show Trending");
            Console.WriteLine("2. ShowAlerts");
            Console.WriteLine("3. ShowElementen");
            Console.WriteLine("4. API Update");
            Console.WriteLine("5. Send Email");
            Console.WriteLine("6. Add politici JSON");
            Console.WriteLine("7. Show testGrafiekData");
            Console.WriteLine("8. Add meldingen");

            DetectMenuAction();
        }

        private static void DetectMenuAction()
        {
            bool inValidAction = false;
            do
            {
                Console.Write("Keuze: ");
                string input = Console.ReadLine();
                int action;
                if (Int32.TryParse(input, out action))
                {
                    switch (action)
                    {
                        case 1:
                            showTrending();
                            break;
                        case 2:
                            showAlerts();
                            break;
                        case 3:
                            showElementen();
                            break;
                        case 4:
                            updateAPIAsync();
                            break;
                        case 5:
                            sendEmail();
                            break;
                        case 6:
                            addPoliticiJSON();
                            break;
                        case 7:
                            showGrafiekData();
                            break;
                        case 8:
                            addMelding();
                            break;
                        default:
                            Console.WriteLine("Foute optie");
                            inValidAction = true;
                            break;
                    }
                }
            } while (inValidAction);
        }

        private static void addMelding()
        {
            IDashboardManager dashboardManager = new DashboardManager();
            Dashboard testDashboard = dashboardManager.getDashboard("thomas.somers@student.kdg.be");

            List<Alert> alerts = dashboardManager.getDashboardAlerts(testDashboard).ToList();

            dashboardManager.createMelding(alerts.First(), 25.0);

        }

        private static void showGrafiekData()
        {
            ElementManager elementManager = new ElementManager();
            DashboardManager dashboardManager = new DashboardManager();
            Element testElement = elementManager.getElementByNaam("Bart De Wever");
            DataConfig testDataConfig = new DataConfig()
            {
                DataConfiguratieId = 100,
                Element = testElement

            };
            Grafiek testGrafiek = new Grafiek()
            {
                DataType = DataType.TOTAAL,
                Tijdschaal = new TimeSpan(7, 0, 0, 0),
                Dataconfigs = new List<DataConfig>()
                {
                    testDataConfig
                },
                GrafiekType = GrafiekType.LINE,
                AantalDataPoints = 12
            };
            string testData = dashboardManager.getGraphData(testGrafiek);
            Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string, string>>(testData);
            foreach (KeyValuePair<string, string> item in data)
            {
                Console.WriteLine("Dataconfig" + item.Key);
                Dictionary<DateTime, int> lijnData = JsonConvert.DeserializeObject<Dictionary<DateTime, int>>(item.Value);
                foreach (KeyValuePair<DateTime, int> lijnItem in lijnData)
                {
                    Console.WriteLine("DAG: " + lijnItem.Key.Day + " WAARDE: " + lijnItem.Value);
                }

            }

        }

        private static void addPoliticiJSON()
        {

            IElementManager elementManager = new ElementManager();

            elementManager.deleteAllPersonen();
            elementManager.addPersonen(elementManager.readJSONPersonen());
        }

        private static void sendEmail()
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("IntegratieProjectTeam20@gmail.com");
            mail.To.Add("IntegratieProjectTeam20@gmail.com");
            mail.Subject = "Test Mail";
            mail.Body = "This is for testing SMTP mail from GMAIL";

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("IntegratieProjectTeam20@gmail.com", "Integratie20");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
        }

        private static void showTrending()
        {
            ElementManager elementManager = new ElementManager();
            elementManager.setTrendingElementen();
            List<Element> trendingElementen = elementManager.getTrendingElementen(3);
            foreach (Element element in trendingElementen)
            {
                Console.WriteLine(element.Naam);
                Console.WriteLine(element.Trend);
            }
        }

        private static void showAlerts()
        {
            Console.WriteLine("Niet meer geïmplementeerd");
        }

        private static void showElementen()
        {
            ElementManager elementManager = new ElementManager();
            elementManager.getAllElementen().ForEach(p => Console.WriteLine(p.Naam + " " + p.Trend));
        }

        private static async void updateAPIAsync()
        {
            PostManager postManager = new PostManager();
            string responseString = await postManager.updatePosts();
            postManager.addJSONPosts(responseString);
            postManager.deleteOldPosts();
        }

        public void deleteOldPosts()
        {
            PostManager postManager = new PostManager();
            postManager.deleteOldPosts();
        }
    }
}


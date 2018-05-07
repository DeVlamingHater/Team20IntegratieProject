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

namespace PolitiekeBarometer_CA
{
    class Program
    {


        private const string Path = "D:\\School\\Academiejaar 2 (2017-2018)\\Integratieproject\\project\\Team20IntegratieProject\\DAL\\politici.json";

        static void Main(string[] args)
        {
            HttpClient client = new HttpClient();

            Console.WriteLine("Politieke Barometer");
            bool afsluiten = false;

            addPoliticiJSON();
            Console.WriteLine("Personen geupdate");
            updateAPIAsync();
            Console.WriteLine("Posts opgehaald");

            while (!afsluiten)
            {
                showMenu();
            }
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
                        default:
                            Console.WriteLine("Foute optie");
                            inValidAction = true;
                            break;
                    }
                }
            } while (inValidAction);
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
                GrafiekType=GrafiekType.LIJN,
                AantalDataPoints = 12
            };
            string testData = dashboardManager.getGrafiekData(testGrafiek);
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


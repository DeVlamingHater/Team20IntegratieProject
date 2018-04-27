using BL.Interfaces;
using BL.Managers;
using DAL;
using Domain;
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

namespace PolitiekeBarometer_CA
{
    class Program
    {
        private const string Path = "D:\\School\\Academiejaar 2 (2017-2018)\\Integratieproject\\project\\Team20IntegratieProject\\DAL\\politici.json";
        private static IElementManager elementManager;
        private static IPostManager postManager;
        private static IDashboardManager dashboardManager;
        private static IPlatformManager platformManager;
        static void Main(string[] args)
        {
            elementManager = new ElementManager();
            postManager = new PostManager();
            dashboardManager = new DashboardManager();
            platformManager = new PlatformManager();
            HttpClient client = new HttpClient();

            Console.WriteLine("Politieke Barometer");
            bool afsluiten = false;
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
                        default:
                            Console.WriteLine("Foute optie");
                            inValidAction = true;
                            break;
                    }
                }
            } while (inValidAction);
        }

        private static void addPoliticiJSON()
        {
            List<Persoon> personen = new List<Persoon>();
            List<PersoonParser> items;
            using (StreamReader r = new StreamReader(Path))
            {
                string json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<List<PersoonParser>>(json);
            }
            foreach (PersoonParser persoon in items)
            {
                Persoon politicus = new Persoon()
                {
                    DateOfBirth = persoon.dateOfBirth,
                    District = persoon.district,
                    Facebook = persoon.facebook,
                    Gender = persoon.gender,
                    Naam = persoon.full_name,
                    Position = persoon.position,
                    Level = persoon.level,
                    Postal_code = persoon.postal_code,
                    Site = persoon.site,
                    Town = persoon.town,
                    Twitter = persoon.twitter
                };
                Organisatie organisatie = (Organisatie)elementManager.getElementByNaam(persoon.organisation);
                if (organisatie == null)
                {
                    organisatie = new Organisatie()
                    {
                        Naam = persoon.organisation,
                        Personen = new List<Persoon>()
                        {
                            politicus
                        }
                    };
                    elementManager.addOrganisatie(organisatie);
                }
                politicus.Organisatie = organisatie;
                personen.Add(politicus);
            }
            elementManager.addPersonen(personen);
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
            elementManager.getAllElementen().ForEach(p => Console.WriteLine(p.Naam + " " + p.Trend));
        }

        //TODO run on timer
        private static async void updateAPIAsync()
        {
            string responseString = await postManager.updatePosts();
            Console.WriteLine(responseString);
            postManager.addJSONPosts(responseString);
            postManager.deleteOldPosts();
        }

        public void deleteOldPosts()
        {
            postManager.deleteOldPosts();
        }
    }
}


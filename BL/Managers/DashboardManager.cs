using BL.Interfaces;
using DAL;
using DAL.Repositories_EF;
using Domain;
using Domain.Dashboards;
using Domain.Platformen;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace BL.Managers
{
    public class DashboardManager : IDashboardManager
    {
        public static int NUMBERDATAPOINTS = 12;

        IDashboardRepository dashboardRepository;
        public UnitOfWorkManager uowManager;
        public DashboardManager()
        {
            dashboardRepository = new DashboardRepository_EF();
        }

        public DashboardManager(UnitOfWorkManager uowManager)
        {
            this.uowManager = uowManager;
            dashboardRepository = new DashboardRepository_EF(uowManager.UnitOfWork);
        }

        public Dashboard getDashboard(int gebruikerId)
        {
            Dashboard dashboard = dashboardRepository.getDashboard(gebruikerId);
            return dashboard;
        }

        public IEnumerable<Item> getItems(int actieveZone)
        {
            return dashboardRepository.getItems(actieveZone);
        }

        public IEnumerable<Zone> getZones(Dashboard dashboard)
        {
            int dashboardId = dashboard.DashboardId;
            return dashboardRepository.getZones(dashboardId);
        }
        public Zone getZone(int zoneId)
        {
            return dashboardRepository.getZone(zoneId);
        }

        public void deleteZone(int zoneId)
        {
            dashboardRepository.deleteZone(zoneId);
        }

        public Zone addZone()
        {
            // GEBRUIKER VAN DASHBOARD VINDEN NIET JUIST
            Dashboard dashboard = this.getDashboard(1);
            IEnumerable<Zone> zones = this.getZones(dashboard);
            Zone zone = new Zone()
            {
                Id = zones.Count() + 1,
                Naam = "NewZone",
                Dashboard = dashboard
            };
            return dashboardRepository.addZone(zone);
        }

        public void changeZone(Zone zone)
        {
            dashboardRepository.UpdateZone(zone);
        }

        public List<Alert> getActiveAlerts()
        {
            initNonExistingRepo(false);
            return dashboardRepository.getActiveAlerts().ToList();
        }

        public DataConfig getAlertDataConfig(Alert alert)
        {
            initNonExistingRepo(false);
            return alert.DataConfig;
        }

        public Gebruiker getAlertGebruiker(Alert alert)
        {
            initNonExistingRepo(false);
            return alert.Dashboard.Gebruiker;
        }

        public List<Alert> getAllAlerts()
        {
            initNonExistingRepo(false);
            return dashboardRepository.getAllAlerts().ToList();
        }

        internal TimeSpan getHistoriek()
        {
            throw new NotImplementedException();
        }

        public void sendAlerts()
        {
            initNonExistingRepo(true);
            PostManager postManager = new PostManager(uowManager);
            List<Alert> activeAlerts = getActiveAlerts();
            foreach (Alert alert in activeAlerts)
            {
                double waarde = postManager.getHuidigeWaarde(alert.DataConfig);
                bool sendMelding = false;
                switch (alert.Operator)
                {
                    case "<":
                        if (waarde < alert.Waarde)
                        {
                            sendMelding = true;
                        }
                        break;
                    case ">":
                        if (waarde > alert.Waarde)
                        {
                            sendMelding = true;
                        }
                        break;
                    default:
                        break;
                }
                if (sendMelding)
                {
                    Console.WriteLine("MELDING!!!");
                    if (alert.EmailMelding)
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
                }
                
            }

        }

        public void initNonExistingRepo(bool createWithUnitOfWork = false)
        {
            // De onderstaande logica is enkel uit te voeren als er nog geen repo bestaat. 
            //Als we een repo met UoW willen gebruiken en als er nog geen uowManager bestaat
            // dan maken we de uowManager aan en gebruiken we de context daaruit om de repo aan te maken.
            if (dashboardRepository == null)
            {
                if (createWithUnitOfWork)
                {
                    if (uowManager == null)
                    {
                        uowManager = new UnitOfWorkManager();
                    }
                    dashboardRepository = new DashboardRepository_EF(uowManager.UnitOfWork);
                }
                // Als we niet met UoW willen werken, dan maken we een repo aan als die nog niet bestaat.
                else
                {
                    dashboardRepository = new DashboardRepository_EF();
                }

            }
        }

        

        TimeSpan IDashboardManager.getHistoriek()
        {
            return dashboardRepository.getPlatform().Historiek;
        }
        public string getLineGraphData(Grafiek grafiek)
        {
            PostManager postManager = new PostManager();
            
            IElementManager elementManager = new ElementManager();
            Element testElement = elementManager.getElementByNaam("Bart De Wever");
            DataConfig testDataConfig = new DataConfig()
            {
                DataConfiguratieId = 100,
                DataType = Domain.DataType.TOTAAL,
                Elementen = new List<Element>()
                {
                    testElement
                }
            };
            Grafiek testGrafiek = new Grafiek()
            {
                tijdschaal = new TimeSpan(1, 0, 0, 0),
                Dataconfigs = new List<DataConfig>()
                {
                    testDataConfig
                }
            };
            grafiek = testGrafiek;
            Dictionary<DateTime, int> data = new Dictionary<DateTime, int>();
            List<DataConfig> dataConfigs = grafiek.Dataconfigs;
            foreach (DataConfig dataConfig in dataConfigs)
            {
                DateTime start = DateTime.Now;
                for (int i = 0; i < NUMBERDATAPOINTS; i++)
                {
                    List<Post> posts = postManager.getDataConfigPosts(dataConfig).ToList();

                    DateTime eind = start.Add(grafiek.tijdschaal);

                    posts = posts.Where(p => p.Date.Subtract(start).TotalDays > 0).Where(p=>p.Date.Subtract(eind).TotalDays<0).ToList();

                    data.Add(start, posts.Count);

                    start = start.Subtract(grafiek.tijdschaal);
                }
                data.Reverse();
            }
            return JsonConvert.SerializeObject(data).ToString();
        }
    }
}

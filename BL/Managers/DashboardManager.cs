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
        #region Constructor
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

        public void initNonExistingRepo(bool createWithUnitOfWork = false)
        {

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
                else
                {
                    dashboardRepository = new DashboardRepository_EF();
                }
            }
        }
        #endregion
    
        #region Dashboard
        public Dashboard getDashboard(string email, Deelplatform deelplatform)
        {
            Dashboard dashboard = dashboardRepository.getDashboard(email, deelplatform);
            return dashboard;
        }
        #endregion

        #region Zone
        public IEnumerable<Zone> getZones(Dashboard dashboard)
        {
            int dashboardId = dashboard.DashboardId;
            return dashboardRepository.getDashboardZones(dashboardId);
        }

        public Zone getZone(int zoneId)
        {
            return dashboardRepository.getZone(zoneId);
        }

        public void deleteZone(int zoneId)
        {
            dashboardRepository.deleteZone(zoneId);
        }

        public Zone addZone(Dashboard dashboard)
        {
            // GEBRUIKER VAN DASHBOARD VINDEN NIET JUIST
            Zone zone = new Zone()
            {
                Naam = "NewZone",
                Dashboard = dashboard
            };
            return dashboardRepository.addZone(zone);
        }

        public void changeZoneName(int zoneId,string naam)
        {
            dashboardRepository.changeZoneName(zoneId,naam);
        }

        public void updateZone(Zone zone)
        {
            dashboardRepository.UpdateZone(zone);
        }
        #endregion

        #region Item
        public IEnumerable<Item> getItems(int zoneId)
        {
            return dashboardRepository.getItems(zoneId);
        }

        public Item getItem(int itemId)
        {
            return dashboardRepository.getItem(itemId);
        }
        #endregion

        #region Grafiek

        public IEnumerable<Grafiek> getGrafieken(int zoneId)
        {
            return dashboardRepository.getGrafieken(zoneId);
        }

        public void addGrafiek(Grafiek grafiek)
        {
            dashboardRepository.addGrafiek(grafiek);
        }
        public Grafiek getGrafiek(int itemId)
        {
            return dashboardRepository.getGrafiek(itemId);
        }
        public Grafiek createGrafiek(Grafiek grafiek)
        {
            dashboardRepository.addGrafiek(grafiek);
            return grafiek;
        }

        public string getGraphData(Grafiek grafiek)
        {
            IPostManager postManager = new PostManager();

            IElementManager elementManager = new ElementManager();
            List<string> data = new List<string>();
            List<DataConfig> dataConfigs = grafiek.Dataconfigs;
            int index = 0;

            foreach (DataConfig dataConfig in dataConfigs)
            {
                //Dictionary van de Data, bevat geformateerde datum en double voor de data
                Dictionary<DateTime, double> grafiekData = new Dictionary<DateTime, double>();

                DateTime start = DateTime.Now.Subtract(grafiek.Tijdschaal);

                TimeSpan interval = new TimeSpan(grafiek.Tijdschaal.Ticks / grafiek.AantalDataPoints);

                for (int i = 0; i < grafiek.AantalDataPoints; i++)
                {
                    List<Post> posts = postManager.getDataConfigPosts(dataConfig).ToList();
                    int totaal = posts.Count();

                    DateTime eind = start.Add(interval);
                    posts = posts.Where(p => p.Date.Subtract(start).TotalDays > 0).Where(p => p.Date.Subtract(eind).TotalDays < 0).ToList();

                    posts = postManager.filterPosts(posts, grafiek.Filters);
                    switch (grafiek.DataType)
                    {
                        case Domain.DataType.TOTAAL:
                            grafiekData.Add(start, (double)posts.Count);
                            break;
                        case Domain.DataType.TREND:
                            double dataPoint = (double)posts.Count / (double)totaal;
                            grafiekData.Add(start, dataPoint);
                            break;
                        case Domain.DataType.SENTIMENT:
                            double average = 0.0;
                            if (posts.Count()!=0)
                            {
                                 average = posts.Average(p => p.Sentiment[0] * p.Sentiment[1]);
                            }
                            grafiekData.Add(start, average);
                            break;
                        default:
                            break;
                    }
                    start = start.Add(interval);
                }
                string dataString = JsonConvert.SerializeObject(grafiekData);
                data.Add(dataString);
                index++;
            }
            return JsonConvert.SerializeObject(data).ToString();
        }
        #endregion

        #region Alert
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
            initNonExistingRepo(false);
            return alert.Dashboard.Gebruiker;
        }

        public List<Alert> getAllAlerts()
        {
            return dashboardRepository.getAllAlerts().ToList();
        }

        public void sendAlerts()
        {
            IPostManager postManager = new PostManager(uowManager);
            List<Alert> activeAlerts = getActiveAlerts();
            double waarde = 0.0;
            foreach (Alert alert in activeAlerts)
            {
                waarde = postManager.getAlertWaarde(alert);
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
                    if (alert.ApplicatieMelding)
                    {
                        createMelding(alert, waarde);
                    }
                    if (alert.BrowserMelding)
                    {
                        //TODO: Push request
                    }
                }
            }
        }

        public void addAlert(Alert alert)
        {
            dashboardRepository.createAlert(alert);
        }

        public IEnumerable<Alert> getDashboardAlerts(Dashboard dashboard)
        {
            return dashboardRepository.getDashboardAlerts(dashboard);
        }

        public Alert getAlert(int id)
        {
            return dashboardRepository.getAlert(id);
        }

        public void createAlert(Alert alert)
        {
            dashboardRepository.createAlert(alert);
        }
        #endregion

        #region Melding

        public Melding createMelding(Alert alert, double waarde)
        {
            Melding melding = new Melding()
            {
                Alert = alert,
                IsActive = true,
                MeldingDateTime = DateTime.Now,
            };

            switch (alert.Operator)
            {
                case "<":
                    melding.IsPositive = false;
                    break;
                case ">":
                    melding.IsPositive = true;
                    break;
                default:
                    melding.IsPositive = false;
                    break;
            }
            StringBuilder message = new StringBuilder("");

            if (alert.DataConfig.Vergelijking == null)
            {
                message.Append("Het element " + alert.DataConfig.Element.Naam + " heeft de waarde " + waarde);
            }
            else
            {
                message.Append("De vergelijking tussen " + alert.DataConfig.Element.Naam + " " + alert.DataConfig.Vergelijking.Naam + " heeft de waarde " + waarde);
            }
            melding.Message = message.ToString();
            melding.Titel = "Melding van " + alert.DataConfig.Element.Naam;
            dashboardRepository.addMelding(melding);
            return melding;
        }

        public IEnumerable<Melding> getActiveMeldingen(Dashboard dashboard)
        {
            return dashboardRepository.getActiveMeldingen(dashboard);
        }
        #endregion      
    }
}

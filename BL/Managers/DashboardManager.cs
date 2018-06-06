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
        public Dashboard getDashboard(string email, string deelplatformS)
        {
            initNonExistingRepo(true);
            IPlatformManager platformManager = new PlatformManager(uowManager);
            Deelplatform deelplatform = platformManager.getDeelplatformByNaam(deelplatformS);
            Dashboard dashboard = dashboardRepository.getDashboard(email, deelplatform);
            uowManager.Save();
            return dashboard;
        }
       
        #endregion

        #region Zone
        public IEnumerable<Zone> getZones(Dashboard dashboard)
        {
            initNonExistingRepo();

            int dashboardId = dashboard.Id;
            return dashboardRepository.getDashboardZones(dashboardId);
        }

        public Zone getZone(int zoneId)
        {
            initNonExistingRepo();

            return dashboardRepository.getZone(zoneId);
        }

        public void deleteZone(int zoneId)
        {
            initNonExistingRepo();

            dashboardRepository.deleteZone(zoneId);
        }

        public Zone addZone(Dashboard dashboard)
        {
            initNonExistingRepo(true);

            Zone zone = new Zone()
            {
                Naam = "NewZone",
                Dashboard = dashboard,
                Items = new List<Item>()
            };
            uowManager.Save();
            return dashboardRepository.addZone(zone);
        }

        public void changeZoneName(int zoneId,string naam)
        {
            initNonExistingRepo();

            dashboardRepository.changeZoneName(zoneId,naam);
        }

        public void updateZone(Zone zone)
        {
            initNonExistingRepo();

            dashboardRepository.UpdateZone(zone);
        }
        #endregion

        #region Item
        public IEnumerable<Item> getItems(int zoneId)
        {
            initNonExistingRepo();

            return dashboardRepository.getItems(zoneId);
        }

        public Item getItem(int itemId)
        {
            initNonExistingRepo();

            return dashboardRepository.getItem(itemId);
        }
        #endregion

        #region Grafiek

        public IEnumerable<Grafiek> getGrafieken(int zoneId)
        {
            initNonExistingRepo();

            return dashboardRepository.getGrafieken(zoneId);
        }

        public void addGrafiek(Grafiek grafiek)
        {
            initNonExistingRepo();

            dashboardRepository.addGrafiek(grafiek);
        }
        public Grafiek getGrafiek(int itemId)
        {
            initNonExistingRepo();

            return dashboardRepository.getGrafiek(itemId);
        }
        public Grafiek createGrafiek(Grafiek grafiek)
        {
            initNonExistingRepo();

            dashboardRepository.addGrafiek(grafiek);

            if (uowManager!=null)
            {
                uowManager.Save();
            }
            return grafiek;
        }

        public Dictionary<string, Dictionary<string, double>> getGraphData(Grafiek grafiek)
        {
            initNonExistingRepo();

            IPostManager postManager = new PostManager();

            IElementManager elementManager = new ElementManager();
            Dictionary<string,Dictionary<string, double>> data = new Dictionary<string, Dictionary<string, double>>();
            List<DataConfig> dataConfigs = grafiek.Dataconfigs;
            int index = 0;

            foreach (DataConfig dataConfig in dataConfigs)
            {
                //Dictionary van de Data, bevat geformateerde datum en double voor de data
                Dictionary<string, double> grafiekData = new Dictionary<string, double>();

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
                            grafiekData.Add(start.ToString("dd/MM/yyyy HH/mm"), (double)posts.Count);
                            break;
                        case Domain.DataType.PERCENTAGE:
                            double dataPoint;
                            if (totaal == 0)
                            {
                                dataPoint = 0;
                            }
                            else
                            {
                             dataPoint = (double)posts.Count / (double)totaal;

                            }
                            grafiekData.Add(start.ToString("dd/MM/yy HH:mm"), dataPoint);
                            break;
                        default:
                            break;
                    }
                    start = start.Add(interval);
                }
                if (dataConfig.Label == null)
                {
                dataConfig.Label = dataConfig.Element.Naam + grafiek.DataType.ToString().ToLower();

                }
                data.Add(dataConfig.Label, grafiekData);
                index++;
            }
            return data;
        }
        #endregion

        #region Alert
        public List<Alert> getActiveAlerts(Dashboard dashboard)
        {
            initNonExistingRepo();

            return dashboardRepository.getActiveAlerts(dashboard).ToList();
        }

        public DataConfig getAlertDataConfig(Alert alert)
        {
            initNonExistingRepo();

            return alert.DataConfig;
        }

        public Gebruiker getAlertGebruiker(Alert alert)
        {
            initNonExistingRepo();

            initNonExistingRepo(false);
            return alert.Dashboard.Gebruiker;
        }

        public List<Alert> getAllAlerts()
        {
            initNonExistingRepo();

            return dashboardRepository.getAllAlerts().ToList();
        }
        public List<Alert> getAllDashboardAlerts(Dashboard dashboard)
        {
            initNonExistingRepo();

            return dashboardRepository.getAllDashboardAlerts(dashboard).ToList();
        }

        public void updateAlert(Alert alert)
        {
            initNonExistingRepo(true);

            dashboardRepository.updateAlert(alert);
            this.uowManager.Save();
        }

        public void sendAlerts()
        {
            initNonExistingRepo();

            IPostManager postManager = new PostManager(uowManager);
            List<Alert> activeAlerts = getAllAlerts().Where(a => a.Status == AlertStatus.ACTIEF).ToList();
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
                        string email = alert.Dashboard.Gebruiker.Email;
                        mail.To.Add(email);
                        mail.Subject = "Barometer " + alert.Dashboard.Deelplatform.Naam;
                        //TODO
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
            initNonExistingRepo();

            dashboardRepository.createAlert(alert);
        }

        public IEnumerable<Alert> getDashboardAlerts(Dashboard dashboard)
        {
            initNonExistingRepo();

            return dashboardRepository.getDashboardAlerts(dashboard);
        }

        public Alert getAlert(int id)
        {
            initNonExistingRepo();

            return dashboardRepository.getAlert(id);
        }

        public void createAlert(Alert alert)
        {
            initNonExistingRepo();

            dashboardRepository.createAlert(alert);

            if (uowManager!=null)
            {
                uowManager.Save();
            }
        }
        #endregion

        #region Melding

        public Melding createMelding(Alert alert, double waarde)
        {
            initNonExistingRepo();

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
            initNonExistingRepo();

            return dashboardRepository.getActiveMeldingen(dashboard);
        }
        #endregion
    }
}

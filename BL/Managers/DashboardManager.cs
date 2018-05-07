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

        public Dashboard getDashboard(string gebruikersNaam)
        {
            Dashboard dashboard = dashboardRepository.getDashboard(gebruikersNaam);
            return dashboard;
        }

        public IEnumerable<Item> getItems(int actieveZone)
        {
            return null;
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
            Dashboard dashboard = this.getDashboard("Sam Claessen");
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
                    if (alert.ApplicatieMelding)
                    {

                    }
                }

            }

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

        public TimeSpan getHistoriek()
        {
            return dashboardRepository.getPlatform().Historiek;
        }
        public string getGraphData(Grafiek grafiek)
        {

            PostManager postManager = new PostManager();

            IElementManager elementManager = new ElementManager();
            Dictionary<string, string> data = new Dictionary<string, string>();
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

                    posts = filterPosts(posts, grafiek.Filters);
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
                            double average = posts.Average(p => p.Sentiment[0] * p.Sentiment[1]);
                            grafiekData.Add(start, average);
                            break;
                        default:
                            break;
                    }
                    start = start.Add(interval);
                }
                string dataString = JsonConvert.SerializeObject(grafiekData);
                data.Add(index.ToString(), dataString);
                index++;
            }
            return JsonConvert.SerializeObject(data).ToString();
        }

        public List<Post> filterPosts(List<Post> posts, List<Filter> filters)
        {
            if (filters == null)
            {
                return posts;
            }
            foreach (Filter filter in filters)
            {
                switch (filter.Type)
                {
                    case FilterType.SENTIMENT:
                        switch (filter.Operator)
                        {
                            case "<":
                                posts = posts.Where(p => p.Sentiment[0] < filter.Waarde).ToList();
                                break;
                            case ">":
                                posts = posts.Where(p => p.Sentiment[0] > filter.Waarde).ToList();
                                break;
                            default:
                                break;
                        }
                        break;
                    case FilterType.AGE:
                        posts = posts.Where(p => p.Age.Equals(filter.Waarde)).ToList();
                        break;
                    case FilterType.RETWEET:
                        posts = posts.Where(p => p.Retweet == true).ToList();
                        break;
                    default:
                        break;
                }
            }
            return posts;
        }

        public string getGrafiekData(Grafiek grafiek)
        {
            IPostManager postManager = new PostManager();
            StringBuilder response = new StringBuilder("");
            List<DataConfig> dataConfigs = grafiek.Dataconfigs;

            if (grafiek.GrafiekType == GrafiekType.LIJN)
            {
                string data = getGraphData(grafiek);
                response.Append(data);
            }
            else if (grafiek.GrafiekType == GrafiekType.PIE || grafiek.GrafiekType == GrafiekType.STAAF)
            {
                string data = getGraphData(grafiek);
            }

            return response.ToString();
        }

        public Grafiek createGrafiek(Domain.DataType dataType, int aantalDataPoints, TimeSpan Tijdschaal, int zoneId, List<Filter> filters, List<DataConfig> dataConfigs)
        { 
            Grafiek grafiek = new Grafiek()
            {
                DataType = dataType,
                AantalDataPoints = aantalDataPoints,
                Tijdschaal = Tijdschaal,
                Zone = getZone(zoneId),
                Filters = new List<Filter>(filters),
                Dataconfigs = new List<DataConfig>(dataConfigs)
            };
            dashboardRepository.addGrafiek(grafiek);
            return grafiek;
        }
    }
}

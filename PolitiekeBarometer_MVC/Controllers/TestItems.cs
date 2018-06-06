using BL.Interfaces;
using BL.Managers;
using Domain;
using Domain.Dashboards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PolitiekeBarometer_MVC.Controllers
{
    public class TestItems
    {
        public static List<Item> GetTestItems(string deelplatform)
        {
            UnitOfWorkManager uowmgr = new UnitOfWorkManager();
            IPlatformManager platformManager = new PlatformManager(uowmgr);
            IElementManager elementManager = new ElementManager(uowmgr);
            List<Item> testItems = new List<Item>();

            Grafiek bar = new Grafiek()
            {
                GrafiekType = GrafiekType.BAR,
                AantalDataPoints = 12,
                Id = 200,
                Dataconfigs = new List<DataConfig>()
            {
                new DataConfig()
                {
                    Element = elementManager.getElementByNaam("Bart De Wever", platformManager.getDeelplatformByNaam(deelplatform)),
                    Label="testBar1",
                    Filters = new List<Filter>()
                    {
                        new Filter()
                        {
                            IsPositive=true,
                            Type = FilterType.SENTIMENT
                        }
                    }
                },
                new DataConfig()
                {
                    Element = elementManager.getElementByNaam("Bart De Wever", platformManager.getDeelplatformByNaam(deelplatform)),
                    Label="testBar2",
                    Filters = new List<Filter>()
                    {
                        new Filter()
                        {
                            IsPositive=false,
                            Type = FilterType.SENTIMENT
                        }
                    }
                }
            },
                DataType = DataType.TOTAAL,
                Size = 2,
                X = 1,
                Y = 1,
                TijdschaalTicks = new TimeSpan(0, 12, 0, 0, 0).Ticks,
                Titel = "testBar"
            };
            testItems.Add(bar);
            Grafiek pie = new Grafiek()
            {
                GrafiekType = GrafiekType.PIE,
                AantalDataPoints = 12,
                Id = 201,
                Dataconfigs = new List<DataConfig>()
            {
                new DataConfig()
                {
                    Element = elementManager.getElementByNaam("Bart De Wever", platformManager.getDeelplatformByNaam(deelplatform)),
                    Label="testBar1",
                    Filters = new List<Filter>()
                    {
                        new Filter()
                        {
                            IsPositive=true,
                            Type = FilterType.SENTIMENT
                        }
                    }
                },
                new DataConfig()
                {
                    Element = elementManager.getElementByNaam("Bart De Wever", platformManager.getDeelplatformByNaam(deelplatform)),
                    Label="testBar2",
                    Filters = new List<Filter>()
                    {
                        new Filter()
                        {
                            IsPositive=false,
                            Type = FilterType.SENTIMENT
                        }
                    }
                }
            },
                DataType = DataType.TOTAAL,
                Size = 2,
                X = 1,
                Y = 1,
                TijdschaalTicks = new TimeSpan(0, 12, 0, 0, 0).Ticks,
                Titel = "testPie"
            };
            testItems.Add(pie);

            Grafiek line = new Grafiek()
            {
                GrafiekType = GrafiekType.LINE,
                AantalDataPoints = 12,
                Id =203,
                Dataconfigs = new List<DataConfig>()
            {
                new DataConfig()
                {
                    Element = elementManager.getElementByNaam("Bart De Wever", platformManager.getDeelplatformByNaam(deelplatform)),
                    Label="testBar1",
                    Filters = new List<Filter>()
                    {
                        new Filter()
                        {
                            IsPositive=true,
                            Type = FilterType.SENTIMENT
                        }
                    }
                },
                new DataConfig()
                {
                    Element = elementManager.getElementByNaam("Bart De Wever", platformManager.getDeelplatformByNaam(deelplatform)),
                    Label="testBar2",
                    Filters = new List<Filter>()
                    {
                        new Filter()
                        {
                            IsPositive=false,
                            Type = FilterType.SENTIMENT
                        }
                    }
                }
            },
                DataType = DataType.TOTAAL,
                Size = 2,
                X = 1,
                Y = 1,
                TijdschaalTicks = new TimeSpan(0, 12, 0, 0, 0).Ticks,
                Titel = "testLine"
            };
            testItems.Add(line);

            return testItems;
        }
    }

}
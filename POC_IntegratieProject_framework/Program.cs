using BL.Interfaces;
using BL.Managers;
using DAL;
using Domain;
using Domain.Elementen;
using Domain.Platformen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PolitiekeBarometer_CA
{
    class Program
    {

        private static IElementManager elementManager;
        private static IPostManager postManager;
        private static IDashboardManager dashboardManager;
        private static IPlatformManager platformManager;
        static void Main(string[] args)
        {
            Console.WriteLine("Versie Sam");
            elementManager = new ElementManager();
            postManager = new PostManager();
            dashboardManager = new DashboardManager();
            platformManager = new PlatformManager();
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
            Console.WriteLine("1. Update Posts");
            Console.WriteLine("2. ShowAlerts");
            Console.WriteLine("3. ShowElementen");
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
                            updatePosts();
                            break;
                        case 2:
                            showAlerts();
                            break;
                        case 3:
                            showElementen();
                            break;
                        default:
                            Console.WriteLine("Foute optie");
                            inValidAction = true;
                            break;
                    }
                }
            } while (inValidAction);
        }

        private static void showElementen()
        {
            List<Element> elementen = elementManager.getAllElementen();
            foreach (Element element in elementen)
            {
                Console.WriteLine("====================");
                Console.WriteLine(element.Naam + ": " + element.Id);

                if (element.GetType().Equals(typeof(Persoon)))
                {
                    Persoon persoon = (Persoon)element;
                    if (persoon.Organisatie != null)
                    {
                        Console.WriteLine("Organisatie: " + persoon.Organisatie.Naam);
                    }
                    else
                    {
                        Console.WriteLine("Persoon behoort nog niet tot een organisatie");
                    }
                }
                else if (element.GetType().Equals(typeof(Thema)))
                {
                    Console.WriteLine("Keywords: ");
                    Thema thema = (Thema)element;
                    if (thema.Keywords != null)
                    {
                        foreach (Keyword word in thema.Keywords)
                        {
                            if (word != null)
                            {
                                Console.WriteLine(word.KeywordNaam);

                            }
                        }
                    }

                }

            }
        }

        private static void showAlerts()
        {
            List<Alert> alerts = dashboardManager.getAllAlerts();

            foreach (Alert alert in alerts)
            {
                Console.WriteLine("=======================");
                Console.WriteLine("Alert: " + alert.AlertId);
                Console.WriteLine("Wanneer " + alert.Waarde + " " + alert.Operator + " " + alert.DataConfig.DataType);
                Console.WriteLine("Status: " + alert.Status);
            }
        }

        private static void updatePosts()
        {
            postManager.updatePosts();
        }

        private static List<Post> ParseTweetsToPost(List<Tweet> tweets)
        {
            int index = 0;
            List<Post> posts = new List<Post>();
            foreach (Tweet tweet in tweets)
            {
                Post post = new Post();
                post.PostId = tweet.TweetId;
                post.Keywords = new List<Keyword>();
                foreach (string word in tweet.Words)
                {
                    post.Keywords.Add(new Keyword()
                    {
                        KeywordId = index,
                        KeywordNaam = word
                    });
                    index++;
                }

                post.Persoon = (Persoon)elementManager.getElementByNaam(tweet.Politician[0] + " " + tweet.Politician[1]);
                post.Source = "Twitter";
                post.Date = tweet.Date;
                posts.Add(post);
                #region parameters
                //    List<Waarde> waardenUrls = new List<Waarde>();
                //    index = 0;
                //    foreach (string url in tweet.Urls)
                //    {
                //        Waarde waarde = new Waarde()
                //        {
                //            Value = url,
                //            Parameter = ElementManager.getParameter("urls"),
                //            WaardeId = index

                //        };
                //        index++;
                //    }
                //    post.Parameters.Add(new Parameter()
                //    {
                //        Naam = "urls",
                //        ParameterId = 1,
                //        ParameterType = ParameterType.STRING,
                //        Post = post,
                //        Waarden = waardenUrls

                //    };

                //    List<Waarde> waardenHashtags = new List<Waarde>();
                //    index = 0;
                //    foreach (string hashtag in tweet.Hashtags)
                //    {
                //        Waarde waarde = new Waarde()
                //        {
                //            Value = hashtag,
                //            Parameter = ElementManager.getParameter("hashtags"),
                //            WaardeId = index
                //        };
                //        index++;
                //    }

                //    post.Parameters.Add(new Parameter()
                //    {
                //        Naam = "hashtags",
                //        ParameterId = 2,
                //        ParameterType = ParameterType.STRING,
                //        Post = post,
                //        Waarden = waardenHashtags
                //    });

                //    List<Waarde> waardenMentions = new List<Waarde>();
                //    index = 0;
                //    foreach (string waarde in tweet.Mentions)
                //    {
                //        Waarde waarde = new Waarde()
                //        {
                //            Value = waarde,
                //            Parameter = ElementManager.getParameter("hashtags"),
                //            WaardeId = index
                //        };
                //        index++;
                //    }

                //    post.Parameters.Add(new Parameter()
                //    {
                //        Naam = "hashtags",
                //        ParameterId = 2,
                //        ParameterType = ParameterType.STRING,
                //        Post = post,
                //        Waarden = waardenMentions
                //    });
                //};
                #endregion
            }

            return posts;
        }


    }
}


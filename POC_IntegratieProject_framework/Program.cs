﻿using BL.Interfaces;
using BL.Managers;
using DAL;
using Domain;
using Domain.Elementen;
using Domain.Platformen;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

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
            Console.WriteLine("5. ");

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
                      default:
                            Console.WriteLine("Foute optie");
                            inValidAction = true;
                            break;
                    }
                }
            } while (inValidAction);
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
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://kdg.textgain.com/query");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //     client.DefaultRequestHeaders.Authorization =
            //new AuthenticationHeaderValue("aEN3K6VJPEoh3sMp9ZVA73kkr");
            client.DefaultRequestHeaders.Add("X-Api-Key", "aEN3K6VJPEoh3sMp9ZVA73kkr");

            DateTime sinceDT = DateTime.Now.AddHours(-1);
            string sinceS = sinceDT.ToString("d MMM yyyy HH:mm:ss");

            Dictionary<string, string> values = new Dictionary<string, string>()
            {
                {"since", "23 Apr 2018 21:10:04" }
            };
            FormUrlEncodedContent content = new FormUrlEncodedContent(values);
            HttpResponseMessage response = await client.PostAsync("http://kdg.textgain.com/query", content);
            string responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseString);
            postManager.addJSONPosts(responseString);
        }

        public void deleteOldPosts()
        {
            postManager.deleteOldPosts();
        }
    }
}


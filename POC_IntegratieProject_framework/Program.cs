using BL.Interfaces;
using BL.Managers;
using DAL;
using Domain;
using Domain.Dashboards;
using Domain.Elementen;
using Domain.Platformen;
using System;
using System.Collections.Generic;

namespace PolitiekeBarometer_CA
{
  class Program
  {

    private static IElementManager elementManager;
    private static IPostManager postManager;
    private static IDashboardManager dashboardManager;
    private static BL.Interfaces.IPlatformManager platformManager;
    static void Main(string[] args)
    {
      elementManager = new ElementManager();
      postManager = new PostManager();
      dashboardManager = new DashboardManager();
      platformManager = new BL.Managers.PlatformManager();
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
            case 4:
              getDashboard();
              break;
            default:
              Console.WriteLine("Foute optie");
              inValidAction = true;
              break;
          }
        }
      } while (inValidAction);
    }

    private static void getDashboard()
    {
      Dashboard dashboard = dashboardManager.getDashboard(1);
      Console.WriteLine(dashboard.Gebruiker.Naam);
    }

    private static void showElementen()
    {



    }

    private static void showAlerts()
    {
      List<Alert> alerts = dashboardManager.getAllAlerts();

      foreach (Alert alert in alerts)
      {
      }
    }

    private static void updatePosts()
    {
      postManager.updatePosts();
      dashboardManager.sendAlerts();
    }
  }
}


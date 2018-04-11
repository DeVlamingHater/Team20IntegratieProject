using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.HtmlControls;
using BL;
using BL.Managers;
using Domain.Dashboards;

namespace PolitiekeBarometer_MVC.Controllers
{
  public class DashboardController : Controller

  {
    private DashboardManager mgr = new DashboardManager();

    public ActionResult getDashboard(int gebruikerId)
    {
      Dashboard dashboard = mgr.getDashboard(gebruikerId);
      IEnumerable<Zone> zones = mgr.getZones(dashboard);
      return View(zones);
    }
    public ActionResult getZone(int zoneId)
    {
      Zone zone = mgr.getZone(zoneId);
      return View(zone);
    }
  }
}
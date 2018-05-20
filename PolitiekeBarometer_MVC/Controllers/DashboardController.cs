using BL.Interfaces;
using BL.Managers;
using Domain.Dashboards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PolitiekeBarometer_MVC.Controllers
{
    public class DashboardController : Controller
    {
        public ActionResult ItemPartial(int ItemId)
        {
            IDashboardManager dashboardManager = new DashboardManager();
            Item item = dashboardManager.getItem(ItemId);
            return View();
        }
    }
}
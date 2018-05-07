﻿using BL.Interfaces;
using BL.Managers;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace PolitiekeBarometer_MVC.Controllers
{
    public class AlertController : Controller
    {
       public ActionResult Alerts()
        {
            IDashboardManager dashboardManager = new DashboardManager();

            return View(dashboardManager.getActiveAlerts());
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PolitiekeBarometer_MVC.Controllers
{
    public class ErrorController : BaseController
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AdminError()
        {
            return View();
        }
    }
}
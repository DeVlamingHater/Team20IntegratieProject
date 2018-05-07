using BL.Interfaces;
using BL.Managers;
using DAL.EF;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PolitiekeBarometer_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PolitiekeBarometer_MVC.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class SuperAdminController : Controller
    {
        PolitiekeBarometerContext context = new PolitiekeBarometerContext();

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser(FormCollection form)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            string naam = form["txtNaam"];
            string userName = form["txtEmail"];
            string email = form["txtEmail"];
            string password = form["txtPassword"];

            var user = new ApplicationUser();
            user.Name = naam;
            user.UserName = userName;
            user.Email = email;
            string pwd = password;

            IPlatformManager platformManager = new PlatformManager();
            
            var newuser = userManager.Create(user, pwd);
           ;

            platformManager.createGebruiker(userManager.Users.First(u => u.Email == user.Email).Id, user.Name, user.Email);
            return View("Index");
        }
        public ActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewRole(FormCollection Form)
        {
            string rolename = Form["RoleName"];
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            if (!roleManager.RoleExists(rolename))
            {
                var role = new IdentityRole(rolename);
                roleManager.Create(role);
            }
            return View("Index");
        }
        public ActionResult AssignRole()
        {
            ViewBag.Roles = context.Roles.Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignRole(FormCollection form)
        {
            string username = form["txtUserName"];
            string role = form["RoleName"];
            ApplicationUser user = (ApplicationUser)context.Users.Where(u => u.UserName.Equals(username, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            userManager.AddToRole(user.Id, role);
            return View("Index");
        }
    }
}
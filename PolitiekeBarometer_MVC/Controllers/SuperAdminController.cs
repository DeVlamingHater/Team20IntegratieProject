﻿using BL.Interfaces;
using BL.Managers;
using DAL.EF;
using Domain.Platformen;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PolitiekeBarometer_MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PolitiekeBarometer_MVC.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class SuperAdminController : Controller
    {
        // GET: SuperAdmin
        // Index pagina om te navigeren naar verschillende beheerpagina's
        public ActionResult Index()
        {
            return View();
        }

        #region Admin
        //GET: SuperAdmin/LijstAdmins
        public ActionResult LijstAdmins()
        {
            PolitiekeBarometerContext context = PolitiekeBarometerContext.Create();

            IPlatformManager platformManager = new PlatformManager();
            List<Gebruiker> gebruikers = platformManager.getAllGebruikers();

            List<ApplicationUser> users = new List<ApplicationUser>();

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            var adminRole = roleManager.FindByName("Admin");

            foreach (var gebruiker in gebruikers)
            {
                ApplicationUser user = userManager.FindByEmail(gebruiker.Email);
                if (user != null)
                {
                    foreach (var role in user.Roles)
                    {
                        if (role.RoleId == adminRole.Id)
                        {
                            users.Add(user);
                        }
                    }

                }
            }

            return View(users);

        }

        // GET: SuperAdmin/Details/5
        public ActionResult DetailsAdmin(string id)
        {
            PolitiekeBarometerContext context = new PolitiekeBarometerContext();

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var user = userManager.FindById(id);
            return View(user);


        }

        // GET: SuperAdmin/Edit/5
        public ActionResult EditAdmin(string id)
        {
            PolitiekeBarometerContext context = new PolitiekeBarometerContext();

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var user = userManager.FindById(id);
            return View(user);
        }

        // POST: SuperAdmin/Edit/5
        [HttpPost]
        public ActionResult EditAdmin(string id, ApplicationUser user)
        {
            try
            {
                PolitiekeBarometerContext dbContext = new PolitiekeBarometerContext();

                dbContext.Entry(user).State = EntityState.Modified;
                dbContext.SaveChanges();

                return RedirectToAction("LijstAdmins");
            }
            catch
            {
                return View();
            }
        }

        // GET: SuperAdmin/Delete/5
        public ActionResult DeleteAdmin(string id)
        {
            PolitiekeBarometerContext context = new PolitiekeBarometerContext();

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var user = userManager.FindById(id);
            return View(user);
        }

        // POST: SuperAdmin/Delete/5
        [HttpPost]
        public ActionResult DeleteAdmin(string id, ApplicationUser user)
        {
            try
            {
                PolitiekeBarometerContext context = new PolitiekeBarometerContext();
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                PlatformManager platformManager = new PlatformManager();

                //  platformManager.deleteGebruiker(id);

                ApplicationUser applicationUser = userManager.Users.Where(u => u.Id == id).FirstOrDefault();

                userManager.Delete(applicationUser);

                return RedirectToAction("LijstAdmins");
            }
            catch
            {
                return View();
            }
        }
        #endregion
    }
}
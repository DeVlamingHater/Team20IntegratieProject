using BL.Interfaces;
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
    public class SuperAdminController : BaseController
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
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var superAdminrole = roleManager.Roles.Where(r => r.Name == "SuperAdmin").First();

            List<ApplicationUser> usersLijst = userManager.Users.ToList();

            List<ApplicationUser> userLijstSuperAdmins = usersLijst.Where(u => u.Roles.Where(r => r.RoleId != superAdminrole.Id).Count() > 0).ToList();
        
            return View(userLijstSuperAdmins);

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

        #region Deelplatform 

        public ActionResult LijstDeelplatformen()
        {
            IPlatformManager platformManager = new PlatformManager();
            List<Deelplatform> deelplatformen = platformManager.getAllDeeplatformen();
            return View(deelplatformen);
        }

        // GET: Admin/Create
        public ActionResult CreateDeelplatform()
        {
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDeelplatform(FormCollection form)
        {
            Deelplatform deelplatform = new Deelplatform()
            {
                Naam = form["txtNaam"],
                PersoonString = form["txtPersoonstring"],
                OrganisatieString = form["txtOrganisatiestring"],
                ThemaString = form["txtThemastring"],
            };
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(PolitiekeBarometerContext.Create()));
            IdentityRole adminRole = new IdentityRole("Admin"+deelplatformURL);
            roleManager.Create(adminRole);
            IPlatformManager platformManager = new PlatformManager();
            platformManager.createDeelplatform(deelplatform);

            return RedirectToAction("LijstDeelplatformen");
        }

        public ActionResult DeleteDeelplatform(int id)
        {
            IPlatformManager platformManager = new PlatformManager();
            var deelplatform = platformManager.getDeelplatformById(id);

            return View(deelplatform);
        }

        [HttpPost]
        public ActionResult DeleteDeelplatform(int id, Deelplatform deelplatform)
        {
            IPlatformManager platformManager = new PlatformManager();
            Deelplatform deelPlatform = platformManager.getDeelplatformById(id);
            platformManager.deleteDeelplatform(deelPlatform);
            return RedirectToAction("Index");
        }

        public ActionResult AssignAdmin()
        {
            IPlatformManager platformManager = new PlatformManager();
            var ctx = PolitiekeBarometerContext.Create();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ctx));
       
            var admins = userManager.Users.Where(u => u.Roles.Count > 0).ToList();

            ViewBag.Deelplatformen = platformManager.getAllDeeplatformen();
            ViewBag.Admins = admins;
            return View();
        }
        [HttpPost]
        public ActionResult AssignAdmin(FormCollection form)
        {
            string email = form["txtAdmin"];
            string deelplatform = form["txtDeelplatform"];
            IPlatformManager platformManager = new PlatformManager();
            var ctx = PolitiekeBarometerContext.Create();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ctx));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(ctx));

            var user = userManager.Users.Where(u => u.Email == email).FirstOrDefault();

            userManager.AddToRole(user.Id, "Admin" + deelplatform);

            return View("Index");
        }

        #endregion

        #region timer
        public ActionResult Timer()
        {
            return View();
        }

        public ActionResult AssingRefreshRate(FormCollection form)
        {
            string refreshRateS = form["txtTimer"];
            int refreshRate = Int32.Parse(refreshRateS);
            Platform.interval = new TimeSpan(0, 0, 0, 0, refreshRate);
            Platform.refreshTimer.Interval = refreshRate;
            return View("Index");
        }

        #endregion

    }
}
using BL.Interfaces;
using BL.Managers;
using DAL.EF;
using Domain;
using Domain.Platformen;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PolitiekeBarometer_MVC.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class AdminController : BaseController
    {
        PolitiekeBarometerContext context = new PolitiekeBarometerContext();


        // GET: Admin
        // Index pagina om te navigeren naar verschillende beheerpagina's
        public ActionResult Index()
        {
            return View();
        }

        #region Users

        // GET: Admin/LijsUsers
        public ActionResult LijstUsers()
        {
            PolitiekeBarometerContext dbContext = new PolitiekeBarometerContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(dbContext));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(dbContext));

            List<ApplicationUser> usersLijst = userManager.Users.ToList();
            List<ApplicationUser> users = new List<ApplicationUser>();

            var adminRole = roleManager.FindByName("Admin");
            var superAdminRole = roleManager.FindByName("SuperAdmin");

            foreach (var user in usersLijst)
            {
                if (user.Roles.Count() == 0)
                {
                    users.Add(user);
                }

            }

            return View(users);
        }



        // GET: Admin/Details/5
        public ActionResult DetailsUser(string id)
        {
            PolitiekeBarometerContext context = new PolitiekeBarometerContext();

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var user = userManager.FindById(id);
            return View(user);
        }

        // GET: Admin/Create
        public ActionResult CreateUser()
        {
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser(FormCollection form)
        {
            IPlatformManager platformManager = new PlatformManager();
            var user = new ApplicationUser();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            string naam = form["txtNaam"];
            string userName = form["txtEmail"];
            string email = form["txtEmail"];
            string password = form["txtPassword"];

            user.Name = naam;
            user.UserName = userName;
            user.Email = email;
            string pwd = password;


            Gebruiker gebruiker = new Gebruiker()
            {
                Email = user.Email,
                Naam = user.Name,
                GebruikerId = user.Id
            };

            user.Gebruiker = gebruiker;

            var newuser = userManager.Create(user, pwd);
            string id = user.Id;

            platformManager.createGebruiker(gebruiker);
            return RedirectToAction("LijstUsers");
        }


        // GET: Admin/Edit/5
        public ActionResult EditUser(string id)
        {
            PolitiekeBarometerContext context = new PolitiekeBarometerContext();

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var user = userManager.FindById(id);
            return View(user);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public ActionResult EditUser(string id, ApplicationUser user)
        {
            try
            {
                PolitiekeBarometerContext dbContext = new PolitiekeBarometerContext();
                IPlatformManager platformManager = new PlatformManager();

                dbContext.Entry(user).State = EntityState.Modified;
                dbContext.SaveChanges();

                var gebruiker = platformManager.getGebruiker(user.Id);
                gebruiker.Naam = user.Name;
                gebruiker.Email = user.Email;
                platformManager.updateGebruiker(gebruiker);

                return RedirectToAction("LijstUsers");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Delete/5
        public ActionResult DeleteUser(string id)
        {
            PolitiekeBarometerContext context = new PolitiekeBarometerContext();

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var user = userManager.FindById(id);
            return View(user);
        }

        // POST: Admin/Delete/5
        [HttpPost]
        public ActionResult DeleteUser(string id, ApplicationUser user)
        {
            try
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                PlatformManager platformManager = new PlatformManager();

                ApplicationUser applicationUser = userManager.Users.Where(u => u.Id == id).FirstOrDefault();

                userManager.Delete(applicationUser);

                return RedirectToAction("LijstUsers");
            }
            catch
            {
                return View();
            }
        }

        #endregion

        #region Personen
        // GET: Admin/LijsUsers
        public ActionResult LijstPersonen()
        {
            IElementManager elementManager = new ElementManager();
            var personen = elementManager.getAllPersonen(Deelplatform);
            return View(personen);


        }

        // GET: Admin/Details/5
        public ActionResult DetailsPersoon(int id)
        {
            IElementManager elementManager = new ElementManager();
            var persoon = elementManager.getElementById(id);
            return View(persoon);
        }

        // GET: Admin/Create
        public ActionResult CreatePersoon()
        {
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePersoon(Persoon persoon)
        {
            IElementManager elementManager = new ElementManager();
            persoon.Deelplatform = Deelplatform;
            elementManager.addPersoon(persoon);

            return RedirectToAction("LijstPersonen");
        }


        // GET: Admin/Edit/5
        public ActionResult EditPersoon(int id)
        {
            IElementManager elementManager = new ElementManager();
            var persoon = elementManager.getElementById(id);
            return View(persoon);

        }

        // POST: Admin/Edit/5
        [HttpPost]
        public ActionResult EditPersoon(int id, Persoon persoon)
        {
            IElementManager elementManager = new ElementManager();
            elementManager.updatePersoon(persoon);
            return RedirectToAction("LijstPersonen");
        }

        // GET: Admin/Delete/5
        public ActionResult DeletePersoon(int id)
        {
            IElementManager elementManager = new ElementManager();
            var persoon = elementManager.getElementById(id);
            return View(persoon);
        }

        // POST: Admin/Delete/5
        [HttpPost]
        public ActionResult DeletePersoon(int id, FormCollection collection)
        {
            IElementManager elementManager = new ElementManager();
            Persoon persoon = (Persoon)elementManager.getElementById(id);
            elementManager.deletePersoon(persoon);
            return RedirectToAction("LijstPersonen");
        }
        #endregion

        #region Thema's
        // GET: Admin/LijsUsers
        public ActionResult LijstThemas()
        {
            IElementManager elementManager = new ElementManager();
            List<Thema> themas = elementManager.getAllThemas(Deelplatform);
            return View(themas);
        }

        // GET: Admin/Details/5
        public ActionResult DetailsThema(int id)
        {
            IElementManager elementManager = new ElementManager();
            Thema thema = (Thema)elementManager.getElementById(id);
            return View(thema);
        }

        // GET: Admin/Create
        public ActionResult CreateThema()
        {
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateThema(Thema thema)
        {
            IElementManager elementManager = new ElementManager();
            thema.Deelplatform = Deelplatform;
            elementManager.addThema(thema);

            return RedirectToAction("LijstThemas");
        }


        // GET: Admin/Edit/5
        public ActionResult EditThema(int id)
        {
            using (PolitiekeBarometerContext dbContext = new PolitiekeBarometerContext())
            {
                return View(dbContext.Themas.Where(p => p.Id == id).FirstOrDefault());
            }
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public ActionResult EditThema(int id, Thema thema)
        {
            IElementManager elementManager = new ElementManager();
            elementManager.updateThema(thema);

            return RedirectToAction("LijstThemas");

        }

        // GET: Admin/Delete/5
        public ActionResult DeleteThema(int id)
        {

            using (PolitiekeBarometerContext dbContext = new PolitiekeBarometerContext())
            {
                return View(dbContext.Themas.Where(p => p.Id == id).FirstOrDefault());
            }
        }

        // POST: Admin/Delete/5
        [HttpPost]
        public ActionResult DeleteThema(int id, FormCollection collection)
        {
            IElementManager elementManager = new ElementManager();
            Thema thema = (Thema)elementManager.getElementById(id);
            elementManager.deleteThema(thema);

            return RedirectToAction("LijstThemas");

        }
        #endregion

        #region Organisaties
        // GET: Admin/LijsUsers
        public ActionResult LijstOrganisaties()
        {
            IElementManager elementManager = new ElementManager();
            return View(elementManager.getAllOrganisaties(Deelplatform));
        }

        // GET: Admin/Details/5
        public ActionResult DetailsOrganisatie(int id)
        {
            IElementManager elementManager = new ElementManager();
            Organisatie organisatie = (Organisatie)elementManager.getElementById(id);
            using (PolitiekeBarometerContext dbContext = new PolitiekeBarometerContext())
            {
                return View(organisatie);
            }
        }

        // GET: Admin/Create
        public ActionResult CreateOrganisatie()
        {
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrganisatie(Organisatie organisatie)
        {
            IElementManager elementManager = new ElementManager();
            elementManager.addOrganisatie(organisatie);

            return RedirectToAction("LijstOrganisaties");
        }


        // GET: Admin/Edit/5
        public ActionResult EditOrganisatie(int id)
        {
            IElementManager elementManager = new ElementManager();
            return View((Organisatie)elementManager.getElementById(id));
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public ActionResult EditOrganisatie(int id, Organisatie organisatie)
        {
            IElementManager elementManager = new ElementManager();
            elementManager.updateOrganisatie(organisatie);

            return RedirectToAction("LijstOrganisaties");
        }

        // GET: Admin/Delete/5
        public ActionResult DeleteOrganisatie(int id)
        {
            return View();
        }

        // POST: Admin/Delete/5
        [HttpPost]
        public ActionResult DeleteOrganisatie(int id, FormCollection collection)
        {
            IElementManager elementManager = new ElementManager();
            Organisatie organisatie = (Organisatie)elementManager.getElementById(id);
            elementManager.deleteOrganisatie(organisatie);
            return RedirectToAction("LijstOrganisaties");

        }
        #endregion

        #region Export

        public void ExportPersonenToExcel()
        {
            IElementManager elementManager = new ElementManager();

            List<Persoon> personen = elementManager.getAllPersonen(Deelplatform);

            var grid = new GridView();

            grid.DataSource = from data in personen
                              select new
                              {
                                  Id = data.Id,
                                  Naam = data.Naam,
                                  Trend = data.Trend,
                                  TrendingPlaats = data.TrendingPlaats,
                                  Organisatie = data.Organisatie,
                                  District = data.District,
                                  Level = data.Level,
                                  Gender = data.Gender,
                                  Twitter = data.Twitter,
                                  Site = data.Site,
                                  DateOfBirth = data.DateOfBirth,
                                  Facebook = data.Facebook,
                                  Postal_code = data.Postal_code,
                                  Position = data.Position,
                                  Organisation = data.Organisation,
                                  Town = data.Town,
                                  Posts = data.Posts
                              };
            ExportData(grid);
        }
        public void ExportData(GridView grid)
        {
            grid.DataBind();

            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.AddHeader("Content-Disposition", "attachment; filename=Team20Personen.xls");
            Response.AddHeader("Content-Type", "application/Excel");
            Response.ContentType = "application/vnd.xls";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htmlTextWriter = new HtmlTextWriter(sw);

            grid.RenderControl(htmlTextWriter);

            Response.Write(sw.ToString());

            Response.End();
        }

        public void ExportThemasToExcel()
        {
            IElementManager elementManager = new ElementManager();
            List<Thema> themas = elementManager.getAllThemas(Deelplatform);

            var grid = new GridView();

            grid.DataSource = from data in themas
                              select new
                              {
                                  Id = data.Id,
                                  Naam = data.Naam,
                                  Trend = data.Trend,
                                  TrendingPlaats = data.TrendingPlaats,
                                  Keywords = data.Keywords
                              };
            ExportData(grid);
        }

        public void ExportOrganisatiesToExcel()
        {
            IElementManager elementManager = new ElementManager();

            List<Organisatie> organisaties = elementManager.getAllOrganisaties(Deelplatform);

            var grid = new GridView();

            grid.DataSource = from data in organisaties
                              select new
                              {
                                  Id = data.Id,
                                  Naam = data.Naam,
                                  Trend = data.Trend,
                                  TrendingPlaats = data.TrendingPlaats,
                                  personen = data.Personen
                              };
            ExportData(grid);
        }

        public void ExportUsersToExcel()
        {

            IPlatformManager platformManager = new PlatformManager();

            List<Gebruiker> gebruikers = platformManager.getAllGebruikers();

            List<ApplicationUser> users = new List<ApplicationUser>();

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            foreach (var gebruiker in gebruikers)
            {
                ApplicationUser user = userManager.FindByEmail(gebruiker.Email);
                if (user != null)
                {
                    users.Add(user);
                }
            }

            var grid = new GridView();

            grid.DataSource = from data in users
                              select new
                              {
                                  Id = data.Id,
                                  Email = data.Email,
                                  EmailConfirmed = data.EmailConfirmed,
                                  PasswordHash = data.PasswordHash,
                                  SecurityStamp = data.SecurityStamp,
                                  PhoneNumber = data.PhoneNumber,
                                  PhoneNumberConfirmed = data.PhoneNumberConfirmed,
                                  TwoFactorEnabled = data.TwoFactorEnabled,
                                  LockOutEndDateUtc = data.LockoutEndDateUtc,
                                  LockoutEnabled = data.LockoutEnabled,
                                  AccessFailedCount = data.AccessFailedCount,
                                  Username = data.UserName,
                                  Name = data.Name,
                                  Gebruiker = data.Gebruiker
                              };
            ExportData(grid);

        }


        #endregion
    }
}

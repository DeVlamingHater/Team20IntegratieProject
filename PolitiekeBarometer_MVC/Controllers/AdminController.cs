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
    public class AdminController : Controller
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
            IPlatformManager platformManager = new PlatformManager();

            var newuser = userManager.Create(user, pwd);
            string id = user.Id;

            platformManager.createGebruiker(gebruiker.GebruikerId, gebruiker.Naam, gebruiker.Email);
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
            var personen = elementManager.getAllPersonen();
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
            try
            {
                PolitiekeBarometerContext dbContext = new PolitiekeBarometerContext();

                dbContext.Entry(persoon).State = EntityState.Modified;
                dbContext.SaveChanges();

                return RedirectToAction("LijstPersonen");
            }
            catch
            {
                return View();
            }
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
            try
            {
                IElementManager elementManager = new ElementManager();

                Persoon persoon = (Persoon)elementManager.getElementById(id);
                elementManager.deletePersoon(persoon);
                

                return RedirectToAction("LijstPersonen");
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region Thema's
        // GET: Admin/LijsUsers
        public ActionResult LijstThemas()
        {
            using (PolitiekeBarometerContext dbContext = new PolitiekeBarometerContext())
            {
                return View(dbContext.Themas.ToList());
            }

        }

        // GET: Admin/Details/5
        public ActionResult DetailsThema(int id)
        {
            using (PolitiekeBarometerContext dbContext = new PolitiekeBarometerContext())
            {
                return View(dbContext.Themas.Where(p => p.Id == id).FirstOrDefault());
            }
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
            using (PolitiekeBarometerContext dbContext = new PolitiekeBarometerContext())
            {
                dbContext.Themas.Add(thema);
                dbContext.SaveChanges();
            }
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
            try
            {
                using (PolitiekeBarometerContext dbContext = new PolitiekeBarometerContext())
                {
                    dbContext.Entry(thema).State = EntityState.Modified;
                    dbContext.SaveChanges();
                }
                return RedirectToAction("LijstThemas");
            }
            catch
            {
                return View();
            }
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
            try
            {
                using (PolitiekeBarometerContext dbContext = new PolitiekeBarometerContext())
                {
                    Thema thema = dbContext.Themas.Where(t => t.Id == id).FirstOrDefault();
                    dbContext.Themas.Remove(thema);
                    dbContext.SaveChanges();
                }

                return RedirectToAction("LijstThemas");
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region Organisaties
        // GET: Admin/LijsUsers
        public ActionResult LijstOrganisaties()
        {
            using (PolitiekeBarometerContext dbContext = new PolitiekeBarometerContext())
            {
                return View(dbContext.Organisaties.ToList());
            }

        }

        // GET: Admin/Details/5
        public ActionResult DetailsOrganisatie(int id)
        {
            using (PolitiekeBarometerContext dbContext = new PolitiekeBarometerContext())
            {
                return View(dbContext.Organisaties.Where(p => p.Id == id).FirstOrDefault());
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
            using (PolitiekeBarometerContext dbContext = new PolitiekeBarometerContext())
            {
                dbContext.Organisaties.Add(organisatie);
                dbContext.SaveChanges();
            }
            return RedirectToAction("LijstOrganisaties");
        }


        // GET: Admin/Edit/5
        public ActionResult EditOrganisatie(int id)
        {
            using (PolitiekeBarometerContext dbContext = new PolitiekeBarometerContext())
            {
                return View(dbContext.Organisaties.Where(p => p.Id == id).FirstOrDefault());
            }
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public ActionResult EditOrganisatie(int id, Organisatie organisatie)
        {
            try
            {
                using (PolitiekeBarometerContext dbContext = new PolitiekeBarometerContext())
                {
                    dbContext.Entry(organisatie).State = EntityState.Modified;
                    dbContext.SaveChanges();
                }
                return RedirectToAction("LijstOrganisaties");
            }
            catch
            {
                return View();
            }
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
            try
            {
                using (PolitiekeBarometerContext dbContext = new PolitiekeBarometerContext())
                {
                    Organisatie organisatie = dbContext.Organisaties.Where(t => t.Id == id).FirstOrDefault();
                    dbContext.Organisaties.Remove(organisatie);
                    dbContext.SaveChanges();
                }

                return RedirectToAction("LijstOrganisaties");
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region Export

        public void ExportPersonenToExcel()
        {
            using (PolitiekeBarometerContext dbContext = new PolitiekeBarometerContext())
            {
                List<Persoon> personen = dbContext.Personen.ToList();

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
        }

        public void ExportThemasToExcel()
        {
            using (PolitiekeBarometerContext dbContext = new PolitiekeBarometerContext())
            {
                List<Thema> themas = dbContext.Themas.ToList();

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

                grid.DataBind();

                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.AddHeader("Content-Disposition", "attachment; filename=Team20Themas.xls");
                Response.AddHeader("Content-Type", "application/Excel");
                Response.ContentType = "application/vnd.xls";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htmlTextWriter = new HtmlTextWriter(sw);

                grid.RenderControl(htmlTextWriter);

                Response.Write(sw.ToString());

                Response.End();
            }



        }

        public void ExportOrganisatiesToExcel()
        {
            using (PolitiekeBarometerContext dbContext = new PolitiekeBarometerContext())
            {
                List<Organisatie> organisaties = dbContext.Organisaties.ToList();

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

                grid.DataBind();

                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.AddHeader("Content-Disposition", "attachment; filename=Team20Organisaties.xls");
                Response.AddHeader("Content-Type", "application/Excel");
                Response.ContentType = "application/vnd.xls";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htmlTextWriter = new HtmlTextWriter(sw);

                grid.RenderControl(htmlTextWriter);

                Response.Write(sw.ToString());

                Response.End();
            }



        }

        public void ExportUsersToExcel()
        {
            using (PolitiekeBarometerContext dbContext = new PolitiekeBarometerContext())
            {
                IPlatformManager platformManager = new PlatformManager();

                List<Gebruiker> gebruikers = dbContext.Gebruikers.ToList();

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

                grid.DataBind();

                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.AddHeader("Content-Disposition", "attachment; filename=Team20Users.xls");
                Response.AddHeader("Content-Type", "application/Excel");
                Response.ContentType = "application/vnd.xls";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htmlTextWriter = new HtmlTextWriter(sw);

                grid.RenderControl(htmlTextWriter);

                Response.Write(sw.ToString());

                Response.End();
            }
        }


        #endregion
    }
}

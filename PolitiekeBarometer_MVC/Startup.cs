using System;
using System.Configuration;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Owin;
using PolitiekeBarometer_MVC.Models;
using PolitiekeBarometer_MVC.Providers;
using DAL.EF;
using System.Timers;
using BL.Managers;
using Domain.Platformen;
using BL.Interfaces;
using System.Threading.Tasks;

[assembly: OwinStartupAttribute(typeof(PolitiekeBarometer_MVC.Startup))]
namespace PolitiekeBarometer_MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            ConfigureOAuthTokenGeneration(app);
            ConfigureOAuthTokenConsumption(app);
            CreateUserAndRoles();
            UnitOfWorkManager uowMgr = new UnitOfWorkManager();
            IElementManager elementManager = new ElementManager(uowMgr);
            PlatformManager platformManager = new PlatformManager(uowMgr);
            Deelplatform deelplatform = platformManager.getDeelplatformByNaam("Politiek");
            elementManager.deleteAllPersonen(deelplatform);
            elementManager.addPersonen(elementManager.readJSONPolitici());
            elementManager.setTrendingElementen(deelplatform);
           UpdateAsync();
            SetTimer();
            uowMgr.Save();
        }

        private void SetTimer()
        {
            Timer refreshTimer = new Timer();

            Platform.refreshTimer.Elapsed += new ElapsedEventHandler(UpdateAPIAsync);
            Platform.refreshTimer.Interval = Platform.interval.TotalMilliseconds;
            Platform.refreshTimer.Enabled = true;
            Platform.refreshTimer.Start();
        }
        private static async Task UpdateAsync()
        {
            UnitOfWorkManager uowManager = new UnitOfWorkManager();
            IElementManager elementManager = new ElementManager(uowManager);
            IDashboardManager dashboardManager = new DashboardManager(uowManager);
            IPlatformManager platformManager = new PlatformManager(uowManager);
            IPostManager postManager = new PostManager(uowManager);
            DateTime lastUpdate = Platform.lastUpdate;
            Deelplatform deelplatform = platformManager.getDeelplatformByNaam("Politiek");

            string responseString = await postManager.updatePosts(Platform.lastUpdate);

            postManager.addJSONPosts(responseString);
            postManager.deleteOldPosts();

            elementManager.setTrendingElementen(deelplatform);
            dashboardManager.sendAlerts();
            Platform.refreshTimer.Interval = Platform.interval.TotalMilliseconds;

            Platform.lastUpdate = DateTime.Now;
            uowManager.Save();
        }
        private static async void UpdateAPIAsync(object source, ElapsedEventArgs e)
        {
             UpdateAsync();
        }

        private void ConfigureOAuthTokenConsumption(IAppBuilder app)
        {
            var issuer = "https://localhost:44301";
            string audienceId = ConfigurationManager.AppSettings["as:AudienceId"];
            byte[] audienceSecret = TextEncodings.Base64Url.Decode(ConfigurationManager.AppSettings["as:AudienceSecret"]);

            // Api controllers with an [Authorize] attribute will be validated with JWT
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    AllowedAudiences = new[] { audienceId },
                    IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                    {
                        new SymmetricKeyIssuerSecurityTokenProvider(issuer, audienceSecret)
                    }
                });
        }

        private void ConfigureOAuthTokenGeneration(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(PolitiekeBarometerContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                //For Dev enviroment only (on production should be AllowInsecureHttp = false)
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/oauth/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new CustomOAuthProvider(),
                AccessTokenFormat = new CustomJwtFormat("https://localhost:44301")
            };

            // OAuth 2.0 Bearer Access Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
        }



        private void CreateUserAndRoles()
        {
            PolitiekeBarometerContext context = new PolitiekeBarometerContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!roleManager.RoleExists("SuperAdmin"))
            {
                var role = new IdentityRole("SuperAdmin");
                roleManager.Create(role);

                var user = new ApplicationUser();
                user.Name = "Thomas Somers";
                user.UserName = "thomas.somers@student.kdg.be";
                user.Email = "thomas.somers@student.kdg.be";
                user.EmailConfirmed = true;

                user.Gebruiker = new Gebruiker()
                {
                    Email = user.Email,
                    GebruikerId = user.Id,
                    Naam = user.Name
                };
                string pwd = "ThomasSuperAdmin20";

                var newuser = userManager.Create(user, pwd);
                if (newuser.Succeeded)
                {
                    userManager.AddToRole(user.Id, "SuperAdmin");
                }
            }

            if (!roleManager.RoleExists("AdminPolitiek"))
            {
                var role = new IdentityRole("Admin"+"Politiek");
                roleManager.Create(role);

                var user = new ApplicationUser();
                user.Name = "Thomas Admin";
                user.UserName = "thomas.somers@live.nl";
                user.Email = "thomas.somers@live.nl";
                user.EmailConfirmed = true;
                string pwd = "ThomasAdmin20";
                user.Gebruiker = new Gebruiker()
                {
                    Email = user.Email,
                    GebruikerId = user.Id,
                    Naam = user.Name
                };
                var newuser = userManager.Create(user, pwd);
                if (newuser.Succeeded)
                {
                    userManager.AddToRole(user.Id, "AdminPolitiek");
                }

                var gewoneGebruiker = new ApplicationUser();
                gewoneGebruiker.Name = "Sam";
                gewoneGebruiker.UserName = "sam.claessen@student.kdg.be";
                gewoneGebruiker.Email = "sam.claessen@student.kdg.be";
                gewoneGebruiker.EmailConfirmed = true;
                string gebruikerPW = "SamUser";
                gewoneGebruiker.Gebruiker = new Gebruiker()
                {
                    Email = gewoneGebruiker.Email,
                    GebruikerId = gewoneGebruiker.Id,
                    Naam = gewoneGebruiker.Name
                };
                var User = userManager.Create(gewoneGebruiker, gebruikerPW);

            }
        }
    }
}

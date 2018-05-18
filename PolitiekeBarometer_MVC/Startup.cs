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
            createUserAndRoles();
           SetTimer();
        }

        private void SetTimer()
        {
            Timer refreshTimer = new Timer();

            Platform.refreshTimer.Elapsed += new ElapsedEventHandler(UpdateAPIAsync);
            Platform.refreshTimer.Interval = Platform.interval;
            Platform.refreshTimer.Enabled = true;
            Platform.refreshTimer.Start();
        }

        private static async void UpdateAPIAsync(object source, ElapsedEventArgs e)
        {
            IElementManager elementManager = new ElementManager();
            IDashboardManager dashboardManager = new DashboardManager();
            IPostManager postManager = new PostManager();
            string responseString = await postManager.updatePosts(DateTime.Now.AddDays(-7));

            postManager.addJSONPosts(responseString);
            postManager.deleteOldPosts();

            elementManager.setTrendingElementen();
            dashboardManager.sendAlerts();
            Platform.refreshTimer.Interval = Platform.interval;
        }
        
        private void ConfigureOAuthTokenConsumption(IAppBuilder app)
        {
            var issuer = "http://localhost:44301";
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
                AccessTokenFormat = new CustomJwtFormat("http://localhost:44301")
            };

            // OAuth 2.0 Bearer Access Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
        }



        private void createUserAndRoles()
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

                user.Gebruiker = new Domain.Platformen.Gebruiker()
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

            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole("Admin");
                roleManager.Create(role);

                var user = new ApplicationUser();
                user.Name = "Thomas Admin";
                user.UserName = "thomas.somers@live.nl";
                user.Email = "thomas.somers@live.nl";
                user.EmailConfirmed = true;
                string pwd = "ThomasAdmin20";
                user.Gebruiker = new Domain.Platformen.Gebruiker()
                {
                    Email = user.Email,
                    GebruikerId = user.Id,
                    Naam = user.Name
                };
                var newuser = userManager.Create(user, pwd);
                if (newuser.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Admin");
                }
            }
        }
    }
}

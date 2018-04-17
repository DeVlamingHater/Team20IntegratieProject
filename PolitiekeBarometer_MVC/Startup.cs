using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using PolitiekeBarometer_MVC.Models;

[assembly: OwinStartupAttribute(typeof(PolitiekeBarometer_MVC.Startup))]
namespace PolitiekeBarometer_MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createUserAndRoles();
        }

        private void createUserAndRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!roleManager.RoleExists("SuperAdmin"))
            {
                var role = new IdentityRole("SuperAdmin");
                roleManager.Create(role);

                var user = new ApplicationUser();
                user.Name = "Thomas SuperAdmin";
                user.UserName = "thomas.somers@student.kdg.be";
                user.Email = "thomas.somers@student.kdg.be";
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
                string pwd = "ThomasAdmin20";

                var newuser = userManager.Create(user, pwd);
                if (newuser.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Admin");
                }
            }
        }
    }
}

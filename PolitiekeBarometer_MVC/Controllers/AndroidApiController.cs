using BL.Interfaces;
using BL.Managers;
using DAL.EF;
using Domain.Platformen;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace PolitiekeBarometer_MVC.Controllers
{

    [Authorize]
    public class AndroidApiController : ApiController
    {

        [HttpGet]
        public String Get()
        {
            String tekst = "hallo";
            return tekst;

        }




        [HttpGet]
        public String Get(string id)
        {
            IPlatformManager platformManager = new PlatformManager();
            DashboardManager dashboardManager = new DashboardManager();

            Gebruiker gebruiker = platformManager.getGebruiker(id);


            return gebruiker.Naam;

        }







        //[HttpGet]
        //[Authorize(Roles ="Admin,SuperAdmin",Claims="Platform|1")]
        //public IEnumerable<string> Get()
        //{
        //    var user = this.User as System.Security.Claims.ClaimsPrincipal;
        //    user.Claims.SingleOrDefault();
        //    //var newClaim = new Claim("Platform", "1");
        //    (this.User.Identity as ClaimsIdentity).HasClaim(c => c.Type == "Platform" && c.Value == "1");
        //    identity.
        //    return new string[] { "User1", "User2" };
        //}



    }
}

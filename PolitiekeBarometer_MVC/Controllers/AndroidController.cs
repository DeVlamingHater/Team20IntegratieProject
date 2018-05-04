using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;


namespace PolitiekeBarometer_MVC.Controllers
{
    public class AndroidController : ApiController
    {

        //public IHttpActionResult Get(int userId)
        //{
        //    dynamic dashboard;
        //    dashboard = BL.GetUser(userId);
        //    return Ok(dashboard);
        //}
        //private ApplicationSignInManager _signInManager;

        //public ApplicationSignInManager SignInManager {
        //    get {
        //        return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
        //    }
        //    private set {
        //        _signInManager = value;
        //    }
        //}
        
        //public async Task<bool> Login(string Email, string Password)
        //{
        //    var result = await SignInManager.PasswordSignInAsync(Email, Password, false, shouldLockout: false);
        //    switch (result)
        //    {
        //        case SignInStatus.Success:
        //            return true;
        //        case SignInStatus.RequiresVerification:
        //            return false;
        //        case SignInStatus.Failure:
        //        default:
        //            return false;
        //    }
        //}


    }
}

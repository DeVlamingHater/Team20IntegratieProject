using DAL.EF;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PolitiekeBarometer_MVC.Controllers.ActionFilters
{
    public class AdminFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Uri url = HttpContext.Current.Request.Url;

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(PolitiekeBarometerContext.Create()));
            var user = System.Web.HttpContext.Current.User.Identity;
            string[] segments = url.AbsolutePath.Split('/');
            string deelplatform = segments[1];
            if (user.IsAuthenticated)
            {
                var roles = userManager.GetRoles(user.GetUserId());
                if (roles.Contains("SuperAdmin") || roles.Contains("Admin" + deelplatform))
                {
                    base.OnActionExecuting(filterContext);
                }
                else
                {
                    filterContext.Result = new RedirectResult("/"+deelplatform+"/Error/AdminError");
                }
            }
            else
            {
                filterContext.Result = new RedirectResult("/"+deelplatform+"/Account/Login");
            }



        }
    }
}
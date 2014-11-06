using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using systems.Models;
using systems.Helpers;
using System.Web.Routing;

namespace systems.Controllers
{
    public class HomeController : Controller
    {

        public LocalBankMembershipProvider MembershipService { get; set; }
        public LocalBankRoleProvider AuthorizationService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (MembershipService == null)
                MembershipService = new LocalBankMembershipProvider();
            if (AuthorizationService == null)
                AuthorizationService = new LocalBankRoleProvider();

            base.Initialize(requestContext);
        }

        //
        // GET: /Home/
        CommonEntities db = new CommonEntities();

        public ActionResult Index()
        {
            return View();
        }

        // Systems Menu
        public ActionResult Menu(UserErrata user)
        {
            FormsAuthentication.SetAuthCookie(user.username, true);
            return View(user);
        }

        // Transfer in client.
        // Return them to the system menu.
        //[HttpPost]
        public ActionResult Transfer(string id, string atype, string target)
        {
            // register the authentication cookie with passed userid
//            FormsAuthentication.SetAuthCookie(id, true);
            // create errata object to pass authentication method
            UserErrata user = new UserErrata();
            user.username = id;
            user.authtype = atype;
            FormsAuthentication.SetAuthCookie(user.username, true);
            //return RedirectToAction("Index", target, user);
            return RedirectToAction("Index", target, id);
            //return RedirectToAction("Faculty");

           // return RedirectToAction("Index", target, new { id = user.username });
            //return RedirectToAction("EditFacultyProfile", target, new { profileId = user.username });
            //return RedirectToAction("EditFacultyProfile", target, new { profileId = "henderson" });
             //return RedirectToAction("EditFacultyProfile", target, new { profileId = "luzadis"});

            //string hijack = fns.hijackRequest(target,id, atype);
           // if (hijack == "" || hijack == null)
           // {
                // pass to menu of applications
                //return RedirectToAction("Menu", "Home", user);
           // }
           // else
            //{
               // return Redirect(hijack);
         //   }
        }

        // Transfer in (single parameter for VBS clients)
        public ActionResult TransferIS(string id)
        {
            string[] param = id.Split(':');
            string usern, atype, xfer = "";
            if (param.Length == 3) // should be username:atype:transfer
            {
                usern = param[0];
                atype = param[1];
                xfer = param[2];
                // register the authentication cookie with passed userid
                FormsAuthentication.SetAuthCookie(usern, true);
                // create errata object to pass authentication method
                UserErrata user = new UserErrata();
                user.username = usern;
                user.authtype = atype;

                string hijack = fns.hijackRequest(xfer, usern, atype);
                if (hijack == "" || hijack == null)
                {
                    // pass to menu of applications
                    return RedirectToAction("Menu", "Home", user);
                }
                else if (xfer == "fpim")
                {
                    return Redirect("http://www.esf.edu/faculty/admin/admit.asp?u=" + usern + "&refer=ESFIS");
                }
                else
                {
                    return Redirect(hijack);
                }
            }
            else
            {
                return RedirectToAction("Menu", "Home", param[0]);
            }
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return Redirect("https://wwwinfo.esf.edu/webscripts/authenticate/");
        }


    }
}

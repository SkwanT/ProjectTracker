using ProjectTracker.DAL;
using ProjectTracker.Models;
using System;
using System.Data;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ProjectTracker.Controllers
{
    public class AccountController : Controller
    {

        private IAccountRepository accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }


        [HttpGet]
        public ActionResult Login()
        {
            //User u = new User();
            //u.UserName = "admin";
            //u.Password = "admin";
            //return View(u);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User model, string returnUrl = "")
        {
            if (ModelState.IsValid)
            {
                string result = accountRepository.Login(model.UserName, model.Password);
                string[] res = result.Split(';');

                if (result != "")
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false);
                    var authTicket = new FormsAuthenticationTicket(1, model.UserName, DateTime.Now, DateTime.Now.AddMinutes(30), false, res[0] + ";" + res[1]);
                    string encriptedTicket = FormsAuthentication.Encrypt(authTicket);
                    var AuthCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encriptedTicket);
                    HttpContext.Response.Cookies.Add(AuthCookie);


                    if (returnUrl != "")
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {

                        return RedirectToAction("Index", "home");
                    }

                }
                else
                {
                    model.Password = "";
                }

            }
            return View(model);
        }

        [Authorize]
        public ActionResult LogOff()
        {

            FormsAuthentication.SignOut();

            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();

            return RedirectToAction("Login", "Account");
        }


        [Authorize]
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(UserChangePassword ucp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    FormsIdentity myid = (FormsIdentity)HttpContext.User.Identity;
                    string[] userdata = myid.Ticket.UserData.ToString().Split(';');
                    if (accountRepository.ChangePassword(ucp, Convert.ToInt32(userdata[0])))
                    {
                        accountRepository.Save();
                        return RedirectToAction("PasswordChanged");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Unable to save changes. Something's wrong.");
                    }

                }
            }
            catch (DataException)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
            }

            return View(ucp);
        }

        [Authorize]
        [HttpGet]
        public ActionResult PasswordChanged()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && accountRepository != null)
                accountRepository.Dispose();
            base.Dispose(disposing);
        }

    }
}
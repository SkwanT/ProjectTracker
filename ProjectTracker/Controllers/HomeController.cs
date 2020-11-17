using System;
using System.Web.Mvc;
using System.Web.Security;

namespace ProjectTracker.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            FormsIdentity myid = (FormsIdentity)HttpContext.User.Identity;
            string[] userdata = myid.Ticket.UserData.ToString().Split(';');
            int user = Convert.ToInt32(userdata[0]);

            System.Web.HttpContext.Current.Session["userID"] = user;

            return View();
        }

    }
}
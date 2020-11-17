using ProjectTracker.DAL;
using ProjectTracker.Helpers;
using ProjectTracker.Models;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Security;


namespace ProjectTracker.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {

        private IAuthorRepository userRepository;
        public UsersController(IAuthorRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpGet]
        public ActionResult Index(string Search, string sortOrder = "lastname")
        {

            ViewBag.LastNameSortParm = sortOrder == "lastname" ? "lastname_desc" : "lastname";
            ViewBag.FirstNameSortParm = sortOrder == "firstname" ? "firstname_desc" : "firstname";
            ViewBag.UserNameSortParm = sortOrder == "username" ? "username_desc" : "username";
            ViewBag.RolesSortParm = sortOrder == "roles" ? "roles_desc" : "roles";
            ViewBag.ActiveSortParm = sortOrder == "active" ? "active_desc" : "active";

            ViewBag.Search = Search;

            var users = userRepository.GetUsers(Search);

            users = SortingHelper.SortUsers(users, sortOrder);

            ViewBag.n = users.Count();
            return View(users.ToList());

        }


        [HttpGet]
        public ActionResult Create()
        {
            NewUser nu = new NewUser();

            ViewBag.RoleID = new SelectList(userRepository.GetRoles(), "ID", "RoleName", nu.RoleID);

            nu.previousurl = HttpContext.Request.UrlReferrer.ToString();
            return View(nu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NewUser nu)
        {

            ViewBag.RoleID = new SelectList(userRepository.GetRoles(), "ID", "RoleName", nu.RoleID);

            try
            {
                if (ModelState.IsValid)
                {
                    FormsIdentity myid = (FormsIdentity)HttpContext.User.Identity;
                    string[] userdata = myid.Ticket.UserData.ToString().Split(';');

                    if (userRepository.InsertUser(nu, Convert.ToInt32(userdata[0])))
                    {
                        userRepository.Save();
                        return Redirect(nu.previousurl);
                    }
                    else
                    {
                        throw new System.Exception();
                    }
                }
            }

            catch
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
            }

            return View(nu);
        }



        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AuthorUserEdit user = userRepository.GetUserByID(id);

            ViewBag.RoleID = new SelectList(userRepository.GetRoles(), "ID", "RoleName", user.RoleID);

            user.previousurl = HttpContext.Request.UrlReferrer.ToString();
            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AuthorUserEdit user)
        {

            ViewBag.RoleID = new SelectList(userRepository.GetRoles(), "ID", "RoleName", user.RoleID);
            try
            {

                if (ModelState.IsValid)
                {
                    FormsIdentity myid = (FormsIdentity)HttpContext.User.Identity;
                    string[] userdata = myid.Ticket.UserData.ToString().Split(';');

                    if (userRepository.UpdateUser(user, Convert.ToInt32(userdata[0])))
                    {
                        userRepository.Save();
                        return Redirect(user.previousurl);
                    }
                    else
                    {
                        throw new System.Exception();
                    }
                }
            }

            catch
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
            }

            return View(user);
        }



        [HttpGet]
        public ActionResult Delete(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AuthorUserEdit user = userRepository.GetUserByID(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            user.previousurl = HttpContext.Request.UrlReferrer.ToString();

            return View(user);
        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string previousurl)
        {

            try
            {
                FormsIdentity myid = (FormsIdentity)HttpContext.User.Identity;
                string[] userdata = myid.Ticket.UserData.ToString().Split(';');

                AuthorUserEdit user = userRepository.GetUserByID(id);
                if (user != null)
                {
                    if (userRepository.DeleteUser(id, Convert.ToInt32(userdata[0])))
                    {
                        userRepository.Save();
                        return Redirect(previousurl);
                    }
                    else
                    {
                        throw new System.Exception();
                    }
                }
            }

            catch
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
            }

            AuthorUserEdit user1 = userRepository.GetUserByID(id);

            if (user1 == null)
            {
                return HttpNotFound();
            }

            user1.previousurl = previousurl;

            return View(user1);
        }

        protected override void Dispose(bool disposing)
        {
            userRepository.Dispose();
            base.Dispose(disposing);
        }


        [HttpGet]
        public ActionResult ResetPassword(int? id, string username)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AuthorUserEdit uuser = userRepository.GetUserByID(id);

            if (uuser == null)
            {
                return HttpNotFound();
            }

            ResetPassword user = new ResetPassword();
            user.ID = id;
            user.UserName = username;
            user.previousurl = HttpContext.Request.UrlReferrer.ToString();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPassword user)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    FormsIdentity myid = (FormsIdentity)HttpContext.User.Identity;
                    string[] userdata = myid.Ticket.UserData.ToString().Split(';');

                    if (userRepository.ResetPassword(user, Convert.ToInt32(userdata[0])))
                    {
                        userRepository.Save();
                        return Redirect(user.previousurl);
                    }
                    else
                    {
                        throw new System.Exception();
                    }
                }
            }

            catch
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
            }

            return View(user);
        }

        [HttpPost]
        public JsonResult IsUserNameExists(string UserName)
        {
            var result = IsExist(UserName)
                ? Json(false, JsonRequestBehavior.AllowGet)
                : Json(true, JsonRequestBehavior.AllowGet);
            return result;
        }
        public bool IsExist(string UserName)
        {
            return userRepository.IsUserNameExists(UserName);

        }

    }
}
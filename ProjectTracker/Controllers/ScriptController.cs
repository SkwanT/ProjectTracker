using OfficeOpenXml;
using ProjectTracker.DAL;
using ProjectTracker.Helpers;
using ProjectTracker.Infrastructure;
using ProjectTracker.Models;
using ProjectTracker.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ProjectTracker.Controllers
{
    [NoCache]
    [Authorize]
    public class ScriptController : Controller
    {

        private IScriptRepository scriptRepository;

        public ScriptController(IScriptRepository scriptRepository)
        {
            this.scriptRepository = scriptRepository;
        }

        public int PageSize = 15;

        public ActionResult Index(SearchFilter searchFilter, string searchText, bool isFilterVisible = false, string sortOrder = "date_desc", int page = 1)
        {

            List<SelectListItem> filterStatus = new List<SelectListItem>() {
                new SelectListItem {Text = "Unfinished", Value = false.ToString()},
                new SelectListItem {Text = "Finished", Value =  true.ToString()                },
            };

            ViewBag.FilterStatus = filterStatus;

            System.Web.HttpContext.Current.Session["URL"] = Request.Url.AbsoluteUri.ToString();
            System.Web.HttpContext.Current.Session["Scriptsort"] = sortOrder.ToString();

            ViewBag.DateSortParam = sortOrder == "date" ? "date_desc" : "date";
            ViewBag.ScriptSortParam = sortOrder == "script" ? "script_desc" : "script";
            ViewBag.TypeSortParam = sortOrder == "type" ? "type_desc" : "type";
            ViewBag.AuthorSortParam = sortOrder == "author" ? "author_desc" : "author";
            ViewBag.ProjectSortParam = sortOrder == "project" ? "project_desc" : "project";
            ViewBag.StatusSortParam = sortOrder == "status" ? "status_desc" : "status";
            ViewBag.LocationSortParam = sortOrder == "location" ? "location_desc" : "location";
            ViewBag.CommentSortParam = sortOrder == "comment" ? "comment_desc" : "comment";

            var scripts = scriptRepository.GetScripts(searchText);

            ViewBag.Search = searchText;
            ViewBag.SearchFilter = searchFilter;

            if (searchFilter != null)
            {
                if (searchFilter.FromDate != null || searchFilter.ToDate != null || !string.IsNullOrEmpty(searchFilter.ScriptName) || searchFilter.ScriptTypeID != null || searchFilter.AuthorID != null || !string.IsNullOrEmpty(searchFilter.ProjectName) || !string.IsNullOrEmpty(searchFilter.ProjectLocation) || searchFilter.isFinished != null)
                {
                    scripts = FilteringHelper.FilterScripts(scripts, searchFilter);
                    isFilterVisible = true;
                }
            }

            ViewBag.SortOrder = sortOrder;

            scripts = SortingHelper.SortScripts(scripts, sortOrder);

            ScriptsListViewModel model = new ScriptsListViewModel
            {
                Scripts = scripts
               .Skip((page - 1) * PageSize)
               .Take(PageSize),

                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = scripts.Count()
                },

                SearchFilter = searchFilter,
                IsFilterVisible = isFilterVisible
            };


            if (Request.IsAuthenticated)
            {
                ViewBag.User = Convert.ToInt32(System.Web.HttpContext.Current.Session["userID"]);
            }

            ViewBag.ScriptTypeID = new SelectList(scriptRepository.GetScriptTypes(), "ID", "Type");
            ViewBag.AuthorID = new SelectList(scriptRepository.GetAuthorByActiveUser(null), "ID", "FullName", model.SearchFilter.AuthorID);


            return View(model);
        }



        [Authorize(Roles = "Admin,Scripter")]
        public ActionResult Create()
        {
            Script script = new Script();

            if (Request.IsAuthenticated && User.IsInRole("Scripter"))
            {
                int user = Convert.ToInt32(System.Web.HttpContext.Current.Session["userID"]);

                ViewBag.AuthorID = new SelectList(scriptRepository.GetAuthorByActiveUser(user), "ID", "FullName", scriptRepository.GetAuthorByActiveUser(user).Select(x => x.ID).FirstOrDefault());
                ViewBag.CoAuthor1ID = new SelectList(scriptRepository.GetCoAuthorByActiveUser(user), "ID", "FullName");
                ViewBag.CoAuthor2ID = new SelectList(scriptRepository.GetCoAuthorByActiveUser(user), "ID", "FullName");

                if (user > 0)
                {
                    script.AuthorID = user;
                }
            }
            else
            {
                ViewBag.AuthorID = new SelectList(scriptRepository.GetAuthorByActiveUser(null), "ID", "FullName");
                ViewBag.CoAuthor1ID = new SelectList(scriptRepository.GetCoAuthorByActiveUser(null), "ID", "FullName");
                ViewBag.CoAuthor2ID = new SelectList(scriptRepository.GetCoAuthorByActiveUser(null), "ID", "FullName");
            }

            ViewBag.ScriptTypeID = new SelectList(scriptRepository.GetScriptTypes(), "ID", "Type");

            return View(script);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Scripter")]
        public ActionResult Create([Bind(Include = "EntryDate,ScriptName,ScriptFamily,ProjectName,ProjectLink,ProjectStatus,ProjectLocation,Comments,AuthorID,ScriptTypeID,CoAuthor1ID,CoAuthor2ID")] Script script)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (script.AuthorID != script.CoAuthor1ID && script.AuthorID != script.CoAuthor2ID)
                    {
                        if (script.CoAuthor1ID != script.CoAuthor2ID || script.CoAuthor1ID == null)
                        {

                            if (scriptRepository.InsertScript(script))
                            {
                                scriptRepository.Save();
                                return RedirectToAction("Index");
                            }

                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Unable to save changes. Same Co-Author 1 and Co-Author 2");
                        }
                    }
                    else

                    {
                        ModelState.AddModelError(string.Empty, "Unable to save changes. Same Author and Co-Author");
                    }
                }
            }
            catch
            {

                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
            }


            ViewBag.ScriptTypeID = new SelectList(scriptRepository.GetScriptTypes(), "ID", "Type", script.ScriptTypeID);

            ViewBag.AuthorID = new SelectList(scriptRepository.GetAuthorByActiveUser(null), "ID", "FullName", script.AuthorID);
            ViewBag.CoAuthor1ID = new SelectList(scriptRepository.GetCoAuthorByActiveUser(null), "ID", "FullName", script.CoAuthor1ID);
            ViewBag.CoAuthor2ID = new SelectList(scriptRepository.GetCoAuthorByActiveUser(null), "ID", "FullName", script.CoAuthor2ID);

            return View(script);
        }


        [Authorize(Roles = "Admin,Scripter,Researcher,Reporter")]
        public ActionResult Edit(int? id)
        {

            ViewBag.Request = System.Web.HttpContext.Current.Session["URL"];

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Script script = scriptRepository.GetScriptByID(id);

            if (script == null)
            {
                return HttpNotFound();
            }


            if (Request.IsAuthenticated && User.IsInRole("Scripter"))
            {
                int user = Convert.ToInt32(System.Web.HttpContext.Current.Session["userID"]);

                ViewBag.AuthorID = new SelectList(scriptRepository.GetAuthorByActiveUser(user), "ID", "FullName", script.AuthorID);
                ViewBag.CoAuthor1ID = new SelectList(scriptRepository.GetCoAuthorByActiveUser(user), "ID", "FullName", script.CoAuthor1ID);
                ViewBag.CoAuthor2ID = new SelectList(scriptRepository.GetCoAuthorByActiveUser(user), "ID", "FullName", script.CoAuthor2ID);
            }
            else
            {
                ViewBag.AuthorID = new SelectList(scriptRepository.GetAllAuthors(), "ID", "FullName", script.AuthorID);
                ViewBag.CoAuthor1ID = new SelectList(scriptRepository.GetAllAuthors(), "ID", "FullName", script.CoAuthor1ID);
                ViewBag.CoAuthor2ID = new SelectList(scriptRepository.GetAllAuthors(), "ID", "FullName", script.CoAuthor2ID);
            }

            ViewBag.ScriptTypeID = new SelectList(scriptRepository.GetScriptTypes(), "ID", "Type", script.ScriptTypeID);

            return View(script);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Scripter,Researcher,Reporter")]
        public ActionResult Edit([Bind(Include = "ID,EntryDate,ScriptName,ScriptFamily,ProjectName,ProjectLink,ProjectStatus,ProjectLocation,Comments,AuthorID,ScriptTypeID,CoAuthor1ID,CoAuthor2ID")] Script script)
        {

            ViewBag.Request = System.Web.HttpContext.Current.Session["URL"];

            try
            {
                if (ModelState.IsValid)
                {
                    if (script.AuthorID != script.CoAuthor1ID && script.AuthorID != script.CoAuthor2ID)
                    {
                        if (script.CoAuthor1ID != script.CoAuthor2ID || script.CoAuthor1ID == null)
                        {

                            if (scriptRepository.UpdateScript(script))
                            {
                                scriptRepository.Save();

                                return Redirect(ViewBag.Request);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Unable to save changes. Same Co-Author 1 and Co-Author 2");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Unable to save changes. Same Author and Co-Author");
                    }
                }
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
            }

            ViewBag.AuthorID = new SelectList(scriptRepository.GetAllAuthors(), "ID", "FullName", script.AuthorID);
            ViewBag.CoAuthor1ID = new SelectList(scriptRepository.GetAllAuthors(), "ID", "FullName", script.CoAuthor1ID);
            ViewBag.CoAuthor2ID = new SelectList(scriptRepository.GetAllAuthors(), "ID", "FullName", script.CoAuthor2ID);

            ViewBag.ScriptTypeID = new SelectList(scriptRepository.GetScriptTypes(), "ID", "Type", script.ScriptTypeID);

            return View(script);

        }


        public ActionResult Details(int? id)
        {
            ViewBag.Request = System.Web.HttpContext.Current.Session["URL"];
            ViewBag.User = Convert.ToInt32(System.Web.HttpContext.Current.Session["userID"]);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ScriptDetailsViewModel script = scriptRepository.GetScriptDetailsByID(id);

            if (script == null)
            {
                return HttpNotFound();
            }
            return View(script);
        }


        [Authorize(Roles = "Admin,Scripter,Researcher,Reporter")]
        public ActionResult Delete(int? id)
        {
            ViewBag.Request = System.Web.HttpContext.Current.Session["URL"];

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ScriptDetailsViewModel script = scriptRepository.GetScriptDetailsByID(id);

            if (script == null)
            {
                return HttpNotFound();
            }
            return View(script);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Scripter,Researcher,Reporter")]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.Request = System.Web.HttpContext.Current.Session["URL"];

            if (scriptRepository.DeleteScript(id))
            {
                scriptRepository.Save();
            }

            return Redirect(ViewBag.Request);
        }


        public bool IsScriptNameExists(string scriptname, int? id)
        {
            return scriptRepository.IsExists(scriptname, id);
        }


        [HttpPost]
        public JsonResult IsScriptExists(string scriptname, int? id)
        {
            var result = IsScriptNameExists(scriptname, id)
                ? Json(false, JsonRequestBehavior.AllowGet)
                : Json(true, JsonRequestBehavior.AllowGet);
            return result;
        }


        public FileContentResult Export(SearchFilter searchFilter, string searchText, string sortOrder = "date_desc")
        {

            var fileDownloadName = String.Format("Scripts_" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss") + ".xlsx");
            const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";


            var scripts = scriptRepository.GetScripts(searchText);

            if (searchFilter != null)
            {
                if (searchFilter.FromDate != null || searchFilter.ToDate != null || !string.IsNullOrEmpty(searchFilter.ScriptName) || searchFilter.ScriptTypeID != null || searchFilter.AuthorID != null || !string.IsNullOrEmpty(searchFilter.ProjectName) || !string.IsNullOrEmpty(searchFilter.ProjectLocation))
                {
                    scripts = FilteringHelper.FilterScripts(scripts, searchFilter);
                }
            }

            ViewBag.Search = searchText;
            ViewBag.SortOrder = sortOrder;

            sortOrder = System.Web.HttpContext.Current.Session["Scriptsort"].ToString();

            scripts = SortingHelper.SortScripts(scripts, sortOrder);

            ExcelPackage package = ExcelPackageHelper.GenerateExcelFile(scripts.ToList());

            FileContentResult file = new FileContentResult(package.GetAsByteArray(), contentType);
            file.FileDownloadName = fileDownloadName;

            return file;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && scriptRepository != null)
                scriptRepository.Dispose();
            base.Dispose(disposing);
        }

    }
}

using OfficeOpenXml;
using ProjectTracker.DAL;
using ProjectTracker.Helpers;
using ProjectTracker.Infrastructure;
using ProjectTracker.Models;
using ProjectTracker.Services;
using ProjectTracker.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace ProjectTracker.Controllers
{
    [NoCache]
    [Authorize(Roles = "Admin,Reporter")]
    public class ReportController : Controller
    {

        private IReportRepository reportRepository;
        private IReportService reportService;

        public ReportController(IReportRepository reportRepository, IReportService reportService)
        {
            this.reportRepository = reportRepository;
            this.reportService = reportService;
        }

        public int PageSize = 15;

        public ActionResult Index(SearchFilter searchFilter, string searchText, bool isFilterVisible = false, string sortOrder = "task_desc", int page = 1)
        {

            List<SelectListItem> filterStatus = new List<SelectListItem>() {
                new SelectListItem {Text = "Unfinished", Value = false.ToString()},
                new SelectListItem {Text = "Finished", Value =  true.ToString()},
            };

            ViewBag.FilterStatus = filterStatus;

            System.Web.HttpContext.Current.Session["URL"] = Request.Url.AbsoluteUri.ToString();
            System.Web.HttpContext.Current.Session["Reportsort"] = sortOrder.ToString();

            ViewBag.ComplexSortParam = sortOrder == "complex" ? "complex_desc" : "complex";
            ViewBag.PointsSortParam = sortOrder == "points" ? "points_desc" : "points";
            ViewBag.CountrySortParam = sortOrder == "country" ? "country_desc" : "country";
            ViewBag.ScriptSortParam = sortOrder == "script" ? "script_desc" : "script";
            ViewBag.TypeSortParam = sortOrder == "type" ? "type_desc" : "type";
            ViewBag.AuthorSortParam = sortOrder == "author" ? "author_desc" : "author";
            ViewBag.ProjectSortParam = sortOrder == "project" ? "project_desc" : "project";
            ViewBag.TaskSortParam = sortOrder == "task" ? "task_desc" : "task";
            ViewBag.StartSortParam = sortOrder == "start" ? "start_desc" : "start";
            ViewBag.EndSortParam = sortOrder == "end" ? "end_desc" : "end";
            ViewBag.StatusSortParam = sortOrder == "status" ? "status_desc" : "status";
            ViewBag.EstimatedSortParam = sortOrder == "estimated" ? "estimated_desc" : "estimated";
            ViewBag.ActualSortParam = sortOrder == "actual" ? "actual_desc" : "actual";
            ViewBag.TestSortParam = sortOrder == "test" ? "test_desc" : "test";
            ViewBag.TestErrorsSortParam = sortOrder == "testerrors" ? "testerrors_desc" : "testerrors";
            ViewBag.FieldErrorsSortParam = sortOrder == "fielderrors" ? "fielderrors_desc" : "fielderrors";
            ViewBag.CommentSortParam = sortOrder == "comment" ? "comment_desc" : "comment";

            var reports = reportRepository.GetReports(searchText);

            ViewBag.Search = searchText;

            ViewBag.SearchFilter = searchFilter;

            if (searchFilter != null)
            {
                if (searchFilter.FromDate != null || searchFilter.ToDate != null || !string.IsNullOrEmpty(searchFilter.ScriptName) || searchFilter.ScriptTypeID != null || searchFilter.AuthorID != null || !string.IsNullOrEmpty(searchFilter.ProjectName) || !string.IsNullOrEmpty(searchFilter.ProjectLocation) || searchFilter.isFinished != null)
                {
                    reports = FilteringHelper.FilterReports(reports, searchFilter);
                    isFilterVisible = true;
                }
            }

            ViewBag.SortOrder = sortOrder;

            reports = SortingHelper.SortReports(reports, sortOrder);

            ReportsListViewModel model = new ReportsListViewModel
            {
                Reports = reports
                .Skip((page - 1) * PageSize)
                .Take(PageSize),

                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = reports.Count()
                },

                SearchFilter = searchFilter,
                IsFilterVisible = isFilterVisible
            };

            ViewBag.ScriptTypeID = new SelectList(reportRepository.GetScriptTypes(), "ID", "Type");
            ViewBag.AuthorID = new SelectList(reportRepository.GetAuthorByActiveUser(null), "ID", "FullName", model.SearchFilter.AuthorID);

            return View(model);
        }



        public ActionResult Create(int? id)
        {
            ViewBag.Request = System.Web.HttpContext.Current.Session["URL"];

            int user = Convert.ToInt32(System.Web.HttpContext.Current.Session["userID"]);

            ViewBag.ComplexityID = new SelectList(reportRepository.GetComplexities(), "ID", "Type");
            ViewBag.CountryID = new SelectList(reportRepository.GetCountries(), "ID", "Code");

            Report report = new Report();

            if (reportRepository.GetScriptByID(id) != null)
            {
                ViewBag.ScriptName = reportRepository.GetScriptByID(id).ScriptName;
                ViewBag.ProjectName = reportRepository.GetScriptByID(id).ProjectName;
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            ReportDataFromBase newReport = reportService.GetDataFromServers(id);

            if (!string.IsNullOrEmpty(newReport.ErrorReportMessage))
            {
                ModelState.AddModelError(string.Empty, newReport.ErrorReportMessage);
            }

            report.TaskSentDate = newReport.InsertTaskDate;
            report.ScriptEntryDate = newReport.StartTaskDate;
            report.ScriptDoneDate = newReport.EndTaskDate;
            report.ActualScriptingHours = newReport.ScriptHours;
            report.ActualTestingHours = newReport.TestHours;
            report.EstimatedScriptingHours = newReport.EstimatedHours;


            if (User.IsInRole("Admin") || User.IsInRole("Reporter"))
            {
                return View(report);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ComplexityID,Points,CountryID,TaskSentDate,ScriptEntryDate,ScriptDoneDate,ScriptStatus,EstimatedScriptingHours,ActualScriptingHours,ActualTestingHours,ScriptInTestErrors,ScriptInFieldErrors,ScriptComments")] Report report)
        {
            ViewBag.Request = System.Web.HttpContext.Current.Session["URL"];

            if (((report.ActualScriptingHours + report.ActualTestingHours + report.EstimatedScriptingHours) < 1 ||
                 (report.ActualScriptingHours == null && report.ActualTestingHours == null && report.EstimatedScriptingHours == null)) && report.ScriptStatus)
            {
                ModelState.AddModelError(string.Empty, "Cannot save, report is not ready yet!");
            }

            if (reportRepository.GetScriptByID(report.ID) != null)
            {
                ViewBag.ScriptName = reportRepository.GetScriptByID(report.ID).ScriptName;
                ViewBag.ProjectName = reportRepository.GetScriptByID(report.ID).ProjectName;
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (ModelState.IsValid)
            {
                if (reportRepository.InsertReport(report))
                {
                    reportRepository.Save();
                    return Redirect(ViewBag.Request);
                }

            }

            ViewBag.ComplexityID = new SelectList(reportRepository.GetComplexities(), "ID", "Type", report.ComplexityID);
            ViewBag.CountryID = new SelectList(reportRepository.GetCountries(), "ID", "Code", report.CountryID);

            return View(report);
        }

        public ActionResult Edit(int? id, bool refresh = false)
        {

            ViewBag.Request = System.Web.HttpContext.Current.Session["URL"];

            int user = Convert.ToInt32(System.Web.HttpContext.Current.Session["userID"]);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Report report = reportRepository.GetReportByID(id);

            ViewBag.ScriptName = reportRepository.GetScriptByID(id).ScriptName;
            ViewBag.ProjectName = reportRepository.GetScriptByID(id).ProjectName;

            if (report == null)
            {
                return RedirectToAction("Create", new { id });
            }

            ViewBag.ComplexityID = new SelectList(reportRepository.GetComplexities(), "ID", "Type", report.ComplexityID);
            ViewBag.CountryID = new SelectList(reportRepository.GetCountries(), "ID", "Code", report.CountryID);

            if (User.IsInRole("Admin") || User.IsInRole("Reporter"))
            {

                if (refresh)
                {
                    ReportDataFromBase newReport = reportService.GetDataFromServers(id);

                    if (!string.IsNullOrEmpty(newReport.ErrorReportMessage))
                    {
                        ModelState.AddModelError(string.Empty, newReport.ErrorReportMessage);
                    }

                    report.TaskSentDate = newReport.InsertTaskDate;

                    if (newReport.StartTaskDate != null)
                        report.ScriptEntryDate = (DateTime)newReport.StartTaskDate;

                    report.ScriptDoneDate = newReport.EndTaskDate;

                    if (newReport.ScriptHours != null)
                        report.ActualScriptingHours = newReport.ScriptHours;
                    if (newReport.TestHours != null)
                        report.ActualTestingHours = newReport.TestHours;
                    if (newReport.EstimatedHours != null)
                        report.EstimatedScriptingHours = newReport.EstimatedHours;
                }

                return View(report);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ComplexityID,Points,CountryID,TaskSentDate,ScriptEntryDate,ScriptDoneDate,ScriptStatus,EstimatedScriptingHours,ActualScriptingHours,ActualTestingHours,ScriptInTestErrors,ScriptInFieldErrors,ScriptComments")] Report report)
        {

            ViewBag.Request = System.Web.HttpContext.Current.Session["URL"];

            if (ModelState.IsValid)
            {
                if (reportRepository.UpdateReport(report))
                {
                    reportRepository.Save();
                    return Redirect(ViewBag.Request);
                }
            }

            ViewBag.ComplexityID = new SelectList(reportRepository.GetComplexities(), "ID", "Type", report.ComplexityID);
            ViewBag.CountryID = new SelectList(reportRepository.GetCountries(), "ID", "Code", report.CountryID);

            return Redirect(ViewBag.Request);
        }

        public ActionResult Delete(int? id)
        {

            ViewBag.Request = System.Web.HttpContext.Current.Session["URL"];

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = reportRepository.GetReportByID(id);

            if (report == null)
            {
                return HttpNotFound();
            }

            return View(report);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            ViewBag.Request = System.Web.HttpContext.Current.Session["URL"];

            if (reportRepository.DeleteReport(id))
            {
                reportRepository.Save();
            }

            return RedirectToAction("Index");
        }

        public FileContentResult Export(SearchFilter searchFilter, string searchText, string sortOrder = "task_desc")
        {

            var fileDownloadName = String.Format("Report_" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss") + ".xlsx");
            const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            var reports = reportRepository.GetReports(searchText);

            if (searchFilter != null)
            {
                if (searchFilter.FromDate != null || searchFilter.ToDate != null || !string.IsNullOrEmpty(searchFilter.ScriptName) || searchFilter.ScriptTypeID != null || searchFilter.AuthorID != null || !string.IsNullOrEmpty(searchFilter.ProjectName) || !string.IsNullOrEmpty(searchFilter.ProjectLocation))
                {
                    reports = FilteringHelper.FilterReports(reports, searchFilter);
                }
            }

            ViewBag.Search = searchText;
            ViewBag.SortOrder = sortOrder;

            sortOrder = System.Web.HttpContext.Current.Session["Reportsort"].ToString();

            reports = SortingHelper.SortReports(reports, sortOrder);

            ExcelPackage package = ExcelPackageHelper.GenerateExcelFile(reports.ToList());

            FileContentResult file = new FileContentResult(package.GetAsByteArray(), contentType);
            file.FileDownloadName = fileDownloadName;

            return file;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && reportRepository != null)
                reportRepository.Dispose();
            base.Dispose(disposing);
        }

    }
}

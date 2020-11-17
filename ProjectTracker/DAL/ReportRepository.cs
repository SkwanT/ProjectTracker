using ProjectTracker.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace ProjectTracker.DAL
{
    public class ReportRepository : IReportRepository
    {
        private ProjectTrackerContext context;
        public ReportRepository(ProjectTrackerContext context)
        {
            this.context = context;
        }

        public Script GetScriptByID(int? id)
        {
            return context.Scripts.Find(id);
        }

        public Report GetReportByID(int? id)
        {
            return context.Reports.Find(id);
        }

        public IEnumerable<Complexity> GetComplexities()
        {
            return context.Complexities.ToList();
        }

        public IEnumerable<Country> GetCountries()
        {
            return context.Countries.ToList();
        }

        public IQueryable<Report> GetReports(string search)
        {
            var reports = context.Reports.Include(r => r.Complexity).Include(r => r.Country).Include(r => r.Script).Include(r => r.Script.Author).Include(r => r.Script.ScriptType);

            if (!string.IsNullOrEmpty(search))
            {
                reports = reports.Where(s => s.Script.ProjectName.Contains(search) || s.Script.ScriptName.Contains(search) || s.Script.Author.FirstName.Contains(search) || s.Script.Author.LastName.Contains(search) || s.Script.ScriptType.Type.Contains(search));
            }

            return reports;
        }

        public bool InsertReport(Report report)
        {
            try
            {
                context.Reports.Add(report);
            }
            catch (Exception ex)
            {
                ProjectTracker.MvcApplication.ErrorLogging("ReportRepository.InsertReport: " + ex.Message);

                return false;
            }

            return true;
        }

        public bool UpdateReport(Report report)
        {

            try
            {
                context.Entry(report).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                ProjectTracker.MvcApplication.ErrorLogging("ReportRepository.UpdateReport: " + ex.Message);

                return false;
            }

            return true;
        }

        public bool DeleteReport(int? id)
        {
            try
            {
                Report report = context.Reports.Find(id);
                context.Reports.Remove(report);
            }
            catch (Exception ex)
            {
                ProjectTracker.MvcApplication.ErrorLogging("ReportRepository.DeleteReport: " + ex.Message);

                return false;
            }

            return true;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public IEnumerable<ScriptType> GetScriptTypes()
        {
            return context.ScriptTypes.ToList();
        }

        public IEnumerable<Author> GetAuthorByActiveUser(int? user)
        {
            if (user != null)
                return context.Authors.Where(r => r.ID == user).OrderBy(o => o.LastName);

            return context.Authors.Include(u => u.Role).Where(s => s.Active == true).Where(r => r.Role.RoleName.Contains("Scripter")).OrderBy(o => o.LastName);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
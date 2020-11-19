using ProjectTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectTracker.DAL
{
    public interface IReportRepository : IDisposable
    {
        Script GetScriptByID(int? id);

        Report GetReportByID(int? id);

        IEnumerable<Complexity> GetComplexities();

        IEnumerable<Country> GetCountries();

        IQueryable<Report> GetReports(string search);

        IEnumerable<ScriptType> GetScriptTypes();

        IEnumerable<Author> GetAuthorByActiveUser(int? user);

        bool InsertReport(Report report);

        bool UpdateReport(Report report);

        bool DeleteReport(int? id);

        void Save();
    }
}
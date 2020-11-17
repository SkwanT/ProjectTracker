using ProjectTracker.Models;
using ProjectTracker.ViewModel;
using System.Linq;

namespace ProjectTracker.Helpers
{
    public static class FilteringHelper
    {
        public static IQueryable<Script> FilterScripts(IQueryable<Script> scripts, SearchFilter searchFilter)
        {
            searchFilter.ScriptName = searchFilter.ScriptName?.Trim();
            searchFilter.ProjectName = searchFilter.ProjectName?.Trim();
            searchFilter.ProjectLocation = searchFilter.ProjectLocation?.Trim();

            if (searchFilter.FromDate != null)
                scripts = scripts.Where(s => s.EntryDate >= searchFilter.FromDate).Where(d => d.Deleted == false);
            if (searchFilter.ToDate != null)
                scripts = scripts.Where(s => s.EntryDate <= searchFilter.ToDate).Where(d => d.Deleted == false);
            if (!string.IsNullOrEmpty(searchFilter.ScriptName))
                scripts = scripts.Where(s => s.ScriptName.Contains(searchFilter.ScriptName)).Where(d => d.Deleted == false);
            if (searchFilter.ScriptTypeID != null)
                scripts = scripts.Where(s => s.ScriptTypeID == searchFilter.ScriptTypeID).Where(d => d.Deleted == false);
            if (searchFilter.AuthorID != null)
                scripts = scripts.Where(s => s.AuthorID == searchFilter.AuthorID).Where(d => d.Deleted == false);
            if (!string.IsNullOrEmpty(searchFilter.ProjectName))
                scripts = scripts.Where(s => s.ProjectName.Contains(searchFilter.ProjectName)).Where(d => d.Deleted == false);
            if (!string.IsNullOrEmpty(searchFilter.ProjectLocation))
                scripts = scripts.Where(s => s.ProjectLocation.Contains(searchFilter.ProjectLocation)).Where(d => d.Deleted == false);
            if (searchFilter.isFinished != null)
                scripts = scripts.Where(s => s.ProjectStatus == searchFilter.isFinished).Where(d => d.Deleted == false);

            return scripts;
        }

        public static IQueryable<Report> FilterReports(IQueryable<Report> reports, SearchFilter searchFilter)
        {
            searchFilter.ScriptName = searchFilter.ScriptName?.Trim();
            searchFilter.ProjectName = searchFilter.ProjectName?.Trim();
            searchFilter.ProjectLocation = searchFilter.ProjectLocation?.Trim();

            if (searchFilter.FromDate != null)
                reports = reports.Where(s => s.ScriptDoneDate >= searchFilter.FromDate);
            if (searchFilter.ToDate != null)
                reports = reports.Where(s => s.ScriptDoneDate <= searchFilter.ToDate);
            if (!string.IsNullOrEmpty(searchFilter.ScriptName))
                reports = reports.Where(s => s.Script.ScriptName.Contains(searchFilter.ScriptName));
            if (searchFilter.ScriptTypeID != null)
                reports = reports.Where(s => s.Script.ScriptTypeID == searchFilter.ScriptTypeID);
            if (searchFilter.AuthorID != null)
                reports = reports.Where(s => s.Script.AuthorID == searchFilter.AuthorID);
            if (!string.IsNullOrEmpty(searchFilter.ProjectName))
                reports = reports.Where(s => s.Script.ProjectName.Contains(searchFilter.ProjectName));
            if (searchFilter.isFinished != null)
                reports = reports.Where(s => s.ScriptStatus == searchFilter.isFinished);

            return reports;
        }

    }

}
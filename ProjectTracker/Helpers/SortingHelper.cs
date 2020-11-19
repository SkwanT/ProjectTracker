using ProjectTracker.Models;
using ProjectTracker.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace ProjectTracker.Helpers
{
    public static class SortingHelper
    {
        public static IQueryable<Script> SortScripts(IQueryable<Script> scripts, string sort)
        {
            switch (sort)
            {
                case "date":
                    scripts = scripts.OrderBy(s => s.EntryDate);
                    break;

                case "script":
                    scripts = scripts.OrderBy(s => s.ScriptName);
                    break;

                case "type":
                    scripts = scripts.OrderBy(s => s.ScriptType.Type);
                    break;

                case "author":
                    scripts = scripts.OrderBy(s => s.Author.LastName);
                    break;

                case "project":
                    scripts = scripts.OrderBy(s => s.ProjectName);
                    break;

                case "status":
                    scripts = scripts.OrderBy(s => s.ProjectStatus);
                    break;

                case "location":
                    scripts = scripts.OrderBy(s => s.ProjectLocation);
                    break;

                case "comment":
                    scripts = scripts.OrderBy(s => s.Comments);
                    break;

                case "date_desc":
                    scripts = scripts.OrderByDescending(s => s.EntryDate).ThenByDescending(s => s.ID);
                    break;

                case "script_desc":
                    scripts = scripts.OrderByDescending(s => s.ScriptName);
                    break;

                case "type_desc":
                    scripts = scripts.OrderByDescending(s => s.ScriptType.Type);
                    break;

                case "author_desc":
                    scripts = scripts.OrderByDescending(s => s.Author.LastName);
                    break;

                case "project_desc":
                    scripts = scripts.OrderByDescending(s => s.ProjectName);
                    break;

                case "status_desc":
                    scripts = scripts.OrderByDescending(s => s.ProjectStatus);
                    break;

                case "location_desc":
                    scripts = scripts.OrderByDescending(s => s.ProjectLocation);
                    break;

                case "comment_desc":
                    scripts = scripts.OrderByDescending(s => s.Comments);
                    break;

                default:
                    scripts = scripts.OrderByDescending(s => s.EntryDate);
                    break;
            }

            return scripts;
        }

        public static IQueryable<Report> SortReports(IQueryable<Report> reports, string sort)
        {
            switch (sort)
            {
                case "complex":
                    reports = reports.OrderBy(s => s.Complexity.Type);
                    break;

                case "points":
                    reports = reports.OrderBy(s => s.Points);
                    break;

                case "country":
                    reports = reports.OrderBy(s => s.Country.ID);
                    break;

                case "script":
                    reports = reports.OrderBy(s => s.Script.ScriptName);
                    break;

                case "type":
                    reports = reports.OrderBy(s => s.Script.ScriptType.Type);
                    break;

                case "author":
                    reports = reports.OrderBy(s => s.Script.Author.LastName);
                    break;

                case "project":
                    reports = reports.OrderBy(s => s.Script.ProjectName);
                    break;

                case "task":
                    reports = reports.OrderBy(s => s.TaskSentDate);
                    break;

                case "start":
                    reports = reports.OrderBy(s => s.ScriptEntryDate);
                    break;

                case "end":
                    reports = reports.OrderBy(s => s.ScriptDoneDate);
                    break;

                case "status":
                    reports = reports.OrderBy(s => s.ScriptStatus);
                    break;

                case "estimated":
                    reports = reports.OrderBy(s => s.EstimatedScriptingHours);
                    break;

                case "actual":
                    reports = reports.OrderBy(s => s.ActualScriptingHours);
                    break;

                case "test":
                    reports = reports.OrderBy(s => s.ActualTestingHours);
                    break;

                case "testerrors":
                    reports = reports.OrderBy(s => s.ScriptInTestErrors);
                    break;

                case "fielderrors":
                    reports = reports.OrderBy(s => s.ScriptInFieldErrors);
                    break;

                case "comment":
                    reports = reports.OrderBy(s => s.ScriptComments);
                    break;

                case "complex_desc":
                    reports = reports.OrderByDescending(s => s.Complexity.Type);
                    break;

                case "points_desc":
                    reports = reports.OrderByDescending(s => s.Points);
                    break;

                case "country_desc":
                    reports = reports.OrderByDescending(s => s.Country.ID);
                    break;

                case "script_desc":
                    reports = reports.OrderByDescending(s => s.Script.ScriptName);
                    break;

                case "type_desc":
                    reports = reports.OrderByDescending(s => s.Script.ScriptType.Type);
                    break;

                case "author_desc":
                    reports = reports.OrderByDescending(s => s.Script.Author.LastName);
                    break;

                case "project_desc":
                    reports = reports.OrderByDescending(s => s.Script.ProjectName);
                    break;

                case "task_desc":
                    reports = reports.OrderByDescending(s => s.TaskSentDate);
                    break;

                case "start_desc":
                    reports = reports.OrderByDescending(s => s.ScriptEntryDate);
                    break;

                case "end_desc":
                    reports = reports.OrderByDescending(s => s.ScriptDoneDate);
                    break;

                case "status_desc":
                    reports = reports.OrderByDescending(s => s.ScriptStatus);
                    break;

                case "estimated_desc":
                    reports = reports.OrderByDescending(s => s.EstimatedScriptingHours);
                    break;

                case "actual_desc":
                    reports = reports.OrderByDescending(s => s.ActualScriptingHours);
                    break;

                case "test_desc":
                    reports = reports.OrderByDescending(s => s.ActualTestingHours);
                    break;

                case "testerrors_desc":
                    reports = reports.OrderByDescending(s => s.ScriptInTestErrors);
                    break;

                case "fielderrors_desc":
                    reports = reports.OrderByDescending(s => s.ScriptInFieldErrors);
                    break;

                case "comment_desc":
                    reports = reports.OrderByDescending(s => s.ScriptComments);
                    break;

                default:
                    reports = reports.OrderByDescending(s => s.TaskSentDate);
                    break;
            }

            return reports;
        }

        public static IEnumerable<AuthorUserView> SortUsers(IEnumerable<AuthorUserView> users, string sort)
        {
            switch (sort)
            {
                case "firstname":
                    users = users.OrderBy(s => s.FirstName);
                    break;

                case "lastname":
                    users = users.OrderBy(s => s.LastName);
                    break;

                case "username":
                    users = users.OrderBy(s => s.UserName);
                    break;

                case "roles":
                    users = users.OrderBy(s => s.RoleName);
                    break;

                case "active":
                    users = users.OrderBy(s => s.Active);
                    break;

                case "firstname_desc":
                    users = users.OrderByDescending(s => s.FirstName);
                    break;

                case "lastname_desc":
                    users = users.OrderByDescending(s => s.LastName);
                    break;

                case "username_desc":
                    users = users.OrderByDescending(s => s.UserName);
                    break;

                case "roles_desc":
                    users = users.OrderByDescending(s => s.RoleName);
                    break;

                case "active_desc":
                    users = users.OrderByDescending(s => s.Active);
                    break;

                default:
                    users = users.OrderBy(s => s.LastName);
                    break;
            }

            return users;
        }
    }
}
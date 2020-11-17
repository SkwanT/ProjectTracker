using ProjectTracker.Models;
using ProjectTracker.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace ProjectTracker.DAL
{
    public class ScriptRepository : IScriptRepository
    {

        private ProjectTrackerContext context;
        public ScriptRepository(ProjectTrackerContext context)
        {
            this.context = context;
        }

        public IEnumerable<Author> GetAllAuthors()
        {
            return context.Authors.Include(u => u.Role).Where(r => r.Role.RoleName.Contains("Scripter")).OrderBy(o => o.LastName);
        }


        public IEnumerable<Author> GetAuthorByActiveUser(int? user)
        {
            if (user != null)
            {
                return context.Authors.Where(r => r.ID == user).OrderBy(o => o.LastName);
            }

            return context.Authors.Include(u => u.Role).Where(s => s.Active == true).Where(r => r.Role.RoleName.Contains("Scripter")).OrderBy(o => o.LastName);
        }

        public IEnumerable<Author> GetCoAuthorByActiveUser(int? user)
        {
            if (user != null)
            {
                return context.Authors.Include(u => u.Role).Where(s => s.Active == true).Where(r => r.Role.RoleName.Contains("Scripter") && r.ID != user).OrderBy(o => o.LastName);
            }

            return context.Authors.Include(u => u.Role).Where(s => s.Active == true).Where(r => r.Role.RoleName.Contains("Scripter")).OrderBy(o => o.LastName);
        }

        public IEnumerable<ScriptType> GetScriptTypes()
        {
            return context.ScriptTypes.ToList();
        }

        public Script GetScriptByID(int? id)
        {
            return context.Scripts.Find(id);
        }

        public ScriptDetailsViewModel GetScriptDetailsByID(int? id)
        {

            SqlParameter par1 = new SqlParameter("ID", System.Data.SqlDbType.Int);
            par1.Value = id;

            string query = "SELECT s.ID, s.AuthorID, ScriptName, t.Type AS ScriptType ,a.LastName + ' ' + a.FirstName AS Author, a1.LastName + ' ' + a1.FirstName AS CoAuthor1, a2.LastName + ' ' + a2.FirstName AS CoAuthor2 ,EntryDate, ProjectName, ProjectLink, ProjectStatus, ProjectLocation, Comments FROM Scripts s LEFT JOIN ScriptTypes t ON s.ScriptTypeID = t.ID   LEFT JOIN Authors a ON s.AuthorID = a.ID  LEFT JOIN Authors a1 ON s.CoAuthor1ID = a1.ID  LEFT JOIN Authors a2 ON s.CoAuthor2ID = a2.ID  WHERE s.ID = @ID";

            var data = context.Database.SqlQuery<ScriptDetailsViewModel>(query, par1);
            var result = data.ToList().FirstOrDefault();

            return result;
        }

        public IQueryable<Script> GetScripts(string search)
        {
            var scripts = context.Scripts.Include(s => s.Author).Include(s => s.ScriptType).Include(s => s.Report).Where(d => d.Deleted == false);

            if (!string.IsNullOrEmpty(search))
            {
                scripts = scripts.Where(s => s.ProjectName.Contains(search) || s.ScriptName.Contains(search) || s.Author.FirstName.Contains(search) || s.Author.LastName.Contains(search) || s.ScriptType.Type.Contains(search) || s.ProjectLocation.Contains(search)).Where(d => d.Deleted == false);
            }

            return scripts;
        }

        public bool InsertScript(Script scripts)
        {
            try
            {
                context.Scripts.Add(scripts);
            }
            catch (Exception ex)
            {
                ProjectTracker.MvcApplication.ErrorLogging("ScriptRepository.InsertScript: " + ex.Message);

                return false;
            }

            return true;
        }

        public bool UpdateScript(Script scripts)
        {
            try
            {
                context.Entry(scripts).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                ProjectTracker.MvcApplication.ErrorLogging("ScriptRepository.UpdateScript: " + ex.Message);

                return false;
            }

            return true;
        }

        public bool DeleteScript(int? id)
        {
            try
            {
                Script script = context.Scripts.Find(id);
                context.Scripts.Remove(script);
            }
            catch (Exception ex)
            {
                ProjectTracker.MvcApplication.ErrorLogging("ScriptRepository.DeleteScript: " + ex.Message);

                return false;
            }

            return true;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public bool IsExists(string scriptname, int? id)
        {
            if (id == null || id == 0)
            {
                return context.Scripts.Any(x => x.ScriptName == scriptname && x.Deleted == false);
            }
            else
            {
                return context.Scripts.Any(x => x.ScriptName == scriptname && x.ID != id && x.Deleted == false);
            }
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
using ProjectTracker.Models;
using ProjectTracker.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectTracker.DAL
{
    public interface IScriptRepository : IDisposable
    {
        Script GetScriptByID(int? id);

        ScriptDetailsViewModel GetScriptDetailsByID(int? id);

        IEnumerable<ScriptType> GetScriptTypes();

        IEnumerable<Author> GetAllAuthors();

        IEnumerable<Author> GetAuthorByActiveUser(int? user);

        IEnumerable<Author> GetCoAuthorByActiveUser(int? user);

        IQueryable<Script> GetScripts(string search);

        bool InsertScript(Script scripts);

        bool UpdateScript(Script scripts);

        bool DeleteScript(int? id);

        void Save();

        bool IsExists(string scriptname, int? id);
    }
}
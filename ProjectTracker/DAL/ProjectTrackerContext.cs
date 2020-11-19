using ProjectTracker.Models;
using System.Data.Entity;

namespace ProjectTracker.DAL
{
    public class ProjectTrackerContext : DbContext
    {
        public ProjectTrackerContext() : base("ProjectTrackerContext")
        {
            Database.SetInitializer<ProjectTrackerContext>(null);
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Script> Scripts { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ScriptType> ScriptTypes { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Complexity> Complexities { get; set; }
    }
}
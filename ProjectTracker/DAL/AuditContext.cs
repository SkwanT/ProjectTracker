using ProjectTracker.Models;
using System.Data.Entity;


namespace ProjectTracker.DAL
{
    public class AuditContext : DbContext
    {
        public AuditContext()
         : base("name=AuditContext")
        {
            Database.SetInitializer<AuditContext>(null);
        }
        public virtual DbSet<AuditTB> AuditTBs { get; set; }
    }
}
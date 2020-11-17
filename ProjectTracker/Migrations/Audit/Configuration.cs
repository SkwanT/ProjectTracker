using ProjectTracker.DAL;
using System.Data.Entity.Migrations;

namespace ProjectTracker.Migrations.Audit
{



    internal sealed class Configuration : DbMigrationsConfiguration<AuditContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\Audit";
        }

        protected override void Seed(AuditContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}

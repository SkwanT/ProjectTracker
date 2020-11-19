using Encryption;
using ProjectTracker.DAL;
using ProjectTracker.Models;
using System;
using System.Data.Entity.Migrations;

namespace ProjectTracker.Migrations.ProjectTracker
{
    internal sealed class Configuration : DbMigrationsConfiguration<ProjectTrackerContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\ProjectTracker";
        }

        protected override void Seed(ProjectTrackerContext context)
        {
            context.Roles.AddOrUpdate(x => x.ID,
                new Role() { ID = 1, RoleName = "Admin" },
                new Role() { ID = 2, RoleName = "Reporter" },
                new Role() { ID = 3, RoleName = "Scripter" },
                new Role() { ID = 4, RoleName = "Researcher" },
                new Role() { ID = 5, RoleName = "Viewer" }
            );

            context.ScriptTypes.AddOrUpdate(x => x.ID,
                new ScriptType() { ID = 1, Type = "CAPI" },
                new ScriptType() { ID = 2, Type = "CATI" },
                new ScriptType() { ID = 3, Type = "CAWI" },
                new ScriptType() { ID = 4, Type = "Data entry" }
            );

            context.Complexities.AddOrUpdate(x => x.ID,
                new Complexity() { ID = 1, Type = "Basic" },
                new Complexity() { ID = 1, Type = "Standard" },
                new Complexity() { ID = 1, Type = "Complex" }
            );

            context.Countries.AddOrUpdate(x => x.ID,
                new Country() { ID = 1, Code = "HR", Name = "Croatia" },
                new Country() { ID = 1, Code = "SI", Name = "Slovenia" },
                new Country() { ID = 1, Code = "BA", Name = "Bosnia and Herzegovina" },
                new Country() { ID = 1, Code = "RS", Name = "Serbia" },
                new Country() { ID = 1, Code = "ME", Name = "Montenegro" },
                new Country() { ID = 1, Code = "MK", Name = "Macedonia" },
                new Country() { ID = 1, Code = "AL", Name = "Albania" },
                new Country() { ID = 1, Code = "XK", Name = "Kosovo" }
            );

            Author admin = new Author();
            SaltedHash sh = new SaltedHash("admin");

            admin.ID = 1;
            admin.FirstName = "admin";
            admin.LastName = "admin";
            admin.UserName = "admin";
            admin.PasswordHash = sh.Hash;
            admin.SecurityStamp = sh.SecurityStamp;
            admin.RoleID = 1;
            admin.Active = true;
            admin.DateAdded = DateTime.Now;
            admin.InsertUserID = "1";
            admin.Deleted = false;
            admin.UpdateDate = DateTime.Now;
            admin.UpdateUserID = "1";

            context.Authors.AddOrUpdate(x => x.ID, admin);
        }
    }
}
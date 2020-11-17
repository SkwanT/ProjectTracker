using Encryption;
using ProjectTracker.Models;
using ProjectTracker.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace ProjectTracker.DAL
{
    public class AuthorRepository : IAuthorRepository, IDisposable
    {

        private ProjectTrackerContext context;

        public AuthorRepository(ProjectTrackerContext context)
        {
            this.context = context;
        }

        public bool DeleteUser(int userID, int EmployeeID)
        {
            bool result = false;
            try
            {
                Author user = context.Authors.Find(userID);
                if (user != null)
                {
                    user.Active = false;
                    user.Deleted = true;
                    user.UpdateDate = DateTime.Now;
                    user.UpdateUserID = EmployeeID.ToString();
                    context.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    result = true;
                }
            }
            catch (Exception ex)
            {
                ProjectTracker.MvcApplication.ErrorLogging("AspNetUserRepository.DeleteUser: " + ex.Message);
            }
            return result;
        }

        public IEnumerable<Role> GetRoles()
        {
            var data = context.Database.SqlQuery<Role>("select ID,RoleName from Roles Order By ID");
            var result = data.ToList();
            return result;
        }

        public AuthorUserEdit GetUserByID(int? userId)
        {
            Author user = context.Authors.Find(userId);
            AuthorUserEdit edituser = new AuthorUserEdit();
            if (user != null)
            {
                edituser.Active = user.Active;
                edituser.FirstName = user.FirstName;
                edituser.ID = user.ID;
                edituser.LastName = user.LastName;

                edituser.RoleID = user.RoleID;
                edituser.Role = user.Role;

                edituser.UserName = user.UserName;

            }
            return edituser;
        }

        public IEnumerable<AuthorUserView> GetUsers(string Search)
        {
            SqlParameter par1 = new SqlParameter("Search", System.Data.SqlDbType.NVarChar, 128);
            par1.Value = "";

            string query = "select Authors.ID, FirstName, LastName, UserName, Active, RoleID, Roles.RoleName from Authors LEFT OUTER JOIN Roles ON Authors.RoleID=Roles.ID order by LastName,FirstName";

            if (!string.IsNullOrEmpty(Search))
            {
                query = "select Authors.ID, FirstName, LastName, UserName, Active, RoleID, Roles.RoleName from Authors LEFT OUTER JOIN Roles ON Authors.RoleID=Roles.ID  where (FirstName like '%' + @Search + '%' or LastName like '%' + @Search + '%') order by LastName,FirstName";
                par1.Value = Search;
            }

            var data = context.Database.SqlQuery<AuthorUserView>(query, par1);
            var result = data.ToList();
            return result;
        }

        public bool InsertUser(NewUser newuser, int EmployeeID)
        {
            bool result = false;
            try
            {
                DateTime dt = DateTime.Now;
                Author user = new Author();
                user.Active = newuser.Active;
                user.Deleted = false;
                user.FirstName = newuser.FirstName;
                user.DateAdded = dt;
                user.InsertUserID = EmployeeID.ToString();
                user.LastName = newuser.LastName;
                SaltedHash sh = new SaltedHash(newuser.Password);
                user.PasswordHash = sh.Hash;
                user.SecurityStamp = sh.SecurityStamp;
                user.AdrianaID = null;
                user.RoleID = newuser.RoleID;
                user.UpdateDate = dt;
                user.UpdateUserID = EmployeeID.ToString();
                user.UserName = newuser.UserName;

                context.Authors.Add(user);
                result = true;
            }
            catch (Exception ex)
            {
                ProjectTracker.MvcApplication.ErrorLogging("AspNetUserRepository.InsertUser: " + ex.Message);
            }
            return result;
        }

        public bool IsUserNameExists(string username)
        {
            Author user = context.Authors.Where(x => x.UserName == username).FirstOrDefault();
            if (user == null)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public bool ResetPassword(ResetPassword rp, int EmployeeID)
        {
            bool result = false;
            try
            {
                Author user = context.Authors.Find(rp.ID);
                if (user != null)
                {
                    SaltedHash sh = new SaltedHash(rp.Password);
                    user.PasswordHash = sh.Hash;
                    user.SecurityStamp = sh.SecurityStamp;
                    user.UpdateUserID = EmployeeID.ToString();
                    user.DateAdded = DateTime.Now;

                    context.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    result = true;
                }
            }
            catch (Exception ex)
            {
                ProjectTracker.MvcApplication.ErrorLogging("AspNetUserRepository.ResetPassword: " + ex.Message);
            }
            return result;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public bool UpdateUser(AuthorUserEdit edituser, int EmployeeID)
        {
            bool result = false;
            try
            {
                Author user = context.Authors.Find(edituser.ID);
                if (user != null)
                {

                    user.Active = edituser.Active;
                    user.FirstName = edituser.FirstName;
                    user.LastName = edituser.LastName;
                    user.RoleID = edituser.RoleID;
                    user.UpdateDate = DateTime.Now;
                    user.UpdateUserID = EmployeeID.ToString();
                    user.UserName = edituser.UserName;
                    context.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    result = true;

                }
            }
            catch (Exception ex)
            {
                ProjectTracker.MvcApplication.ErrorLogging("AspNetUserRepository.UpdateUser: " + ex.Message);
            }
            return result;
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
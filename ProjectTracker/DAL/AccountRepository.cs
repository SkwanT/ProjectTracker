using Encryption;
using ProjectTracker.Models;
using System;
using System.Linq;

namespace ProjectTracker.DAL
{
    public class AccountRepository : IAccountRepository, IDisposable
    {
        private ProjectTrackerContext context;

        public AccountRepository(ProjectTrackerContext context)
        {
            this.context = context;
        }

        public string Login(string UserName, string Password)
        {

            string result = "";

            Author users = context.Authors.Where(user => user.UserName == UserName && user.Active == true).SingleOrDefault();

            if (users != null)
            {
                if (SaltedHash.Verify(users.SecurityStamp, users.PasswordHash, Password) == true)
                {
                    result = users.ID + ";" + users.Role.RoleName;
                }
            }

            return result;
        }


        public bool ChangePassword(UserChangePassword ucp, int EmployeeID)
        {
            bool result = false;

            Author user = context.Authors.Find(EmployeeID);

            if (user != null)
            {
                if (SaltedHash.Verify(user.SecurityStamp, user.PasswordHash, ucp.OldPassword) == true)
                {

                    SaltedHash sh = new SaltedHash(ucp.Password);
                    user.PasswordHash = sh.Hash;
                    user.SecurityStamp = sh.SecurityStamp;

                    user.UpdateDate = DateTime.Now;
                    user.UpdateUserID = EmployeeID.ToString();

                    context.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    result = true;
                }
            }

            return result;
        }

        public void Save()
        {
            context.SaveChanges();
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
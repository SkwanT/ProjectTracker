using ProjectTracker.Models;
using System;

namespace ProjectTracker.DAL
{
    public interface IAccountRepository : IDisposable
    {
        string Login(string UserName, string Password);
        bool ChangePassword(UserChangePassword ucp, int EmployeeID);
        void Save();

    }
}

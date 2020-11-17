using ProjectTracker.Models;
using ProjectTracker.ViewModel;
using System;
using System.Collections.Generic;

namespace ProjectTracker.DAL
{
    public interface IAuthorRepository : IDisposable
    {
        IEnumerable<Role> GetRoles();
        IEnumerable<AuthorUserView> GetUsers(string Search);
        AuthorUserEdit GetUserByID(int? userId);
        bool InsertUser(NewUser newuser, int EmployeeID);
        bool DeleteUser(int userID, int EmployeeID);
        bool UpdateUser(AuthorUserEdit edituser, int EmployeeID);
        bool ResetPassword(ResetPassword rp, int EmployeeID);
        bool IsUserNameExists(string username);
        void Save();
    }
}

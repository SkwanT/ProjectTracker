using System.ComponentModel.DataAnnotations;

namespace ProjectTracker.ViewModel
{
    public class AuthorUserView
    {
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Username")]
        public string UserName { get; set; }

        public int RoleID { get; set; }
        public string RoleName { get; set; }

        [Display(Name = "Active")]
        public bool Active { get; set; }
    }
}
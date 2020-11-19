using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ProjectTracker.Models
{
    public class Author
    {
        public int ID { get; set; }

        [Required]
        [StringLength(128)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(128)]
        public string LastName { get; set; }

        [StringLength(256)]
        [Display(Name = "Author")]
        public string FullName
        {
            get { return LastName + " " + FirstName; }
        }

        [Required]
        [MaxLength(256)]
        public string UserName { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string SecurityStamp { get; set; }

        public int RoleID { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<Script> Scripts { get; set; }

        public bool Active { get; set; }
        public DateTime DateAdded { get; set; }

        [Required]
        [MaxLength(128)]
        public string InsertUserID { get; set; }

        [Required]
        public bool Deleted { get; set; }

        [Required]
        [MaxLength(128)]
        public string UpdateUserID { get; set; }

        [Required]
        public DateTime UpdateDate { get; set; }

        public long? AdrianaID { get; set; }
    }

    public class AuthorUserEdit
    {
        [Required]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Required]
        [MaxLength(128)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(128)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [MaxLength(256)]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        public int RoleID { get; set; }

        [Display(Name = "Role")]
        public virtual Role Role { get; set; }

        [Required]
        [Display(Name = "Active")]
        public bool Active { get; set; }

        public string previousurl { get; set; }
    }

    public class NewUser
    {
        [Required]
        [MaxLength(128)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(128)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [MaxLength(256)]
        [Display(Name = "Username")]
        [Remote("IsUserNameExists", "Users", HttpMethod = "POST", ErrorMessage = "User name already exists. Please enter a different user name.")]
        public string UserName { get; set; }

        [Required]
        [MaxLength(128)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public int RoleID { get; set; }

        [Display(Name = "Role")]
        public virtual Role Role { get; set; }

        [Required]
        [Display(Name = "Active")]
        public bool Active { get; set; }

        public string previousurl { get; set; }
    }

    public class ResetPassword
    {
        public int? ID { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Display(Name = "Password")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password")]
        public string ConfirmPassword { get; set; }

        public string previousurl { get; set; }
    }

    public class previousUrl
    {
        public string previousurl { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace ProjectTracker.Models
{
    public class User
    {
        [Display(Name = "Username")]
        [Required]
        [MaxLength(256)]
        public string UserName { get; set; }

        [Display(Name = "Password")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class UserChangePassword
    {
        [Display(Name = "Old Password")]
        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Display(Name = "New Password")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm New Password")]
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

    }

}
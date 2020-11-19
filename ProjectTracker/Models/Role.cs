using System.ComponentModel.DataAnnotations;

namespace ProjectTracker.Models
{
    public class Role
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }

        public object RoleID { get; internal set; }
    }
}
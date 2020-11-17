using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectTracker.Models
{
    public class ScriptType
    {
        public int ID { get; set; }
        [Required]
        [StringLength(50)]
        public string Type { get; set; }
        public virtual ICollection<Script> Scripts { get; set; }
    }
}
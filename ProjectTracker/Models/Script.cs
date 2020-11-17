using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ProjectTracker.Models
{
    public class Script
    {
        public int ID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Entry Date")]
        public DateTime EntryDate { get; set; } = DateTime.Now;


        [Display(Name = "Script Name")]
        [Remote("IsScriptExists", "Script", AdditionalFields = "ID", HttpMethod = "POST", ErrorMessage = "Script name already exists. Please enter a different script name.")]
        [Required]
        [StringLength(255)]
        public string ScriptName { get; set; }

        [Required]
        [Display(Name = "Type")]
        public int ScriptTypeID { get; set; }
        public virtual ScriptType ScriptType { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [StringLength(500)]
        [Display(Name = "Project Link")]
        public string ProjectLink { get; set; }

        [Display(Name = "Status")]
        public bool ProjectStatus { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Project Location")]
        public string ProjectLocation { get; set; }

        public string Comments { get; set; }
        [Required]
        [Display(Name = "Author")]
        public int AuthorID { get; set; }
        public virtual Author Author { get; set; }

        [Display(Name = "Co-Author 1")]
        public int? CoAuthor1ID { get; set; }

        [Display(Name = "Co-Author 2")]
        public int? CoAuthor2ID { get; set; }
        public virtual Report Report { get; set; }

        [Required]
        public bool Deleted { get; set; }

    }
}
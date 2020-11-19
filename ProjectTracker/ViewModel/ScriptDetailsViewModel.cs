using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectTracker.ViewModel
{
    public class ScriptDetailsViewModel
    {
        public int ID { get; set; }
        public int AuthorID { get; set; }

        [Display(Name = "Script Name")]
        public string ScriptName { get; set; }

        [Display(Name = "Type")]
        public string ScriptType { get; set; }

        [Display(Name = "Author")]
        public string Author { get; set; }

        [Display(Name = "Co-Author 1")]
        public string CoAuthor1 { get; set; }

        [Display(Name = "Co-Author 2")]
        public string CoAuthor2 { get; set; }

        [Display(Name = "Entry Date")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EntryDate { get; set; }

        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [Display(Name = "Project Link")]
        public string ProjectLink { get; set; }

        [Display(Name = "Status")]
        public bool ProjectStatus { get; set; }

        [Display(Name = "Project Location")]
        public string ProjectLocation { get; set; }

        public string Comments { get; set; }
    }
}
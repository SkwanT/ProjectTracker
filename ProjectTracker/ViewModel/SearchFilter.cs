using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectTracker.ViewModel
{
    public class SearchFilter
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "From Date")]
        public DateTime? FromDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "To Date")]
        public DateTime? ToDate { get; set; }

        [Display(Name = "Script Name")]
        [StringLength(255)]
        public string ScriptName { get; set; }

        [Display(Name = "Type")]
        public int? ScriptTypeID { get; set; }

        [Display(Name = "Author")]
        public int? AuthorID { get; set; }

        [StringLength(500)]
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [StringLength(500)]
        [Display(Name = "Project Location")]
        public string ProjectLocation { get; set; }

        [Display(Name = "Status")]
        public bool? isFinished { get; set; }

    }
}
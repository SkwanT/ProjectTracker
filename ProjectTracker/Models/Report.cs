using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectTracker.Models
{
    public class Report
    {
        [ForeignKey("Script")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [Display(Name = "Complexity")]
        public int ComplexityID { get; set; }

        public virtual Complexity Complexity { get; set; }

        [Display(Name = "Country")]
        public int CountryID { get; set; }

        public virtual Country Country { get; set; }

        [Display(Name = "Points")]
        [Range(0, 99999)]
        public int? Points { get; set; }

        [Display(Name = "Task Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? TaskSentDate { get; set; }

        [Display(Name = "Script Start")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ScriptEntryDate { get; set; }

        [Display(Name = "Script End")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ScriptDoneDate { get; set; }

        [Display(Name = "Status")]
        public bool ScriptStatus { get; set; }

        [Display(Name = "Estimated Hours")]
        [Range(0, 999.99)]
        public decimal? EstimatedScriptingHours { get; set; }

        [Display(Name = "Actual Hours")]
        [Range(0, 999.99)]
        public decimal? ActualScriptingHours { get; set; }

        [Display(Name = "Test Hours")]
        [Range(0, 999.99)]
        public decimal? ActualTestingHours { get; set; }

        [Display(Name = "Test Errors")]
        public bool ScriptInTestErrors { get; set; }

        [Display(Name = "Field Errors")]
        public bool ScriptInFieldErrors { get; set; }

        [Display(Name = "Comments")]
        public string ScriptComments { get; set; }

        public virtual Script Script { get; set; }
    }
}
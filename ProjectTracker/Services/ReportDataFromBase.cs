using System;

namespace ProjectTracker.Services
{
    public class ReportDataFromBase
    {
        public decimal? ScriptHours { get; set; }
        public decimal? TestHours { get; set; }
        public decimal? EstimatedHours { get; set; }
        public DateTime? InsertTaskDate { get; set; }
        public DateTime? StartTaskDate { get; set; }
        public DateTime? EndTaskDate { get; set; }
        public string ErrorReportMessage { get; set; }
    }
}
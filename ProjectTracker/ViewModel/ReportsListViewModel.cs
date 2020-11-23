using ProjectTracker.Helpers;
using ProjectTracker.Models;
using System.Linq;

namespace ProjectTracker.ViewModel
{
    public class ReportsListViewModel
    {
        public IQueryable<Report> Reports { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public SearchFilter SearchFilter { get; set; }
        public ReportsSort ReportsSort { get; set; }
        public bool IsFilterVisible { get; set; } = false;
    }
}
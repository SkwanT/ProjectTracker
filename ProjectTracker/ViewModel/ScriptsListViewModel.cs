using ProjectTracker.Helpers;
using ProjectTracker.Models;
using System.Linq;

namespace ProjectTracker.ViewModel
{
    public class ScriptsListViewModel
    {
        public IQueryable<Script> Scripts { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public SearchFilter SearchFilter { get; set; }
        public bool IsFilterVisible { get; set; } = false;
    }
}
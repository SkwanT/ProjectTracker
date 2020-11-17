using ProjectTracker.DAL;

namespace ProjectTracker.Services
{
    public class ReportService : IReportService
    {
        private IReportRepository reportRepository;

        public ReportService(IReportRepository reportRepository)
        {
            this.reportRepository = reportRepository;
        }

        public ReportDataFromBase GetDataFromServers(int? id)
        {
            var report = new ReportDataFromBase();

            //Getting data from multiple servers....
            //
            //

            return report;
        }
    }
}
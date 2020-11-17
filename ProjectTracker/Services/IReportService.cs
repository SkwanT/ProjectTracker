namespace ProjectTracker.Services
{
    public interface IReportService
    {
        ReportDataFromBase GetDataFromServers(int? id);
    }
}

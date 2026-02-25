namespace ReportGenerator
{
    public interface IReportGenerator
    {
        Task<string> GenerateAsync(DateTime extractLocalTime, CancellationToken cancellationToken = default);
    }
}

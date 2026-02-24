namespace ReportGenerator.WindowsService
{
    public class ReportOptions
    {
        public string OutputFolder { get; set; } = "./Reports";
        public int IntervalMinutes { get; set; } = 15;
        public string LondonTimeZoneId { get; set; } = "GMT Standard Time";
    }
}

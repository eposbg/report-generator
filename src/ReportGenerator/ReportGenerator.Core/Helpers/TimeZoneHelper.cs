using Microsoft.Extensions.Logging;

namespace ReportGenerator.Core.Helpers
{
    public class TimeZoneHelper(ILogger<TimeZoneHelper> _logger)
    {
        public TimeZoneInfo GetLondonTimeZone(string timeZoneId)
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            }
            catch
            {
                _logger.LogWarning("Failed to find timezone with id {TimeZoneId}. Defaulting to UTC.", timeZoneId);
                return TimeZoneInfo.Utc;
            }
        }
    }
}

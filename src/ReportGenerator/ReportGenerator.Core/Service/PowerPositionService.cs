using Microsoft.Extensions.Logging;
using ReportGenerator.Core.Helpers;
using ReportGenerator.Interfaces;
using Services;

namespace ReportGenerator
{
    public class PowerPositionService(ILogger<PowerPositionService> _logger, IPowerService _powerService, TimeZoneHelper _timeZoneHelper) : IPowerPositionService
    {
        public async Task GenerateIntradayReport(string timeZoneId, string outputFolder, int intervalInMins,  CancellationToken ct)
        {
            _logger.LogInformation("PowerPositionService starting. OutputFolder={outputFolder} IntervalMinutes={intervalInMins}", outputFolder, intervalInMins);

            var londonTz = _timeZoneHelper.GetLondonTimeZone(timeZoneId);

            var utcNow = DateTime.UtcNow;
            var londonNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, londonTz);

            _logger.LogInformation("Starting extract at London local time {LondonNow}", londonNow);

            var targetDate = londonNow.Date;

            IEnumerable<PowerTrade> trades;
            try
            {
                trades = await _powerService.GetTradesAsync(targetDate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve trades for date {TargetDate}", targetDate);
                return;
            }

            var aggregated = TradeAggregator.Aggregate(trades, targetDate, londonTz);

            try
            {
                var path = await CsvExporter.ExportAsync(aggregated, outputFolder ?? "", londonNow, ct);
                _logger.LogInformation("Extract written to {Path}", path);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to write CSV");
            }

        }

    }
}

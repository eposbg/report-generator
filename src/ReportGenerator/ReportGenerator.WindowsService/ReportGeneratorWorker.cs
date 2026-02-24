using Microsoft.Extensions.Options;
using ReportGenerator.Interfaces;

namespace ReportGenerator.WindowsService
{
    public class ReportGeneratorWorker(ILogger<ReportGeneratorWorker> _logger, IPowerPositionService _powerPositionService, IOptions<ReportOptions> options) : BackgroundService
    {
        private readonly ReportOptions _reportOptions = options.Value;

        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    if (_logger.IsEnabled(LogLevel.Information))
                    {
                        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    }

                    await _powerPositionService.GenerateIntradayReport(_reportOptions.LondonTimeZoneId, _reportOptions.OutputFolder, _reportOptions.IntervalMinutes, ct);
                    await Task.Delay(TimeSpan.FromMinutes(_reportOptions.IntervalMinutes), ct);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during initial extract");
                }

            }
        }
    }
}

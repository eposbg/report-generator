using Microsoft.Extensions.Logging;
using Moq;
using ReportGenerator.Core.Helpers;
using ReportGenerator.Core.Interfaces;
using Services;

namespace ReportGenerator.Core.Tests.Service
{
    public class PowerPositionServiceTests
    {
        [Fact]
        public async Task GenerateIntradayReport_WritesCsvFile_WhenTradesAvailable()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<PowerPositionService>>();
            var powerServiceMock = new Mock<IPowerService>();
            var tradeAggregatorMock = new Mock<ITradeAggregator>();

            powerServiceMock.Setup(s => s.GetTradesAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(new PowerTrade[0]);

            tradeAggregatorMock.Setup(a => a.Aggregate(It.IsAny<IEnumerable<PowerTrade>>(), It.IsAny<DateTime>(), It.IsAny<TimeZoneInfo>()))
                .Returns(() =>
                {
                    var targetDate = DateTime.UtcNow.Date; // value not used directly
                    var baseLocal = targetDate.AddDays(-1).AddHours(23);
                    return new Dictionary<DateTime, double> { [baseLocal] = 123.45 };
                });

            var timeZoneHelper = new TimeZoneHelper(new Mock<ILogger<TimeZoneHelper>>().Object);

            var svc = new ReportGenerator.PowerPositionService(loggerMock.Object, powerServiceMock.Object, timeZoneHelper, tradeAggregatorMock.Object);

            var outDir = Path.Combine(Path.GetTempPath(), "PowerPositionTests", Guid.NewGuid().ToString());
            Directory.CreateDirectory(outDir);

            // Act
            await svc.GenerateIntradayReport(TimeZoneInfo.Utc.Id, outDir, 15, CancellationToken.None);

            var files = Directory.GetFiles(outDir, "PowerPosition_*.csv");
            Assert.Single(files);

            var content = File.ReadAllText(files[0]);
            Assert.Contains("Local Time,Volume", content);
            Assert.Contains("23:00", content);

            Directory.Delete(outDir, true);
        }

        [Fact]
        public async Task GenerateIntradayReport_LogsError_WhenGetTradesThrows()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<PowerPositionService>>();
            var powerServiceMock = new Mock<IPowerService>();
            var tradeAggregatorMock = new Mock<ITradeAggregator>();

            powerServiceMock.Setup(s => s.GetTradesAsync(It.IsAny<DateTime>()))
                .ThrowsAsync(new Exception("fail"));

            var timeZoneHelper = new TimeZoneHelper(new Mock<ILogger<TimeZoneHelper>>().Object);

            var svc = new PowerPositionService(loggerMock.Object, powerServiceMock.Object, timeZoneHelper, tradeAggregatorMock.Object);

            var outDir = Path.Combine(Path.GetTempPath(), "PowerPositionTests", Guid.NewGuid().ToString());
            Directory.CreateDirectory(outDir);

            // Act
            await svc.GenerateIntradayReport(TimeZoneInfo.Utc.Id, outDir, 15, CancellationToken.None);

            // Assert - no files created
            var files = Directory.GetFiles(outDir, "PowerPosition_*.csv");
            Assert.Empty(files);

            loggerMock.Verify(l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Failed to retrieve trades")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);

            Directory.Delete(outDir, true);
        }
    }
}

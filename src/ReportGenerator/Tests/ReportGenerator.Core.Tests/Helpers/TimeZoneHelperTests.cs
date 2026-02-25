using Microsoft.Extensions.Logging;
using Moq;
using ReportGenerator.Core.Helpers;

namespace ReportGenerator.Core.Tests.Helpers
{
    public class TimeZoneHelperTests
    {
        [Fact]
        public void GetLondonTimeZone_ValidId_ReturnsTimeZone()
        {
            var loggerMock = new Mock<ILogger<TimeZoneHelper>>();
            var helper = new TimeZoneHelper(loggerMock.Object);

            // Use UTC which is available across platforms
            var tz = helper.GetLondonTimeZone(TimeZoneInfo.Utc.Id);

            Assert.Equal(TimeZoneInfo.Utc.Id, tz.Id);
            // No warning should be logged for valid id
            loggerMock.Verify(l => l.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Never);
        }

        [Fact]
        public void GetLondonTimeZone_InvalidId_ReturnsUtcAndLogsWarning()
        {
            var loggerMock = new Mock<ILogger<TimeZoneHelper>>();
            var helper = new TimeZoneHelper(loggerMock.Object);

            var tz = helper.GetLondonTimeZone("Invalid/Timezone/Id");

            Assert.Equal(TimeZoneInfo.Utc.Id, tz.Id);

            // Verify a warning was logged containing the expected text
            loggerMock.Verify(l => l.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Failed to find timezone")),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
        }
    }
}

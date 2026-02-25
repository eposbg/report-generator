using Services;

namespace ReportGenerator.Core.Tests.Aggregation
{
    public class TradeAggregatorTests
    {
        [Fact]
        public void Aggregate_ExampleFromSpecification_ReturnsExpectedHourlySums()
        {
            var date = new DateTime(2015, 4, 1);
            var trade1 = PowerTrade.Create(date, 24);
            for (int i = 0; i < trade1.Periods.Length; i++)
            {
                trade1.Periods[i].Volume = 100.0;
            }

            var trade2 = PowerTrade.Create(date, 24);
            for (int i = 0; i < 11; i++) trade2.Periods[i].Volume = 50.0;
            for (int i = 11; i < 24; i++) trade2.Periods[i].Volume = -20.0;

            var aggregator = new TradeAggregator();

            // Act
            var aggregated = aggregator.Aggregate(new[] { trade1, trade2 }, date, TimeZoneInfo.Utc);

            // Assert
            Assert.Equal(24, aggregated.Count);

            var baseLocal = date.Date.AddDays(-1).AddHours(23); 

            for (int i = 0; i < 11; i++)
            {
                var key = baseLocal.AddHours(i);
                Assert.Equal(150.0, aggregated[key]);
            }

            for (int i = 11; i < 24; i++)
            {
                var key = baseLocal.AddHours(i);
                Assert.Equal(80.0, aggregated[key]);
            }
        }
    }
}

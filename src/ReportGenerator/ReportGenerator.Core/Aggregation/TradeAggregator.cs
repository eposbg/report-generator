using ReportGenerator.Core.Interfaces;
using Services;

namespace ReportGenerator
{
    public class TradeAggregator: ITradeAggregator
    {
        public IDictionary<DateTime, double> Aggregate(IEnumerable<PowerTrade> trades, DateTime date, TimeZoneInfo londonTz)
        {
            var result = new SortedDictionary<DateTime, double>();

            // Base start: previous day at 23:00 local
            var baseLocal = date.Date.AddDays(-1).AddHours(23);

            foreach (var trade in trades ?? Enumerable.Empty<PowerTrade>())
            {
                var periods = trade.Periods;
                if (periods == null) continue;

                foreach (var period in periods)
                {
                    var periodNumber = period.Period; 
                    var volume = period.Volume;

                    var localTime = baseLocal.AddHours(periodNumber - 1);

                    if (!result.ContainsKey(localTime)) result[localTime] = 0;
                    result[localTime] += volume;
                }
            }

            return result;
        }
    }
}

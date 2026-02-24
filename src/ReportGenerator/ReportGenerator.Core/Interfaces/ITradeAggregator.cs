using Services;

namespace ReportGenerator.Core.Interfaces
{
    
    /// <summary>
    /// Defines a contract for aggregating power trade data into a collection of values grouped by date.
    /// </summary>
    public interface ITradeAggregator
    {
        IDictionary<DateTime, double> Aggregate(IEnumerable<PowerTrade> trades, DateTime date, TimeZoneInfo londonTz);
    }
}

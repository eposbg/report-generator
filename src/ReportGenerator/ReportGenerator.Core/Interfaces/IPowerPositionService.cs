namespace ReportGenerator.Interfaces
{
    public  interface IPowerPositionService
    {
        /// <summary>
        /// Generates an intraday report using the specified time zone.
        /// </summary>
        /// <remarks>Use this method when reports need to reflect data according to a specific time zone.
        /// Supplying an invalid time zone ID will result in an exception.</remarks>
        /// <param name="timeZoneId">The identifier of the time zone to use for report generation. Must be a valid time zone ID, such as 'Pacific
        /// Standard Time'.</param>
        /// <param name="stoppingToken">A cancellation token that can be used to cancel the report generation operation.</param>
        /// <param name="outputFolder">The folder where the generated report should be saved. Must be a valid directory path.</param>
        /// <param name="intervalInMins">The interval in minutes for aggregating the report data. Must be a positive integer.</param>
        /// <returns>A task that represents the asynchronous operation of generating the intraday report.</returns>
        Task GenerateIntradayReport(string timeZoneId, string outputFolder, int intervalInMins, CancellationToken stoppingToken);
    }
}

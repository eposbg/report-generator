using System.Globalization;
using System.Text;

namespace ReportGenerator
{
    public static class CsvExporter
    {
        public static async Task<string> ExportAsync(IDictionary<DateTime, double> aggregated, string folder, DateTime extractLocalTime, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(folder)) throw new ArgumentException("Output folder not configured", nameof(folder));

            Directory.CreateDirectory(folder);

            var fileName = $"PowerPosition_{extractLocalTime:yyyyMMdd}_{extractLocalTime:HHmm}.csv";
            var path = Path.Combine(folder, fileName);

            var sb = new StringBuilder();
            sb.AppendLine("Local Time,Volume");

            foreach (var kv in aggregated.OrderBy(k => k.Key))
            {
                sb.AppendLine($"{kv.Key:HH:mm},{kv.Value.ToString(CultureInfo.InvariantCulture)}");
            }

            await File.WriteAllTextAsync(path, sb.ToString(), cancellationToken).ConfigureAwait(false);
            return path;
        }
    }
}

using System.Globalization;

namespace ReportGenerator.Core.Tests.NewFolder
{
    public class CsvExporterTests
    {
        [Fact]
        public async Task ExportAsync_WritesCsv_WithHeaderAndRows()
        {
            // Arrange
            var tempDir = Path.Combine(Path.GetTempPath(), "CsvExporterTests", Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                var baseLocal = new DateTime(2025, 1, 2).Date.AddDays(-1).AddHours(23);
                var aggregated = new SortedDictionary<DateTime, double>();
                aggregated.Add(baseLocal, 123.45);
                aggregated.Add(baseLocal.AddHours(1), 0);
                aggregated.Add(baseLocal.AddHours(2), -10.5);

                var extractTime = DateTime.Now;

                // Act
                var path = await CsvExporter.ExportAsync(aggregated, tempDir, extractTime);

                // Assert
                Assert.True(File.Exists(path));

                var lines = await File.ReadAllLinesAsync(path);
                Assert.Equal(1 + aggregated.Count, lines.Length);

                Assert.Equal("Local Time,Volume", lines[0]);

                var expectedFirst = $"{baseLocal:HH:mm},{aggregated[baseLocal].ToString(CultureInfo.InvariantCulture)}";
                Assert.Equal(expectedFirst, lines[1]);
            }
            finally
            {
                try { Directory.Delete(tempDir, true); } 
                catch { }
            }
        }

        [Fact]
        public async Task ExportAsync_Throws_WhenExportFolderNotConfigured()
        {
            // Arrange
            var aggregated = new Dictionary<DateTime, double>();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => CsvExporter.ExportAsync(aggregated, string.Empty, DateTime.Now));
        }
    }
}

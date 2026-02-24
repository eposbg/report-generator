using ReportGenerator;
using ReportGenerator.Core.Helpers;
using ReportGenerator.Core.Interfaces;
using ReportGenerator.Interfaces;
using ReportGenerator.WindowsService;
using Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.Configure<ReportOptions>(builder.Configuration.GetSection("Report"));
builder.Services.AddTransient<IPowerService, PowerService>();
builder.Services.AddTransient<IPowerPositionService, PowerPositionService>();
builder.Services.AddTransient<ITradeAggregator, TradeAggregator>();

builder.Services.AddSingleton<TimeZoneHelper>();
builder.Services.AddHostedService<ReportGeneratorWorker>();

var host = builder.Build();
host.Run();

using Azure.Monitor.OpenTelemetry.Exporter;
using Ingestion.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var configBuilder = new ConfigurationBuilder();

#if DEBUG
configBuilder.AddJsonFile("appsettings.development.json");
#endif
configBuilder.AddJsonFile("appsettings.json");
configBuilder.AddEnvironmentVariables();

var configuration = configBuilder.Build();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOpenTelemetryTracing(builder =>
{
    builder.SetResourceBuilder(ResourceBuilder.CreateDefault()
            .AddService("IngestionAPI")
            .AddTelemetrySdk()
            .AddEnvironmentVariableDetector())
        .AddAspNetCoreInstrumentation()
        .AddAzureMonitorTraceExporter(o =>
        {
            o.ConnectionString = configuration["ApplicationInsightsConnectionString"];
        });
});

builder.Services.AddSingleton<IFileDirectory>(new FileDirectory(configuration["FileShareBasePath"]));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
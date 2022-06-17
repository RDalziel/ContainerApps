using Ingestion.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

builder.Services.AddSingleton<IFileDirectory>(new FileDirectory(configuration["FileShareBasePath"]));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
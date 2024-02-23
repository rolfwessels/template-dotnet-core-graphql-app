using TemplateDotnetCoreConsoleApp.Api;
using TemplateDotnetCoreConsoleApp.Core;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFilesAndEnvironment();
builder.Host.UseSerilog((_, lc) =>
  lc.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .WriteTo.Console());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTemplateDotnetCoreConsoleAppCore();
builder.Services.AddTemplateDotnetCoreConsoleAppServer();


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}
app.UseWebSockets();
app.UseAuthorization();
app.MapControllers();
app.MapGraphQL();
app.MapApi();
app.Run();

namespace TemplateDotnetCoreConsoleApp.Api
{
  public partial class Program
  {
  }
}

using TemplateDotnetCoreConsoleApp.Core;
using Serilog;
using Serilog.Events;


namespace TemplateDotnetCoreConsoleApp.Api;

public class Program
{
  public static async Task Main(params string[] args)
  {
    try
    {
      var builder = WebApplication.CreateBuilder(args);
      builder.Configuration.AddJsonFilesAndEnvironment();
      builder.Host.UseSerilog(ConfigureLogger);

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
      await app.RunAsync();
    }
    catch (Exception ex)
    {
      Log.Fatal(ex, "Host terminated unexpectedly");
      throw;
    }
    finally
    {
      await Log.CloseAndFlushAsync();
    }
  }

  private static void ConfigureLogger(HostBuilderContext _, LoggerConfiguration lc)
  {
    lc.MinimumLevel.Debug()
      .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
      .Enrich.FromLogContext()
      .WriteTo.Console();
  }
}

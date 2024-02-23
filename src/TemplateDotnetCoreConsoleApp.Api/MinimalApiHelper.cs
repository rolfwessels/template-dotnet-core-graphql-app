namespace TemplateDotnetCoreConsoleApp.Api;

internal static class MinimalApiHelper
{
  public static void MapApi(this WebApplication app)
  {
    app.MapGet("/", () => "<html><a href='/swagger'/></html>").ExcludeFromDescription();
    app.MapGet("/api/ping", () => new PingResponse(ConfigurationHelper.Env(), Environment.MachineName, ConfigurationHelper.ProductVersion));
  }

  public record PingResponse(string Env, string MachineName, string Version);
}

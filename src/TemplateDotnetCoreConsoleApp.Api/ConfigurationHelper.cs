using System.Reflection;

namespace TemplateDotnetCoreConsoleApp.Api;

public static class ConfigurationHelper
{
  public static IConfigurationBuilder AddJsonFilesAndEnvironment(this IConfigurationBuilder config,
    string? environment = null)
  {
    config.AddJsonFile("appsettings.json", true, false)
      .AddJsonFile($"appsettings.{environment ?? Env()}.json", true, false)
      .AddEnvironmentVariables();
    return config;
  }

  public static string Env()
  {
    return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToLower() ?? "local";
  }

  public static string ProductVersion => Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "0.0.0";
}

using TemplateDotnetCoreConsoleApp.Core.Components;
using Microsoft.Extensions.DependencyInjection;
namespace TemplateDotnetCoreConsoleApp.Core;

public static class CoreIoc
{
  public static void AddTemplateDotnetCoreConsoleAppCore(this IServiceCollection services)
  {
    services.AddSingleton<MovieStore>();
  }
}

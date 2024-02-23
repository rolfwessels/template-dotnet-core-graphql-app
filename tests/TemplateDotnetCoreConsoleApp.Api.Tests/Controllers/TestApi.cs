using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace TemplateDotnetCoreConsoleApp.Api.Tests.Controllers;

public class TestApi : WebApplicationFactory<Program>
{
  protected override IHost CreateHost(IHostBuilder builder)
  {
    builder.ConfigureServices(services =>
    {
    });

    return base.CreateHost(builder);
  }

}

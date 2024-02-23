using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TemplateDotnetCoreConsoleApp.Api.Tests.GraphQL;
using FluentAssertions;
using NUnit.Framework;

namespace TemplateDotnetCoreConsoleApp.Api.Tests.Controllers;

public class MinimalApiHelperTests : BaseApiTests
{
  [Test]
  [Category("Integration")]
  public async Task ApiPing_WhenCalled_ShouldReturnApiVersionAndEnvironment()
  {
    //action
    var pingResponse = await TestClient.GetFromJsonAsync<MinimalApiHelper.PingResponse>("/api/ping");
    // assert
    pingResponse!.Env.Should().Be("local");
    pingResponse.MachineName.Should().NotBeEmpty();
    pingResponse.Version.Should().Contain(".");
  }

  
}

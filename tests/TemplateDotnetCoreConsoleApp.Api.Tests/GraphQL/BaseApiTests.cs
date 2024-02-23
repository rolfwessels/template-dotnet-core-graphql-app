using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Bumbershoot.Utilities.Helpers;
using TemplateDotnetCoreConsoleApp.Api.Tests.Controllers;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using StrawberryShake.Transport.WebSockets;
using WebSocketClient = Microsoft.AspNetCore.TestHost.WebSocketClient;

namespace TemplateDotnetCoreConsoleApp.Api.Tests.GraphQL;

public class BaseApiTests
{
  private readonly Lazy<TestApi> _api = new(() => new TestApi());
  private static readonly Lazy<(string, Task)> _lazyRealClient;
  protected TestApi Api => _api.Value;
  public HttpClient TestClient => Api.CreateClient();

  static BaseApiTests()
  {
    _lazyRealClient = new Lazy<(string, Task)>(() =>
    {
      var baseUrl = $"http://localhost:{Random.Shared.Next(5000, 9000)}/";
      return (baseUrl, Program.Main($"--urls={baseUrl}"));
    });
  }

  public BaseApiTests()
  {
    Services = FakeService();
  }

  public ServiceProvider RealService()
  {
    var baseUrl = _lazyRealClient.Value.Item1;
    var url = baseUrl.UriCombine("graphql");
    var serviceCollection = new ServiceCollection();
    serviceCollection.AddApiClient()
      .ConfigureHttpClient(
        c => { c.BaseAddress = new Uri(url); }
      ).ConfigureWebSocketClient(client => client.Uri = new Uri(url.Replace("http:", "ws:")));

    var buildServiceProvider = serviceCollection.BuildServiceProvider();
    return buildServiceProvider;
  }

  private ServiceProvider FakeService()
  {
    var serviceCollection = new ServiceCollection();
    serviceCollection.AddApiClient().ConfigureHttpClient(
      c => { c.BaseAddress = new Uri(Api.Server.BaseAddress, "graphql"); },
      c => c.ConfigurePrimaryHttpMessageHandler(() => Api.Server.CreateHandler())
    ).ConfigureWebSocketClient(client => client.Uri = new Uri(Api.Server.BaseAddress, "graphql"));


    var buildServiceProvider = serviceCollection.BuildServiceProvider();
    return buildServiceProvider;
  }

  public ServiceProvider Services { get; }


  [OneTimeTearDown]
  public void BaseTearDown()
  {
    if (_api.IsValueCreated)
    {
      _api.Value.Dispose();
    }
  }
}

internal class FakeSocketClientFactory : ISocketClientFactory
{
  private readonly WebSocketClient _webSocketClient;

  public FakeSocketClientFactory(WebSocketClient webSocketClient)
  {
    _webSocketClient = webSocketClient;
  }

  #region Implementation of ISocketClientFactory

  public ISocketClient CreateClient(string name)
  {
    var socketProtocolFactories = new List<ISocketProtocolFactory>();
    return new StrawberryShake.Transport.WebSockets.WebSocketClient("test", socketProtocolFactories);
  }

  #endregion
}

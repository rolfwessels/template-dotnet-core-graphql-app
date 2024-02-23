using System;
using System.Collections.Generic;
using System.Net.Http;
using TemplateDotnetCoreConsoleApp.Api.Tests.Controllers;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using StrawberryShake.Transport.WebSockets;
using WebSocketClient = Microsoft.AspNetCore.TestHost.WebSocketClient;

namespace TemplateDotnetCoreConsoleApp.Api.Tests.GraphQL;

public class BaseApiTests
{
  private readonly Lazy<TestApi> _api = new(() => new TestApi());
  protected TestApi Api => _api.Value;
  public HttpClient TestClient => Api.CreateClient();


  public BaseApiTests()
  {
    Services = FakeService();
  }

  public ServiceProvider RealService(string url = "http://localhost:5085/graphql")
  {
    var serviceCollection = new ServiceCollection();
    serviceCollection.AddApiClient()
      .ConfigureHttpClient(
      c => { c.BaseAddress = new Uri(url); }
    ).ConfigureWebSocketClient(client => client.Uri = new Uri(url.Replace("http:","ws:")));

    var buildServiceProvider = serviceCollection.BuildServiceProvider();
    return buildServiceProvider;
  }

  private ServiceProvider FakeService()
  {
    var serviceCollection = new ServiceCollection();
    serviceCollection.AddApiClient().ConfigureHttpClient(
      c => { c.BaseAddress = new Uri(Api.Server.BaseAddress, "graphql"); },
      c => c.ConfigurePrimaryHttpMessageHandler(() => Api.Server.CreateHandler())
    ).ConfigureWebSocketClient(client => client.Uri =  new Uri(Api.Server.BaseAddress, "graphql"));
    
    
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
    return new StrawberryShake.Transport.WebSockets.WebSocketClient("test",socketProtocolFactories);
  }

  #endregion
}

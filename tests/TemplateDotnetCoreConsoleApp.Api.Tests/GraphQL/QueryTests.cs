using System.Threading.Tasks;
using TemplateDotnetCoreConsoleApp.Client;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace TemplateDotnetCoreConsoleApp.Api.Tests.GraphQL;

public class QueryTests : BaseApiTests
{ 
  [Test]
  [Category("Integration")]
  public async Task AllMovies_GivenClient_ShouldReturnMovies()
  {
    // arrange
    var httpClient = Services.GetRequiredService<IApiClient>();

    // action
    var operationResult = await httpClient.AllMovies.ExecuteAsync();
    // assert
    operationResult.Errors.Should().BeNullOrEmpty();
    operationResult.Data!.Movies.Should().HaveCount(2);
  }

  [Test]
  [Category("Integration")]
  public async Task Movies_GivenClient_ShouldReturnMovies()
  {
    // arrange
    var httpClient = Services.GetRequiredService<IApiClient>();
    // action
    var operationResult = await httpClient.MovieById.ExecuteAsync(1);
    // assert
    operationResult.Errors.Should().BeNullOrEmpty();
    operationResult.Data!.MovieById.Id.Should().Be(1);
  }

}

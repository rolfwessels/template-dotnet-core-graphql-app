using System.Threading.Tasks;
using TemplateDotnetCoreConsoleApp.Client;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace TemplateDotnetCoreConsoleApp.Api.Tests.GraphQL;

public class MutationTests : BaseApiTests
{ 
  [Test]
  [Category("Integration")]
  public async Task Movies_GivenClient_ShouldReturnMovies()
  {
    // arrange
    var client = Services.GetRequiredService<IApiClient>();
    var moviesBefore = await client.AllMovies.ExecuteAsync();
    // action
    var operationResult = await client.AddMovie.ExecuteAsync(new AddMovieDetailsInput(){Title = "Sample1",ActorIds =new []{1,2} });
    var moviesAfter = await client.AllMovies.ExecuteAsync();
    // assert
    operationResult.Data!.AddMovie.Id.Should().Be(3);
    moviesBefore.Data!.Movies.Should().HaveCount(2);
    moviesAfter.Data!.Movies.Should().HaveCount(3);
  }

}

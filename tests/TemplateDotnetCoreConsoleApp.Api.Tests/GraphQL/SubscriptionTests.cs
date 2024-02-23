using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bumbershoot.Utilities.Helpers;
using TemplateDotnetCoreConsoleApp.Client;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace TemplateDotnetCoreConsoleApp.Api.Tests.GraphQL;

public class SubscriptionTests : BaseApiTests
{
  [Test]
  [Category("Integration")]
  public async Task Movies_GivenClient_ShouldReturnMovies()
  {
    // arrange
    // todo: Rolf Would be nice if we could simulate the web sockets in the client
    var client = RealService().GetRequiredService<IApiClient>();
    var ids = new List<int>();
    var observable = client.MoviesAddedSubscription.Watch();
    using var disposable = observable.Subscribe(x => { ids.Add(x.Data!.MovieAdded.Id); });
    await Task.Delay(1000); // ugly but we need to wait for the subscription!
    // action
    var operationResult = await client.AddMovie.ExecuteAsync(new AddMovieDetailsInput()
      { Title = "Sample1", ActorIds = new[] { 1, 2 } });
    // assert
    operationResult.Data!.AddMovie.Id.Should().BeGreaterThan(1);
    ids.WaitFor(x => x.Count > 0).Should().HaveCount(1);
  }
}

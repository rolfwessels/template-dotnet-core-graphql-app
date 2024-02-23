using TemplateDotnetCoreConsoleApp.Core.Components;
using HotChocolate.Subscriptions;

namespace TemplateDotnetCoreConsoleApp.Api.GraphQL;

public class Mutation
{
  public async Task<Movie> AddMovie([Service] MovieStore movieStore, AddMovieDetails detail, [Service] ITopicEventSender sender)
  {
    var movie = new Movie
    {
      Id = movieStore.Movies().Count + 1,
      Title = detail.Title,
      ActorIds = detail.ActorIds
    };
    movieStore.Movies().Add(movie);
    await sender.SendAsync(Subscription.Topics.MovieAdded, movie);
    return movie;
  }

  public record AddMovieDetails(string Title, int[] ActorIds);

}

using TemplateDotnetCoreConsoleApp.Core.Components;

namespace TemplateDotnetCoreConsoleApp.Api.GraphQL.Movies;

[ExtendObjectType(typeof(Movie))]
public class MovieExtenstion
{
  public Task<IReadOnlyList<Actor>> GetActors([Parent] Movie movie,[Service] ActorDataLoader actorDataLoader)
  {
    return actorDataLoader.LoadAsync(movie.ActorIds);
  }
}

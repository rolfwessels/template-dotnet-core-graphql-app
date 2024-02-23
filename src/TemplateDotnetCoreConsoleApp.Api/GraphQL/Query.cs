using TemplateDotnetCoreConsoleApp.Core.Components;

namespace TemplateDotnetCoreConsoleApp.Api.GraphQL;

public class Query
{
  public List<Movie> GetMovies([Service] MovieStore movieStore) => movieStore.Movies();

  public Movie? GetMovieById(int id,[Service] MovieStore movieStore) => movieStore.Movies().FirstOrDefault(x => x.Id == id);
}


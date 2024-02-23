using TemplateDotnetCoreConsoleApp.Core.Components;

namespace TemplateDotnetCoreConsoleApp.Api.GraphQL;

public class Subscription
{
  public class Topics
  {
    public const string MovieAdded = "MovieAdded";
  }

  [Subscribe]
  [Topic(Topics.MovieAdded)]
  public Movie MovieAdded([EventMessage] Movie book) => book;
}

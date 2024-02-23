using System.Collections.Generic;
using System.Linq;

namespace TemplateDotnetCoreConsoleApp.Core.Components;

public class MovieStore
{
  private readonly List<Movie> _movies;
  private readonly List<Actor> _actors;

  public MovieStore()
  {
    _actors = new List<Actor>
    {
      new()
      {
        Id = 1,
        FirstName = "Bob",
        LastName = "Kante"
      },
      new()
      {
        Id = 2,
        FirstName = "Mary",
        LastName = "Poppins"
      }
    };
    _movies = new List<Movie>
    {
      new()
      {
        Id = 1,
        Title = "The Rise of the GraphQL Warrior",
        ActorIds = _actors.Select(x => x.Id).Take(1).ToArray()
      },
      new()
      {
        Id = 2,
        Title = "The Rise of the GraphQL Warrior Part 2",
        ActorIds = _actors.Select(x => x.Id).ToArray()
      }
    };
  }

  public List<Movie> Movies()
  { 
    return _movies;
  }

  public List<Actor> Actors()
  {
    return _actors;
  }
}

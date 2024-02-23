namespace TemplateDotnetCoreConsoleApp.Core.Components;

public class Movie
{
  public int Id { get; set; }
  public string Title { get; set; } = null!;
  public int[] ActorIds { get; set; } = null!;
}

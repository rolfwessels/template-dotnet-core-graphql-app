using TemplateDotnetCoreConsoleApp.Core.Components;

namespace TemplateDotnetCoreConsoleApp.Api.GraphQL.Movies;

public class ActorDataLoader : BatchDataLoader<int, Actor>
{
  private readonly MovieStore _movieStore;

  public ActorDataLoader(IBatchScheduler batchScheduler, MovieStore movieStore)
    : base(batchScheduler)
  {
    _movieStore = movieStore;
  }

  protected override Task<IReadOnlyDictionary<int, Actor>> LoadBatchAsync(IReadOnlyList<int> keys,
    CancellationToken cancellationToken)
  {
    return Task.FromResult<IReadOnlyDictionary<int, Actor>>(_movieStore.Actors().Where(x => keys.Contains(x.Id))
      .ToDictionary(x => x.Id));
  }
}

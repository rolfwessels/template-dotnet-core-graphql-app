using System.ComponentModel.DataAnnotations;
using TemplateDotnetCoreConsoleApp.Api.GraphQL;
using TemplateDotnetCoreConsoleApp.Api.GraphQL.Movies;
using Serilog;

namespace TemplateDotnetCoreConsoleApp.Api;

internal static class GraphQlSetupHelper
{
  public static void AddTemplateDotnetCoreConsoleAppServer(this IServiceCollection services)
  {
    services.AddGraphQLServer()
      .AddQueryType<Query>()
      .AddMutationType<Mutation>()
      .AddSubscriptionType<Subscription>()
      .AddDataLoader<ActorDataLoader>()
      .AddTypeExtension<MovieExtenstion>()
      .AddInMemorySubscriptions()
      .AddErrorFilter(error =>
      {
        if (error.Exception != null) Log.Error(error.Exception, "Error on graphql request:" + error.Exception.Message);
        return error.Exception switch
        {
          ValidationException c => error.WithMessage(c.Message).WithCode("Validation"),
          KeyNotFoundException c => error.WithMessage(c.Message).WithCode("KeyNotFound"),
          _ => error
        };
      });
  }
}

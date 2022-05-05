using Adventure.Domain.ValueTypes;

namespace BaseTests;

public static class HelperTests
{
    public static Adventure.Domain.User User => new Adventure.Domain.User("test.com");
    public static Adventure.Dto.User UserDto => new Adventure.Dto.User("test.com");

    public static Adventure.Domain.Adventure Adventure => new Adventure.Domain.Adventure("Test", new List<Route>()
    {
        new Route("", "Do I want a doughnut?", new List<Route>()
        {
            new Route("Yes", null, new List<Route>()
            {
                new Route("", "Do I Deserve it?", new List<Route>()
                {
                    new Route("Yes", "Are you sure?", new List<Route>()
                    {
                        new Route("Yes", "Get it.", null),
                        new Route("No", "Do jumping jacks.", null)
                    }),
                    new Route("No", "Is it a good doughnut?", new List<Route>()
                    {
                        new Route("Yes", "What are you waiting for? Grab it now.", null),
                        new Route("No", "Wait 'til you find a sinful unforgettable doughnut.", null)
                    })
                })
            }),
            new Route("No", "Maybe you want an apple?", null)
        })
    });

    public static Adventure.Dto.Adventure AdventureDto => new Adventure.Dto.Adventure("Test", new List<Adventure.Dto.Route>()
    {
        new Adventure.Dto.Route("", "Do I want a doughnut?", new List<Adventure.Dto.Route>()
        {
            new Adventure.Dto.Route("Yes", null, new List<Adventure.Dto.Route>()
            {
                new Adventure.Dto.Route("", "Do I Deserve it?", new List<Adventure.Dto.Route>()
                {
                    new Adventure.Dto.Route("Yes", "Are you sure?", new List<Adventure.Dto.Route>()
                    {
                        new Adventure.Dto.Route("Yes", "Get it.", null),
                        new Adventure.Dto.Route("No", "Do jumping jacks.", null)
                    }),
                    new Adventure.Dto.Route("No", "Is it a good doughnut?", new List<Adventure.Dto.Route>()
                    {
                        new Adventure.Dto.Route("Yes", "What are you waiting for? Grab it now.", null),
                        new Adventure.Dto.Route("No", "Wait 'til you find a sinful unforgettable doughnut.", null)
                    })
                })
            }),
            new Adventure.Dto.Route("No", "Maybe you want an apple?", null)
        })
    });

    public static Adventure.Domain.UserAdventureAggregate UserAdventureAggregate => new Adventure.Domain.UserAdventureAggregate(User, Adventure, DateTime.UtcNow, new List<SelectedRoute>()
    {
        new SelectedRoute("", "Do I want a doughnut?"),
        new SelectedRoute("Yes", null),
        new SelectedRoute("", "Do I Deserve it?"),
        new SelectedRoute("Yes", "Are you sure?"),
        new SelectedRoute("Yes", "Get it.")
    });

    public static List<Adventure.Dto.SelectedRoute> SelectedRoutesDto => new List<Adventure.Dto.SelectedRoute>()
    {
        new Adventure.Dto.SelectedRoute("", "Do I want a doughnut?"),
        new Adventure.Dto.SelectedRoute("Yes", null),
        new Adventure.Dto.SelectedRoute("", "Do I Deserve it?"),
        new Adventure.Dto.SelectedRoute("Yes", "Are you sure?"),
        new Adventure.Dto.SelectedRoute("Yes", "Get it.")
    };
}

using GraphQL.Types;

namespace GraphDemo.Models
{
    public class GraphQLRoute : ObjectGraphType<Route>
    {
        public GraphQLRoute()
        {
            Name = "Route";

            Field(route => route.Start, type: typeof(GraphQLAirport)).Description("This routes start.");
            Field(route => route.Distance, type: typeof(IntGraphType)).Description("The routes distance.");
            Field(route => route.Destination, type: typeof(GraphQLAirport)).Description("The routes destination.");
        }
    }
}

using GraphDemo.Interfaces;
using GraphQL.Types;

namespace GraphDemo.Models
{
    public class GraphQLAirport : ObjectGraphType<Airport>
    {
        public GraphQLAirport(
            IAirportResolver airportResolver
            )
        {
            Name = "Airport";

            Field(airPort => airPort.Id, type: typeof(StringGraphType)).Description("Internal Id of the airport node");
            Field(airPort => airPort.PartitionKey, type: typeof(StringGraphType)).Description("Internal PartitionKey of the airport node");
            Field(airPort => airPort.Label, type: typeof(StringGraphType)).Description("Internal label of the airport node");
            Field(airPort => airPort.Code, type: typeof(StringGraphType)).Description("The airports three-letter IATA code.");
            Field(airPort => airPort.City, type: typeof(StringGraphType)).Description("The location of the airport.");
            Field(airPort => airPort.Runways, type: typeof(IntGraphType)).Description("The number of runways at the airport.");
            Field(airPort => airPort.Lat, type: typeof(DecimalGraphType)).Description("The airports lat. coordinates.");
            Field(airPort => airPort.Lon, type: typeof(DecimalGraphType)).Description("The airports lon. coordinates.");

            FieldAsync<ListGraphType<GraphQLRoute>>(
                   "OutboundRoutes",
                   "List of all outbound routes.",
                   resolve: async resolveFieldContext => await airportResolver.GetOutboundRoutesByAirportCode(resolveFieldContext));

            FieldAsync<ListGraphType<GraphQLRoute>>(
                   "InboundRoutes",
                   "List of all intbound routes.",
                   resolve: async resolveFieldContext => await airportResolver.GetInboundRoutesByAirportCode(resolveFieldContext));

            FieldAsync<ListGraphType<GraphQLStaff>>(
                  "AirportStaff",
                  "All staff at this airport.",
                  resolve: async resolveFieldContext => await airportResolver.GetAirportStaff(resolveFieldContext));
        }
    }
}

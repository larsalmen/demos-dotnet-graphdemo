using GraphDemo.Interfaces;
using GraphDemo.Models;
using GraphQL;
using GraphQL.Types;

namespace GraphDemo.Query
{
    public class GraphQLQuery : ObjectGraphType<object>
    {
        public GraphQLQuery(
             IAirportResolver airportResolver,
             IStaffResolver staffResolver
            )
        {
            // Uppdelad i likhet med REST
            FieldAsync<ListGraphType<GraphQLAirport>>(
                   "Airports",
                   "List of all airports.",
                   resolve: async resolveFieldContext => await airportResolver.GetAirports(resolveFieldContext));

            FieldAsync<GraphQLAirport>(
                   "Airport",
                   "Get airport by IATA code.",
                    arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "code", Description = "IATA-code of the airport." }
                    ),
                   resolve: async resolveFieldContext =>
                   await airportResolver.GetAirportByCode(
                       resolveFieldContext.GetArgument<string>("code"))
                   );

            // Samma metod, med valbar parameter.
            FieldAsync<ListGraphType<GraphQLStaff>>(
                   "Staff",
                   "Staff. Filters on optional 'city' parameter.",
                   arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "city", Description = "City parameter to filter staff on." }
                    ),
                   resolve: async resolveFieldContext =>
                   {
                       var city = resolveFieldContext.GetArgument<string>("city");

                       if (string.IsNullOrEmpty(city))
                       {
                           return await staffResolver.GetAllStaff();
                       }
                       else
                       {
                           return await staffResolver.GetStaffByCity(city);
                       }
                   });
        }
    }
}

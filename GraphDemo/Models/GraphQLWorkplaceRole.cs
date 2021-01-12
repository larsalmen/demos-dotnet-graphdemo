using GraphQL.Types;

namespace GraphDemo.Models
{
    public class GraphQLWorkplaceRole : ObjectGraphType<WorkplaceRole>
    {
        public GraphQLWorkplaceRole()
        {
            Name = "WorkplaceRole";

            Field(role => role.Workplace, type: typeof(GraphQLAirport)).Description("Workplace");
            Field(role => role.Role, type: typeof(StringGraphType)).Description("Role at workplace");
        }
    }
}

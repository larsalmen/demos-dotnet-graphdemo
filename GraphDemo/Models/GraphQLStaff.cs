using GraphDemo.Interfaces;
using GraphQL.Types;

namespace GraphDemo.Models
{
    public class GraphQLStaff : ObjectGraphType<Staff>
    {
        public GraphQLStaff(
            IStaffResolver staffResolver
            )
        {
            Name = "Staff";

            Field(staff => staff.Id, type: typeof(StringGraphType)).Description("Internal Id of the staff node");
            Field(staff => staff.PartitionKey, type: typeof(StringGraphType)).Description("Internal PartitionKey of the staff node");
            Field(staff => staff.Label, type: typeof(StringGraphType)).Description("Internal label of the staff node");
            Field(staff => staff.Name, type: typeof(StringGraphType)).Description("Name of the staff");

            FieldAsync<ListGraphType<GraphQLWorkplaceRole>>(
                   "Workplaces",
                   "Workplaces",
                   resolve: async resolveFieldContext => await staffResolver.GetWorkplaces(resolveFieldContext));

            FieldAsync<ListGraphType<GraphQLStaff>>(
                   "WorksWith",
                   "Co-workers",
                   resolve: async resolveFieldContext => await staffResolver.GetCoWorkersAsync(resolveFieldContext));

            // Future fun
            //FieldAsync<ListGraphType<GraphQLStaff>>(
            //       "Manages",
            //       "Minions",
            //       resolve: async resolveFieldContext => await staffResolver.GetManagesAsync(resolveFieldContext));

            //FieldAsync<ListGraphType<GraphQLStaff>>(
            //       "ManagedBy",
            //       "Managers",
            //       resolve: async resolveFieldContext => await staffResolver.GetManagedByAsync(resolveFieldContext));
        }
    }
}

using GraphQL.Utilities;
using System;

namespace GraphDemo.Query
{
    public class Schema : GraphQL.Types.Schema
    {
        public Schema(IServiceProvider provider) : base(provider)
        {
            Query = provider.GetRequiredService<GraphQLQuery>();
        }
    }
}

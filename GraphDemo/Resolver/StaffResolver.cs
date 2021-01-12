using GraphDemo.Interfaces;
using GraphDemo.Models;
using GraphDemo.Repositories;
using GraphQL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphDemo.Resolver
{
    public class StaffResolver : IStaffResolver
    {
        private readonly IGraphRepository _repository;
        public StaffResolver(
            IGraphRepository repository
            )
        {
            _repository = repository;
        }

        public async Task<List<Staff>> GetAllStaff()
        {
            var completeQuery =
                $"g.V()" +
                $".hasLabel('staff')" +
                $".{PreparedGremlinProjections.StaffProjection}.fold()";

            var result = await _repository.SubmitGremlinQuery<List<Staff>>(completeQuery);

            return result;
        }

        public async Task<List<Staff>> GetCoWorkersAsync(IResolveFieldContext<Staff> resolveFieldContext)
        {
            var traversalParameters = new Dictionary<string, object>
            {
                {"partitionKey", "staff" },
                {"id", resolveFieldContext.Source.Id }
            };

            var completeQuery =
                $"g.V([partitionKey, id])" +
                $".out('workplace')" +
                $".in('workplace')" +
                $".has('id',neq(id))" +
                $".{PreparedGremlinProjections.StaffProjection}" +
                $".dedup()" +
                $".fold()";

            var result = await _repository.SubmitGremlinQuery<List<Staff>>(completeQuery, traversalParameters);

            return result;
        }

        public async Task<List<Staff>> GetStaffByCity(string city)
        {
            var traversalParameters = new Dictionary<string, object>
            {
                {"city", city }
            };

            var completeQuery =
                $"g.V()" +
                $".hasLabel('staff')" +
                $".where(outE('workplace')" +
                $".inV()" +
                $".has('city', city))" +
                $".{PreparedGremlinProjections.StaffProjection}" +
                $".fold()";

            var result = await _repository.SubmitGremlinQuery<List<Staff>>(completeQuery, traversalParameters);

            return result;
        }

        public async Task<List<WorkplaceRole>> GetWorkplaces(IResolveFieldContext<Staff> resolveFieldContext)
        {
            var traversalParameters = new Dictionary<string, object>
            {
                {"partitionKey", "staff" },
                {"id", resolveFieldContext.Source.Id }
            };

            var completeQuery =
                $"g.V([partitionKey, id])" +
                $".outE('workplace')" +
                $".as('edge')" +
                $".project('workplace', 'role')" +
                $".by(select('edge')" +
                    $".inV().{PreparedGremlinProjections.AirportProjection})" +
                $".by(coalesce(select('edge')" +
                    $".values('role'), constant('')))" +
                $".fold()";

            var result = await _repository.SubmitGremlinQuery<List<WorkplaceRole>>(completeQuery, traversalParameters);

            return result;
        }

        // Future
        public async Task<List<Staff>> GetManagedByAsync(IResolveFieldContext<Staff> resolveFieldContext)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<Staff>> GetManagesAsync(IResolveFieldContext<Staff> resolveFieldContext)
        {
            throw new System.NotImplementedException();
        }
    }
}

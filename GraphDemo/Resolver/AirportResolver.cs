using GraphDemo.Interfaces;
using GraphDemo.Models;
using GraphDemo.Repositories;
using GraphQL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphDemo.Resolver
{
    public class AirportResolver : IAirportResolver
    {
        private readonly IGraphRepository _repository;
        public AirportResolver(
            IGraphRepository repository
            )
        {
            _repository = repository;
        }

        public async Task<Airport> GetAirportByCode(string iataCode)
        {
            var traversalParameters = new Dictionary<string, object>
            {
                {"iataCode", iataCode }
            };

            var completeQuery =
                $"g.V()" +
                $".has('code', iataCode)" +
                $".{GraphRepository.AirportProjection}";

            var result = await _repository.SubmitGremlinQuery<Airport>(completeQuery, traversalParameters);

            return result;
        }

        public async Task<List<Airport>> GetAirports(IResolveFieldContext resolveFieldContext)
        {
            var completeQuery =
                $"g.V()" +
                $".hasLabel('airport')" +
                $".{GraphRepository.AirportProjection}" +
                $".fold()";

            var result = await _repository.SubmitGremlinQuery<List<Airport>>(completeQuery);

            return result;
        }

        public async Task<List<Route>> GetInboundRoutesByAirportCode(IResolveFieldContext<Airport> resolveFieldContext)
        {
            var traversalParameters = new Dictionary<string, object>
            {
                {"code", resolveFieldContext.Source.Code }
            };

            var completeQuery =
                $"g.V()" +
                $".has('code', code)" +
                $".{GraphRepository.InboundRouteProjection}" +
                $".fold()";

            var result = await _repository.SubmitGremlinQuery<List<Route>>(completeQuery, traversalParameters);

            return result;
        }

        public async Task<List<Route>> GetOutboundRoutesByAirportCode(IResolveFieldContext<Airport> resolveFieldContext)
        {
            var traversalParameters = new Dictionary<string, object>
            {
                {"code", resolveFieldContext.Source.Code }
            };

            var completeQuery =
                $"g.V()" +
                $".has('code', code)" +
                $".{GraphRepository.OutboundRouteProjection}" +
                $".fold()";

            var result = await _repository.SubmitGremlinQuery<List<Route>>(completeQuery, traversalParameters);

            return result;
        }

        public async Task<List<Staff>> GetAirportStaff(IResolveFieldContext<Airport> resolveFieldContext)
        {
            var traversalParameters = new Dictionary<string, object>
            {
                {"partitionKey", "airports" },
                {"id", resolveFieldContext.Source.Id }
            };

            var completeQuery =
                $"g.V([partitionKey, id])" +
                $".in('workplace')" +
                $".{GraphRepository.StaffProjection}" +
                $".dedup()" +
                $".fold()";

            var result = await _repository.SubmitGremlinQuery<List<Staff>>(completeQuery, traversalParameters);

            return result;
        }
    }
}

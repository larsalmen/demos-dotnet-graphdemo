using GraphDemo.Models;
using GraphQL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphDemo.Interfaces
{
    public interface IAirportResolver
    {
        Task<List<Airport>> GetAirports(IResolveFieldContext resolveFieldContext);
        Task<List<Route>> GetOutboundRoutesByAirportCode(IResolveFieldContext<Airport> resolveFieldContext);
        Task<List<Route>> GetInboundRoutesByAirportCode(IResolveFieldContext<Airport> resolveFieldContext);
        Task<Airport> GetAirportByCode(string iataCode);
        Task<List<Staff>> GetAirportStaff(IResolveFieldContext<Airport> resolveFieldContext);
    }
}

using GraphDemo.Models;
using GraphQL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphDemo.Interfaces
{
    public interface IStaffResolver
    {
        Task<List<Staff>> GetCoWorkersAsync(IResolveFieldContext<Staff> resolveFieldContext);
        Task<List<Staff>> GetManagesAsync(IResolveFieldContext<Staff> resolveFieldContext);
        Task<List<Staff>> GetManagedByAsync(IResolveFieldContext<Staff> resolveFieldContext);
        Task<List<WorkplaceRole>> GetWorkplaces(IResolveFieldContext<Staff> resolveFieldContext);

        Task<List<Staff>> GetAllStaff();
        Task<List<Staff>> GetStaffByCity(string city);
    }
}

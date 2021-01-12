using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphDemo.Interfaces
{
    public interface IGraphRepository
    {
        Task<T> SubmitGremlinQuery<T>(string query, Dictionary<string, object> traversalParameters = null);
    }
}

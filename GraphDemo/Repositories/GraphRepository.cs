using GraphDemo.Interfaces;
using Gremlin.Net.Driver;
using Gremlin.Net.Structure.IO.GraphSON;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace GraphDemo.Repositories
{
    public class GraphRepository : IGraphRepository
    {
        private readonly GremlinClient _gremlinClient;
        private readonly ILogger _logger;

        public GraphRepository(
            IOptions<RepositoryConfiguration> configuration,
            ILogger<GraphRepository> logger
            )
        {
            var gremlinServer = new GremlinServer(
                   configuration.Value.Hostname,
                   int.Parse(configuration.Value.Port),
                   true,
                   configuration.Value.Username,
                   configuration.Value.AuthKey
                   );

            var connectionPoolSettings = new ConnectionPoolSettings()
            {
                MaxInProcessPerConnection = 10,
                PoolSize = 30,
                ReconnectionAttempts = 3,
                ReconnectionBaseDelay = TimeSpan.FromMilliseconds(500)
            };

            var webSocketConfiguration = new Action<ClientWebSocketOptions>(options =>
            {
                options.KeepAliveInterval = TimeSpan.FromSeconds(10);
            });

            _gremlinClient = new GremlinClient(
                    gremlinServer,
                    new GraphSON2Reader(),
                    new GraphSON2Writer(),
                    GremlinClient.GraphSON2MimeType,
                    connectionPoolSettings,
                    webSocketConfiguration);

            _logger = logger;
        }

        public async Task<T> SubmitGremlinQuery<T>(string query, Dictionary<string, object> traversalParameters = null)
        {
            _logger.LogInformation($"Submitting {nameof(query)}: {query}");

            try
            {
                var resultSet = await _gremlinClient.SubmitAsync<object>(query, traversalParameters);

                if (resultSet?.Any() != true)
                {
                    return default;
                }

                try
                {
                    var serializedResult = JsonConvert.SerializeObject(resultSet);
                    var returnObjectList = JsonConvert.DeserializeObject<List<T>>(serializedResult);

                    return returnObjectList.FirstOrDefault();
                }
                catch (JsonException ex)
                {
                    _logger.LogError(ex, $"Deserialization of {resultSet} failed.");

                    throw new Exception("Deserialization exception", ex);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(SubmitGremlinQuery)} failed for {nameof(query)}: {query}");

                throw;
            }
        }
    }
}

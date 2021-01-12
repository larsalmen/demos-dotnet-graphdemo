// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Gremlin.Net.Driver;
using System.Net.WebSockets;
using Gremlin.Net.Structure.IO.GraphSON;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Azure.Storage.Blobs;
using System.Linq;
using System.IO;
using System.Threading;
using System.Text;
using System.Collections.Generic;

namespace BlobReader
{
    public class BlobReaderFunction
    {
        private GremlinClient _gremlinClient;
        private BlobContainerClient _blobContainerClient;

        [FunctionName("BlobReaderFunction")]
        public async Task Run([EventGridTrigger] EventGridEvent eventGridEvent, ILogger log)
        {
            log.LogInformation(eventGridEvent.Data.ToString());

            try
            {

                var host = Environment.GetEnvironmentVariable("GremlinHost");
                var port = Environment.GetEnvironmentVariable("GremlinPort");
                var authKey = Environment.GetEnvironmentVariable("GremlinAuthKey");
                var database = Environment.GetEnvironmentVariable("GremlinDatabase");
                var container = Environment.GetEnvironmentVariable("GremlinContainer");
                var containerLink = $"/dbs/{database}/colls/{container}";

                var gremlinServer = new GremlinServer(
                    host,
                    int.Parse(port),
                    true,
                    containerLink,
                    authKey
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
            }
            catch (Exception ex)
            {
                log.LogError(ex, $"Setup of GremlinClient failed, message: {ex.Message}");
                throw;
            }

            try
            {

                var storageConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
                var blobContainerName = Environment.GetEnvironmentVariable("BlobContainerName");

                _blobContainerClient = new BlobContainerClient(storageConnectionString, blobContainerName);
            }
            catch (Exception ex)
            {
                log.LogError(ex, $"Setup of BlobClient failed, message: {ex.Message}");
                throw;
            }

            string blobUrl;
            try
            {
                var parsedObject = JObject.Parse(eventGridEvent.Data.ToString());

                log.LogInformation(parsedObject["url"].ToString());
                blobUrl = parsedObject["url"].ToString();
            }
            catch (Exception ex)
            {
                log.LogError(ex, $"Parsing data broke, ex: {ex}");
                throw;
            }

            // Read blob
            var queryList = new List<string>();
            try
            {
                var blobName = blobUrl.Split('/').Last();

                var client = _blobContainerClient.GetBlobClient(blobName);

                await using var memoryStream = new MemoryStream();
                var result = await client.DownloadToAsync(memoryStream, CancellationToken.None);

                var text = Encoding.UTF8.GetString(memoryStream.ToArray());

                log.LogInformation($"FileContent as string: {text}");

                queryList = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();
            }
            catch (Exception ex)
            {
                log.LogError(ex, $"Reading blob failed, ex: {ex}");
                throw;
            }

            try
            {
                foreach (var query in queryList)
                {
                    await _gremlinClient.SubmitAsync<dynamic>(query);
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex, $"Gremlin broke, ex: {ex}");

                throw;
            }
        }
    }
}

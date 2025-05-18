using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;

namespace TheFloor.Function
{
    public static class broadcastPlayer
    {
        // Define a class to match your expected JSON structure
        private class PlayerData
        {
            public int Count { get; set; }
            // Add other properties if needed
        }

        [FunctionName("broadcastPlayer")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log, [SignalR(HubName = "TheFloorHub")] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                log.LogInformation($"Received body: {requestBody}");

                // Try to parse as a complex object first
                var data = JsonConvert.DeserializeObject<PlayerData>(requestBody);
                int playerCount = data.Count;

                log.LogInformation($"Player count: {playerCount}");
                await signalRMessages.AddAsync(
                    new SignalRMessage
                    {
                        Target = "broadcastPlayer",
                        Arguments = new object[] { playerCount }
                    });

                return new OkResult();
            }
            catch (Exception ex)
            {
                log.LogError($"Error processing request: {ex.Message}");
                return new BadRequestObjectResult($"Invalid request format: {ex.Message}");
            }
        }
    }
}

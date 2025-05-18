using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace TheFloor.Function
{
    public static class negotiate
    {
        [FunctionName("negotiate")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log, [SignalRConnectionInfo(HubName = "TheFloorHub")] SignalRConnectionInfo connectionInfo)
        {
            return new OkObjectResult(connectionInfo);
        }
    }
}

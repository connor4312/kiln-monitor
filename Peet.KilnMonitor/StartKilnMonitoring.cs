namespace Peet.KilnMonitor
{
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Peet.KilnMonitor.TableEntities;

    public static class StartKilnMonitoring
    {
        // [FunctionName(nameof(StartKilnMonitoring))]
        // public static async Task<IActionResult> Run(
        //     [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "api/kilns/{kiln}/{firing}/poll")]
        //     HttpRequest req,
        //     DurableOrchestrationClient starter)
        // {
        //     // log.LogInformation("C# HTTP trigger function processed a request.");
        //     //
        //     // string name = req.Query["name"];
        //     //
        //     // string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        //     // dynamic data = JsonConvert.DeserializeObject(requestBody);
        //     // name = name ?? data?.name;
        //     //
        //     // return name != null
        //     //     ? (ActionResult)new OkObjectResult($"Hello, {name}")
        //     //     : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        // }
    }
}

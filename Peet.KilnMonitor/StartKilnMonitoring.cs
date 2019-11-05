namespace Peet.KilnMonitor
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Autofac;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Peet.KilnMonitor.Bartinst;
    using Peet.KilnMonitor.Contracts;
    using Peet.KilnMonitor.Web;

    /// <summary>
    /// HTTP endpoint that starts a loop to monitor a kiln firing, and then
    /// redirects to the public endpoint.
    /// </summary>
    public static class StartKilnMonitoring
    {
        private static string StartFileContents = null;

        [FunctionName(nameof(StartKilnMonitoring) + nameof(StartKilnMonitoring.Get))]
        public static IActionResult Get(
            ExecutionContext context,
            [HttpTrigger(AuthorizationLevel.Admin, "get", Route = "start")]
            HttpRequest req,
            [OrchestrationClient] DurableOrchestrationClientBase starter)
        {
            return new ContentResult()
            {
                Content = GetTemplateFile(context),
                ContentType = "text/html",
                StatusCode = 200
            };
        }

        [FunctionName(nameof(StartKilnMonitoring) + nameof(StartKilnMonitoring.Post))]
        public static async Task<IActionResult> Post(
            [HttpTrigger(AuthorizationLevel.Admin, "post", Route = "start")]
            HttpRequest req,
            [OrchestrationClient] DurableOrchestrationClientBase starter)
        {
            try
            {
                var body = req.ReadBody<StartMonitoringRequest>();
                var credentials = await Dependencies.Container
                    .Resolve<IBartinstApi>()
                    .AuthenticateAsync(body.Email, body.Password);

                if (credentials.NeedsToAcceptTos)
                {
                    throw new ErrorResponseException(ErrorCode.NeedsTOS, "User must accept terms of service in the Bartlett app.");
                }

                await starter.StartNewAsync(
                    nameof(KilnMonitoringLoop) + nameof(KilnMonitoringLoop.OrchestrateLoop),
                    new DurableMonitorRequest()
                    {
                        Email = body.Email,
                        Token = credentials.Token
                    });

                return new OkObjectResult(new {});
            }
            catch (ErrorResponseException ers)
            {
                return req.WriteError(ers.Response);
            }
        }

        private static string GetTemplateFile(ExecutionContext context)
        {
            if (StartKilnMonitoring.StartFileContents == null)
            {
                StartKilnMonitoring.StartFileContents = File.ReadAllText(
                    Path.Combine(context.FunctionAppDirectory, "Views", "start.html"));
            }

            return StartKilnMonitoring.StartFileContents;
        }
    }
}

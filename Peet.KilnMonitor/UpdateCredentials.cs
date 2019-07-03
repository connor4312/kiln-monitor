using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Peet.KilnMonitor.Contracts;
using Peet.KilnMonitor.Web;
using Peet.KilnMonitor.Bartinst;
using Autofac;
using Peet.KilnMonitor.TableEntities;
using Microsoft.WindowsAzure.Storage.Table;

namespace Peet.KilnMonitor
{
    /// <summary>
    /// Updates the bartinst credentials for the current user.
    /// </summary>
    public static class UpdateCredentials
    {
        [FunctionName(nameof(UpdateCredentials))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "/api/authenticate")] HttpRequest req,
            [Table(Tables.Accounts)] CloudTable table)
        {
            try
            {
                var body = req.ReadBody<Contracts.AuthenticateRequest>();
                var res = await Dependencies.Container.Resolve<IBartinstApi>().AuthenticateAsync(body.Email, body.Password);

                await table.ExecuteAsync(TableOperation.InsertOrReplace(new AccountEntity()
                {
                    UserId = req.GetUserId(),
                    Email = body.Email,
                    AuthKey = res.Token,
                }));

                return new NoContentResult();
            }
            catch (ErrorResponseException e)
            {
                return req.WriteError(e.Response);
            }
        }
    }
}

namespace Peet.KilnMonitor
{
    using System.Threading.Tasks;
    using Autofac;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.WindowsAzure.Storage.Table;
    using Peet.KilnMonitor.Bartinst;
    using Peet.KilnMonitor.Contracts;
    using Peet.KilnMonitor.TableEntities;
    using Peet.KilnMonitor.Web;

    /// <summary>
    /// Updates the bartinst credentials for the current user.
    /// </summary>
    public static class UpdateCredentials
    {
        [FunctionName(nameof(UpdateCredentials))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "/api/authenticate")]
            HttpRequest req,
            [Table(Tables.Accounts)] CloudTable table)
        {
            try
            {
                var body = req.ReadBody<Contracts.AuthenticateRequest>();
                AuthenticateResponse res = await Dependencies.Container.Resolve<IBartinstApi>()
                    .AuthenticateAsync(body.Email, body.Password);

                await table.ExecuteAsync(
                    TableOperation.InsertOrReplace(
                        new AccountEntity()
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

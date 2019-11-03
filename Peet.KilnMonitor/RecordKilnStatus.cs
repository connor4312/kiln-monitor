namespace Peet.KilnMonitor
{
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Autofac;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Microsoft.WindowsAzure.Storage.Table;
    using Peet.KilnMonitor.Bartinst;
    using Peet.KilnMonitor.Contracts;
    using Peet.KilnMonitor.TableEntities;
    using Peet.KilnMonitor.Web;

    /// <summary>
    ///     Function that records the kiln's status at the current time.
    /// </summary>
    public static class RecordKilnStatus
    {
        [FunctionName(nameof(RecordKilnStatus))]
        public static async Task<KilnStatus> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "api/kilns/{kiln}/firings/{firing}/poll")]
            HttpRequest req,
            [Table(Tables.Firings, "{kiln}", "{firing}")]
            FiringEntity firing,
            [Table(Tables.Accounts)]
            CloudTable accounts)
        {
            try
            {
                if (firing == null || firing.OwnerUserId != req.GetUserId())
                {
                    throw new ErrorResponseException(ErrorCode.FiringNotFound, "Firing not found", HttpStatusCode.NotFound);
                }

                var result = await accounts.ExecuteAsync(TableOperation.Retrieve<AccountEntity>(firing.OwnerUserId, firing.OwnerUserId));
                var account = (AccountEntity)result.Result;

                var data = await Dependencies.Container
                    .Resolve<IBartinstApi>()
                    .QueryAsync(account.Email, account.AuthKey);

                return 
            }
            catch (ErrorResponseException e)
            {
                return req.WriteError(e.Response);
            }
        }
    }
}

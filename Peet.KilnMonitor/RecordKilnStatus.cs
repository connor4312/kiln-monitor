using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.WindowsAzure.Storage.Blob;
using Peet.KilnMonitor.Contracts;
using Peet.KilnMonitor.TableEntities;

namespace Peet.KilnMonitor
{
    /// <summary>
    ///     Function that records the kiln's status at the current time.
    /// </summary>
    public static class RecordKilnStatus
    {
        [FunctionName(nameof(RecordKilnStatus))]
        public static async Task<KilnStatus> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "api/kilns/{kiln}/{firing}/poll")]
            [Table(Tables.Firings, )]
            [Blob("")] CloudAppendBlob blob)
        {
            try
            {
                
            }
            catch (ErrorResponseException e)
            {
                return req.WriteError(e.Response);
            }
        }
    }
}
namespace Peet.KilnMonitor
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.WindowsAzure.Storage.Table;
    using Peet.KilnMonitor.Contracts;
    using Peet.KilnMonitor.TableEntities;
    using Peet.KilnMonitor.Web;

    /// <summary>
    /// Returns a list of firing records for the current user.
    /// </summary>
    public static class GetKilnFirings
    {
        [FunctionName(nameof(GetKilnFirings))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "/api/firings")]
            HttpRequest req,
            [Table(Tables.Firings)] CloudTable table)
        {
            var query = new TableQuery<FiringEntity>();
            query.Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, req.GetUserId()));
            query.Take(30);

            string continuation = req.GetContinuationToken();
            if (continuation != null)
            {
                query.Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.GreaterThan, continuation));
            }

            var output = new List<Firing>();

            string outputContinuationToken = null;
            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<FiringEntity> results = await table.ExecuteQuerySegmentedAsync(query, token);
                token = results.ContinuationToken;

                foreach (FiringEntity entity in results.Results)
                {
                    output.Add(entity.Contract);
                    outputContinuationToken = entity.RowKey;
                }
            } while (token != null);

            if (outputContinuationToken != null)
            {
                req.SetContinuationToken(outputContinuationToken);
            }

            return new OkObjectResult(output);
        }
    }
}

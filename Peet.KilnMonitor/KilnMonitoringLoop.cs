namespace Peet.KilnMonitor
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading;
    using System.Threading.Tasks;
    using Autofac;
    using InfluxDB.Collector;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Logging;
    using Peet.KilnMonitor.Bartinst;
    using Peet.KilnMonitor.Contracts;
    using Peet.KilnMonitor.Web;

    /// <summary>
    /// Provides durable functions that monitor the kiln.
    /// </summary>
    public static class KilnMonitoringLoop
    {
        private const int FailureRetryDelay = 1000 * 20;
        private const int MaxFailures = 5;

        [FunctionName(nameof(KilnMonitoringLoop) + nameof(KilnMonitoringLoop.OrchestrateLoop))]
        public static async Task OrchestrateLoop([OrchestrationTrigger] DurableOrchestrationContext monitorContext, ILogger logger)
        {
            var req = monitorContext.GetInput<DurableMonitorRequest>();
            var failures = 0;

            while (true)
            {
                var delay = await monitorContext.CallActivityAsync<int>(nameof(KilnMonitoringLoop) + nameof(KilnMonitoringLoop.RunQuery), req);
                if (delay == int.MaxValue)
                {
                    return; // done monitoring
                }

                if (delay == -1)
                {
                    if (failures++ > KilnMonitoringLoop.MaxFailures)
                    {
                        logger.LogError("Aborting monitoring due to repeated errors");
                        return;
                    }

                    delay = KilnMonitoringLoop.FailureRetryDelay;
                }
                else
                {
                    failures = 0;
                }

                await monitorContext.CreateTimer(monitorContext.CurrentUtcDateTime.AddMilliseconds(delay), CancellationToken.None);
            }
        }

        [FunctionName(nameof(KilnMonitoringLoop) + nameof(KilnMonitoringLoop.RunQuery))]
        public static async Task<int> RunQuery([ActivityTrigger] DurableMonitorRequest request, ILogger logger)
        {
            IList<KilnStatus> results;
            try
            {
                results = await Dependencies.Container
                    .Resolve<IBartinstApi>()
                    .QueryAsync(request.Email, request.Token);
            }
            catch (Exception e)
            {
                logger.LogError("Error querying kiln status", e);
                return -1;
            }

            var metrics = Dependencies.Container.Resolve<MetricsCollector>();
            var pollDelay = int.MaxValue;

            foreach (var result in results)
            {
                var tags = new Dictionary<string, string>()
                {
                    { "kiln", result.Id.ToString() },
                };

                var fields = new Dictionary<string, object>()
                {
                    { "op_mode", result.OperationalMode ?? KilnMode.NotConnected },
                };

                // Temperatures are returned as strings, for some reason. Try
                // to number-ify any numeric strings we see to correct this.
                foreach (var field in result.Firing)
                {
                    if (field.Value == null)
                    {
                        continue;
                    }

                    object value;
                    switch (field.Key)
                    {
                        case "set_point":
                        case "zone_one_temperature":
                        case "zone_two_temperature":
                        case "zone_three_temperature":
                            value = int.TryParse(field.Value.ToString(), out var f)
                                ? f
                                : 0;
                            break;
                        case "hold_remaining":
                        case "firing_time":
                            var parts = field.Value.ToString().Split(':');
                            if (parts.Length == 2)
                            {
                                value = int.Parse(parts[0]) * 60 + int.Parse(parts[1]);
                            }
                            else
                            {
                                value = 0;
                            }
                            break;
                        case "timestamp":
                        case "status":
                            continue;
                        default:
                            value = field.Value;
                            break;
                    }

                    fields[field.Key] = value;
                }

                metrics.Write(
                    "firings",
                    new ReadOnlyDictionary<string, object>(fields),
                    new ReadOnlyDictionary<string, string>(tags));

                if (KilnMode.AwaitingStart.Contains(result.OperationalMode))
                {
                    pollDelay = Math.Min(pollDelay, 1000 * 60 * 10);
                }
                else if (KilnMode.ActiveModes.Contains(result.OperationalMode))
                {
                    pollDelay = Math.Min(pollDelay, 1000 * 60);
                }
            }

            return pollDelay;
        }
    }
}

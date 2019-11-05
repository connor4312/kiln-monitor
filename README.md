# Peet.KilnMonitor

This project is a simple set of Azure functions which read pottery kiln information from the Bartlett API, and insert it into an InfluxDB instance which can be monitored and observed with tools such as [Grafana](https://grafana.com/). It has four functions:

 1. `StartKilnMonitoring.Get` is a small UI for entering Bartlett credentials. This calls...
 2. `StartKilnMonitoring.Post` accepts credentials sent from the UI, and exchanges them for an access token. It then triggers the monitoring loop, which is a [Durable Function](https://docs.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-overview).
 3. `KilnMonitoringLoop.OrchestrateLoop` is the durable function orchestrator. The pattern here is identical to [the use-case in their docs](https://docs.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-overview#monitoring)--it calls a query function every minute, until we see that the kiln has completed its cycle or was otherwise shut down.
 4. `KilnMonitoringLoop.RunQuery` is called by the orchestration loop. It queries the Bartless API, then parses and inserts results into the Influx collector.
 
To run the app, you should set the `KILN_INFLUX_ENDPOINT`, `KILN_INFLUX_USERNAME`, `KILN_INFLUX_PASSWORD` in your app settings (environment variables) in order to ingest data.

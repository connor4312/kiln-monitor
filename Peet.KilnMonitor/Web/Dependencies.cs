namespace Peet.KilnMonitor.Web
{
    using System;
    using Autofac;
    using InfluxDB.Collector;
    using Peet.KilnMonitor.Bartinst;

    class Dependencies
    {
        public static readonly string InfluxEndpoint = Environment.GetEnvironmentVariable("KILN_INFLUX_ENDPOINT");
        public static readonly string InfluxUsername = Environment.GetEnvironmentVariable("KILN_INFLUX_USERNAME");
        public static readonly string InfluxPassword = Environment.GetEnvironmentVariable("KILN_INFLUX_PASSWORD");

        private static IContainer containerInstance;

        /// <summary>
        /// Gets the dependency contain instance.
        /// </summary>
        public static IContainer Container
        {
            get
            {
                if (Dependencies.containerInstance == null)
                {
                    Dependencies.containerInstance = Dependencies.BuildContainer();
                }

                return Dependencies.containerInstance;
            }
        }

        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<BartinstApi>().As<IBartinstApi>();
            builder.Register(ctx => Dependencies.CreateInfluxClient()).AsSelf();
            return builder.Build();
        }

        private static MetricsCollector CreateInfluxClient()
        {
            return new CollectorConfiguration()
                .Batch.AtInterval(TimeSpan.FromSeconds(2))
                .WriteTo.InfluxDB(Dependencies.InfluxEndpoint, "data", Dependencies.InfluxUsername, Dependencies.InfluxPassword)
                .CreateCollector();
        }
    }
}

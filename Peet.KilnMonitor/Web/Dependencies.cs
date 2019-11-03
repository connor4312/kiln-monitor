namespace Peet.KilnMonitor.Web
{
    using Autofac;
    using Peet.KilnMonitor.Bartinst;

    class Dependencies
    {
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

            return builder.Build();
        }
    }
}

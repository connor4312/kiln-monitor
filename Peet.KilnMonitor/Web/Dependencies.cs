using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Peet.KilnMonitor.Bartinst;

namespace Peet.KilnMonitor.Web
{
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
        }4
        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<BartinstApi>().As<IBartinstApi>();

            return builder.Build();
        }
    }
}

using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Nancy.Bootstrappers.Autofac;
using Nancy.Configuration;
using System;

namespace NYTimes.NancyApi.NancyModules
{
    internal sealed class NancyBootstraper : AutofacNancyBootstrapper
    {
        private readonly IServiceCollection _services;

        public NancyBootstraper(IServiceCollection services)
        {
            _services = services;
        }

        public override INancyEnvironment GetEnvironment() 
            => ApplicationContainer.Resolve<INancyEnvironment>();

        protected override void ConfigureApplicationContainer(ILifetimeScope container)
        {
            base.ConfigureApplicationContainer(container);

            container.Update(builder =>
            {
                builder.Populate(_services);
            });
        }

        protected override INancyEnvironmentConfigurator GetEnvironmentConfigurator() 
            => ApplicationContainer.Resolve<INancyEnvironmentConfigurator>();

        protected override void RegisterNancyEnvironment(ILifetimeScope container, INancyEnvironment environment)
        {
            container.Update(builder => builder.RegisterInstance(environment));
        }
    }
}

using System;
using Booli.API;
using Autofac;
using Booli.ML;
using Booli.ML.Interfaces;
using BooliAPI;

namespace Booli.Console
{
    public class DependencyInjection
    {
        public static ILifetimeScope BuildDependencies()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ListingModelTrainer>().As<ITrainer>();
            builder.RegisterType<ListingModelPredictor>().As<IPredictor>();
            builder.RegisterType<BooliRepository>().As<IRepository>().SingleInstance();
            builder.Register(c => new BooliApiClient(Environment.GetEnvironmentVariable("ApiKey"),
                Environment.GetEnvironmentVariable("CallerId"))).As<IAPIClient>();

            return builder.Build().BeginLifetimeScope();
        }
    }
}
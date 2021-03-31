using Autofac;
using Booli.API;
using Booli.ML;
using Booli.ML.Interfaces;
using BooliAPI;
using BooliAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booli.APP
{
  public class DependencyInjection
  {
    public static ILifetimeScope BuildDependencies()
    {
      var builder = new ContainerBuilder();

      builder.RegisterType<ListingModelTrainer>().As<ITrainer>();
      builder.RegisterType<ListingModelPredictor>().As<IPredictor>();
      builder.RegisterType<BooliRepository>().As<IRepository>().SingleInstance();
      builder.Register(c => new BooliApiClient(ConfigurationManager.AppSettings["ApiKey"], ConfigurationManager.AppSettings["CallerId"])).As<IAPIClient>();
      //builder.RegisterType<BooliApiClient>().As<IAPIClient>().WithParameter("apiKey", )
      //                                                       .WithParameter("callerId", );

      return builder.Build().BeginLifetimeScope();
    }
  }
}

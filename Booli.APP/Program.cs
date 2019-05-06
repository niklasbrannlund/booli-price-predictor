using Booli.API;
using Booli.ML;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booli.APP
{
  class Program
  {
    static void Main(string[] args)
    {
      var client = new BooliApiClient(ConfigurationManager.AppSettings["ApiKey"], ConfigurationManager.AppSettings["CallerId"]);
      //var listings = client.GetListings("svedmyra");

      //Console.WriteLine($"\r\n----------- FETCHED RESULT ----------\r\n");
      //listings.Result.CurrentListings.ToList().ForEach(s => Console.WriteLine($"Living area: {s.LivingArea}m^2\r\n" +
      //                                                                        $"Rooms: {s.Rooms}\r\n" +
      //                                                                        $"Rent: {s.Rent}\r\n" +
      //                                                                        $"Year: {s.ConstructionYear}\r\n" +
      //                                                                        $"Address: {s.Location.Address.StreetAddress}\r\n" +
      //                                                                        $"ListPrice: {s.ListPrice}\r\n" +
      //                                                                        $"PlotArea: {s.PlotArea}\r\n" +
      //                                                                        $"ObjectType: {s.ObjectType}\r\n" +
      //                                                                        $"IsNewConstruction: {s.IsNewConstruction}\r\n" +
      //                                                                        $"Names areas: {s.Location.NamedAreas.Select(x => x + ", ")}\r\n" +
      //                                                                        $"\r\n-----------------\r\n"));
      ////listings.Result.CurrentListings.ToList().ForEach(s => Console.WriteLine($"Living area: {s.LivingArea}m^2\r\nRooms: {s.Rooms}\r\nRent: {s.Rent}\r\nYear: {s.ConstructionYear}\r\nAddress: {s.Location.Address.StreetAddress}\r\nFloor: {s.Floor}\r\nSoldPrice: {s.SoldPrice}\r\n-----------------\r\n"));

      ModelTrainer model = new ModelTrainer(client, "svedmyra");
      model.GetTrainingDataAndTrainModel();
      model.EvaluateCurrentListings();
    }
  }
}

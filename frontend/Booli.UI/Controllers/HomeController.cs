using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Booli.UI.Models;
using BooliAPI.Models;
using Booli.ML.Interfaces;
using System.Linq;

namespace Booli.UI.Controllers
{
  public class HomeController : Controller
  {
    private readonly IRepository _repo;

    public HomeController(IRepository repo)
    {
      this._repo = repo;
    }

    public IActionResult Index()
    {
      var prediction = _repo.GetPredictions();
      return View(prediction);
    }

    public IActionResult Privacy()
    {
      return View();  
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}

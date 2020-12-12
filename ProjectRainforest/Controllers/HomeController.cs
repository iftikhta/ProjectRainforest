using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectRainforest.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectRainforest.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public static RainforestDBContext context = new RainforestDBContext();

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            Product foundProduct = context.Products.FirstOrDefault();
            ProductInfo foundProductInfo = context.ProductInfos.FirstOrDefault();
            List<Product> allP = context.Products.ToList();
            List<ProductInfo> allD = context.ProductInfos.ToList();
            if (allP.Count >= 3)
            {
                if (allP[1] != null)
                {
                    ViewBag.item1 = allP[0];
                    ViewBag.detail1 = allD[0];
                }
                if (allP[1] != null)
                {
                    ViewBag.item2 = allP[1];
                    ViewBag.detail2 = allD[1];
                }
                if (allP[2] != null)
                {
                    ViewBag.item3 = allP[2];
                    ViewBag.detail3 = allD[2];
                }
                return View();

            }

            return View("NoProducts");

                    
        }

        [Authorize(Roles = "Vendor")] //Example of how to make a page "Vendor only"
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

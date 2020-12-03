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
    public class HomeController : Controller
    {
        static RainforestDBContext context = new RainforestDBContext();

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ViewProducts()
        {
            
            ViewBag.items = context.Products.ToList();
            ViewBag.details = context.ProductInfos.ToList();
            return View();
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            //returning AddProduct
            return View();
        }

        [HttpGet]
        public IActionResult ViewProduct(int productID)
        {
            //ShowAProduct
            Product foundProduct = context.Products.First(x=> x.ProductId.Equals(productID));

            return View(foundProduct);
        }

        [HttpPost]
        public ViewResult AddProduct(ProductInfo productResponse, string name, int vendorId)
        {
            if (ModelState.IsValid)
            {

                Product newProduct = new Product();
                newProduct.ProductName = name;
                newProduct.VendorId = vendorId;
                context.Products.Add(newProduct);
                Product x = context.Products.Find(newProduct);
                productResponse.ProductId = x.ProductId;
                context.ProductInfos.Add(productResponse);
                context.SaveChanges();

                return View("ViewProducts");
            }
            else
            {
                // there is a validation error
                return View("Index");
            }
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

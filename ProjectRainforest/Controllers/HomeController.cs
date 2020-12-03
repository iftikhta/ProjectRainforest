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
        public static RainforestDBContext context = new RainforestDBContext();

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

        [HttpPost]
        public ViewResult AddProduct(ProductInfo productResponse, string name, int vendorId)
        {
            if (ModelState.IsValid)
            {
                int id = 0;
                int i = context.Products.ToList().Count() + 1;

               /* foreach (Product prod in (context.Products.ToList()))
                {
                    i++;
                }*/
                Product newProduct = new Product();
                newProduct.ProductId = i;
                newProduct.ProductName = name;
                newProduct.VendorId = vendorId;
                context.Products.Add(newProduct);
                //Product x = context.Products.Find(newProduct);
                context.SaveChanges();
                productResponse.Product = newProduct;
                productResponse.ProductId = i;
                context.ProductInfos.Add(productResponse);
                context.SaveChanges();
                ViewBag.items = context.Products.ToList();
                ViewBag.details = context.ProductInfos.ToList();
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

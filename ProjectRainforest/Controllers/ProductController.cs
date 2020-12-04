using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectRainforest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;


namespace ProjectRainforest.Controllers
{
    public class ProductController : Controller
    {
        public static RainforestDBContext context = new RainforestDBContext();

        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        public IActionResult ViewProducts()
        {
            ViewBag.items = context.Products.ToList();
            ViewBag.details = context.ProductInfos.ToList();
            return View();
        }


        [HttpGet]
        public IActionResult AddAProduct()
        {
            //returning AddProduct
            return View();
        }

        [HttpGet]
        public IActionResult ViewProduct(int productID)
        {
            int id = int.Parse(RouteData.Values["id"].ToString());

            Product foundProduct = context.Products.First(x => x.ProductId.Equals(id));
            ProductInfo foundProductInfo = context.ProductInfos.First(x => x.ProductId.Equals(id));
            return View(foundProduct);
        }



        [HttpPost]
        public ViewResult AddAProduct(ProductInfo productResponse, string name, int vendorId)
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

    }
}

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
    //Taha
    public class ProductController : Controller
    {
        public static RainforestDBContext context = new RainforestDBContext();

        //private readonly ILogger<ProductController> _logger;

        //public ProductController(ILogger<ProductController> logger)
        //{
        //    _logger = logger;
        //}

        public ProductController()
        {

        }

        //Tommas
        public IActionResult ViewAllProducts()
        {
            ViewBag.items = context.Products.ToList();
            ViewBag.details = context.ProductInfos.ToList();
            return View();
        }

        //Tommas
        [HttpGet]
        public IActionResult AddNewProduct()
        {
            //returning AddProduct
            return View();
        }

        //Taha
        [HttpGet]
        public ViewResult ViewProduct(string productID)
        {
            //maybe remove parameter above
            int id = int.Parse(RouteData.Values["id"].ToString());

            Product foundProduct = context.Products.FirstOrDefault(x => x.ProductId.Equals(id));
            ProductInfo foundProductInfo = context.ProductInfos.FirstOrDefault(x => x.ProductId.Equals(id));
            ViewData.Model = foundProduct;
            return View();
        }



        //Tommas
        [HttpPost]
        public ViewResult AddNewProduct(ProductInfo productResponse, string name, int vendorId)
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

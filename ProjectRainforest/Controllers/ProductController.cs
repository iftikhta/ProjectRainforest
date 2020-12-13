using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectRainforest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using ProjectRainforest.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace ProjectRainforest.Controllers
{
    //Taha
    [Authorize]
    public class ProductController : Controller
    {
        public static RainforestDBContext context = new RainforestDBContext();

        //private readonly ILogger<ProductController> _logger;

        //public ProductController(ILogger<ProductController> logger)
        //{
        //    _logger = logger;
        //}
        private readonly UserManager<RainforestUser> _userManager;
        public static string uuid;

        public ProductController(UserManager<RainforestUser> userManager)
        {
            _userManager = userManager;

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
        [Authorize(Roles = "Vendor")]
        public IActionResult AddNewProduct()
        {
            //returning AddProduct
            return View();
        }

        //Taha
        [HttpGet]
        public IActionResult ViewProduct(int productID)
        {
            //maybe remove parameter above
            //int id = int.Parse(RouteData.Values["id"].ToString());
            int id = productID;

            Product foundProduct = context.Products.FirstOrDefault(x => x.ProductId.Equals(id));
            ProductInfo foundProductInfo = context.ProductInfos.FirstOrDefault(x => x.ProductId.Equals(id));
            //ViewData.Model = foundProduct;
            ViewBag.details = context.ProductInfos.ToList();
            ViewBag.vendor = context.Vendor.FirstOrDefault(x => x.VendorId.Equals(foundProduct.VendorId));
            return View(foundProduct);
        }



        //Tommas
        [HttpPost]
        [Authorize(Roles = "Vendor")]
        public ViewResult AddNewProduct(ProductInfo productResponse, string name)
        {
            if (ModelState.IsValid)
            {


                //Getting user id and vendor id of current user
                string userId = _userManager.GetUserId(HttpContext.User);

                int vendId = (int)context.AspNetUsers.Find(userId).VendorId;

                //Creating new product and info to add to db
                Product newProduct = new Product();
                newProduct.ProductName = name;
                newProduct.VendorId = vendId;
                context.Products.Add(newProduct);
                context.SaveChanges();
                int max = context.Products.Max(p => p.ProductId);
                productResponse.Product = newProduct;
                productResponse.ProductId = max;
                productResponse.DateAdded = DateTime.Now;
                context.ProductInfos.Add(productResponse);
                context.SaveChanges();
                //Passing data to ViewAllProductsPage
                ViewBag.items = context.Products.ToList();
                ViewBag.details = context.ProductInfos.ToList();
                return View("ViewAllProducts");
            }
            else
            {
                // there is a validation error
                return View("AddNewProduct");
            }
        }

    }
}

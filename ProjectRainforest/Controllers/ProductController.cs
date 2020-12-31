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
    //Tommas
    [Authorize]
    public class ProductController : Controller
    {
        public static RainforestDBContext context = new RainforestDBContext();
        private readonly UserManager<RainforestUser> _userManager;
        public static string uuid;

        public ProductController(UserManager<RainforestUser> userManager)
        {
            //Required to get current user
            _userManager = userManager;

        }

        //Tommas
        public IActionResult ViewAllProducts()
        {
            //Passing data to ViewAllProductsPage
            //required to make products visible on ViewAllProducts Page
            ViewBag.items = context.Products.ToList();
            ViewBag.details = context.ProductInfos.ToList();
            return View();
        }

        //Jordan
        [HttpGet]
        [Authorize(Roles = "Vendor")]
        public async Task<IActionResult> AddNewProduct()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);


            if (user.VendorID == null)
            {
                return RedirectToAction("SignUp", "Vendor");
            }

            //returning AddProduct
            return View();
        }

        //Vlad
        [HttpGet]
        public IActionResult ViewProduct(int productID)
        {
            
            int id = productID;

            Product foundProduct = context.Products.FirstOrDefault(x => x.ProductId.Equals(id));
            ProductInfo foundProductInfo = context.ProductInfos.FirstOrDefault(x => x.ProductId.Equals(id));
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
                //required to make sure product has correct vendor id
                string userId = _userManager.GetUserId(HttpContext.User);

                int vendId = (int)context.AspNetUsers.Find(userId).VendorId;

                //Creating new product and info to add to db
                //required to add product so it is visible to other users
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
                //required to make products visible on ViewAllProducts Page
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
        
        [HttpGet]
        [Authorize(Roles = "Vendor")]
        public ViewResult GrabData() 
        {
            return View();
        }

    }
}

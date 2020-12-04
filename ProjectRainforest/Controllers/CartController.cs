using Microsoft.AspNetCore.Mvc;
using ProjectRainforest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ProjectRainforest.Controllers
{
    public class CartController : Controller
    {
        public static RainforestDBContext context = new RainforestDBContext();

        private readonly ILogger<CartController> _logger;

        public CartController(ILogger<CartController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        //called when you are adding a product to your cart, expects a user_id, prodct_id and quantity
        [HttpPost]
        public ViewResult AddToCart(int userId, int productId, int quantity)
        {
            //testing stuff delete later
            userId = 2;

            if (ModelState.IsValid)
            {
                //Create a new cart row if one doesnt exist already
                Cart newCartRow = new Cart();
                newCartRow.UserId = userId;
                newCartRow.ProductId = productId;
                newCartRow.Quantity = quantity;

                context.Carts.Add(newCartRow);
                context.SaveChanges();

                //int id = 0;
                //int i = context.Products.ToList().Count() + 1;

                /* foreach (Product prod in (context.Products.ToList()))
                 {
                     i++;
                 }*/
                //Product newProduct = new Product();
                //newProduct.ProductId = i;
                //newProduct.ProductName = name;
                //newProduct.VendorId = vendorId;
                //context.Products.Add(newProduct);
                ////Product x = context.Products.Find(newProduct);
                //context.SaveChanges();
                //productResponse.Product = newProduct;
                //productResponse.ProductId = i;
                //context.ProductInfos.Add(productResponse);
                //context.SaveChanges();
                //ViewBag.items = context.Products.ToList();
                //ViewBag.details = context.ProductInfos.ToList();
                return View("ViewProducts"); //go to cart view or some shit
            }
            else
            {
                // there is a validation error
                return View("Index");
            }
        }
    }
}

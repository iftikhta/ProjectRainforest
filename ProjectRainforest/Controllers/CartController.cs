using Microsoft.AspNetCore.Mvc;
using ProjectRainforest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

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

        //Taha
        //called when you are adding a product to your cart, expects a user_id, prodct_id and quantity
        [HttpPost]
        public ViewResult AddToCart(int userId, int productId, int quantity)
        {
            //fix how to recieve data 
            //testing stuff delete later
            userId = 2;

            if (ModelState.IsValid)
            {

                //check if it exists already and if so add/subtract from row
                var existingCart = context.Cart.Find(userId, productId);
                if (existingCart != null)
                {
                    existingCart.Quantity += quantity;
                    context.Entry(existingCart).State = EntityState.Modified;
                }
                else
                {
                    //Create a new cart row if one doesnt exist already
                    Cart newCartRow = new Cart();
                    newCartRow.UserId = userId.ToString();
                    newCartRow.ProductId = productId;
                    newCartRow.Quantity = quantity;

                    context.Cart.Add(newCartRow);
                }
          
                context.SaveChanges();
       

                //return (ViewResult)new ProductController().ViewProduct(productId);
                return View("ViewProducts"); //go here after finsihing update/adding new
            }
            else
            {
                // there is a validation error
                return View("Index");
            }
        }
    }
}

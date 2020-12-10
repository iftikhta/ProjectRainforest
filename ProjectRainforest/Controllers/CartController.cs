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
        //ViewCurrentCart
        public ViewResult ViewCart()
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
                var existingCart = context.Carts.Find(userId, productId);
                if (existingCart != null)
                {
                    existingCart.Quantity += quantity;
                    context.Entry(existingCart).State = EntityState.Modified;
                }
                else
                {
                    //Create a new cart row if one doesnt exist already
                    Cart newCartRow = new Cart();
                    newCartRow.UserId = userId;
                    newCartRow.ProductId = productId;
                    newCartRow.Quantity = quantity;

                    context.Carts.Add(newCartRow);
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


        //Taha
        //For Cart Page itself
        [HttpPost]
        public ViewResult UpdateCartItem(int userId, int productId, int quantity)
        {
            userId = 2;
            if (ModelState.IsValid)
            {
                //check if it exists already and if so add/subtract from row
                var existingCart = context.Carts.Find(userId, productId);
                if (existingCart != null)
                {
                    existingCart.Quantity = quantity;
                    context.Entry(existingCart).State = EntityState.Modified;
                }
                else
                {                
                    //Do nothing for now because this shouldnt be necessary anyway
                }
                context.SaveChanges();


                //return (ViewResult)new ProductController().ViewProduct(productId);
                return View("ViewCart"); //go here after finsihing update/adding new
            }
            else
            {
                // there is a validation error
                return View("Index");
            }

        }


    }
}

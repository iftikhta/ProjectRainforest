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
        [HttpGet]
        public IActionResult ViewCart(string userId)
        {
            //Get all carts for current id as 
            List<Cart> cartItems = context.Carts.Where(x => x.UserId.Equals(userId)).ToList();

            //Get all users products and product infos listed in cart by product_id 
            List<Product> cartProducts = new List<Product>();
            List<ProductInfo> cartProductInfos = new List<ProductInfo>();

            float cartTotal = 0;
            foreach (Cart c in cartItems){ //fuck im good
                Product currProduct = context.Products.Find(c.ProductId);
                ProductInfo currProductInfo = context.ProductInfos.Find(c.ProductId);
                cartProducts.Append(currProduct);
                cartProductInfos.Append(currProductInfo);

                //Some extra useful values
                cartTotal += currProductInfo.ProductPrice;
            }
            
            ViewBag.cartTotal = cartTotal;
            ViewBag.carts = cartItems;
            ViewBag.products = cartProducts;
            ViewBag.productsInfo = cartProductInfos;
            return View();
        }


        //Taha
        //called when you are adding a product to your cart, expects a user_id, prodct_id and quantity
        [HttpGet]
        public ActionResult AddToCart(string userId, int productId, int quantity)
        {
            //fix how to recieve data 
            //testing stuff delete later
            //userId = 2;

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

                ViewBag.items = context.Products.ToList();
                ViewBag.details = context.ProductInfos.ToList();
                //return (ViewResult)new ProductController().ViewProduct(productId);
                return View("ViewCart", userId); //go here after finsihing update/adding new
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
        public ViewResult UpdateCartItem(string userId, int productId, int quantity)
        {
            //userId = 2;
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

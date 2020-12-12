using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectRainforest.Areas.Identity.Data;
using ProjectRainforest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxServiceReference;

namespace ProjectRainforest.Controllers
{
    public class OrderController : Controller
    {
        public static RainforestDBContext context = new RainforestDBContext();
        private readonly UserManager<RainforestUser> _userManager;
        private readonly ILogger<OrderController> _logger;

        public OrderController(ILogger<OrderController> logger, UserManager<RainforestUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        //required functionality
            //Confirm Order page, displaying everything and have a submit button
                //Submit button that confirms order by creating a new order and removing all cart information
            //View Orders page that lets you view all order details etc


        //get access to addres etc etc
        //The view page
        public async Task<IActionResult> ConfirmOrder()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            string userId = user.Id;
            string userAddress = user.Address;

            //Jordan create whatever contract will give me access to users address field
            //string userAddress = _userManager.GetUserAddress(HttpContext.User);

            List<Cart> cartItems = context.Carts.Where(x => x.UserId.Equals(userId)).ToList();

            //Get all users products and product infos listed in cart by product_id 
            List<Product> cartProducts = new List<Product>();
            List<ProductInfo> cartProductInfos = new List<ProductInfo>();

            float cartTotal = 0;
            foreach (Cart c in cartItems)
            { //fuck im good
                Product currProduct = context.Products.Find(c.ProductId);
                ProductInfo currProductInfo = context.ProductInfos.Find(c.ProductId);
                cartProducts.Add(currProduct);
                cartProductInfos.Add(currProductInfo);

                //Some extra useful values
                cartTotal += currProductInfo.ProductPrice * c.Quantity;
            }
            ///Start Tax test
            TaxServiceClient TaxMan = new TaxServiceClient();
            double withTax = await TaxMan.CalculateTaxAsync(cartTotal).ConfigureAwait(false);

            ///End Tax test
            ViewBag.address = userAddress;
            ViewBag.cartTotalWithTax = withTax;
            ViewBag.cartTotal = cartTotal;
            ViewBag.carts = cartItems;
            ViewBag.products = cartProducts;
            ViewBag.productsInfo = cartProductInfos;
            return View();
        }


        //Add adress to the order table and then pass it here as well later
        //remove and create cart
        //post
        [HttpPost]
        public async Task<IActionResult> PlaceOrder(double total)
        {
            string userId = _userManager.GetUserId(HttpContext.User);
       

            //Store all order contents in seperate table
            List<Cart> cartItems = context.Carts.Where(x => x.UserId.Equals(userId)).ToList();

            float cartTotal = 0;
            foreach (Cart c in cartItems)
            { 
                Product currProduct = context.Products.Find(c.ProductId);
                ProductInfo currProductInfo = context.ProductInfos.Find(c.ProductId);

                //Create Cart Content




                //End Create Cart Content
            
                cartTotal += currProductInfo.ProductPrice * c.Quantity;
            }
          


            //Create an order based off currently available information
            Order newOrder = new Order();
            newOrder.UserId = userId;
            newOrder.DatePlaced = DateTimeOffset.Now;
            newOrder.OrderStatus = "Processing";
            newOrder.Total = cartTotal;


            return View();
        }



        public IActionResult ViewOrders()
        {
            return View();
        }


    }
}

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


        //Use on Cart Page to checkout
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


        //Used on Confirm Order Page to complete submission
        [HttpPost]
        public IActionResult PlaceOrder() //make async if necessary
        {
            string userId = _userManager.GetUserId(HttpContext.User);
       

            //Store all order contents in seperate table
            List<Cart> cartItems = context.Carts.Where(x => x.UserId.Equals(userId)).ToList();


            //Create an order based off currently available information
            Order newOrder = new Order();
            newOrder.UserId = userId;
            newOrder.DatePlaced = DateTimeOffset.Now;
            newOrder.OrderStatus = "Processing";
            //put into database before generating order contents
            context.Order.Add(newOrder);
            context.SaveChanges();

            //haveto get the generated id out 
            Order LastOrder = context.Order.Where(x => x.UserId.Equals(userId)).FirstOrDefault();

            float cartTotal = 0;
            foreach (Cart c in cartItems)
            { 
                Product currProduct = context.Products.Find(c.ProductId);
                ProductInfo currProductInfo = context.ProductInfos.Find(c.ProductId);

                //Create order Content

                OrderContents currOrderContent = new OrderContents();
                currOrderContent.OrderId = LastOrder.OrderId;
                currOrderContent.ProductId = c.ProductId;
                currOrderContent.Quantity = c.Quantity;
                currOrderContent.PricePaid = currProductInfo.ProductPrice;

                context.OrderContents.Add(currOrderContent);
                context.SaveChanges();

                //End Create order Content
       
                cartTotal += currProductInfo.ProductPrice * c.Quantity;

                //remove from cart table

                context.Carts.Remove(c); //if it wont work then use some kinda 
                context.SaveChanges();
            }


            //set this after getting calculations
            newOrder.Total = cartTotal;

            return View();
        }



        //View a summary of the orders
        public IActionResult ViewOrders()
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            List<Order> allOrders = context.Order.Where(x => x.UserId.Equals(userId)).ToList();
            //List<OrderContents> allOrderContents = new List<OrderContents>();
        

            //pass into viewbags so these can be displayed nicely on the front end
            return View(allOrders);
        }


        //View a detail of a specific order
        public IActionResult ViewOrderDetails(int orderId)
        {
            //can create a check if you are not the matching user
            //List<Order> allOrders = context.Order.Where(x => x.UserId.Equals(userId)).ToList();
            List<OrderContents> orderContentDetails = context.OrderContents.Where(x => x.OrderId.Equals(orderId)).ToList();


            //pass into viewbags so these can be displayed nicely on the front end
            return View(orderContentDetails);
        }


    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectRainforest.Areas.Identity.Data;
using ProjectRainforest.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using TaxServiceReference;

namespace ProjectRainforest.Controllers
{
    [Authorize]
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
            ViewBag.cartTotalWithTax = Math.Round(withTax, 2);
            ViewBag.cartTotal = cartTotal;
            ViewBag.carts = cartItems;
            ViewBag.products = cartProducts;
            ViewBag.productsInfo = cartProductInfos;
            return View();
        }


        //Used on Confirm Order Page to complete submission
       
        public IActionResult PlaceOrder() //make async if necessary
        {
            string userId = _userManager.GetUserId(HttpContext.User);
       

            //Store all order contents in seperate table
            List<Cart> cartItems = context.Carts.Where(x => x.UserId.Equals(userId)).ToList();
            if (cartItems.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            //Create an order based off currently available information
            Order newOrder = new Order();
            newOrder.UserId = userId;
            newOrder.DatePlaced = DateTimeOffset.Now;
            newOrder.OrderStatus = "Processing";
            //put into database before generating order contents
            context.Order.Add(newOrder);
            context.SaveChanges();

            //haveto get the generated id out 
            Order LastOrder = context.Order.Where(x => x.UserId.Equals(userId)).ToList().LastOrDefault();

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
            LastOrder.Total = cartTotal;
            context.Entry(LastOrder).State = EntityState.Modified;
            context.SaveChanges();


            //return View("ViewOrders");
            return RedirectToAction("ViewOrders");
        }


        //View a summary of the orders, keep it simple, fill any null data
        public IActionResult ViewOrders()
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            List<Order> allOrders = context.Order.Where(x => x.UserId.Equals(userId)).ToList();

            return View(allOrders);
        }


        //View a detail of a specific order
        [HttpGet]
        [Route("Order/ViewOrderDetails/{orderId:int}")]
        public async Task<IActionResult> ViewOrderDetails(int orderId)
        {
            
            List<OrderContents> orderContentDetails = context.OrderContents.Where(x => x.OrderId.Equals(orderId)).ToList();
            List<Product> orderProducts = new List<Product>(); //context.Products.Where(x=> x.ProductId.Equals)


            var user = await _userManager.GetUserAsync(HttpContext.User);
            string userAddress = user.Address;


            double subtotal = 0;
            foreach (OrderContents o in orderContentDetails)
            {
                orderProducts.Add(context.Products.Find(o.ProductId));
                subtotal += o.PricePaid * o.Quantity;
            }

            TaxServiceClient TaxMan = new TaxServiceClient();
            double withTax = await TaxMan.CalculateTaxAsync(subtotal).ConfigureAwait(false);


            //Get price paid and quantity from ViewBag.Orders
            //Get product name from ViewBag.Products
            //Get delivery address from Viewbag.address

            //Get subtotal
            //get post tax
            ViewBag.Orders = orderContentDetails;
            ViewBag.Products = orderProducts;
            ViewBag.Address = userAddress;
            ViewBag.Subtotal = subtotal;
            ViewBag.TotalWithTax = Math.Round(withTax, 2);

            return View();
        }


    }
}

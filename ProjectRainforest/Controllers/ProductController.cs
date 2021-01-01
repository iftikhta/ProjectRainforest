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
using System.Net.Http;
using Newtonsoft.Json;

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



        //Create a model to read data into/create objects of the book type, currently hardcoded for this purpose
        //Convert data into the model type 
        //Convert that model type into product/product info types and add them to the database
        [HttpPost]
        [Authorize(Roles = "Vendor")]
        public async Task<ViewResult> AddProductsFromAPI()
        {
            string UrlInput = Request.Form["url"];

            HttpClient client = new HttpClient();
            var httpResponse = await client.GetAsync(UrlInput);


            string ReadContent = await httpResponse.Content.ReadAsStringAsync();

            var TutorList = JsonConvert.DeserializeObject<Tutors>(ReadContent).Data;
            
           
            

            string userId = _userManager.GetUserId(HttpContext.User);
            int vendId = (int)context.AspNetUsers.Find(userId).VendorId;

            foreach (Tutor t in TutorList)
            { 
                Product newProduct = new Product();
                newProduct.ProductName = t.Tutor_name;
                newProduct.VendorId = vendId;
                context.Products.Add(newProduct);
                context.SaveChanges();
                int max = context.Products.Max(p => p.ProductId);

                ProductInfo pInfo = new ProductInfo();
                pInfo.Product = newProduct;
                pInfo.ProductId = max;
                pInfo.DateAdded = t.Tutor_date_joined;
                pInfo.ProductDescription = t.Tutor_description + "\nSubject names: " + t.Tutor_Subjects;
                pInfo.ProductImg = t.Tutor_img;
                pInfo.ProductPrice = t.Tutor_rate;
                pInfo.ProductRating = t.Tutor_rating;

                
                context.ProductInfos.Add(pInfo);
                context.SaveChanges();
                //Passing data to ViewAllProductsPage
                //required to make products visible on ViewAllProducts Page
                //ViewBag.items = context.Products.ToList();
                //ViewBag.details = context.ProductInfos.ToList();
            }

            return View("AddProductsFromAPI", "Complete");

        }

        //Edit page should DISPLAY the current image and ideally update to the newly entered image on the same page using set image button
        //Edit page should also show non editable properties like product ID etc but grey them out so user can see but not edit them
        //Ensure date time is formated as appropriate string with some kind of calendar UI if possible
        [HttpGet]
        [Authorize(Roles = "Vendor")]
        public ViewResult ViewEditProduct(int ProductID)
        {
            Product product = context.Products.Find(ProductID);
            ProductInfo productInfo = context.ProductInfos.Find(ProductID);

            ViewBag.Product = product;
            ViewBag.ProductInfo = productInfo;
            return View();
        }


        //in future we should add a date modified/updated property and update it when edited and saved
        [HttpPost]
        [Authorize(Roles = "Vendor")]
        public ViewResult EditProduct(int ProductID)
        {

            string PName = Request.Form["pName"];
            string PDescription = Request.Form["pDescription"];
            string PPrice = Request.Form["pPrice"];
            string PImage = Request.Form["pImage"];
            string PRating = Request.Form["pRating"];
            string PRatingCount = Request.Form["pRatingCount"];


            //if updates fail consider using .Entry / states
            //Update Product
            Product product = context.Products.Find(ProductID);
            product.ProductName = PName;
            context.SaveChanges();

            //Update Product Info
            ProductInfo productInfo = context.ProductInfos.Find(ProductID);
            productInfo.ProductDescription = PDescription;
            productInfo.ProductPrice = Int32.Parse(PPrice); //consider trypare or convert
            productInfo.ProductImg = PImage;
            productInfo.ProductRating = Int32.Parse(PRating);
            productInfo.ProductRating = Int32.Parse(PRatingCount);
            context.SaveChanges();


            ViewBag.Product = product;
            ViewBag.Product = productInfo;

            //Pass current ID to page if problems occour
            return View("ViewEditProduct");
        }

        [Authorize(Roles = "Vendor")]
        public ViewResult ShowMyProducts()
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            int vendId = (int)context.AspNetUsers.Find(userId).VendorId;
            List<Product> products = context.Products.ToList();
            List<Product> ownedProducst = new List<Product>();
            foreach (Product p in products)
            {
                if (p.VendorId == vendId)
                {
                    ownedProducst.Add(p);
                }
            }
            return View(ownedProducst);
        }
        //Edit page should DISPLAY the current image and ideally update to the newly entered image on the same page using set image button
        //Edit page should also show non editable properties like product ID etc but grey them out so user can see but not edit them
        //Ensure date time is formated as appropriate string with some kind of calendar UI if possible
        [HttpGet]
        [Authorize(Roles = "Vendor")]
        public ViewResult ViewEditProduct(int ProductID)
        {
            Product product = context.Products.Find(ProductID);
            ProductInfo productInfo = context.ProductInfos.Find(ProductID);

            ViewBag.Product = product;
            ViewBag.ProductInfo = productInfo;
            return View();
        }


        //in future we should add a date modified/updated property and update it when edited and saved
        [HttpPost]
        [Authorize(Roles = "Vendor")]
        public ViewResult EditProduct(int ProductID)
        {

            string PName = Request.Form["pName"];
            string PDescription = Request.Form["pDescription"];
            string PPrice = Request.Form["pPrice"];
            string PImage = Request.Form["pImage"];
            string PRating = Request.Form["pRating"];
            string PRatingCount = Request.Form["pRatingCount"];


            //if updates fail consider using .Entry / states
            //Update Product
            Product product = context.Products.Find(ProductID);
            product.ProductName = PName;
            context.SaveChanges();

            //Update Product Info
            ProductInfo productInfo = context.ProductInfos.Find(ProductID);
            productInfo.ProductDescription = PDescription;
            productInfo.ProductPrice = Int32.Parse(PPrice); //consider trypare or convert
            productInfo.ProductImg = PImage;
            productInfo.ProductRating = Int32.Parse(PRating);
            productInfo.ProductRating = Int32.Parse(PRatingCount);
            context.SaveChanges();


            ViewBag.Product = product;
            ViewBag.Product = productInfo;
            
            //Pass current ID to page if problems occour
            return View("ViewEditProduct");
        }

        [HttpGet]
        [Authorize(Roles = "Vendor")]
        public ViewResult ShowMyProducts()
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            int vendId = (int)context.AspNetUsers.Find(userId).VendorId;
            List<Product> products = context.Products.ToList();
            List<Product> ownedProducst = new List<Product>();
            foreach (Product p in products)
            {
                if (p.VendorId == vendId)
                {
                    ownedProducst.Add(p);
                }
            }
            return View(ownedProducst);
        }



    }
}

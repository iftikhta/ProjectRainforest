using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectRainforest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectRainforest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorProductController : ControllerBase
    {
        public static RainforestDBContext context = new RainforestDBContext();

        [HttpGet]
        [Route("[Action]")]
        public IActionResult GetAllProducts()
        {
            return Ok(context.Products.ToList());
        }


        //Includes Products and ProductInfos
        //VERY useful URL that solved issues https://stackoverflow.com/questions/59199593/net-core-3-0-possible-object-cycle-was-detected-which-is-not-supported
        [HttpGet]
        [Route("[Action]/{VendorID}")]
        public IActionResult GetVendorProducts(int VendorID) //use [FromRoute] before int if problems
        {
            List<Product> MatchedProducts = context.Products.Where(x=> x.VendorId.Equals(VendorID)).ToList();
            List<ProductInfo> MatchedInfos = new List<ProductInfo>();
            foreach (Product p in MatchedProducts)
            {
                MatchedInfos.Add(context.ProductInfos.Find(p.ProductId));
            }

            //var CombinedData = new List<dynamic>(); //beautiful 
            //CombinedData.Add(MatchedProducts);
            //CombinedData.Add(MatchedInfos);
            return Ok(MatchedProducts);
        }
    }
}

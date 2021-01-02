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


        //Must also include product infos
        [HttpGet]
        [Route("[Action]/{VendorID}")]
        public IActionResult GetVendorProducts(int VendorID) //use [FromRoute] before int if problems
        {
            var MatchedProducts = context.Products.Where(x=> x.VendorId.Equals(VendorID)).ToList();
            List<ProductInfo> MatchedInfos = new List<ProductInfo>();
            foreach (Product p in MatchedProducts)
            {
                MatchedInfos.Add(context.ProductInfos.FirstOrDefault(x=> x.ProductId.Equals(p.ProductId)));
            }
            //var MatchedInfos = context.ProductInfos.Where(x => x.ProductId. MatchedProducts);
            return Ok(MatchedProducts);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectRainforest.Areas.Identity.Data;
using ProjectRainforest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectRainforest.Controllers
{
    //Jordan
    [Authorize(Roles = "Vendor")]
    public class VendorController : Controller
    {
        private readonly UserManager<RainforestUser> _userManager;
        public static RainforestDBContext context = new RainforestDBContext();

        public VendorController(UserManager<RainforestUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            //var user = _userManager.GetUserId(HttpContext.User);
            return View();
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View("SignUp");
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(String title, String description, String url)
        {
            Vendor vendor = new Vendor();
            vendor.VendorTitle = title;
            vendor.VendorDescription = description;
            vendor.VendorImg = url;


            context.Vendor.Add(vendor);
            context.SaveChanges();

            int i = context.Vendor.ToList().Last<Vendor>().VendorId;

            var user = await _userManager.GetUserAsync(HttpContext.User);
            user.VendorID = i;
            await _userManager.UpdateAsync(user);

            //return View("../Home/Index");
            return RedirectToAction("Index", "Home");
        }
    }
}

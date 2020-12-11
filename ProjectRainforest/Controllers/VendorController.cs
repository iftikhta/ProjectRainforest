﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectRainforest.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectRainforest.Controllers
{
    [Authorize(Roles = "Vendor")]
    public class VendorController : Controller
    {
        private readonly UserManager<RainforestUser> _userManager;

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
    }
}

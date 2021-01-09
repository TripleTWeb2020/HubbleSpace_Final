using HubbleSpace_Final.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HubbleSpace_Final.Entities;
using HubbleSpace_Final.Services;

namespace HubbleSpace_Final.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly MyDbContext _context;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        public HomeController(ILogger<HomeController> logger, MyDbContext context,IUserService userService,IEmailService emailService)
        {
            _logger = logger;
            _context = context;
            _userService = userService;
            _emailService = emailService;

        }

        public async Task<IActionResult> Index()
        {

            //UserEmailOptions options = new UserEmailOptions
            //{
            //    ToEmails = new List<string>()
            //    {
            //        "test@gmail.com"
            //    },
            //    PlaceHolders = new List<KeyValuePair<string, string>>()
            //    {
            //        new KeyValuePair<string, string>("{{Username}}","Tuyen")
            //    }

            //};
            //await _emailService.SendTestEmail(options);
            string? userId = _userService.GetUserId();
            var isLoggedIn = _userService.IsAuthenticated();
            return View(await _context.Banner.ToListAsync());
        }

        public IActionResult Admin()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Cart()
        {
            return View();
        }
        public IActionResult Checkout_AddInfo()
        {
            return View();
        }
        public IActionResult Checkout_Delivery()
        {
            return View();
        }
        public IActionResult Checkout_Review()
        {
            return View();
        }
        public IActionResult Checkout_Payment()
        {
            return View();
        }
        public async Task<IActionResult> Categories(string Object="", string Name="")
        {
            ViewData["Object"] = Object;
            ViewData["Name"] = Name;
            return View(await _context.Color_Product.Include(p => p.product)
                                                    .Include(p => p.product.category)
                                                    .Where(p => p.product.category.Object.Contains(Object))
                                                    .Where(p => p.product.category.Category_Name.Contains(Name))
                                                    .ToListAsync());
        }
        public async Task<IActionResult> Filter(string Brand="")
        {
            return View("Categories", await _context.Color_Product.Where(p => p.product.Brand.Brand_Name.Contains(Brand))
                                              .ToListAsync());
        }
        
        public async Task<IActionResult> Product_Detail(int id)
        {
            return View(await _context.Img_Product.Include(p => p.color_Product.product)
                                                  .Include(p => p.color_Product.product.category)
                                                  .Where(p => p.ID_Color_Product == id)
                                                  .ToListAsync());
        }
        public async Task<IActionResult> GetSize(int id)
        {
            return PartialView(await _context.Size.Where(p => p.ID_Color_Product == id).ToListAsync());
        }
        public async Task<IActionResult> GetColor(int id)
        {
            return View(await _context.Img_Product.Where(p => p.ID_Color_Product == id).ToListAsync());
        }

        public IActionResult Contact()
        {
            return View();
        }

       

            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
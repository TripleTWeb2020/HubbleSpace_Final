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

        public HomeController(ILogger<HomeController> logger, MyDbContext context,IUserService userService)
        {
            _logger = logger;
            _context = context;
            _userService = userService;

        }

        public async Task<IActionResult> Index()
        {
            string? userId = _userService.GetUserId();
            var isLoggedIn = _userService.IsAuthenticated();
            return View(await _context.Banner.ToListAsync());
        }

        public async Task<IActionResult> GetNewProducts()
        {
            return PartialView(await _context.Color_Product.Include(p => p.product)
                                                    .Include(p => p.product.category)
                                                    .OrderBy(p => p.Date)
                                                    .ToListAsync());
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
        
        public async Task<IActionResult> Categories(int CountForTake = 1, string Object="", string Name="")
        {
            int take = 5;
            int toltal_take = (int)Math.Ceiling((double)_context.Color_Product
                                                        .Include(p => p.product)
                                                        .Include(p => p.product.category)
                                                        .Where(p => p.product.category.Object.Contains(Object))
                                                        .Where(p => p.product.category.Category_Name.Contains(Name))
                                                        .Count() / take);

            var Color_Products = _context.Color_Product.Include(p => p.product)
                                                       .Include(p => p.product.category)
                                                       .Where(p => p.product.category.Object.Contains(Object))
                                                       .Where(p => p.product.category.Category_Name.Contains(Name))
                                                       .OrderBy(p => p.Date)
                                                       .Take(CountForTake * take)
                                                       .ToListAsync();
            ViewData["toltal_take"] = toltal_take;
            ViewData["CountForTake"] = CountForTake + 1;
            ViewData["products_taked"] = take * CountForTake;
            ViewData["Object"] = Object;
            ViewData["Name"] = Name;
            return View(await Color_Products);
        }

        public IActionResult Search()
        {
            var search = Request.Form["Search"].ToString().ToLower();
            ViewData["Object"] = "Search";
            ViewData["Name"] = "";
            if (search == "")
            {
                return View("Categories", _context.Color_Product.Include(p => p.product));
            }
            return View("Categories", _context.Color_Product.Where(m => m.product.Product_Name.ToLower().Contains(search)).Include(p => p.product));
        }

        [HttpPost]
        public IActionResult Filter()
        {
            if (Request.Form["brand"] == "Adidas")
            {
                ViewData["Object"] = Request.Form["brand"];
                return View("Categories", _context.Color_Product.Include(p => p.product).Where(p => p.product.Brand.Brand_Name == "Adidas"));
            }
            if (Request.Form["brand"] == "Nike")
            {
                ViewData["Object"] = Request.Form["brand"];
                return View("Categories", _context.Color_Product.Include(p => p.product).Where(p => p.product.Brand.Brand_Name == "Nike"));
            }
            if (Request.Form["brand"] == "Reebok")
            {
                ViewData["Object"] = Request.Form["brand"];
                return View("Categories", _context.Color_Product.Include(p => p.product).Where(p => p.product.Brand.Brand_Name == "Reebok"));
            }
            if (Request.Form["brand"] == "Puma")
            {
                ViewData["Object"] = Request.Form["brand"];
                return View("Categories", _context.Color_Product.Include(p => p.product).Where(p => p.product.Brand.Brand_Name == "Puma"));
            }
            if (Request.Form["price"] == "0")
            {
                ViewData["Object"] = Request.Form["price"];
                return View("Categories", _context.Color_Product.Include(p => p.product).Where(p => p.product.Price_Sale < 1000000));
            }
            if (Request.Form["price"] == "1000000")
            {
                ViewData["Object"] = Request.Form["price"];
                return View("Categories", _context.Color_Product.Include(p => p.product).Where(p => p.product.Price_Sale >= 1000000 && p.product.Price_Sale < 3000000));
            }
            if (Request.Form["price"] == "3000000")
            {
                ViewData["Object"] = Request.Form["price"];
                return View("Categories", _context.Color_Product.Include(p => p.product).Where(p => p.product.Price_Sale >= 3000000 && p.product.Price_Sale < 5000000));
            }
            if (Request.Form["price"] == "5000000")
            {
                ViewData["Object"] = Request.Form["price"];
                return View("Categories", _context.Color_Product.Include(p => p.product).Where(p => p.product.Price_Sale >= 5000000));
            }
            return View("Categories", _context.Color_Product.Include(p => p.product));
        }
        
        public async Task<IActionResult> Product_Detail(int id)
        {
            return View(await _context.Img_Product.Include(p => p.color_Product.product)
                                                  .Include(p => p.color_Product.product.category)
                                                  .Include(p => p.color_Product.product.Brand)
                                                  .Where(p => p.ID_Color_Product == id)
                                                  .ToListAsync());
        }
        public async Task<IActionResult> GetSize(int id)
        {
            return PartialView(await _context.Size.Where(p => p.ID_Color_Product == id).ToListAsync());
        }
        public async Task<IActionResult> GetColor(int id)
        {
            return PartialView(await _context.Color_Product.Where(p => p.ID_Product == id).ToListAsync());
        }
        public async Task<IActionResult> GetRecommendProducts(string Object="", string Name ="", string Brand_Name = "")
        {
            return PartialView(await _context.Color_Product.Include(p => p.product)
                                                    .Include(p => p.product.category)
                                                    .Where(p => p.product.category.Object.Contains(Object))
                                                    .Where(p => p.product.category.Category_Name.Contains(Name))
                                                    .Where(p => p.product.Brand.Brand_Name.Contains(Brand_Name))
                                                    .ToListAsync());
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
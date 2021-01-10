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
        
        public async Task<IActionResult> Categories(string sortOrder,string brand, string price, string searchString, int CountForTake = 1, string Object="", string Name="")
        {
            ViewData["Name"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["Price"] = sortOrder == "Price" ? "price_desc" : "Price";
            ViewData["Date"] = sortOrder == "Date" ? "date_desc" : "Date";

            ViewData["Adidas"] = brand == "Adidas" ? "" : "Adidas";
            ViewData["Nike"] = brand == "Nike" ? "" : "Nike";
            ViewData["Puma"] = brand == "Puma" ? "" : "Puma";
            ViewData["Reebok"] = brand == "Reebok" ? "" : "Reebok";

            ViewData["0"] = price == "0" ? "" : "0";
            ViewData["1000000"] = price == "1000000" ? "" : "1000000";
            ViewData["3000000"] = price == "3000000" ? "" : "3000000";
            ViewData["5000000"] = price == "5000000" ? "" : "5000000";

            ViewData["Search"] = searchString;

            

            var color_Products = from p in _context.Color_Product.Include(p => p.product).Include(p => p.product.category)
                                 select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                color_Products = color_Products.Where(p => p.product.Product_Name.Contains(searchString));
            }

            switch (brand)
            {
                case "Adidas":
                    color_Products = color_Products.Where(p => p.product.Brand.Brand_Name == brand);
                    break;
                case "Nike":
                    color_Products = color_Products.Where(p => p.product.Brand.Brand_Name == brand);
                    break;
                case "Puma":
                    color_Products = color_Products.Where(p => p.product.Brand.Brand_Name == brand);
                    break;
                case "Reebok":
                    color_Products = color_Products.Where(p => p.product.Brand.Brand_Name == brand);
                    break;
                default:
                    break;
            }

            switch (price)
            {
                case "0":
                    color_Products = color_Products.Where(p => p.product.Price_Sale < 1000000);
                    break;
                case "1000000":
                    color_Products = color_Products.Where(p => p.product.Price_Sale > 1000000 && p.product.Price_Sale < 3000000);
                    break;
                case "3000000":
                    color_Products = color_Products.Where(p => p.product.Price_Sale > 3000000 && p.product.Price_Sale < 5000000);
                    break;
                case "5000000":
                    color_Products = color_Products.Where(p => p.product.Price_Sale > 5000000);
                    break;
                default:
                    break;
            }

            switch (sortOrder)
            {
                case "name_desc":
                    color_Products = color_Products.OrderByDescending(p => p.product.Product_Name);
                    break;
                case "Date":
                    color_Products = color_Products.OrderByDescending(p => p.Date);
                    break;
                case "date_desc":
                    color_Products = color_Products.OrderBy(p => p.Date);
                    break;
                case "Price":
                    color_Products = color_Products.OrderBy(p => p.product.Price_Sale);
                    break;
                case "price_desc":
                    color_Products = color_Products.OrderByDescending(p => p.product.Price_Sale);
                    break;
                default:
                    color_Products = color_Products.OrderBy(p => p.product.Product_Name);
                    break;
            }

            int take = 5;
            double total_product = color_Products.Count();

            int total_take = (int)Math.Ceiling(total_product / take);

            color_Products = color_Products.Take(CountForTake * take);
            ViewData["total_take"] = total_take ;
            ViewData["CountForTake"] = CountForTake + 1;
            ViewData["total_product"] = total_product;
            ViewData["products_taked"] = color_Products.Count();
            ViewData["Object"] = Object;
            ViewData["Name"] = Name;

            return View(await color_Products.AsNoTracking().ToListAsync());

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
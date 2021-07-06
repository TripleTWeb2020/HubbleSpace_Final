﻿using HubbleSpace_Final.Entities;
using HubbleSpace_Final.Models;
using HubbleSpace_Final.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HubbleSpace_Final.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly MyDbContext _context;
        private readonly IUserService _userService;

        public HomeController(ILogger<HomeController> logger, MyDbContext context, IUserService userService)
        {
            _logger = logger;
            _context = context;
            _userService = userService;

        }

        public async Task<IActionResult> Index()
        {
            //string? userId = _userService.GetUserId();
            //var isLoggedIn = _userService.IsAuthenticated();
            // Query for notification
            var notification = from o in _context.Notifications.Include(o => o.User)
                               where o.ReadStatus == ReadStatus.Unread && o.User.Id == _userService.GetUserId()
                               select o.ID_Notifcation;
            int ress = await notification.CountAsync();
            if (ress != 0)
            {
                ViewData["Notifications"] = ress;
            }
            else
            {
                ViewData["Notifications"] = 0;
            }

            return View(await _context.Banner.ToListAsync());
        }

        public async Task<IActionResult> GetNewProducts()
        {
            return PartialView(await _context.Color_Product.Include(p => p.product)
                                                    .Include(p => p.product.category)
                                                    .OrderBy(p => p.Date)
                                                    .Distinct()
                                                    .ToListAsync());
        }

        public async Task<IActionResult> GetBanners()
        {
            return PartialView(await _context.Banner.ToListAsync());
        }

        public IActionResult GetBestSale()
        {
            var list_sale = from c in _context.Color_Product
                            select new BestSaleModel
                            {
                                ID_Color_Product = c.ID_Color_Product,
                                NameColor = c.Color_Name,
                                Image = c.Image,
                                Product = c.product,
                                Date = c.Date,
                                Quantity = _context.OrderDetail.Where(o => o.ID_Color_Product == c.ID_Color_Product).Sum(o => o.Quantity)
                            };
            list_sale = list_sale.Distinct().OrderByDescending(p => p.Quantity);

            List<BestSaleModel> listSales = new List<BestSaleModel>();

            foreach (var item in list_sale.Take(8))
            {
                listSales.Add(item);

            }
            return PartialView(listSales);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public ActionResult Categories(string sortOrder, string Object, string CategoriesName, string brand, string price, string searchString, int page = 1)
        {
            ViewData["Search"] = searchString;
            ViewData["CategoriesName"] = CategoriesName;
            ViewData["Object"] = Object;
            ViewData["sortOrder"] = sortOrder;

            var color_Products = from p in _context.Color_Product.Include(p => p.product).Include(p => p.product.category)
                                 select p;

            if (!String.IsNullOrEmpty(Object))
            {
                color_Products = color_Products.Where(p => p.product.category.Object == Object);
            }

            if (!String.IsNullOrEmpty(CategoriesName))
            {
                color_Products = color_Products.Where(p => p.product.category.Category_Name == CategoriesName);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                color_Products = color_Products.Where(p => p.product.Product_Name.Contains(searchString));
            }

            switch (brand)
            {
                case "Adidas":
                    color_Products = color_Products.Where(p => p.product.Brand.Brand_Name == brand);
                    ViewData["brand"] = brand;
                    break;
                case "Nike":
                    color_Products = color_Products.Where(p => p.product.Brand.Brand_Name == brand);
                    ViewData["brand"] = brand;
                    break;
                case "Puma":
                    color_Products = color_Products.Where(p => p.product.Brand.Brand_Name == brand);
                    ViewData["brand"] = brand;
                    break;
                case "Reebok":
                    color_Products = color_Products.Where(p => p.product.Brand.Brand_Name == brand);
                    ViewData["brand"] = brand;
                    break;
                default:
                    break;
            }

            switch (price)
            {
                case "0":
                    color_Products = color_Products.Where(p => p.product.Price_Sale < 1000000);
                    ViewData["price"] = price;
                    break;
                case "1000000":
                    color_Products = color_Products.Where(p => p.product.Price_Sale > 1000000 && p.product.Price_Sale < 3000000);
                    ViewData["price"] = price;
                    break;
                case "3000000":
                    color_Products = color_Products.Where(p => p.product.Price_Sale > 3000000 && p.product.Price_Sale < 5000000);
                    ViewData["price"] = price;
                    break;
                case "5000000":
                    color_Products = color_Products.Where(p => p.product.Price_Sale > 5000000);
                    ViewData["price"] = price;
                    break;
                default:
                    break;
            }

            switch (sortOrder)
            {
                case "Z-A":
                    color_Products = color_Products.OrderByDescending(p => p.product.Product_Name);
                    ViewData["sortOrder"] = sortOrder;
                    break;
                case "New":
                    color_Products = color_Products.OrderByDescending(p => p.Date);
                    ViewData["sortOrder"] = sortOrder;
                    break;
                case "Price(low-high)":
                    color_Products = color_Products.OrderBy(p => p.product.Price_Sale);
                    ViewData["sortOrder"] = sortOrder;
                    break;
                case "Price(high-low)":
                    color_Products = color_Products.OrderByDescending(p => p.product.Price_Sale);
                    ViewData["sortOrder"] = sortOrder;
                    break;
                default:
                    color_Products = color_Products.OrderBy(p => p.product.Product_Name);
                    ViewData["sortOrder"] = sortOrder;
                    break;
            }


            ViewData["products_taked"] = color_Products.Count();
            ViewData["Object"] = Object;
            ViewData["CategoriesName"] = CategoriesName;

            PagedList<Color_Product> model = new PagedList<Color_Product>(color_Products, page, 6);

            return View(model);

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
            return PartialView(await _context.Size.Where(s => s.ID_Color_Product == id).Include(s => s.color_Product.product).OrderBy(s => s.ID_Size_Product).ToListAsync());
        }

        public async Task<IActionResult> GetColor(int id)
        {
            return PartialView(await _context.Color_Product.Where(p => p.ID_Product == id).ToListAsync());
        }

        public async Task<IActionResult> GetRecommendProducts(string Object = "", string Name = "", string Brand_Name = "")
        {
            var recommendProduct = from rp in _context.Color_Product
                                   select rp;
            Console.WriteLine(recommendProduct.Count());
            recommendProduct = recommendProduct.Include(p => p.product.category)
                                                .Where(p => p.product.category.Object.Contains(Object))
                                                .Where(p => p.product.category.Category_Name.Contains(Name))
                                                .Where(p => p.product.Brand.Brand_Name.Contains(Brand_Name));
            Console.WriteLine(recommendProduct.Count());
            if (recommendProduct.Count() >= 4)
                recommendProduct = recommendProduct.Take(4);
            Console.WriteLine(recommendProduct.Count());
            return PartialView(await recommendProduct.Distinct().ToListAsync());
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
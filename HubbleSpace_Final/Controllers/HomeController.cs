using AspNetCore.SEOHelper.Sitemap;
using HubbleSpace_Final.Entities;
using HubbleSpace_Final.Models;
using HubbleSpace_Final.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubbleSpace_Final.Controllers
{
    public class HomeController : Controller
    {

        private readonly MyDbContext _context;
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _env;
        public HomeController(ILogger<HomeController> logger, MyDbContext context, IUserService userService, IWebHostEnvironment env)
        {
            _context = context;
            _userService = userService;
            _env = env;
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

        public async Task<IActionResult> GetBestSale()
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

            return PartialView(await list_sale.Take(8).ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Categories(string Object, string name, string? brand, string? price, string? sort, string? search, int page = 1)
        {

            var color_Products = from p in _context.Color_Product.Include(p => p.product).Include(p => p.product.category)
                                 select p;

            if (!String.IsNullOrEmpty(Object))
            {
                color_Products = color_Products.Where(p => p.product.category.Object == Object);
                ViewData["Object"] = Object;
            }

            if (!String.IsNullOrEmpty(name))
            {
                color_Products = color_Products.Where(p => p.product.category.Category_Name == name);
                ViewData["name"] = name;
            }

            if (!String.IsNullOrEmpty(search))
            {
                color_Products = color_Products.Where(p => p.product.Product_Name.Contains(search));
                ViewData["Search"] = search;
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

            switch (sort)
            {
                case "Z-A":
                    color_Products = color_Products.OrderByDescending(p => p.product.Product_Name);
                    ViewData["sort"] = sort;
                    break;
                case "New":
                    color_Products = color_Products.OrderByDescending(p => p.Date);
                    ViewData["sort"] = sort;
                    break;
                case "Price(low-high)":
                    color_Products = color_Products.OrderBy(p => p.product.Price_Sale);
                    ViewData["sort"] = sort;
                    break;
                case "Price(high-low)":
                    color_Products = color_Products.OrderByDescending(p => p.product.Price_Sale);
                    ViewData["sort"] = sort;
                    break;
                default:
                    color_Products = color_Products.OrderBy(p => p.product.Product_Name);
                    ViewData["sort"] = sort;
                    break;
            }


            ViewData["products_taked"] = color_Products.Count();

            PagedList<Color_Product> model = new PagedList<Color_Product>(color_Products, page, 6);

            return View(model);

        }

        public async Task<IActionResult> Product_Detail(string category,string name, string color)
        {
            return View(await _context.Img_Product.Include(p => p.color_Product.product)
                                                  .Include(p => p.color_Product.product.category)
                                                  .Include(p => p.color_Product.product.Brand)
                                                  .Where(p => p.color_Product.Color_Name.Replace(" ","") == color.Replace("-","/") && p.color_Product.product.Product_Name.ToLower() == name.ToLower().Replace("-"," ") && p.color_Product.product.category.Category_Name.ToLower() == category.ToLower() )
                                                  .ToListAsync());
        }

        public async Task<IActionResult> GetRecommendProducts(double price)
        {
            var recommendProduct = _context.Color_Product.Include(p => p.product).Where(p => p.product.Price_Sale - price > 0).OrderBy(p => p.product.Price_Sale).Take(2);
            recommendProduct = recommendProduct.Concat(_context.Color_Product.Include(p => p.product).Where(p => p.product.Price_Sale - price < 0).OrderByDescending(p => p.product.Price_Sale).Take(2));

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

        [Route("/robots.txt")]
        public ContentResult RobotsTxt()
        {
            var sb = new StringBuilder();
            sb.AppendLine("User-agent: *")
                .AppendLine("Disallow:")
                .Append("sitemap: ")
                .Append(this.Request.Scheme)
                .Append("://")
                .Append(this.Request.Host)
                .AppendLine("/sitemap.xml");

            return this.Content(sb.ToString(), "text/plain", Encoding.UTF8);
        }

        public string CreateSitemapInRootDirectory()
        {
            var list = new List<SitemapNode>();
            list.Add(new SitemapNode { LastModified = DateTime.UtcNow, Priority = 0.8, Url = "https://localhost:44336/Home/Product_Detail/43", Frequency = SitemapFrequency.Daily });
            new SitemapDocument().CreateSitemapXML(list, _env.ContentRootPath);
            return "sitemap.xml file should be create in root directory";
        }
    }
}
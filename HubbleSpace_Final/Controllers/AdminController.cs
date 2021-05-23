using HubbleSpace_Final.Entities;
using HubbleSpace_Final.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HubbleSpace_Final.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly MyDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(MyDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Index()
        {
            ViewData["User Account"] = _context.Users.Count();
            ViewData["Total Orders"] = _context.Order.Count();
            var query = from total in _context.Order where total.TotalMoney >= 0 select total.TotalMoney;
            ViewData["Total Earnings"] = query.Sum().ToString("n0");
            var querySold = from item in _context.OrderDetail where item.Quantity >= 0 select item.Quantity;
            var queryQuantity = from item in _context.Size where item.Quantity >= 0 select item.Quantity;
            ViewData["Merchandise Inventory"] = queryQuantity.Sum() - querySold.Sum();

            // Item sold for bar chart
            ViewData["Item Sold"] = querySold.Sum();

            // pie chart
            var queryMenSold = (from od in _context.OrderDetail
                                join cp in _context.Color_Product on od.ID_Color_Product.ToString() equals cp.ID_Color_Product.ToString()
                                join p in _context.Product on cp.ID_Product.ToString() equals p.ID_Product.ToString()
                                join c in _context.Category on p.ID_Categorie.ToString() equals c.ID_Categorie.ToString()
                                where c.Object.Equals("Men")
                                select od.Quantity);

            var queryWomenSold = from od in _context.OrderDetail
                                 join cp in _context.Color_Product on od.ID_Color_Product.ToString() equals cp.ID_Color_Product.ToString()
                                 join p in _context.Product on cp.ID_Product.ToString() equals p.ID_Product.ToString()
                                 join c in _context.Category on p.ID_Categorie.ToString() equals c.ID_Categorie.ToString()
                                 where c.Object.Equals("Women")
                                 select od.Quantity;
            var queryKidsSold = (from od in _context.OrderDetail
                                 join cp in _context.Color_Product on od.ID_Color_Product.ToString() equals cp.ID_Color_Product.ToString()
                                 join p in _context.Product on cp.ID_Product.ToString() equals p.ID_Product.ToString()
                                 join c in _context.Category on p.ID_Categorie.ToString() equals c.ID_Categorie.ToString()
                                 where c.Object.Equals("Kids")
                                 select od.Quantity);
            ViewData["Men Item Sold"] = queryMenSold.Sum();
           
            ViewData["Women Item Sold"] = queryWomenSold.Sum();
            ViewData["Kids Item Sold"] = queryKidsSold.Sum();

            // bar chart

            //Profit
            var queryProfitJan = from od in _context.Order
                               where od.Date_Create.Month == 01 select od.TotalMoney;
            var queryProfitFeb = from od in _context.Order
                               where od.Date_Create.Month == 02
                               select od.TotalMoney;
            var queryProfitMarch = from od in _context.Order
                                 where od.Date_Create.Month == 03
                                 select od.TotalMoney;
            var queryProfitApr = from od in _context.Order
                               where od.Date_Create.Month == 04
                               select od.TotalMoney;
            var queryProfitMay = from od in _context.Order
                               where od.Date_Create.Month == 05
                               select od.TotalMoney;
            ViewData["Profit Jan"] = queryProfitJan.Sum();
            ViewData["Profit Feb"] = queryProfitFeb.Sum();
            ViewData["Profit Mar"] = queryProfitMarch.Sum();
            ViewData["Profit Apr"] = queryProfitApr.Sum();
            ViewData["Profit May"] = queryProfitMay.Sum();


            //Quantity
            
            var queryQuanJan = from od in _context.OrderDetail
                                 join profit in _context.Order on od.ID_Order.ToString() equals profit.ID_Order.ToString()
                                 where profit.Date_Create.Month == 01 select od.Quantity;
            var queryQuanFeb = from od in _context.OrderDetail
                                 join profit in _context.Order on od.ID_Order.ToString() equals profit.ID_Order.ToString()
                                 where profit.Date_Create.Month == 02
                                 select od.Quantity;
            var queryQuanMarch = from od in _context.OrderDetail
                                   join profit in _context.Order on od.ID_Order.ToString() equals profit.ID_Order.ToString()
                                   where profit.Date_Create.Month == 03
                                   select od.Quantity;
            var queryQuanApr = from od in _context.OrderDetail
                                 join profit in _context.Order on od.ID_Order.ToString() equals profit.ID_Order.ToString()
                                 where profit.Date_Create.Month == 04
                                 select od.Quantity;
            var queryQuanMay = from od in _context.OrderDetail
                                 join profit in _context.Order on od.ID_Order.ToString() equals profit.ID_Order.ToString()
                                 where profit.Date_Create.Month == 05
                                 select od.Quantity;

            ViewData["Quan Jan"] = queryQuanJan.Sum();
            ViewData["Quan Feb"] = queryQuanFeb.Sum();
            ViewData["Quan Mar"] = queryQuanMarch.Sum();
            ViewData["Quan Apr"] = queryQuanApr.Sum();
            ViewData["Quan May"] = queryQuanMay.Sum();

            //User

            var queryDate = DateTime.Today.Day.ToString();
            var queryMonth = DateTime.Today.Month.ToString();
            var queryYear = DateTime.Today.Year.ToString();
            var queryNow = queryDate + "/" + queryMonth + "/" + queryYear;
            ViewData["Today"] = queryNow;
            return View();
        }

        public async Task<IActionResult> statistic(string time, string sortOrder, string searchString, int CountForTake = 1)
        {
            ViewData["Date"] = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewData["Process"] = sortOrder == "Process" ? "process_desc" : "Process";
            ViewData["Search"] = searchString;

            var Orders = from o in _context.Order.Include(o => o.User)
                            select o;

            if (!String.IsNullOrEmpty(time))
            {
                if (time == "year")
                {
                    Orders = Orders.Where(o => o.Date_Create.Year == DateTime.Now.Year);
                }
                if(time == "month")
                {
                    Orders = Orders.Where(o => o.Date_Create.Month == DateTime.Now.Month && o.Date_Create.Year == DateTime.Now.Year);
                }
            }
                

            if (!String.IsNullOrEmpty(searchString))
            {
                Orders = Orders.Where(o => o.SDT.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "date_desc":
                    Orders = Orders.OrderBy(o => o.Date_Create);
                    break;
                case "Process":
                    Orders = Orders.OrderBy(o => o.Process);
                    break;
                case "process_desc":
                    Orders = Orders.OrderByDescending(o => o.Process);
                    break;
                default:
                    Orders = Orders.OrderByDescending(o => o.Date_Create);
                    break;
            }
            
            //Lấy doanh thu
            ViewData["totalMoney"] = Orders.Sum(o => o.TotalMoney);

            int take = 10;
            double total_product = Orders.Count();

            int total_take = (int)Math.Ceiling(total_product / take);

            Orders = Orders.Skip((CountForTake - 1) * take).Take(take);
            ViewData["total_take"] = total_take;
            ViewData["CountForTake"] = CountForTake + 1;
            return View(await Orders.AsNoTracking().ToListAsync());
        }
        

    }
}

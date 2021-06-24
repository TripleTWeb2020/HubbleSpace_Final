using HubbleSpace_Final.Entities;
using HubbleSpace_Final.Models;
using HubbleSpace_Final.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
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
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserService _userService;

        public AdminController(MyDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUserService userService)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        public async Task<IActionResult> Index()
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
            var queryProfitJune = from od in _context.Order
                                  where od.Date_Create.Month == 06
                                  select od.TotalMoney;

            ViewData["Profit Jan"] = queryProfitJan.Sum();
            ViewData["Profit Feb"] = queryProfitFeb.Sum();
            ViewData["Profit Mar"] = queryProfitMarch.Sum();
            ViewData["Profit Apr"] = queryProfitApr.Sum();
            ViewData["Profit May"] = queryProfitMay.Sum();
            ViewData["Profit June"] = queryProfitJune.Sum();

            // Query for notification
            var noti = from o in _context.Notifications.Include(o => o.User)
                       where o.ReadStatus == ReadStatus.Unread && o.User.Id == _userService.GetUserId()
                       select o.ID_Notifcation;
            int res = await noti.CountAsync();
			if(res != 0)
			{
                ViewData["Notifications"] = res;
			}
			else
			{
                ViewData["Notifications"] = 0;
			}


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
            var queryQuanJune = from od in _context.OrderDetail
                               join profit in _context.Order on od.ID_Order.ToString() equals profit.ID_Order.ToString()
                               where profit.Date_Create.Month == 06
                               select od.Quantity;


            ViewData["Quan Jan"] = queryQuanJan.Sum();
            ViewData["Quan Feb"] = queryQuanFeb.Sum();
            ViewData["Quan Mar"] = queryQuanMarch.Sum();
            ViewData["Quan Apr"] = queryQuanApr.Sum();
            ViewData["Quan May"] = queryQuanMay.Sum();
            ViewData["Quan June"] = queryQuanJune.Sum();
            //User toDo task

            var queryDate = DateTime.Today.Day.ToString();
            var queryMonth = DateTime.Today.Month.ToString();
            var queryYear = DateTime.Today.Year.ToString();
            var queryHour = DateTime.Today.Hour.ToString();
            var queryMinute = DateTime.Today.Minute.ToString();
            var queryNow = queryDate + "/" + queryMonth + "/" + queryYear;
            var queryNow2 = DateTime.Today.ToString();
            ViewData["Today"] = queryNow;
            ViewData["TodayNoti"] = queryNow2;

            // Query for top shoe
            var queryTopShoe = from p in _context.Product
                           join cp in _context.Color_Product on p.ID_Product.ToString() equals cp.ID_Product.ToString() into tb1

                           from tbl1 in tb1
                           join item in _context.OrderDetail on tbl1.ID_Color_Product.ToString() equals item.ID_Color_Product.ToString() into tb2

                           from tbl2 in tb2
                           //orderby tbl2.Quantity descending
                           group tbl2 by new { p.Product_Name } into g
                           select new
                           {
                               product_Name = g.Key.Product_Name,
                               sum = g.Sum(item => item.Quantity),
                           };
            var queryShoeRank = queryTopShoe.OrderByDescending(s => s.sum).ToList();
            ViewData["query1stShoeName"] = queryShoeRank.ElementAt(0).product_Name;
            ViewData["query1stShoeQuan"] = queryShoeRank.ElementAt(0).sum;
            ViewData["query2ndShoeName"] = queryShoeRank.ElementAt(1).product_Name;
            ViewData["query2ndShoeQuan"] = queryShoeRank.ElementAt(1).sum;
            ViewData["query3rdShoeName"] = queryShoeRank.ElementAt(2).product_Name;
            ViewData["query3rdShoeQuan"] = queryShoeRank.ElementAt(2).sum;
            ViewData["query4thShoeName"] = queryShoeRank.ElementAt(3).product_Name;
            ViewData["query4thShoeQuan"] = queryShoeRank.ElementAt(3).sum;
            ViewData["query5thShoeName"] = queryShoeRank.ElementAt(4).product_Name;
            ViewData["query5thShoeQuan"] = queryShoeRank.ElementAt(4).sum;

            // Query for ToDoTask
            var Task = from o in _context.Schedule.Include(o => o.User).OrderBy(o => o.Date_Created) select o;

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
            var list = from o in _context.Notifications
                       where o.ReadStatus == ReadStatus.Unread && o.User.Id == _userService.GetUserId()
                       select o;
            var listt = list.ToList();

            var lists = from o in _context.Notifications
                        where o.User.Id == _userService.GetUserId()
                        select o;
            var listts = lists.OrderByDescending(o => o.Date_Created).Take(5);
            ViewData["Noti"] = listts.ToList();

            foreach (NotificationPusher notif in listt)
            {
                notif.ReadStatus = ReadStatus.Read;
                _context.Update(notif);
                await _context.SaveChangesAsync();
            }


            return View(await Task.AsNoTracking().ToListAsync());
           

        }

        public async Task<IActionResult> Statistic(string time, string sortOrder, string searchString, int page = 1)
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

            PagedList<Order> model = new PagedList<Order>(Orders, page, 10);

            return View(model);
        }

        public IActionResult Create()
        {
            ViewData["id"] = new SelectList(_context.Users, "Id");
            return View();
        }

        // POST: Brands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_ToDo,Date_Created,Title,Description,status")] Schedule schedule)
        {
            var userId = _userService.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);
            if (ModelState.IsValid)
            {
                schedule.User = user;
                _context.Add(schedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["id"] = new SelectList(_context.Users, "Id", schedule.User.Id);
            return View(schedule);
        }


    }
}

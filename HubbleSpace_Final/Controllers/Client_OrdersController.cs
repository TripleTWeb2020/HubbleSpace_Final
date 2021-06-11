using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HubbleSpace_Final.Entities;
using HubbleSpace_Final.Models;
using HubbleSpace_Final.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HubbleSpace_Final.Controllers
{
    public class Client_OrdersController : Controller
    {
        private readonly MyDbContext _context;
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public Client_OrdersController( MyDbContext context, IUserService userService, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userService = userService;
            _userManager = userManager;
            _roleManager = roleManager;

        }

        [Route("/HistoryOrder", Name = "HistoryOrder")]
        public async Task<IActionResult> HistoryOrder(string sortOrder, string searchString, int CountForTake = 1)
        {
            var userId = _userService.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);
            ViewData["Date"] = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewData["Process"] = sortOrder == "Process" ? "process_desc" : "Process";
            ViewData["Search"] = searchString;



            var Orders = from o in _context.Order.Include(o => o.User).Where(o => o.User == user)
                         select o;

            if (!String.IsNullOrEmpty(searchString))
            {
                Orders = Orders.Where(o => o.Date_Create.Date.ToString() == searchString);
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

            int take = 5;
            double total_product = Orders.Count();

            int total_take = (int)Math.Ceiling(total_product / take);

            Orders = Orders.Skip((CountForTake - 1) * take).Take(take);
            ViewData["total_take"] = total_take;
            ViewData["CountForTake"] = CountForTake + 1;

            return View(await Orders.AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult> OrderDetail(int id, string sortOrder, string searchString, int CountForTake = 1)
        {
            ViewData["Name"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["ColorName"] = sortOrder == "ColorName" ? "colorname_desc" : "ColorName";
            ViewData["Quantity"] = sortOrder == "Quantity" ? "quantity_desc" : "Quantity";
            ViewData["Search"] = searchString;

            var orderDetails = from o in _context.OrderDetail.Where(c => c.ID_Order == id)
                                                            .Include(o => o.Color_Product)
                                                            .Include(o => o.order)
                                                            .Include(o => o.Color_Product.product)
                               select o;
                
            if (!String.IsNullOrEmpty(searchString))
            {
                orderDetails = orderDetails.Where(a => a.Color_Product.product.Product_Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    orderDetails = orderDetails.OrderByDescending(a => a.Color_Product.product.Product_Name);
                    break;
                case "ColorName":
                    orderDetails = orderDetails.OrderBy(a => a.Color_Product.Color_Name);
                    break;
                case "colorname_desc":
                    orderDetails = orderDetails.OrderByDescending(a => a.Color_Product.Color_Name);
                    break;
                case "Quantity":
                    orderDetails = orderDetails.OrderBy(a => a.Quantity);
                    break;
                case "quantity_desc":
                    orderDetails = orderDetails.OrderByDescending(a => a.Quantity);
                    break;
                default:
                    orderDetails = orderDetails.OrderBy(a => a.Color_Product.product.Product_Name);
                    break;
            }

            int take = 5;
            double total_product = orderDetails.Count();

            int total_take = (int)Math.Ceiling(total_product / take);

            orderDetails = orderDetails.Skip((CountForTake - 1) * take).Take(take);
            ViewData["total_take"] = total_take;
            ViewData["CountForTake"] = CountForTake + 1;

            return View(await orderDetails.AsNoTracking().ToListAsync());
        }

    }
}

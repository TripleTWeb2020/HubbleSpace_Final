using HubbleSpace_Final.Entities;
using HubbleSpace_Final.Models;
using HubbleSpace_Final.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HubbleSpace_Final.Controllers
{
    public class Client_OrdersController : Controller
    {
        private readonly MyDbContext _context;
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;

        public Client_OrdersController(MyDbContext context, IUserService userService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userService = userService;
            _userManager = userManager;

        }

        [Route("/HistoryOrder", Name = "HistoryOrder")]
        public async Task<IActionResult> HistoryOrder(string sortOrder, string search, int page = 1)
        {
            var userId = _userService.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);
            ViewData["Date"] = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewData["Process"] = sortOrder == "Process" ? "process_desc" : "Process";
            ViewData["Search"] = search;



            var Orders = from o in _context.Order.Include(o => o.User).Where(o => o.User == user)
                         select o;

            if (!String.IsNullOrEmpty(search))
            {
                Orders = Orders.Where(o => o.Date_Create.Date.ToString() == search);
            }

            Orders = sortOrder switch
            {
                "date_desc" => Orders.OrderBy(o => o.Date_Create),
                "Process" => Orders.OrderBy(o => o.Process),
                "process_desc" => Orders.OrderByDescending(o => o.Process),
                _ => Orders.OrderByDescending(o => o.Date_Create),
            };
            PagedList<Order> model = new PagedList<Order>(Orders, page, 10);

            return View(model);
        }

        public ActionResult OrderDetail(int id, string sortOrder, string search, int page = 1)
        {
            ViewData["Name"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["ColorName"] = sortOrder == "ColorName" ? "colorname_desc" : "ColorName";
            ViewData["Quantity"] = sortOrder == "Quantity" ? "quantity_desc" : "Quantity";
            ViewData["Search"] = search;

            var orderDetails = from o in _context.OrderDetail.Where(c => c.ID_Order == id)
                                                            .Include(o => o.Color_Product)
                                                            .Include(o => o.order)
                                                            .Include(o => o.Color_Product.product)
                               select o;

            if (!String.IsNullOrEmpty(search))
            {
                orderDetails = orderDetails.Where(a => a.Color_Product.product.Product_Name.Contains(search));
            }

            orderDetails = sortOrder switch
            {
                "name_desc" => orderDetails.OrderByDescending(a => a.Color_Product.product.Product_Name),
                "ColorName" => orderDetails.OrderBy(a => a.Color_Product.Color_Name),
                "colorname_desc" => orderDetails.OrderByDescending(a => a.Color_Product.Color_Name),
                "Quantity" => orderDetails.OrderBy(a => a.Quantity),
                "quantity_desc" => orderDetails.OrderByDescending(a => a.Quantity),
                _ => orderDetails.OrderBy(a => a.Color_Product.product.Product_Name),
            };
            PagedList<OrderDetail> model = new PagedList<OrderDetail>(orderDetails, page, 10);

            return View(model);
        }

    }
}

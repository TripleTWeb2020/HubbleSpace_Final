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

        public Client_OrdersController( MyDbContext context, IUserService userService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userService = userService;
            _userManager = userManager;

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

            int take = 5;
            double total_product = Orders.Count();

            int total_take = (int)Math.Ceiling(total_product / take);

            Orders = Orders.Skip((CountForTake - 1) * take).Take(take);
            ViewData["total_take"] = total_take;
            ViewData["CountForTake"] = CountForTake + 1;

            return View(await Orders.AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult> OrderDetail(int? id, string sortOrder, string searchString, int CountForTake = 1)
        {
            ViewData["Name"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["ColorName"] = sortOrder == "ColorName" ? "colorname_desc" : "ColorName";
            ViewData["Quantity"] = sortOrder == "Quantity" ? "quantity_desc" : "Quantity";


            ViewData["Search"] = searchString;

            if (id == null)
            {
                var OrderDetails = from o in _context.OrderDetail.Include(o => o.Color_Product)
                                                                 .Include(o => o.order)
                                                                 .Include(o => o.Color_Product.product)
                                   select o;
                if (!String.IsNullOrEmpty(searchString))
                {
                    OrderDetails = OrderDetails.Where(a => a.Color_Product.product.Product_Name.Contains(searchString));
                }

                switch (sortOrder)
                {
                    case "name_desc":
                        OrderDetails = OrderDetails.OrderByDescending(a => a.Color_Product.product.Product_Name);
                        break;
                    case "ColorName":
                        OrderDetails = OrderDetails.OrderBy(a => a.Color_Product.Color_Name);
                        break;
                    case "colorname_desc":
                        OrderDetails = OrderDetails.OrderByDescending(a => a.Color_Product.Color_Name);
                        break;
                    case "Quantity":
                        OrderDetails = OrderDetails.OrderBy(a => a.Quantity);
                        break;
                    case "quantity_desc":
                        OrderDetails = OrderDetails.OrderByDescending(a => a.Quantity);
                        break;
                    default:
                        OrderDetails = OrderDetails.OrderBy(a => a.Color_Product.product.Product_Name);
                        break;
                }

                int take = 5;
                double total_product = OrderDetails.Count();

                int total_take = (int)Math.Ceiling(total_product / take);

                OrderDetails = OrderDetails.Skip((CountForTake - 1) * take).Take(take);
                ViewData["total_take"] = total_take;
                ViewData["CountForTake"] = CountForTake + 1;

                return View(await OrderDetails.AsNoTracking().ToListAsync());
            }
            else
            {
                var OrderDetails = from o in _context.OrderDetail.Include(o => o.Color_Product)
                                                                 .Include(o => o.order)
                                                                 .Include(o => o.Color_Product.product)
                                                                 .Where(m => m.ID_Order == id)
                                   select o;
                if (OrderDetails == null)
                {
                    return NotFound();
                }


                if (!String.IsNullOrEmpty(searchString))
                {
                    OrderDetails = OrderDetails.Where(a => a.Color_Product.product.Product_Name.Contains(searchString));
                }

                switch (sortOrder)
                {
                    case "name_desc":
                        OrderDetails = OrderDetails.OrderByDescending(a => a.Color_Product.product.Product_Name);
                        break;
                    case "ColorName":
                        OrderDetails = OrderDetails.OrderBy(a => a.Color_Product.Color_Name);
                        break;
                    case "colorname_desc":
                        OrderDetails = OrderDetails.OrderByDescending(a => a.Color_Product.Color_Name);
                        break;
                    case "Quantity":
                        OrderDetails = OrderDetails.OrderBy(a => a.Quantity);
                        break;
                    case "quantity_desc":
                        OrderDetails = OrderDetails.OrderByDescending(a => a.Quantity);
                        break;
                    default:
                        OrderDetails = OrderDetails.OrderBy(a => a.Color_Product.product.Product_Name);
                        break;
                }

                int take = 10;
                double total_product = OrderDetails.Count();

                int total_take = (int)Math.Ceiling(total_product / take);

                OrderDetails = OrderDetails.Skip((CountForTake - 1) * take).Take(take);
                ViewData["total_take"] = total_take;
                ViewData["CountForTake"] = CountForTake + 1;

                return View(await OrderDetails.AsNoTracking().ToListAsync());

            }

        }

    }
}

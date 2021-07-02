using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HubbleSpace_Final.Entities;
using HubbleSpace_Final.Helpers;
using HubbleSpace_Final.Services;
using Microsoft.AspNetCore.Identity;
using HubbleSpace_Final.Models;
using Microsoft.AspNetCore.Authorization;
using PagedList.Core;

namespace HubbleSpace_Final.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrdersController : Controller
    {
        private readonly MyDbContext _context;
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrdersController(MyDbContext context,IUserService userService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userService = userService;
            _userManager = userManager;
        }

        // GET: Orders
        public ActionResult Index(string sortOrder, string searchString, int page = 1)
        {
            ViewData["Date"] = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewData["Process"] = sortOrder == "Process" ? "process_desc" : "Process";
            ViewData["Search"] = searchString;



            var Orders = from o in _context.Order.Include(o => o.User)
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

            PagedList<Order> model = new PagedList<Order>(Orders, page, 10);

            return View(model);
        }


        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.ID_Order == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["id"] = new SelectList(_context.Users, "Id", "Email");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_Order,TotalMoney,Date_Create,Address,Receiver,SDT,ID_Account,Process")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["id"] = new SelectList(_context.Users, "Id", "Email", order.User.Id);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.Include(o => o.User).Where(o => o.ID_Order == id).FirstOrDefaultAsync();
            if (order == null)
            {
                return NotFound();
            }
            
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Order,TotalMoney,Date_Create,Address,Receiver,SDT,Process,PaymentStatus")] Order order)
        {
            
            if (id != order.ID_Order)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    _context.Update(order);
                    await _context.SaveChangesAsync();
                    var userId = _userService.GetUserId();
                    var user = await _userManager.FindByIdAsync(userId);

                    var orrder = await _context.Order.Include(o => o.User).Where(o => o.ID_Order == id).Select(o => o.User).FirstOrDefaultAsync();
                    
                    var data = new
                    {
                        message = System.String.Format("ID of #{0}, Your order status is changed by {1} to {2} ", order.ID_Order, user,order.Process)
                    };
                    await ChannelHelper.Trigger(data, "Clientnotification", "new_Clientnotification");

                    var message = new NotificationPusher()
                    {
                        User = orrder,
                        Date_Created = DateTime.Now,
                        Content = System.String.Format("ID of #{0}, Your order status is changed by {1} to {2} ", order.ID_Order, user, order.Process),
                        ReadStatus = ReadStatus.Unread
                    };
                    _context.Add(message);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.ID_Order))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["id"] = new SelectList(_context.Users, "Id", "Email", order.User.Id);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.ID_Order == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Order.FindAsync(id);
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.ID_Order == id);
        }
    }
}

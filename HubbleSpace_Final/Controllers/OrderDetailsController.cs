using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HubbleSpace_Final.Entities;

namespace HubbleSpace_Final.Controllers
{
    public class OrderDetailsController : Controller
    {
        private readonly MyDbContext _context;

        public OrderDetailsController(MyDbContext context)
        {
            _context = context;
        }

        // GET: OrderDetails
        public async Task<IActionResult> Index(int? id, string sortOrder, string searchString, int CountForTake = 1)
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

                int take = 10;
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


        // GET: OrderDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetail
                .Include(o => o.Color_Product)
                .Include(o => o.order)
                .Include(o => o.Color_Product.product)
                .FirstOrDefaultAsync(m => m.ID_OrderDetail == id);
            if (orderDetail == null)
            {
                return NotFound();
            }
            return View(orderDetail);
        }

        // GET: OrderDetails/Create
        public IActionResult Create()
        {
            var Product_Color_Name = from c in _context.Color_Product
                                     select new
                                     {
                                         ID_Color_Product = c.ID_Color_Product,
                                         Name = c.product.Product_Name + " - " + c.Color_Name
                                     };
            ViewData["ID_Color_Product"] = new SelectList(Product_Color_Name, "ID_Color_Product", "Name");

            var Order_Name = from c in _context.Order
                             select new
                             {
                                 ID_Order = c.ID_Order,
                                 Name = c.Address + " - " + c.SDT
                             };
            ViewData["ID_Order"] = new SelectList(Order_Name, "ID_Order", "Name");
            return View();
        }

        public IActionResult GetSize(int? id)
        {
            var Get_Size = from c in _context.Size
                           where c.ID_Color_Product == id
                           select new
                           {
                               ID_Size_Product = c.ID_Size_Product,
                               SizeNumber = c.SizeNumber
                           };
            if (Get_Size == null)
            {
                return NotFound();
            }
            ViewData["Size"] = new SelectList(Get_Size, "SizeNumber", "SizeNumber");
            return PartialView();

        }

        // POST: OrderDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_OrderDetail,ID_Color_Product,Size,Quantity,ID_Order")] OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var Product_Color_Name = from c in _context.Color_Product
                                     select new
                                     {
                                         ID_Color_Product = c.ID_Color_Product,
                                         Name = c.product.Product_Name + " - " + c.Color_Name
                                     };
            ViewData["ID_Color_Product"] = new SelectList(Product_Color_Name, "ID_Color_Product", "Name", orderDetail.ID_Color_Product);

            var Order_Name = from c in _context.Order
                             select new
                             {
                                 ID_Order = c.ID_Order,
                                 Name = c.Address+ " - " +c.SDT
                             };
            ViewData["ID_Order"] = new SelectList(Order_Name, "ID_Order", "Name", orderDetail.ID_Order);
            return View(orderDetail);
        }

        // GET: OrderDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetail.FindAsync(id);
            if (orderDetail == null)
            {
                return NotFound();
            }
            var Product_Color_Name = from c in _context.Color_Product
                                     select new
                                     {
                                         ID_Color_Product = c.ID_Color_Product,
                                         Name = c.product.Product_Name + " - " + c.Color_Name
                                     };
            ViewData["ID_Color_Product"] = new SelectList(Product_Color_Name, "ID_Color_Product", "Name", orderDetail.ID_Color_Product);

            var Order_Name = from c in _context.Order
                             select new
                             {
                                 ID_Order = c.ID_Order,
                                 Name = c.Address + " - " + c.SDT
                             };
            ViewData["ID_Order"] = new SelectList(Order_Name, "ID_Order", "Name", orderDetail.ID_Order);
            return View(orderDetail);
        }

        // POST: OrderDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_OrderDetail,ID_Color_Product,Size,Quantity,ID_Order")] OrderDetail orderDetail)
        {
            if (id != orderDetail.ID_OrderDetail)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderDetailExists(orderDetail.ID_OrderDetail))
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

            var Product_Color_Name = from c in _context.Color_Product
                                     select new
                                     {
                                         ID_Color_Product = c.ID_Color_Product,
                                         Name = c.product.Product_Name + " - " + c.Color_Name
                                     };
            ViewData["ID_Color_Product"] = new SelectList(Product_Color_Name, "ID_Color_Product", "Name", orderDetail.ID_Color_Product);

            var Order_Name = from c in _context.Order
                                     select new
                                     {
                                         ID_Order = c.ID_Order,
                                         Name = c.Address + " - "+ c.SDT
                                     };
            ViewData["ID_Order"] = new SelectList(Order_Name, "ID_Order", "Name", orderDetail.ID_Order);
            return View(orderDetail);
        }

        // GET: OrderDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetail
                .Include(o => o.Color_Product)
                .Include(o => o.order)
                .Include(o => o.Color_Product.product)
                .FirstOrDefaultAsync(m => m.ID_OrderDetail == id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // POST: OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderDetail = await _context.OrderDetail.FindAsync(id);
            _context.OrderDetail.Remove(orderDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderDetailExists(int id)
        {
            return _context.OrderDetail.Any(e => e.ID_OrderDetail == id);
        }

        public IActionResult Search(string term)
        {
            return View("Index", _context.OrderDetail.Include(o => o.Color_Product).Include(o => o.order).Include(o => o.Color_Product.product).Where(m => m.Color_Product.product.Product_Name.Contains(term)));
        }
    }
}


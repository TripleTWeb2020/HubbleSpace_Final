using HubbleSpace_Final.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using System;
using System.Linq;
using System.Threading.Tasks;

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
        public ActionResult Index(int? id, string sortOrder, string searchString, int page = 1)
        {
            ViewData["Name"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["ColorName"] = sortOrder == "ColorName" ? "colorname_desc" : "ColorName";
            ViewData["Quantity"] = sortOrder == "Quantity" ? "quantity_desc" : "Quantity";
            ViewData["Search"] = searchString;

            var OrderDetails = from o in _context.OrderDetail.Include(o => o.Color_Product)
                                                                .Include(o => o.order)
                                                                .Include(o => o.Color_Product.product)
                               select o;
            if (id > 0)
            {
                OrderDetails = OrderDetails.Where(o => o.ID_Order == id);
                ViewData["ID_Order"] = id;
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

            PagedList<OrderDetail> model = new PagedList<OrderDetail>(OrderDetails, page, 10);

            return View(model);
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
            ViewData["ID_Order"] = orderDetail.ID_Order;
            return View(orderDetail);
        }

        // GET: OrderDetails/Create
        public IActionResult Create(int? id)
        {

            var colorProduct = from c in _context.Color_Product
                               select new
                               {
                                   ID_Color_Product = c.ID_Color_Product,
                                   Name = c.product.Product_Name + " - " + c.Color_Name,
                               };

            ViewData["ColorProduct_Select"] = new SelectList(colorProduct, "ID_Color_Product", "Name");

            var order = from c in _context.Order
                        select new
                        {
                            ID_Order = c.ID_Order,
                            Name = c.Address + " - " + c.SDT
                        };
            if (id > 0)
            {
                order = order.Where(o => o.ID_Order == id);
                ViewData["ID_Order"] = id;
            }
            ViewData["orderSelect"] = new SelectList(order, "ID_Order", "Name");

            return View();
        }

        // POST: OrderDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? id, [Bind("ID_OrderDetail,ID_Color_Product,Size,Quantity,ID_Order")] OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { @id = id });
            }
            var colorProduct = from c in _context.Color_Product
                               select new
                               {
                                   ID_Color_Product = c.ID_Color_Product,
                                   Name = c.product.Product_Name + " - " + c.Color_Name,
                               };

            ViewData["ColorProduct_Select"] = new SelectList(colorProduct, "ID_Color_Product", "Name", orderDetail.ID_Color_Product);

            var order = from c in _context.Order
                        select new
                        {
                            ID_Order = c.ID_Order,
                            Name = c.Address + " - " + c.SDT
                        };
            if (id > 0)
            {
                order = order.Where(o => o.ID_Order == id);
                ViewData["ID_Order"] = id;
            }
            ViewData["orderSelect"] = new SelectList(order, "ID_Order", "Name", orderDetail.ID_Order);
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

            var colorProduct = from c in _context.Color_Product
                               select new
                               {
                                   ID_Color_Product = c.ID_Color_Product,
                                   Name = c.product.Product_Name + " - " + c.Color_Name
                               };
            ViewData["ColorProduct_Select"] = new SelectList(colorProduct, "ID_Color_Product", "Name", orderDetail.ID_Color_Product);

            var order = from c in _context.Order
                        where (c.ID_Order == orderDetail.ID_Order)
                        select new
                        {
                            ID_Order = c.ID_Order,
                            Name = c.Address + " - " + c.SDT
                        };
            ViewData["orderSelect"] = new SelectList(order, "ID_Order", "Name", orderDetail.ID_Order);
            ViewData["ID_Order"] = orderDetail.ID_Order;
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
                return RedirectToAction("Index", new { @id = orderDetail.ID_Order });
            }

            var colorProduct = from c in _context.Color_Product
                               select new
                               {
                                   ID_Color_Product = c.ID_Color_Product,
                                   Name = c.product.Product_Name + " - " + c.Color_Name
                               };
            ViewData["ColorProduct_Select"] = new SelectList(colorProduct, "ID_Color_Product", "Name", orderDetail.ID_Color_Product);

            var order = from c in _context.Order
                        where (c.ID_Order == orderDetail.ID_Order)
                        select new
                        {
                            ID_Order = c.ID_Order,
                            Name = c.Address + " - " + c.SDT
                        };
            ViewData["orderSelect"] = new SelectList(order, "ID_Order", "Name", orderDetail.ID_Order);
            ViewData["ID_Order"] = orderDetail.ID_Order;

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
            ViewData["ID_Order"] = orderDetail.ID_Order;

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
            return RedirectToAction("Index", new { @id = orderDetail.ID_Order });
        }

        private bool OrderDetailExists(int id)
        {
            return _context.OrderDetail.Any(e => e.ID_OrderDetail == id);
        }

        public IActionResult Search(string term)
        {
            return View("Index", _context.OrderDetail.Include(o => o.Color_Product).Include(o => o.order).Include(o => o.Color_Product.product).Where(m => m.Color_Product.product.Product_Name.Contains(term)));
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
    }
}


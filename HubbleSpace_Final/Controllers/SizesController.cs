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
    public class SizesController : Controller
    {
        private readonly MyDbContext _context;

        public SizesController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Sizes
        public async Task<IActionResult> Index(string sortOrder, string searchString, int? id, int CountForTake = 1)
        {
            ViewData["Name"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["ColorName"] = sortOrder == "ColorName" ? "colorname_desc" : "ColorName";
            ViewData["Size"] = sortOrder == "Size" ? "size_desc" : "Size";
            ViewData["Quantity"] = sortOrder == "Quantity" ? "quantity_desc" : "Quantity";
            ViewData["Search"] = searchString;



            var Sizes = from s in _context.Size.Include(s => s.color_Product).Include(s => s.color_Product.product)
                        select s;

            if(id > 0)
            {
                ViewData["ID_ColorProduct"] = id;
                Sizes = Sizes.Where(s => s.color_Product.ID_Color_Product == id);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                Sizes = Sizes.Where(s => s.color_Product.product.Product_Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    Sizes = Sizes.OrderByDescending(c => c.color_Product.product.Product_Name);
                    break;
                case "ColorName":
                    Sizes = Sizes.OrderBy(c => c.color_Product.Color_Name);
                    break;
                case "colorname_desc":
                    Sizes = Sizes.OrderByDescending(c => c.color_Product.Color_Name);
                    break;
                case "Size":
                    Sizes = Sizes.OrderBy(c => c.SizeNumber);
                    break;
                case "size_desc":
                    Sizes = Sizes.OrderByDescending(c => c.SizeNumber);
                    break;
                case "Quantity":
                    Sizes = Sizes.OrderBy(c => c.Quantity);
                    break;
                case "quantity_desc":
                    Sizes = Sizes.OrderByDescending(c => c.Quantity);
                    break;
                default:
                    Sizes = Sizes.OrderBy(c => c.color_Product.product.Product_Name);
                    break;
            }

            int take = 10;
            double total_product = Sizes.Count();

            int total_take = (int)Math.Ceiling(total_product / take);

            Sizes = Sizes.Skip((CountForTake - 1) * take).Take(take);
            ViewData["total_take"] = total_take;
            ViewData["CountForTake"] = CountForTake + 1;

            return View(await Sizes.AsNoTracking().ToListAsync());
        }


        // GET: Sizes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var size = await _context.Size
                .Include(s => s.color_Product)
                .Include(s => s.color_Product.product)
                .FirstOrDefaultAsync(m => m.ID_Size_Product == id);
            if (size == null)
            {
                return NotFound();
            }
            ViewData["ID_ColorProduct"] = size.ID_Color_Product;

            return View(size);
        }

        // GET: Sizes/Create
        public IActionResult Create(int? id)
        {
            var ColorProduct = from c in _context.Color_Product
                                     select new
                                     {
                                         ID_Color_Product = c.ID_Color_Product,
                                         Name = c.product.Product_Name + " - " + c.Color_Name
                                     };
            if (id > 0)
            {
                ViewData["ID_ColorProduct"] = id;
                ColorProduct = ColorProduct.Where(s => s.ID_Color_Product == id);
            }
            ViewData["ColorProduct_Select"] = new SelectList(ColorProduct, "ID_Color_Product", "Name");
            return View();
        }

        // POST: Sizes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? id, [Bind("ID_Size_Product,SizeNumber,Quantity,ID_Color_Product")] Size size)
        {
            if (ModelState.IsValid)
            {
                if (SizeNumberExists(size.ID_Size_Product, size.ID_Color_Product, size.SizeNumber))
                {
                    ViewData["Message"] = "Mẫu giày này đã tồn tại size trên. " +
                        "Vui lòng chọn chỉnh sửa tại màn hình Quản lý size của sản phẩm nếu muốn điều chỉnh số lượng của size này";
                }
                else 
                {
                    _context.Add(size);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", new { @id = id });
                }
            }
            var ColorProduct = from c in _context.Color_Product
                                     select new
                                     {
                                         ID_Color_Product = c.ID_Color_Product,
                                         Name = c.product.Product_Name + " - " + c.Color_Name
                                     };
            if (id > 0)
            {
                ViewData["ID_ColorProduct"] = id;
                ColorProduct = ColorProduct.Where(s => s.ID_Color_Product == id);
            }
            ViewData["ColorProduct_Select"] = new SelectList(ColorProduct, "ID_Color_Product", "Name", size.ID_Color_Product);
            return View(size);
        }

        // GET: Sizes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var size = await _context.Size.FindAsync(id);
            if (size == null)
            {
                return NotFound();
            }
            var ColorProduct = from c in _context.Color_Product
                                     select new
                                     {
                                         ID_Color_Product = c.ID_Color_Product,
                                         Name = c.product.Product_Name + " - " + c.Color_Name
                                     };
            ViewData["ColorProduct_Select"] = new SelectList(ColorProduct, "ID_Color_Product", "Name", size.ID_Color_Product);

            ViewData["ID_ColorProduct"] = size.ID_Color_Product;
            return View(size);
        }

        // POST: Sizes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Size_Product,SizeNumber,Quantity,ID_Color_Product")] Size size)
        {
            if (id != size.ID_Size_Product)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(size);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SizeExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new { @id = size.ID_Color_Product });
            }
            var ColorProduct = from c in _context.Color_Product
                                     select new
                                     {
                                         ID_Color_Product = c.ID_Color_Product,
                                         Name = c.product.Product_Name + " - " + c.Color_Name
                                     };
            ViewData["ColorProduct_Select"] = new SelectList(ColorProduct, "ID_Color_Product", "Name", size.ID_Color_Product);

            ViewData["ID_ColorProduct"] = size.ID_Color_Product;
            return View(size);
        }

        // GET: Sizes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var size = await _context.Size
                .Include(s => s.color_Product)
                .Include(s => s.color_Product.product)
                .FirstOrDefaultAsync(m => m.ID_Size_Product == id);
            if (size == null)
            {
                return NotFound();
            }
            ViewData["ID_ColorProduct"] = size.ID_Color_Product;

            return View(size);
        }

        // POST: Sizes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var size = await _context.Size.FindAsync(id);
            _context.Size.Remove(size);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { @id = size.ID_Color_Product });
        }

        private bool SizeExists(int id)
        {
            return _context.Size.Any(e => e.ID_Size_Product == id);
        }

        private bool SizeNumberExists(int? id, int colorProduct, string sizeNumber)
        {
            var sizes = _context.Size.Where(s => s.ID_Color_Product == colorProduct).ToList();
            sizeNumber = sizeNumber.Trim().ToLower();

            foreach (var item in sizes)
            {
                if (_context.Size.Any(e => e.ID_Size_Product == id || e.SizeNumber.Trim().ToLower() == sizeNumber))
                    return true;
            }
            return false;
        }
    }
}

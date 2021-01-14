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
    public class Color_ProductController : Controller
    {
        private readonly MyDbContext _context;

        public Color_ProductController(MyDbContext context)
        {
            _context = context;
        }



        // GET: Color_Product
        public async Task<IActionResult> Index(string sortOrder, string searchString, int id, int CountForTake = 1)
        {
            ViewData["Date"] = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewData["Name"] = sortOrder == "Name" ? "name_desc" : "Name";
            ViewData["ColorName"] = sortOrder == "ColorName" ? "colorname_desc" : "ColorName";

            ViewData["Search"] = searchString;



            var color_Products = from c in _context.Color_Product.Include(c => c.product)
                           select c;
            if(id != null & id != 0)
            {
                color_Products = color_Products.Where(c => c.product.ID_Product == id);
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                color_Products = color_Products.Where(c => c.product.Product_Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "date_desc":
                    color_Products = color_Products.OrderBy(c => c.Date);
                    break;
                case "Name":
                    color_Products = color_Products.OrderBy(c => c.product.Product_Name);
                    break;
                case "name_desc":
                    color_Products = color_Products.OrderByDescending(c => c.product.Product_Name);
                    break;
                case "ColorName":
                    color_Products = color_Products.OrderBy(c => c.Color_Name);
                    break;
                case "colorname_desc":
                    color_Products = color_Products.OrderByDescending(c => c.Color_Name);
                    break;
                default:
                    color_Products = color_Products.OrderByDescending(c => c.Date);
                    break;
            }

            int take = 10;
            double total_product = color_Products.Count();

            int total_take = (int)Math.Ceiling(total_product / take);

            color_Products = color_Products.Skip((CountForTake - 1) * take).Take(take);
            ViewData["total_take"] = total_take;
            ViewData["CountForTake"] = CountForTake + 1;

            return View(await color_Products.AsNoTracking().ToListAsync());
        }
        
        // GET: Color_Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var color_Product = await _context.Color_Product
                .Include(c => c.product)
                .FirstOrDefaultAsync(m => m.ID_Color_Product == id);
            if (color_Product == null)
            {
                return NotFound();
            }

            return View(color_Product);
        }

        // GET: Color_Product/Create
        public IActionResult Create()
        {
            ViewData["ID_Product"] = new SelectList(_context.Product, "ID_Product", "Product_Name");
            return View();
        }

        // POST: Color_Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_Color_Product,Color_Name,ID_Product,Image")] Color_Product color_Product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(color_Product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ID_Product"] = new SelectList(_context.Product, "ID_Product", "Product_Name", color_Product.ID_Product);
            return View(color_Product);
        }

        // GET: Color_Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var color_Product = await _context.Color_Product.FindAsync(id);
            if (color_Product == null)
            {
                return NotFound();
            }
            ViewData["ID_Product"] = new SelectList(_context.Product, "ID_Product", "Product_Name", color_Product.ID_Product);
            return View(color_Product);
        }

        // POST: Color_Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Color_Product,Color_Name,ID_Product,Image")] Color_Product color_Product)
        {
            if (id != color_Product.ID_Color_Product)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(color_Product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Color_ProductExists(color_Product.ID_Color_Product))
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
            ViewData["ID_Product"] = new SelectList(_context.Product, "ID_Product", "Product_Name", color_Product.ID_Product);
            return View(color_Product);
        }

        // GET: Color_Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var color_Product = await _context.Color_Product
                .Include(c => c.product)
                .FirstOrDefaultAsync(m => m.ID_Color_Product == id);
            if (color_Product == null)
            {
                return NotFound();
            }

            return View(color_Product);
        }

        // POST: Color_Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var color_Product = await _context.Color_Product.FindAsync(id);
            _context.Color_Product.Remove(color_Product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Color_ProductExists(int id)
        {
            return _context.Color_Product.Any(e => e.ID_Color_Product == id);
        }

        public IActionResult Search(string term)
        {
            return View("Index", _context.Color_Product.Where(m => m.Color_Name.Contains(term)));
        }
    }
}

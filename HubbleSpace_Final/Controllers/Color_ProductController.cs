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
        public async Task<IActionResult> Index()
        {
            var myDbContext = _context.Color_Product.Include(c => c.Product);
            return View(await myDbContext.ToListAsync());
        }

        // GET: Color_Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var color_Product = await _context.Color_Product
                .Include(c => c.Product)
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
        public async Task<IActionResult> Create([Bind("ID_Color_Product,Color_Name,ID_Product")] Color_Product color_Product)
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
        public async Task<IActionResult> Edit(int id, [Bind("ID_Color_Product,Color_Name,ID_Product")] Color_Product color_Product)
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
                .Include(c => c.Product)
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
    }
}

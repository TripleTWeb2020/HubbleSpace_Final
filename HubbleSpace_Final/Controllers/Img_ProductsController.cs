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
    public class Img_ProductsController : Controller
    {
        private readonly MyDbContext _context;

        public Img_ProductsController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Img_Product
        public async Task<IActionResult> Index()
        {
            var myDbContext = _context.Img_Product.Include(i => i.color_Product);
            return View(await myDbContext.ToListAsync());
        }

        // GET: Img_Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var img_Product = await _context.Img_Product
                .Include(i => i.color_Product)
                .FirstOrDefaultAsync(m => m.ID_Img_Product == id);
            if (img_Product == null)
            {
                return NotFound();
            }

            return View(img_Product);
        }

        // GET: Img_Product/Create
        public IActionResult Create()
        {
            ViewData["ID_Color_Product"] = new SelectList(_context.Color_Product, "ID_Color_Product", "Color_Name");
            return View();
        }

        // POST: Img_Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_Img_Product,Photo,ID_Color_Product")] Img_Product img_Product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(img_Product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ID_Color_Product"] = new SelectList(_context.Color_Product, "ID_Color_Product", "Color_Name", img_Product.ID_Color_Product);
            return View(img_Product);
        }

        // GET: Img_Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var img_Product = await _context.Img_Product.FindAsync(id);
            if (img_Product == null)
            {
                return NotFound();
            }
            ViewData["ID_Color_Product"] = new SelectList(_context.Color_Product, "ID_Color_Product", "Color_Name", img_Product.ID_Color_Product);
            return View(img_Product);
        }

        // POST: Img_Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Img_Product,Photo,ID_Color_Product")] Img_Product img_Product)
        {
            if (id != img_Product.ID_Img_Product)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(img_Product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Img_ProductExists(img_Product.ID_Img_Product))
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
            ViewData["ID_Color_Product"] = new SelectList(_context.Color_Product, "ID_Color_Product", "Color_Name", img_Product.ID_Color_Product);
            return View(img_Product);
        }

        // GET: Img_Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var img_Product = await _context.Img_Product
                .Include(i => i.color_Product)
                .FirstOrDefaultAsync(m => m.ID_Img_Product == id);
            if (img_Product == null)
            {
                return NotFound();
            }

            return View(img_Product);
        }

        // POST: Img_Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var img_Product = await _context.Img_Product.FindAsync(id);
            _context.Img_Product.Remove(img_Product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Img_ProductExists(int id)
        {
            return _context.Img_Product.Any(e => e.ID_Img_Product == id);
        }
    }
}

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
    public class DiscountUsedsController : Controller
    {
        private readonly MyDbContext _context;

        public DiscountUsedsController(MyDbContext context)
        {
            _context = context;
        }

        // GET: DiscountUseds
        public async Task<IActionResult> Index()
        {
            var myDbContext = _context.DiscountUsed.Include(d => d.Account).Include(d => d.Discount);
            return View(await myDbContext.ToListAsync());
        }

        // GET: DiscountUseds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discountUsed = await _context.DiscountUsed
                .Include(d => d.Account)
                .Include(d => d.Discount)
                .FirstOrDefaultAsync(m => m.ID_DiscountUsed == id);
            if (discountUsed == null)
            {
                return NotFound();
            }

            return View(discountUsed);
        }

        // GET: DiscountUseds/Create
        public IActionResult Create()
        {
            ViewData["ID_Account"] = new SelectList(_context.Account, "ID_Account", "Email");
            ViewData["ID_Discount"] = new SelectList(_context.Discount, "ID_Discount", "Code_Discount");
            return View();
        }

        // POST: DiscountUseds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_DiscountUsed,ID_Account,ID_Discount")] DiscountUsed discountUsed)
        {
            if (ModelState.IsValid)
            {
                _context.Add(discountUsed);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ID_Account"] = new SelectList(_context.Account, "ID_Account", "Email", discountUsed.ID_Account);
            ViewData["ID_Discount"] = new SelectList(_context.Discount, "ID_Discount", "Code_Discount", discountUsed.ID_Discount);
            return View(discountUsed);
        }

        // GET: DiscountUseds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discountUsed = await _context.DiscountUsed.FindAsync(id);
            if (discountUsed == null)
            {
                return NotFound();
            }
            ViewData["ID_Account"] = new SelectList(_context.Account, "ID_Account", "Email", discountUsed.ID_Account);
            ViewData["ID_Discount"] = new SelectList(_context.Discount, "ID_Discount", "Code_Discount", discountUsed.ID_Discount);
            return View(discountUsed);
        }

        // POST: DiscountUseds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_DiscountUsed,ID_Account,ID_Discount")] DiscountUsed discountUsed)
        {
            if (id != discountUsed.ID_DiscountUsed)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(discountUsed);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiscountUsedExists(discountUsed.ID_DiscountUsed))
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
            ViewData["ID_Account"] = new SelectList(_context.Account, "ID_Account", "Email", discountUsed.ID_Account);
            ViewData["ID_Discount"] = new SelectList(_context.Discount, "ID_Discount", "Code_Discount", discountUsed.ID_Discount);
            return View(discountUsed);
        }

        // GET: DiscountUseds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discountUsed = await _context.DiscountUsed
                .Include(d => d.Account)
                .Include(d => d.Discount)
                .FirstOrDefaultAsync(m => m.ID_DiscountUsed == id);
            if (discountUsed == null)
            {
                return NotFound();
            }

            return View(discountUsed);
        }

        // POST: DiscountUseds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var discountUsed = await _context.DiscountUsed.FindAsync(id);
            _context.DiscountUsed.Remove(discountUsed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiscountUsedExists(int id)
        {
            return _context.DiscountUsed.Any(e => e.ID_DiscountUsed == id);
        }
    }
}

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
    public class DiscountsController : Controller
    {
        private readonly MyDbContext _context;

        public DiscountsController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Discounts
        public async Task<IActionResult> Index(string sortOrder, string searchString, int CountForTake = 1)
        {
            ViewData["Date"] = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewData["Name"] = sortOrder == "Name" ? "name_desc" : "Name";
            ViewData["Value"] = sortOrder == "Value" ? "value_desc" : "Value";
            ViewData["Turn"] = sortOrder == "Turn" ? "turn_desc" : "Turn";


            ViewData["Search"] = searchString;



            var Discounts = from d in _context.Discount
                           select d;

            if (!String.IsNullOrEmpty(searchString))
            {
                Discounts = Discounts.Where(d => d.Code_Discount.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "date_desc":
                    Discounts = Discounts.OrderByDescending(d => d.Expire);
                    break;
                case "Name":
                    Discounts = Discounts.OrderBy(d => d.Code_Discount);
                    break;
                case "name_desc":
                    Discounts = Discounts.OrderByDescending(d => d.Code_Discount);
                    break;
                case "Value":
                    Discounts = Discounts.OrderBy(d => d.Value);
                    break;
                case "value_desc":
                    Discounts = Discounts.OrderByDescending(d => d.Value);
                    break;
                case "Turn":
                    Discounts = Discounts.OrderByDescending(d => d.NumberofTurns);
                    break;
                case "turn_desc":
                    Discounts = Discounts.OrderBy(d => d.NumberofTurns);
                    break;
                default:
                    Discounts = Discounts.OrderBy(d => d.Expire);
                    break;
            }

            int take = 10;
            double total_product = Discounts.Count();

            int total_take = (int)Math.Ceiling(total_product / take);

            Discounts = Discounts.Skip((CountForTake - 1) * take).Take(take);
            ViewData["total_take"] = total_take;
            ViewData["CountForTake"] = CountForTake + 1;

            return View(await Discounts.AsNoTracking().ToListAsync());
        }

        // GET: Discounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discount = await _context.Discount
                .FirstOrDefaultAsync(m => m.ID_Discount == id);
            if (discount == null)
            {
                return NotFound();
            }

            return View(discount);
        }

        // GET: Discounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Discounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_Discount,Code_Discount,Expire,Value,NumberofTurns")] Discount discount)
        {
            if (ModelState.IsValid)
            {
                _context.Add(discount);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(discount);
        }

        // GET: Discounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discount = await _context.Discount.FindAsync(id);
            if (discount == null)
            {
                return NotFound();
            }
            return View(discount);
        }

        // POST: Discounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Discount,Code_Discount,Expire,Value,NumberofTurns")] Discount discount)
        {
            if (id != discount.ID_Discount)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(discount);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiscountExists(discount.ID_Discount))
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
            return View(discount);
        }

        // GET: Discounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discount = await _context.Discount
                .FirstOrDefaultAsync(m => m.ID_Discount == id);
            if (discount == null)
            {
                return NotFound();
            }

            return View(discount);
        }

        // POST: Discounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var discount = await _context.Discount.FindAsync(id);
            _context.Discount.Remove(discount);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiscountExists(int id)
        {
            return _context.Discount.Any(e => e.ID_Discount == id);
        }
        public IActionResult Search(string term)
        {
            return View("Index", _context.Discount.Where(m =>m.Code_Discount.Contains(term)));
        }
    }
}

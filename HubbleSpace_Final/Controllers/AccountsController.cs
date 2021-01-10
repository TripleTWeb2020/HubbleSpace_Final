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
    public class AccountsController : Controller
    {
        private readonly MyDbContext _context;

        public AccountsController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Accounts
        public async Task<IActionResult> Index(string sortOrder, string searchString, int CountForTake = 1)
        {
            ViewData["Date"] = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewData["Name"] = sortOrder == "Name" ? "name_desc" : "Name";
            ViewData["Level"] = sortOrder == "Level" ? "level_desc" : "Name";

            ViewData["Search"] = searchString;



            var Accounts = from a in _context.Account
                                 select a;

            if (!String.IsNullOrEmpty(searchString))
            {
                Accounts = Accounts.Where(p => p.UserName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "date_desc":
                    Accounts = Accounts.OrderBy(a => a.Date_Create);
                    break;
                case "Name":
                    Accounts = Accounts.OrderBy(a => a.UserName);
                    break;
                case "name_desc":
                    Accounts = Accounts.OrderByDescending(a => a.UserName);
                    break;
                case "Level":
                    Accounts = Accounts.OrderBy(a => a.Level);
                    break;
                case "level_desc":
                    Accounts = Accounts.OrderByDescending(a => a.Level);
                    break;
                default:
                    Accounts = Accounts.OrderByDescending(a => a.Date_Create);
                    break;
            }

            int take = 5;
            double total_product = Accounts.Count();

            int total_take = (int)Math.Ceiling(total_product / take);

            Accounts = Accounts.Skip((CountForTake - 1) * take).Take(take);
            ViewData["total_take"] = total_take;
            ViewData["CountForTake"] = CountForTake + 1;

            return View(await Accounts.AsNoTracking().ToListAsync());
        }
        
        // GET: Accounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Account
                .FirstOrDefaultAsync(m => m.ID_Account == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // GET: Accounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_Account,UserName,Password,Email,Level,Date_Create")] Account account)
        {
            if (ModelState.IsValid)
            {
                _context.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        // GET: Accounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Account.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Account,UserName,Password,Email,Level,Date_Create")] Account account)
        {
            if (id != account.ID_Account)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.ID_Account))
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
            return View(account);
        }

        // GET: Accounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Account
                .FirstOrDefaultAsync(m => m.ID_Account == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var account = await _context.Account.FindAsync(id);
            _context.Account.Remove(account);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(int id)
        {
            return _context.Account.Any(e => e.ID_Account == id);
        }

    }
}

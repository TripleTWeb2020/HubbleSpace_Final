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
    public class ClientsController : Controller
    {
        private readonly MyDbContext _context;

        public ClientsController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Clients
        public async Task<IActionResult> Index(string sortOrder, string searchString, int CountForTake = 1)
        {
            ViewData["Name"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["Date"] = sortOrder == "Date" ? "date_desc" : "Date";

            ViewData["Search"] = searchString;



            var Clients = from b in _context.Client.Include(c => c.Account)
                         select b;


            if (!String.IsNullOrEmpty(searchString))
            {
                Clients = Clients.Where(c => c.Account.Email.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "date_desc":
                    Clients = Clients.OrderByDescending(c => c.DOB);
                    break;
                case "Date":
                    Clients = Clients.OrderBy(c => c.DOB);
                    break;
                case "name_desc":
                    Clients = Clients.OrderByDescending(c => c.Client_Name);
                    break;
                default:
                    Clients = Clients.OrderBy(c => c.Client_Name);
                    break;
            }

            int take = 10;
            double total_product = Clients.Count();

            int total_take = (int)Math.Ceiling(total_product / take);

            Clients = Clients.Skip((CountForTake - 1) * take).Take(take);
            ViewData["total_take"] = total_take;
            ViewData["CountForTake"] = CountForTake + 1;

            return View(await Clients.AsNoTracking().ToListAsync());
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Client
                .Include(c => c.Account)
                .FirstOrDefaultAsync(m => m.ID_Client == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            ViewData["ID_Account"] = new SelectList(_context.Account, "ID_Account", "Email");
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_Client,Client_Name,Phone,DOB,Gender,CreditCard,ID_Account")] Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ID_Account"] = new SelectList(_context.Account, "ID_Account", "Email", client.ID_Account);
            return View(client);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Client.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            ViewData["ID_Account"] = new SelectList(_context.Account, "ID_Account", "Email", client.ID_Account);
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Client,Client_Name,Phone,DOB,Gender,CreditCard,ID_Account")] Client client)
        {
            if (id != client.ID_Client)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.ID_Client))
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
            ViewData["ID_Account"] = new SelectList(_context.Account, "ID_Account", "Email", client.ID_Account);
            return View(client);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Client
                .Include(c => c.Account)
                .FirstOrDefaultAsync(m => m.ID_Client == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.Client.FindAsync(id);
            _context.Client.Remove(client);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
            return _context.Client.Any(e => e.ID_Client == id);
        }

        public IActionResult Search(string term)
        {
            return View("Index", _context.Client.Where(m => m.Client_Name.Contains(term)));
        }
    }
}

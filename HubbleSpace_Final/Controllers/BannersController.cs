﻿using HubbleSpace_Final.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HubbleSpace_Final.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BannersController : Controller
    {
        private readonly MyDbContext _context;

        public BannersController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Banners
        public ActionResult Index(string sortOrder, string search, int page = 1)
        {
            ViewData["Date"] = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewData["Name"] = sortOrder == "Name" ? "name_desc" : "Name";

            ViewData["Search"] = search;



            var Banners = from b in _context.Banner
                          select b;

            if (!String.IsNullOrEmpty(search))
            {
                Banners = Banners.Where(b => b.Banner_Name.Contains(search));
            }

            switch (sortOrder)
            {
                case "date_desc":
                    Banners = Banners.OrderBy(a => a.Date_Upload);
                    break;
                case "Name":
                    Banners = Banners.OrderBy(a => a.Banner_Name);
                    break;
                case "name_desc":
                    Banners = Banners.OrderByDescending(a => a.Banner_Name);
                    break;
                default:
                    Banners = Banners.OrderByDescending(a => a.Date_Upload);
                    break;
            }

            PagedList<Banner> model = new PagedList<Banner>(Banners, page, 10);

            return View(model);
        }

        // GET: Banners/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banner = await _context.Banner
                .FirstOrDefaultAsync(m => m.ID_Banner == id);
            if (banner == null)
            {
                return NotFound();
            }

            return View(banner);
        }

        // GET: Banners/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Banners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_Banner,Banner_Name,Photo,Date_Upload")] Banner banner)
        {
            if (ModelState.IsValid)
            {
                _context.Add(banner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(banner);
        }

        // GET: Banners/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banner = await _context.Banner.FindAsync(id);
            if (banner == null)
            {
                return NotFound();
            }
            return View(banner);
        }

        // POST: Banners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Banner,Banner_Name,Photo,Date_Upload")] Banner banner)
        {
            if (id != banner.ID_Banner)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(banner);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BannerExists(banner.ID_Banner))
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
            return View(banner);
        }

        // GET: Banners/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banner = await _context.Banner
                .FirstOrDefaultAsync(m => m.ID_Banner == id);
            if (banner == null)
            {
                return NotFound();
            }

            return View(banner);
        }

        // POST: Banners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var banner = await _context.Banner.FindAsync(id);
            _context.Banner.Remove(banner);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BannerExists(int id)
        {
            return _context.Banner.Any(e => e.ID_Banner == id);
        }

        public IActionResult Search(string term)
        {
            return View("Index", _context.Banner.Where(m => m.Banner_Name.Contains(term)));
        }
    }
}

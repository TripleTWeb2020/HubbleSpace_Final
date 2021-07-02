﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HubbleSpace_Final.Entities;
using Microsoft.AspNetCore.Authorization;
using PagedList.Core;

namespace HubbleSpace_Final.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly MyDbContext _context;

        public CategoriesController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Categories
        public ActionResult Index(string sortOrder, string searchString, int page = 1)
        {
            ViewData["Name"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["Object"] = sortOrder == "Object" ? "object_desc" : "Object";
            ViewData["Search"] = searchString;

            var Categories = from c in _context.Category
                         select c;

            if (!String.IsNullOrEmpty(searchString))
            {
                Categories = Categories.Where(c => c.Category_Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    Categories = Categories.OrderByDescending(c => c.Category_Name);
                    break;
                case "Object":
                    Categories = Categories.OrderBy(c => c.Object);
                    break;
                case "object_desc":
                    Categories = Categories.OrderByDescending(a => a.Object);
                    break;
                default:
                    Categories = Categories.OrderBy(c => c.Category_Name);
                    break;
            }

            PagedList<Category> model = new PagedList<Category>(Categories, page, 10);

            return View(model);
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.ID_Categorie == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_Categorie,Category_Name,Object")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Categorie,Category_Name,Object")] Category category)
        {
            if (id != category.ID_Categorie)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.ID_Categorie))
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
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.ID_Categorie == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Category.FindAsync(id);
            _context.Category.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.ID_Categorie == id);
        }

        public IActionResult Search(string term)
        {
            return View("Index", _context.Category.Where(m => m.Category_Name.Contains(term)));
        }
    }
}

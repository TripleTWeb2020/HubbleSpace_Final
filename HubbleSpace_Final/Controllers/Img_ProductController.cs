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
    public class Img_ProductController : Controller
    {
        private readonly MyDbContext _context;

        public Img_ProductController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Img_Product
        public async Task<IActionResult> Index(string sortOrder, string searchString, int id, int CountForTake = 1)
        {
            ViewData["Name"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["ColorName"] = sortOrder == "ColorName" ? "colorname_desc" : "ColorName";

            ViewData["Search"] = searchString;



            var Img_Products = from i in _context.Img_Product.Include(i => i.color_Product).Include(i => i.color_Product.product)
                                 select i;
            if (id != null && id != 0)
            {
                TempData["ID_Color_Product"] = id;
                Img_Products = Img_Products.Where(s => s.color_Product.ID_Color_Product == id);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                Img_Products = Img_Products.Where(i => i.color_Product.product.Product_Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    Img_Products = Img_Products.OrderByDescending(c => c.color_Product.product.Product_Name);
                    break;
                case "ColorName":
                    Img_Products = Img_Products.OrderBy(c => c.color_Product.Color_Name);
                    break;
                case "colorname_desc":
                    Img_Products = Img_Products.OrderByDescending(c => c.color_Product.Color_Name);
                    break;
                default:
                    Img_Products = Img_Products.OrderBy(c => c.color_Product.product.Product_Name);
                    break;
            }

            int take = 10;
            double total_product = Img_Products.Count();

            int total_take = (int)Math.Ceiling(total_product / take);

            Img_Products = Img_Products.Skip((CountForTake - 1) * take).Take(take);
            ViewData["total_take"] = total_take;
            ViewData["CountForTake"] = CountForTake + 1;

            return View(await Img_Products.AsNoTracking().ToListAsync());
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
                .Include(i => i.color_Product.product)
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
            var Product_Color_Name = from c in _context.Color_Product
                                     select new
                                     {
                                         ID_Color_Product = c.ID_Color_Product,
                                         Name = c.Color_Name + " - " + c.product.Product_Name
                                     };
            ViewData["ID_Color_Product"] = new SelectList(Product_Color_Name, "ID_Color_Product", "Name");
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
            var Product_Color_Name = from c in _context.Color_Product
                                     select new
                                     {
                                         ID_Color_Product = c.ID_Color_Product,
                                         Name = c.Color_Name + " - " + c.product.Product_Name
                                     };
            ViewData["ID_Color_Product"] = new SelectList(Product_Color_Name, "ID_Color_Product", "Name", img_Product.ID_Color_Product);
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
            var Product_Color_Name = from c in _context.Color_Product
                                     select new
                                     {
                                         ID_Color_Product = c.ID_Color_Product,
                                         Name = c.Color_Name + " - " + c.product.Product_Name
                                     };
            ViewData["ID_Color_Product"] = new SelectList(Product_Color_Name, "ID_Color_Product", "Name", img_Product.ID_Color_Product);
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
            var Product_Color_Name = from c in _context.Color_Product
                                     select new
                                     {
                                         ID_Color_Product = c.ID_Color_Product,
                                         Name = c.Color_Name + " - " + c.product.Product_Name
                                     };
            ViewData["ID_Color_Product"] = new SelectList(Product_Color_Name, "ID_Color_Product", "Name", img_Product.ID_Color_Product); return View(img_Product);
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
                .Include(i => i.color_Product.product)
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

        public IActionResult Search(string term)
        {
            return View("Index", _context.Img_Product.Include(i => i.color_Product).Include(i => i.color_Product.product).Where(m => m.color_Product.product.Product_Name.Contains(term)));
        }
    }
}

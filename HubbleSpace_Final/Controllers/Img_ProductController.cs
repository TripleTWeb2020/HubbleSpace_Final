using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HubbleSpace_Final.Entities;
using PagedList.Core;

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
        public async Task<IActionResult> Index(string sortOrder, string searchString, int? id, int page = 1)
        {
            ViewData["Name"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["ColorName"] = sortOrder == "ColorName" ? "colorname_desc" : "ColorName";
            ViewData["Search"] = searchString;

            var Img_Products = from i in _context.Img_Product.Include(i => i.color_Product).Include(i => i.color_Product.product)
                               select i;

            if (id > 0)
            {
                ViewData["ID_ColorProduct"] = id;
                Img_Products = Img_Products.Where(s => s.color_Product.ID_Color_Product == id);
                ViewData["ID_Product"] = _context.Color_Product.Find(id).ID_Product;
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

            PagedList<Img_Product> model = new PagedList<Img_Product>(Img_Products, page, 10);


            return View(model);
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
            ViewData["ID_ColorProduct"] = img_Product.ID_Color_Product;
            return View(img_Product);
        }

        // GET: Img_Product/Create
        public IActionResult Create(int id)
        {
            var Color_Product = from c in _context.Color_Product where(c.ID_Color_Product == id)
                                     select new
                                     {
                                         ID_Color_Product = c.ID_Color_Product,
                                         Name = c.Color_Name + " - " + c.product.Product_Name
                                     };
            ViewData["ColorProduct_Select"] = new SelectList(Color_Product, "ID_Color_Product", "Name");
            ViewData["ID_ColorProduct"] = id;
            return View();
        }

        // POST: Img_Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("ID_Img_Product,Photo,ID_Color_Product")] Img_Product img_Product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(img_Product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { @id = img_Product.ID_Color_Product });
            }
            var Color_Product = from c in _context.Color_Product
                                where (c.ID_Color_Product == id)
                                select new
                                {
                                    ID_Color_Product = c.ID_Color_Product,
                                    Name = c.Color_Name + " - " + c.product.Product_Name
                                };
            ViewData["ColorProduct_Select"] = new SelectList(Color_Product, "ID_Color_Product", "Name", img_Product.ID_Color_Product);
            ViewData["ID_ColorProduct"] = id; 
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
            var Color_Product = from c in _context.Color_Product
                                where (c.ID_Color_Product == img_Product.ID_Color_Product)
                                select new
                                {
                                    ID_Color_Product = c.ID_Color_Product,
                                    Name = c.Color_Name + " - " + c.product.Product_Name
                                };
            ViewData["ColorProduct_Select"] = new SelectList(Color_Product, "ID_Color_Product", "Name", img_Product.ID_Color_Product);
            ViewData["ID_ColorProduct"] = img_Product.ID_Color_Product;
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
                return RedirectToAction("Index",new { @id = img_Product.ID_Color_Product });
            }
            var Color_Product = from c in _context.Color_Product
                                where (c.ID_Color_Product == img_Product.ID_Color_Product)
                                select new
                                {
                                    ID_Color_Product = c.ID_Color_Product,
                                    Name = c.Color_Name + " - " + c.product.Product_Name
                                };
            ViewData["ColorProduct_Select"] = new SelectList(Color_Product, "ID_Color_Product", "Name", img_Product.ID_Color_Product);
            ViewData["ID_ColorProduct"] = img_Product.ID_Color_Product;
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
                .Include(i => i.color_Product.product)
                .FirstOrDefaultAsync(m => m.ID_Img_Product == id);
            if (img_Product == null)
            {
                return NotFound();
            }
            ViewData["ID_ColorProduct"] = img_Product.ID_Color_Product;
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
            return RedirectToAction("Index", new { @id = img_Product.ID_Color_Product });
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

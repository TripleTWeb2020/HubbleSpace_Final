using HubbleSpace_Final.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HubbleSpace_Final.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly MyDbContext _context;

        public ProductsController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public ActionResult Index(string sortOrder, string search, int page = 1)
        {
            ViewData["Name"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["Price"] = sortOrder == "Price" ? "price_desc" : "Price";
            ViewData["Search"] = search;



            var Products = from p in _context.Product.Include(p => p.Brand).Include(p => p.category)
                           select p;

            if (!String.IsNullOrEmpty(search))
            {
                Products = Products.Where(a => a.Product_Name.Contains(search));
            }

            Products = sortOrder switch
            {
                "name_desc" => Products.OrderByDescending(p => p.Product_Name),
                "Price" => Products.OrderBy(p => p.Price_Sale),
                "price_desc" => Products.OrderByDescending(p => p.Price_Sale),
                _ => Products.OrderBy(p => p.Product_Name),
            };
            PagedList<Product> model = new PagedList<Product>(Products, page, 10);

            return View(model);
        }


        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Brand)
                .Include(p => p.category)
                .FirstOrDefaultAsync(m => m.ID_Product == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["ID_Brand"] = new SelectList(_context.Brand, "ID_Brand", "Brand_Name");
            var Category_Name = from c in _context.Category
                                select new
                                {
                                    c.ID_Categorie,
                                    Category_Name = c.Category_Name + " - " + c.Object
                                };
            ViewData["ID_Categorie"] = new SelectList(Category_Name, "ID_Categorie", "Category_Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_Product,Product_Name,Image,Price_Product,Price_Sale,ID_Brand,ID_Categorie")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ID_Brand"] = new SelectList(_context.Brand, "ID_Brand", "Brand_Name", product.ID_Brand);
            var Category_Name = from c in _context.Category
                                select new
                                {
                                    c.ID_Categorie,
                                    Category_Name = c.Category_Name + " - " + c.Object
                                };
            ViewData["ID_Categorie"] = new SelectList(Category_Name, "ID_Categorie", "Category_Name", product.ID_Categorie);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["ID_Brand"] = new SelectList(_context.Brand, "ID_Brand", "Brand_Name", product.ID_Brand);
            var Category_Name = from c in _context.Category
                                select new
                                {
                                    c.ID_Categorie,
                                    Category_Name = c.Category_Name + " - " + c.Object
                                };
            ViewData["ID_Categorie"] = new SelectList(Category_Name, "ID_Categorie", "Category_Name", product.ID_Categorie);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Product,Product_Name,Image,Price_Product,Price_Sale,ID_Brand,ID_Categorie")] Product product)
        {
            if (id != product.ID_Product)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ID_Product))
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
            ViewData["ID_Brand"] = new SelectList(_context.Brand, "ID_Brand", "Brand_Name", product.ID_Brand);
            var Category_Name = from c in _context.Category
                                select new
                                {
                                    c.ID_Categorie,
                                    Category_Name = c.Category_Name + " - " + c.Object
                                };
            ViewData["ID_Categorie"] = new SelectList(Category_Name, "ID_Categorie", "Category_Name", product.ID_Categorie);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Brand)
                .Include(p => p.category)
                .FirstOrDefaultAsync(m => m.ID_Product == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ID_Product == id);
        }

        public IActionResult Search(string term)
        {
            return View("Index", _context.Product.Where(m => m.Product_Name.Contains(term)));
        }
    }
}

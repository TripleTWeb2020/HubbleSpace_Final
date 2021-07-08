using System;
using System.Linq;
using System.Threading.Tasks;
using HubbleSpace_Final.Entities;
using HubbleSpace_Final.Models;
using Microsoft.AspNetCore.Mvc;
using PagedList.Core;
using ClosedXML.Excel;
using System.Data;
using System.IO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HubbleSpace_Final.Controllers
{
    public class EmailSubscriptionsController : Controller
    {
        private readonly MyDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        public EmailSubscriptionsController(MyDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;

        }
        public ActionResult Index(string sortOrder, string searchString, int page = 1)
        {
            ViewData["Email"] = String.IsNullOrEmpty(sortOrder) ? "Email_desc" : "";
            ViewData["Date_Created"] = sortOrder == "Date_Created" ? "date_desc" : "date_Created";
            ViewData["Search"] = searchString;

            var Email = from p in _context.EmailSubscription select p;
            if (!String.IsNullOrEmpty(searchString))
            {
                Email = Email.Where(a => a.Email.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Email_desc":
                    Email = Email.OrderByDescending(p => p.Email);
                    break;
                case "date_Created":
                    Email = Email.OrderBy(p => p.Date_Created);
                    break;
                case "date_desc":
                    Email = Email.OrderByDescending(p => p.Date_Created);
                    break;
                default:
                    Email = Email.OrderBy(p => p.Email);
                    break;
            }
            PagedList<EmailSubscription> model = new PagedList<EmailSubscription>(Email, page, 10);

            return View(model);
        }

        


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Subscribe([FromForm] EmailSubscriptionModel emailSubscriptionModel)
        {
            EmailSubscription EmailRequest = new Entities.EmailSubscription()
            {
                Email = emailSubscriptionModel.Email,
                Date_Created = DateTime.Now,
                subscribed_Status = Subscribed_Status.Subscribed
            };

            _context.Add(EmailRequest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public FileResult ExportToExcel()
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[4]
            {
                new DataColumn("No"),
                new DataColumn("Email"),
                new DataColumn("Date_Subscribed"),
                new DataColumn("Subscribed_Status"),
            });
            var ExportEmailSubscription = from p in _context.EmailSubscription select p;
            foreach (var file in ExportEmailSubscription)
            {
                dt.Rows.Add(file.ID_EmailSubscription, file.Email, file.Date_Created, file.subscribed_Status);
            }
            using(XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream()) //using System.IO;  
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "EmailSubscription.xlsx");
                }
            }

        }

        // GET: EmailSubscription/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var email = await _context.EmailSubscription
                .FirstOrDefaultAsync(m => m.ID_EmailSubscription == id);
            if (email == null)
            {
                return NotFound();
            }

            return View(email);
        }

        // GET: EmailSubscriptions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_EmailSubscription,Date_Created,Email,subscribed_Status")] EmailSubscription emailSubscription)
        {
            if (ModelState.IsValid)
            {
                _context.Add(emailSubscription);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(emailSubscription);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var email = await _context.EmailSubscription.FindAsync(id);
            if (email == null)
            {
                return NotFound();
            }
            return View(email);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_EmailSubscription,Date_Created,Email,subscribed_Status")] EmailSubscription emailSubscription)
        {
            if (id != emailSubscription.ID_EmailSubscription)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(emailSubscription);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmailExists(emailSubscription.ID_EmailSubscription))
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
            return View(emailSubscription);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var email = await _context.EmailSubscription
                .FirstOrDefaultAsync(m => m.ID_EmailSubscription == id);
            if (email == null)
            {
                return NotFound();
            }

            return View(email);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var email = await _context.EmailSubscription.FindAsync(id);
            _context.EmailSubscription.Remove(email);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmailExists(int id)
        {
            return _context.EmailSubscription.Any(e => e.ID_EmailSubscription == id);
        }

    }
}

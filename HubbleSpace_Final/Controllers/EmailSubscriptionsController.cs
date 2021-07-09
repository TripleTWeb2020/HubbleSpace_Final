using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HubbleSpace_Final.Entities;
using PagedList.Core;
using System.Data;
using ClosedXML.Excel;
using System.IO;
using HubbleSpace_Final.Models;

namespace HubbleSpace_Final.Controllers
{
    public class EmailSubscriptionsController : Controller
    {
        private readonly MyDbContext _context;

        public EmailSubscriptionsController(MyDbContext context)
        {
            _context = context;
        }

        // GET: EmailSubscriptions
        public ActionResult Index(string sortOrder, string searchString, int page = 1)
        {
            ViewData["Email"] = sortOrder == "Email" ? "Email_desc" : "Email";
            ViewData["date_Created"] = String.IsNullOrEmpty(sortOrder)? "" : "date_Created";
            ViewData["subscribed_Status"] = sortOrder == "subscribed_Status" ? "subscribedStatus_desc" : "subscribed_Status";

            ViewData["Search"] = searchString;

            var Email = from p in _context.EmailSubscription select p;
            if (!String.IsNullOrEmpty(searchString))
            {
                Email = Email.Where(a => a.Email.Contains(searchString));
            }

            Email = sortOrder switch
            {
                "Email_desc" => Email.OrderByDescending(p => p.Email),
                "Email" => Email.OrderBy(p => p.Email),
                "date_Created" => Email.OrderBy(p => p.Date_Created),
                "subscribed_Status" => Email.OrderBy(p => p.subscribed_Status),
                "subscribedStatus_desc" => Email.OrderByDescending(p => p.subscribed_Status),
                _ => Email.OrderByDescending(p => p.Date_Created),
            };
            PagedList<EmailSubscription> model = new PagedList<EmailSubscription>(Email, page, 10);

            return View(model);
        }

        
        public string Subscribe(string email)
        {
            if(_context.EmailSubscription.Where(c=>c.Email == email).Count() != 0)
                return "You have subscribed!";
            EmailSubscription EmailRequest = new EmailSubscription()
            {
                Email = email,
                subscribed_Status = Subscribed_Status.Subscribed
            };

            _context.Add(EmailRequest);
            var result = _context.SaveChanges();
            if (result == 0)
                return "Subscribe Failed!";
            return "You have subscribed successfully!";
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
                if(file.subscribed_Status == Subscribed_Status.Subscribed)
                {
                    dt.Rows.Add(file.ID_EmailSubscription, file.Email, file.Date_Created, file.subscribed_Status);
                }
               
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

        // GET: EmailSubscriptions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emailSubscription = await _context.EmailSubscription
                .FirstOrDefaultAsync(m => m.ID_EmailSubscription == id);
            if (emailSubscription == null)
            {
                return NotFound();
            }

            return View(emailSubscription);
        }

        // GET: EmailSubscriptions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EmailSubscriptions/Create
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

        // GET: EmailSubscriptions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emailSubscription = await _context.EmailSubscription.FindAsync(id);
            if (emailSubscription == null)
            {
                return NotFound();
            }
            return View(emailSubscription);
        }

        // POST: EmailSubscriptions/Edit/5
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
                    if (!EmailSubscriptionExists(emailSubscription.ID_EmailSubscription))
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

        // GET: EmailSubscriptions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emailSubscription = await _context.EmailSubscription
                .FirstOrDefaultAsync(m => m.ID_EmailSubscription == id);
            if (emailSubscription == null)
            {
                return NotFound();
            }

            return View(emailSubscription);
        }

        // POST: EmailSubscriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var emailSubscription = await _context.EmailSubscription.FindAsync(id);
            _context.EmailSubscription.Remove(emailSubscription);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmailSubscriptionExists(int id)
        {
            return _context.EmailSubscription.Any(e => e.ID_EmailSubscription == id);
        }
    }
}

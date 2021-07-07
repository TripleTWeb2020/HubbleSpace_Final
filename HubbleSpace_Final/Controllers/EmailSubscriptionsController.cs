using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HubbleSpace_Final.Entities;
using HubbleSpace_Final.Models;
using Microsoft.AspNetCore.Mvc;
using PagedList.Core;
using ClosedXML.Excel;
using System.Data;
using System.IO;

namespace HubbleSpace_Final.Controllers
{
    public class EmailSubscriptionsController : Controller
    {
        private readonly MyDbContext _context;
        public EmailSubscriptionsController(MyDbContext context)
        {
            _context = context;
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
        public async Task<IActionResult> Subscribe(EmailSubscriptionModel emailSubscriptionModel)
        {
            if (ModelState.IsValid)
            {
                var SubscriptionRequest = new Entities.EmailSubscription()
                {
                    Date_Created = DateTime.Now,
                    subscribed_Status = Subscribed_Status.Subscribed,
                    Email = emailSubscriptionModel.Email
                };
            _context.Add(SubscriptionRequest); ;
            await _context.SaveChangesAsync();
            }
            return View(emailSubscriptionModel);
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
    }
}

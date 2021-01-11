using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HubbleSpace_Final.Controllers
{
    public class Client_OrdersController : Controller
    {
        public IActionResult HistoryOrder()
        {
            return View();
        }
    }
}

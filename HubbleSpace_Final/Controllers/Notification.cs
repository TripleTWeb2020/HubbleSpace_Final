﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HubbleSpace_Final.Controllers
{
	public class Notification : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}

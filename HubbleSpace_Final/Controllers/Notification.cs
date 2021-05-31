using HubbleSpace_Final.Entities;
using HubbleSpace_Final.Models;
using HubbleSpace_Final.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HubbleSpace_Final.Controllers
{
	public class Notification : Controller
	{
		private readonly MyDbContext _context;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IUserService _userService;

		public Notification(MyDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUserService userService)
		{
			_context = context;
			_roleManager = roleManager;
			_userManager = userManager;
			_signInManager = signInManager;
			_userService = userService;
		}
		public async Task<IActionResult> Index()
		{
			var Task = from o in _context.Notifications.Include(o => o.User).OrderBy(o => o.Date_Created) select o;
			return View(await Task.AsNoTracking().ToListAsync());
		}
		public async Task<IActionResult> ClientNotification()
		{
			var Task = from o in _context.Notifications.Include(o => o.User).OrderBy(o => o.Date_Created) select o;
			return View(await Task.AsNoTracking().ToListAsync());
		}
	}

}

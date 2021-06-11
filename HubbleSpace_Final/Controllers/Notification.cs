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
			var Task = from o in _context.Notifications.Include(o => o.User).OrderByDescending(o => o.Date_Created) select o;
			// Query for notification
			var noti = from o in _context.Notifications.Include(o => o.User)
					   where o.ReadStatus == ReadStatus.Unread && o.User.Id == _userService.GetUserId()
					   select o.ID_Notifcation;
			int res = await noti.CountAsync();
			if (res != 0)
			{
				ViewData["Notifications"] = res;
			}
			else
			{
				ViewData["Notifications"] = 0;
			}
			var list = from o in _context.Notifications
					   where o.ReadStatus == ReadStatus.Unread && o.User.Id == _userService.GetUserId()
					   select o;
			var listt = list.ToList();

			var lists = from o in _context.Notifications
						where o.User.Id == _userService.GetUserId()
						select o;
			var listts = lists.OrderByDescending(o => o.Date_Created).Select(o => o.Content).ToList();
			ViewData["Noti1st"] = listts.ElementAt(0);
			ViewData["Noti2nd"] = listts.ElementAt(1);
			ViewData["Noti3rd"] = listts.ElementAt(2);
			ViewData["Noti4th"] = listts.ElementAt(3);


			foreach (NotificationPusher notif in listt)
			{
				notif.ReadStatus = ReadStatus.Read;
				_context.Update(notif);
				await _context.SaveChangesAsync();
			}
			return View(await Task.AsNoTracking().ToListAsync());
		}
		public async Task<IActionResult> ClientNotification()
		{
			var Task = from o in _context.Notifications.Include(o => o.User).OrderByDescending(o => o.Date_Created) select o;
			return View(await Task.AsNoTracking().ToListAsync());
		}
	}

}

﻿using HubbleSpace_Final.Entities;
using HubbleSpace_Final.Models;
using HubbleSpace_Final.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var notification = from o in _context.Notifications.Include(o => o.User)
                               where o.ReadStatus == ReadStatus.Unread && o.User.Id == _userService.GetUserId()
                               select o.ID_Notifcation;
            int ress = await notification.CountAsync();
            if (ress != 0)
            {
                ViewData["Notifications"] = ress;
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
            var listts = lists.OrderByDescending(o => o.Date_Created);
            ViewData["Noti"] = listts.ToList();



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

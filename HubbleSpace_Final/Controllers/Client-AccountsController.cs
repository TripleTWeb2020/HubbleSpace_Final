using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HubbleSpace_Final.Models;
using HubbleSpace_Final.Repository;
using Microsoft.AspNetCore.Mvc;

namespace HubbleSpace_Final.Controllers
{
    public class Client_AccountsController : Controller
       
    {
        private readonly IAccountRepository accountRepository;
        public Client_AccountsController(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }
       [Route("signup")]
        public IActionResult SignUp()
        {   
            return View();
        }
        [Route("signup")]
        [HttpPost]
        public async Task<IActionResult> SignUpAsync(SignUpUserModel userModel)
        {
            if (ModelState.IsValid)
            {
                var result = await this.accountRepository.CreateUserAsync(userModel);
                if (!result.Succeeded)
                {
                    foreach (var errorMessage in result.Errors)
                    {
                        ModelState.AddModelError("", errorMessage.Description);
                    }
                    return View(userModel);
                }
                ModelState.Clear();
            }
            return View(userModel);
        }
    }
}

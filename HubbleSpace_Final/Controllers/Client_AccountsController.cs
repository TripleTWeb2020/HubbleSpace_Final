﻿using System;
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
        private readonly IAccountRepository _accountRepository;
        public Client_AccountsController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
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
                var result = await _accountRepository.CreateUserAsync(userModel);
                if (!result.Succeeded)
                {
                    foreach (var errorMessage in result.Errors)
                    {
                        ModelState.AddModelError("", errorMessage.Description);
                    }
                    return View(userModel);
                }
                ModelState.Clear();
                return RedirectToAction("ConfirmEmail", new { email = userModel.Email });
            }
            return View(userModel);
        }
        [Route("Signin")]
        public IActionResult Signin()
        {
            return View();
        }
        [Route("Signin")]
        [HttpPost]
        public async Task<IActionResult> Login(SignInModel signInModel)
        {
            if (ModelState.IsValid)
            {
               var result=await _accountRepository.PasswordSignInAsync(signInModel);
               if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                if (result.IsNotAllowed)
                {
                    ModelState.AddModelError("", "Not Allowed to login");
                }
                else { ModelState.AddModelError("", "Invalid credentials"); }
                
            }
            return View(signInModel);
        }
        [Route("Signout")]
        public async Task<IActionResult> Signout()
        {
            await _accountRepository.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [Route("change-password")]
        public async Task<IActionResult> ChangePassword()
        {
            //await _accountRepository.SignOutAsync();
            return View();
        }
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.ChangePasswordAsync(model);
                if (result.Succeeded)
                {
                    ViewBag.IsSuccess = true;
                    ModelState.Clear();
                    return View();
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string uid,string token)
        {
            if(!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(token))
            {
                token = token.Replace(' ', '+');
                var result = await _accountRepository.ConfirmEmailAsync(uid, token);
                if (result.Succeeded)
                {
                    ViewBag.IsSuccess = true;
                }
            }
            return View();
           
        }
    }
}
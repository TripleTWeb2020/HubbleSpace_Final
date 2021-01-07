using HubbleSpace_Final.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HubbleSpace_Final.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountRepository(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser>signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IdentityResult> CreateUserAsync(SignUpUserModel userModel)
        {
            var user = new ApplicationUser()
            {
                LastName=userModel.LastName,
                FirstName=userModel.FirstName,
                Email = userModel.Email,
                UserName = userModel.Username,
               
            };
            var result = await _userManager.CreateAsync(user, userModel.Password);
            return result;

        }
        public async Task<SignInResult> PasswordSignInAsync(SignInModel signInModel)
        {
            var result= await _signInManager.PasswordSignInAsync(signInModel.Email, signInModel.Password, signInModel.RememberMe, false);
            return result;

        }
    }
}

using HubbleSpace_Final.Models;
using HubbleSpace_Final.Services;
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
        private readonly IUserService _userService;

        public AccountRepository(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser>signInManager,IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
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

            return await _signInManager.PasswordSignInAsync(signInModel.Username, signInModel.Password, signInModel.RememberMe,false);
            
           
            

        }
        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
        public async Task<IdentityResult>ChangePasswordAsync(ChangePasswordModel model)
        {
            var userId = _userService.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);
            return await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
        }
    }
}

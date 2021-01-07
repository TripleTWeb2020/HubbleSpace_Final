using HubbleSpace_Final.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace HubbleSpace_Final.Repository
{
    public interface IAccountRepository
    {
        Task<IdentityResult> CreateUserAsync(SignUpUserModel userModel);
    }
}
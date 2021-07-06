using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HubbleSpace_Final.Models
{
    public class SignInModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
        // AuthenticationScheme is in Microsoft.AspNetCore.Authentication namespace
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

    }
}

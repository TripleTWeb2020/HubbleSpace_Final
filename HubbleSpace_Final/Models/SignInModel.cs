using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HubbleSpace_Final.Models
{
    public class SignInModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name="Remember me")]
        public bool RememberMe { get; set; }

    }
}

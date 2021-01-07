using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HubbleSpace_Final.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int Gender { get; set; }
        public string CreditCard { get; set; }
    }
}

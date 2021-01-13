using HubbleSpace_Final.Entities;
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
        public Nullable<DateTime> DateOfBirth { get; set; }
        public int Gender { get; set; }
        public string CreditCard { get; set; } 
        public string Address { get; set; }
        public int level { get; set; }
        public virtual ICollection<Order> ToDoes { get; set; }
    }
}

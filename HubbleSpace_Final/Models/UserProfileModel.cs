using HubbleSpace_Final.Binder;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HubbleSpace_Final.Models
{
    public class UserProfileModel
    {
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter your first name")]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter your email")]
        [Display(Name = "Email address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email")]
        public string Email { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please enter your phone number")]
        [Display(Name = "Phone Number")]
        [Phone(ErrorMessage = "Please enter a valid Phone Number")]
        public string Phone { get; set; }

        [Display(Name = "Gender")]
        public bool Gender { get; set; }

        [Display(Name = "Your CreditCard")]
        [DataType(DataType.CreditCard)]
        public string CreditCard { get; set; }

        [Display(Name = "Your Date of Birth")]
        [DataType(DataType.Date)]
        
        public DateTime? DOB { get; set; }

        public virtual ApplicationUser applicationUser { get; set; }
    }
}

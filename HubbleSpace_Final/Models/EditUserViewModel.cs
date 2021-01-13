using HubbleSpace_Final.Binder;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HubbleSpace_Final.Models
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            Claims = new List<string>();
            Roles = new List<string>();
        }

        public string Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter your first name")]
        public string FirstName { get; set; }

        public string LastName { get; set; }


        [Required]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please enter your phone number")]
        [Phone(ErrorMessage = "Please enter a valid Phone Number")]
        public string Phone { get; set; }

        [DataType(DataType.CreditCard)]
        public string CreditCard { get; set; }

        [Display(Name = "Your Date of Birth")]
        [DataType(DataType.Date)]
        [ModelBinder(BinderType = typeof(DayMonthYearBinder))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd//MM/yyyy}")]
        public DateTime? DOB { get; set; }

        public List<string> Claims { get; set; }

        public IList<string> Roles { get; set; }
    }
}

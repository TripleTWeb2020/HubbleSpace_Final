using HubbleSpace_Final.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace HubbleSpace_Final.Entities
{
    [Table("DiscountUsed")]
    public class DiscountUsed
    {
        private readonly UserManager<ApplicationUser> _userManager;

        [Key]
        public int ID_DiscountUsed { get; set; }

        [Display(Name = "User")]
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        public int ID_Discount { get; set; }
        [ForeignKey("ID_Discount")]
        public Discount Discount { get; set; }
    }
}

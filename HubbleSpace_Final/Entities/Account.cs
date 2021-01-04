using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HubbleSpace_Final.Entities
{
    public enum level { Admin, Member}
    [Table("Account")]
    public class Account
    {
        [Key]
        public int ID_Account { get; set; }

        [Display(Name = "User Name")]
        [Required(ErrorMessage = "Không được để trống")]
        [MaxLength(100, ErrorMessage = "Không hợp lệ")]
        public string UserName { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Không được để trống")]
        [DataType(DataType.Password, ErrorMessage = "Không hợp lệ")]
        public string Password { get; set; }

        [Display(Name = "Avatar")]
        [DataType(DataType.ImageUrl, ErrorMessage = "Không hợp lệ")]
        public string Avatar { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Không được để trống")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Không hợp lệ")]
        public string Email { get; set; }

        [Display(Name = "Level")]
        [Required(ErrorMessage = "Không được để trống")]
        public level Level { get; set; }

        [Display(Name = "Ngày tạo")]
        [Required(ErrorMessage = "Không được để trống")]
        [DataType(DataType.DateTime, ErrorMessage = "Không hợp lệ")]
        public DateTime Date_Create { get; set; }

        public ICollection<Order> Orders { get; set; }

        public ICollection<DiscountUsed> DiscountUseds { get; set; }

        public Account()
        {
            Orders = new HashSet<Order>();
            Date_Create = DateTime.Now;
            DiscountUseds = new HashSet<DiscountUsed>();
        }
    }
}

using HubbleSpace_Final.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HubbleSpace_Final.Entities
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public int ID_Order { get; set; }

        [Display(Name = "Tổng tiền")]
        [Required(ErrorMessage = "Không được để trống")]
        public double TotalMoney { get; set; }

        [Display(Name = "Ngày bán")]
        [Required(ErrorMessage = "Không được để trống")]
        [DataType(DataType.DateTime, ErrorMessage = "Không hợp lệ")]
        public DateTime Date_Create { get; set; }

        [Display(Name = "Địa chỉ")]
        [Required(ErrorMessage = "Không được để trống")]
        public string Address { get; set; }

        [Display(Name = "Người nhận")]
        [Required(ErrorMessage = "Không được để trống")]
        public string Receiver { get; set; }

        [Display(Name = "SĐT")]
        [Required(ErrorMessage = "Không được để trống")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Không hợp lệ")]
        public string SDT { get; set; }

        [Display(Name = "Email")]
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [Display(Name = "Tình trạng")]
        [Required(ErrorMessage = "Không được để trống")]
        public string Process { get; set; }


        public ICollection<OrderDetail> OrderDetails { get; set; }
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
            Date_Create = DateTime.Now;
        }
    }
}

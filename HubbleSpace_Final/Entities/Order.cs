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
        public DateTime Date_Create => DateTime.Now;

        [Display(Name = "Địa chỉ")]
        [Required(ErrorMessage = "Không được để trống")]
        public string Address { get; set; }

        [Display(Name = "Người nhận")]
        [Required(ErrorMessage = "Không được để trống")]
        public string Receiver { get; set; }

        [Display(Name = "SĐT Người nhận")]
        [Required(ErrorMessage = "Không được để trống")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Không hợp lệ")]
        public string SDT { get; set; }

        [Display(Name = "Người đặt")]
        public int ID_Account { get; set; }
        [ForeignKey("ID_Account")]
        public Account account { get; set; }

        [Display(Name = "Tình trạng")]
        [Required(ErrorMessage = "Không được để trống")]
        public string Process { get; set; }


        public ICollection<OrderDetail> OrderDetails { get; set; }
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }
    }
}

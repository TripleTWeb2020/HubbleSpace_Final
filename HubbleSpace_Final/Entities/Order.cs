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
        public int Date { get; set; }

        [Display(Name = "Khách hàng")]
        public int Client { get; set; }
        [ForeignKey("ID_Client")]
        public Client client { get; set; }

        [Display(Name = "Thu ngân")]
        public int Cashier { get; set; }
        [ForeignKey("ID_Employee")]
        public Employee Employee { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }
    }
}

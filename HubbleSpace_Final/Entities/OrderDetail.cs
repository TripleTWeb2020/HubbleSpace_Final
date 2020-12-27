using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HubbleSpace_Final.Entities
{
    [Table("OrderDetail")]
    public class OrderDetail
    {
        [Key]
        public int ID_OrderDetail { get; set; }

        [Display(Name = "Tên sản phẩm")]
        public int ID_Product { get; set; }
        [ForeignKey("ID_Product")]
        public Product Product { get; set; }

        [Display(Name = "Đơn giá")]
        public double UnitPrice => Product.Price_Product ;

        [Display(Name = "Số lượng")]
        [Required(ErrorMessage = "Không được để trống")]
        [Range(1, 10, ErrorMessage = "Không hợp lệ")]
        public int Quantity { get; set; }

        [Display(Name = "Thành tiền")]
        public double TotalPrice => UnitPrice * Quantity;

        [Display(Name = "Mã đơn hàng")]
        public int ID_Order { get; set; }
        [ForeignKey("ID_Order")]
        public Order Order { get; set; }
    }
}

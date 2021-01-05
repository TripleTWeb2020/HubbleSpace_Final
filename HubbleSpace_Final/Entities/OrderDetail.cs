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

        [Display(Name = "Sản phẩm")]
        public int ID_Color_Product { get; set; }
        [ForeignKey("ID_Color_Product")]
        public Color_Product Color_Product { get; set; }

        [Display(Name = "Size")]
        public String Size { get; set; }

        [Display(Name = "Số lượng")]
        [Required(ErrorMessage = "Không được để trống")]
        [Range(1, 10, ErrorMessage = "Không hợp lệ")]
        public int Quantity { get; set; }

        [Display(Name = "Mã đơn hàng")]
        public int ID_Order { get; set; }
        [ForeignKey("ID_Order")]
        public Order order { get; set; }
    }
}

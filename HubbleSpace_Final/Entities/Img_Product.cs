using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HubbleSpace_Final.Entities
{
    [Table("Img_Product")]
    public class Img_Product
    {
        [Key]
        public int ID_Img_Product { get; set; }

        [Display(Name = "Hình ảnh")]
        [Required(ErrorMessage = "Không được để trống")]
        [DataType(DataType.Upload, ErrorMessage ="Không hợp lệ")]
        public string Photo { get; set; }

        [Display(Name = "Màu")]
        public int ID_Color_Product { get; set; }
        [ForeignKey("ID_Color_Product")]
        public Color_Product Color_Product { get; set; }

        [Display(Name = "Sản phẩm")]
        public string Product_Name => Color_Product.Product.Product_Name;

    }
}

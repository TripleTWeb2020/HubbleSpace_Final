using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HubbleSpace_Final.Entities
{
    
    [Table("Size")]
    public class Size
    {
        [Key]
        public int ID_Size_Product { get; set; }

        [Required(ErrorMessage ="Không được để trống")]
        [Display(Name = "Size")]
        public string SizeNumber { get; set; }

        [Required(ErrorMessage = "Không được để trống")]
        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }

        [Display(Name = "Màu")]
        public int ID_Color_Product { get; set; }
        [ForeignKey("ID_Color_Product")]
        public Color_Product color_Product { get; set; }

    }
}

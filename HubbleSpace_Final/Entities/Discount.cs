using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HubbleSpace_Final.Entities
{
    [Table("Discount")]
    public class Discount
    {
        [Key]
        public int ID_Discount { get; set; }

        [Display(Name = "Mã khuyến mãi")]
        [Required(ErrorMessage = "Không được để trống")]
        [MaxLength(100, ErrorMessage = "Không hợp lệ")]
        public string Code_Discount { get; set; }

        [Display(Name = "Hạn sử dụng")]
        [Required(ErrorMessage = "Không được để trống")]
        [DataType(DataType.DateTime, ErrorMessage = "Không hợp lệ")]
        public DateTime Expire { get; set; }

        [Display(Name = "Trị giá")]
        [Required(ErrorMessage = "Không được để trống")]
        [Range(0, 1000000, ErrorMessage = "Không hợp lệ")]
        public int Value { get; set; }

        [Display(Name = "Số lượt còn lại")]
        [Required(ErrorMessage = "Không được để trống")]
        [Range(0,100,ErrorMessage ="Không hợp lệ")]
        public int NumberofTurns { get; set; }

        public ICollection<DiscountUsed> DiscountUseds { get; set; }
        public Discount()
        {
            DiscountUseds = new HashSet<DiscountUsed>();

        }
    }
}

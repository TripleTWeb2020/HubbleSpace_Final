using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HubbleSpace_Final.Entities
{
    [Table("Banner")]
    public class Banner
    {
        [Key]
        public int ID_Banner { get; set; }

        [Display(Name = "Tên Banner")]
        [Required(ErrorMessage = "Không được để trống")]
        [MaxLength(100)]
        public string Banner_Name { get; set; }

        [Display(Name = "Hình ảnh")]
        [Required(ErrorMessage = "Không được để trống")]
        [DataType(DataType.ImageUrl, ErrorMessage = "Không hợp lệ")]
        public String Photo { get; set; }

        [Display(Name = "Ngày cập nhật")]
        [Required(ErrorMessage = "Không được để trống")]
        [DataType(DataType.DateTime, ErrorMessage = "Không hợp lệ")]
        public DateTime Date_Upload { get; set; }

        public Banner()
        {
            Date_Upload = DateTime.Now;
        }

    }
}

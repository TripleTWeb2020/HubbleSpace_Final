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
        [Required]
        [DataType(DataType.Upload, ErrorMessage = "Không được để trống")]
        public string Photo { get; set; }

        [Display(Name = "Ngày cập nhật")]
        [DataType(DataType.DateTime, ErrorMessage = "Không được để trống")]
        public string Date_Upload { get; set; }

    }
}

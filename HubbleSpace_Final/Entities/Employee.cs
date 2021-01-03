using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HubbleSpace_Final.Entities
{
    [Table("Employee")]
    public class Employee
    {
        [Key]
        public int ID_Employee { get; set; }

        [Display(Name = "Họ tên")]
        [Required(ErrorMessage = "Không được để trống")]
        [MaxLength(100, ErrorMessage = "Không hợp lệ")]
        public string Employee_Name { get; set; }

        [Display(Name = "SĐT")]
        [Required(ErrorMessage = "Không được để trống")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Không hợp lệ")]
        public string Phone { get; set; }
        
        [Display(Name = "Ngày sinh")]
        [Required(ErrorMessage = "Không được để trống")]
        [DataType(DataType.Date, ErrorMessage = "Không hợp lệ")]
        public DateTime DOB { get; set; }

        [Display(Name = "Giới tính")]
        [Required(ErrorMessage = "Không được để trống")]
        public Gender Gender { get; set; }

        [Display(Name = "Lương")]
        [Required(ErrorMessage = "Không được để trống")]
        [Range(0, 10000000, ErrorMessage = "Không hợp lệ")]
        public double Salary { get; set; }

        [Display(Name = "Account")]
        public int ID_Account { get; set; }
        [ForeignKey("ID_Account")]
        public Account Account { get; set; }
    }
}

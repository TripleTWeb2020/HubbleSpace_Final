using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HubbleSpace_Final.Entities
{
    public enum Gender { Nam, Nữ}
    [Table("Client")]
    public class Client
    {
        [Key]
        public int ID_Client { get; set; }

        [Display(Name = "Họ tên")]
        [Required(ErrorMessage = "Không được để trống")]
        [MaxLength(100, ErrorMessage = "Không hợp lệ")]
        public string Client_Name { get; set; }

        [Display(Name = "SĐT")]
        [Required(ErrorMessage = "Không được để trống")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Không hợp lệ")]
        public string Phone{ get; set; }

        [Display(Name = "Ngày sinh")]
        [Required(ErrorMessage = "Không được để trống")]
        [DataType(DataType.DateTime, ErrorMessage = "Không hợp lệ")]
        public DateTime DOB { get; set; }

        [Display(Name = "Giới tính")]
        [Required(ErrorMessage = "Không được để trống")]
        public Gender Gender { get; set; }

        [Display(Name = "Credit Card")]
        [DataType(DataType.CreditCard, ErrorMessage = "Không hợp lệ")]
        public string CreditCard { get; set; }

        [Display(Name = "Account")]
        public int ID_Account { get; set; }
        [ForeignKey("ID_Account")]
        public Account Account { get; set; }


    }
}

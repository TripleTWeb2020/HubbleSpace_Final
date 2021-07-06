using HubbleSpace_Final.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HubbleSpace_Final.Entities
{
    public enum ReadStatus { Read, Unread }
    [Table("NotificationsPusher")]
    public class NotificationPusher
    {
        [Key]
        public int ID_Notifcation { get; set; }

        [Display(Name = "User")]
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [Display(Name = "DateCreated")]
        [Required(ErrorMessage = "Không được để trống")]
        [DataType(DataType.DateTime, ErrorMessage = "Không hợp lệ")]
        public DateTime Date_Created { get; set; }

        [Display(Name = "Content")]
        [Required(ErrorMessage = "Không được để trống")]
        public string Content { get; set; }

        [Display(Name = "Status")]
        [Required(ErrorMessage = "Không được để trống")]
        public ReadStatus ReadStatus { get; set; }


        public NotificationPusher()
        {
        }
    }
}

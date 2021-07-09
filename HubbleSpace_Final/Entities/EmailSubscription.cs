using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HubbleSpace_Final.Entities
{
	public enum Subscribed_Status { Subscribed, Unscribed }
	[Table("EmailSubscription")]
	public class EmailSubscription
    {
		[Key]
		public int ID_EmailSubscription { get; set; }

		[Display(Name = "DateCreated")]
		[Required(ErrorMessage = "Không được để trống")]
		[DataType(DataType.DateTime, ErrorMessage = "Không hợp lệ")]
		public DateTime Date_Created { get; set; }

		[Display(Name = "Email")]
		[Required(ErrorMessage = "Không được để trống")]
		[DataType(DataType.EmailAddress,ErrorMessage ="Invalid email")]
		public string Email { get; set; }

		[Display(Name = "Status")]
		[Required(ErrorMessage = "Không được để trống")]
		public Subscribed_Status subscribed_Status { get; set; }


		public EmailSubscription()
		{
			Date_Created = DateTime.Now;
		}
	}
}

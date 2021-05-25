using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using HubbleSpace_Final.Models;

namespace HubbleSpace_Final.Entities
{
	public enum Status { Done, InProgress, ToDo }
	[Table("Schedule")]
	public class Schedule
	{
		[Key]
		public int ID_ToDo { get; set; }

		[Display(Name = "Email")]
		[ForeignKey("UserId")]
		public virtual ApplicationUser User { get; set; }

		[Display(Name = "DateCreated")]
		[Required(ErrorMessage = "Không được để trống")]
		[DataType(DataType.DateTime, ErrorMessage = "Không hợp lệ")]
		public DateTime Date_Created { get; set; }

		[Display(Name = "Title")]
		[Required(ErrorMessage = "Không được để trống")]
		public string Title { get; set; }

		[Display(Name = "Description")]
		[Required(ErrorMessage = "Không được để trống")]
		public string Description { get; set; }

		[Display(Name = "Status")]
		[Required(ErrorMessage = "Không được để trống")]
		public Status status { get; set; }

		public Schedule()
		{
		}

	}
}

using HubbleSpace_Final.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace HubbleSpace_Final.Entities
{
    [Table("DiscountUsed")]
    public class DiscountUsed
    {
        [Key]
        public int ID_DiscountUsed { get; set; }

        [Display(Name = "User")]
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        public int ID_Discount { get; set; }
        [ForeignKey("ID_Discount")]
        public Discount Discount { get; set; }
    }
}

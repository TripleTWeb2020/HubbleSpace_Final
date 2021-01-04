using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HubbleSpace_Final.Entities
{
    [Table("DiscountUsed")]
    public class DiscountUsed
    {
        [Key]
        public int ID_DiscountUsed { get; set; }

        public int ID_Account { get; set; }
        [ForeignKey("ID_Account")]
        public Account Account { get; set; }

        public int ID_Discount { get; set; }
        [ForeignKey("ID_Discount")]
        public Discount Discount { get; set; }
    }
}

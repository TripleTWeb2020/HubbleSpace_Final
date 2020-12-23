using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HubbleSpace_Final.Entities
{
    [Table("Type")]
    public class Type
    {
        [Key]
        public int ID_Type { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name_Type { get; set; }

        public ICollection<Product> Products { get; set; }
        public Type()
        {
            Products = new HashSet<Product>();
        }
    }
}

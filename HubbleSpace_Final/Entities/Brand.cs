using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HubbleSpace_Final.Entities
{
    [Table("Brand")]
    public class Brand
    {
        [Key]
        public int ID_Brand { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name_Brand { get; set; }
        
        public ICollection<Product> Products { get; set; }
        public Brand()
        {
            Products = new HashSet<Product>();
        }
    }
}

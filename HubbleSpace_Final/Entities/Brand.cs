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

        [Display(Name = "Thương hiệu")]
        [Required(ErrorMessage = "Không được để trống")]
        [MaxLength(100)]
        public string Brand_Name { get; set; }
        
        public ICollection<Product> Products { get; set; }
        public Brand()
        {
            Products = new HashSet<Product>();
        }
    }
}

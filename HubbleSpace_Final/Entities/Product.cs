using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HubbleSpace_Final.Entities
{
    

    [Table("Product")]
    public class Product
    {
        [Key]
        public int ID_Product { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name_Product { get; set; }

        [Required]
        [DataType(DataType.Upload)]
        public string Img_Product { get; set; }

        [Required]
        public Customers Customers { get; set; }

        [Required]
        public int Price_Product { get; set; }

        public int Price_Sale { get; set; }

        public int ID_Brand { get; set; }
        [ForeignKey("ID_Brand")]
        public Brand Brand { get; set; }

        public int ID_Type { get; set; }
        [ForeignKey("ID_Type")]
        public Type Type { get; set; }

    }

    public enum Customers { Men, Women, Kids }
}

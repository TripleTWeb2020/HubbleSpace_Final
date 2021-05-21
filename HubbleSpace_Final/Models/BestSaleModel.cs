using HubbleSpace_Final.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HubbleSpace_Final.Models
{
    public class BestSaleModel
    {
        public int ID_Color_Product { get; set; }
        public string NameColor { get; set; }
        public string Image { get; set; }
        public int ID_Product { get; set; }
        [ForeignKey("ID_Product")]
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
    }
}

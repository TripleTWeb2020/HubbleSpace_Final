using HubbleSpace_Final.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HubbleSpace_Final.Models
{
    public class CartItemModel
    {
        [Range(1,10)]
        public int Amount { get; set; }
        public Color_Product Color_Product { get; set; }
        public String Name { get; set; }
        public double Price { get; set; }
        public String Size { get; set; }
        public String Discount { get; set; }
        public double Value_Discount { get; set; }

    }
}

using HubbleSpace_Final.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace HubbleSpace_Final.Models
{
    public class CartItemModel
    {
        [Range(1, 10)]
        public int Amount { get; set; }
        public Color_Product Color_Product { get; set; }
        public String Name { get; set; }
        public double Price { get; set; }
        public String Size { get; set; }
        public int Discount { get; set; }
        public double Value_Discount { get; set; }

    }
}

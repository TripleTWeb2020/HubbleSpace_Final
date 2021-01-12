using HubbleSpace_Final.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HubbleSpace_Final.Models
{

    public class OrderDetailsModel
    {
        public Color_Product colorProduct { get; set; }
        public string Size { get; set; }
        public int Quantity { get; set; }
        public Order order { get; set; }
    }
}

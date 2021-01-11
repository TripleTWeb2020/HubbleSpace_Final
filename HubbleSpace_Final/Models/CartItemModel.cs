using HubbleSpace_Final.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HubbleSpace_Final.Models
{
    public class CartItemModel
    {
        public int Amount { get; set; }
        public Color_Product product { get; set; }
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HubbleSpace_Final.Models
{
    public class CheckOutViewModel
    {
        public List<CartItemModel> CartItems { get; set; }

        public CheckoutRequest CheckoutModel { get; set; }
        public virtual ApplicationUser appUser { get; set; }
    }
}

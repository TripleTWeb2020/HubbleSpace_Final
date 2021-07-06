using System.Collections.Generic;

namespace HubbleSpace_Final.Models
{
    public class CheckOutViewModel
    {
        public List<CartItemModel> CartItems { get; set; }

        public CheckoutRequest CheckoutModel { get; set; }
        public virtual ApplicationUser appUser { get; set; }
    }
}

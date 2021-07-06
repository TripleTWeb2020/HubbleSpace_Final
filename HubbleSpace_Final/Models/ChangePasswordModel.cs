using System.ComponentModel.DataAnnotations;

namespace HubbleSpace_Final.Models
{
    public class ChangePasswordModel
    {
        [Required, DataType(DataType.Password), Display(Name = "Current password")]
        public string CurrentPassword { get; set; }
        [Required, DataType(DataType.Password), Display(Name = "New password")]
        public string NewPassword { get; set; }
        [Required, DataType(DataType.Password), Display(Name = "Confirm New password")]
        [Compare("NewPassword", ErrorMessage = "Confirm new password does not match")]
        public string ConfirmNewPassword { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace HubbleSpace_Final.Models
{
    public class CreateRoleViewModel
    {
        [Required]
        [Display(Name = "Role")]
        public string RoleName { get; set; }
    }
}

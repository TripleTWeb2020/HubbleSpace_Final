using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HubbleSpace_Final.Models
{
    public class ManageRoleModel
    {
        public string RoleId { get; set; }
        
        public string RoleName { get; set; }
        public bool IsSelected { get; set; }

    }
    
}

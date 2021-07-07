using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HubbleSpace_Final.Models
{
    public class EmailSubscriptionModel
    {
        public string Email { get; set; }
        public DateTime Date_Subscribed { get; set; }
        public bool Subscribed_Status { get; set; }
    }
}

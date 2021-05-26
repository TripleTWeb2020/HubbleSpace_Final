using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PusherServer;

namespace HubbleSpace_Final.Helpers
    {
        public class ChannelHelper
        {
            public static async Task<IActionResult> Trigger(object data, string channelName, string eventName)
            {
                var options = new PusherOptions
                {
                    Cluster = "ap1",
                    
                    Encrypted = true
          
                };

                var pusher = new Pusher(
                 "1209415",
                 "8d3e1104be3eacf91241",
                 "c182166f41c68a4a28b2",
                 options
               );
                var result = await pusher.TriggerAsync(channelName,eventName,data);
                return new OkObjectResult(data);
            }
        }
    }
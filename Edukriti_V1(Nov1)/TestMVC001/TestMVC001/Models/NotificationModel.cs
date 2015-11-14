using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace TestMVC001.Models
{
    public class NotificationModel
    {
        [JsonProperty("tophonenumber")]
        public string ToPhoneNumber { get; set; }
        [JsonProperty("message")]
        public string Message{ get; set; }
        /*[JsonProperty("dtnotification")]
        public DateTime DtNotification{ get ; set; }*/
    }


}
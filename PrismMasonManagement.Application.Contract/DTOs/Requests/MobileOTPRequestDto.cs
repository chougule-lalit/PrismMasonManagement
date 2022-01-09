using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismMasonManagement.Application.Contracts.DTOs.Requests
{
    [Serializable]
    public class MobileOTPRequestDto
    {
        [JsonProperty("expiry")]
        public int Expiry { get; set; } = 900;

        [JsonProperty("message")]
        public string Message { get; set; } = "Your otp code is {code}";

        [JsonProperty("mobile")]
        public string Mobile { get; set; }

        [JsonProperty("sender_id")]
        public string SenderId { get; set; } = "SMSInfo";
    }
}

using CyberSourceTestApp_ones.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CyberSourceTestApp_ones.Requests
{
    [JsonObject(Description = "getcybersourcemerchantposinfo")]
    public class LoginRequest : BaseRequest
    {

        public LoginRequest(string json)
        {

        }
        [JsonProperty(PropertyName = "merchantId")]
        public string merchantId { get; set; }
        [JsonProperty(PropertyName = "merchantIdentifier")]
        public string merchantIdentifier { get; set; }
        [JsonProperty(PropertyName = "merchantSecret")]
        public string merchantSecret { get; set; }
    }
}

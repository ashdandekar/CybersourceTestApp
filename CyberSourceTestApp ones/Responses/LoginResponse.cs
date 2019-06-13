using CyberSourceTestApp_ones.Requests;
using Newtonsoft.Json;

namespace CyberSourceTestApp_ones.Responses
{
    [JsonObject(Description = "getcybersourcemerchantposinfo")]
    public class LoginResponse : ResponseWrapper, IBaseResponse
    {

        [JsonProperty(PropertyName="merchantId")]
        public string merchantid { get; set; }
    }
}

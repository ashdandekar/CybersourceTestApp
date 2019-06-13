using CyberSourceTestApp_ones.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSourceTestApp_ones.Requests
{
    public class ResponseWrapper
    {

        [JsonProperty(PropertyName = "response")]
        public IBaseResponse response { get; set; }

        [JsonProperty(PropertyName = "response")]
        public string status { get; set; }

        public ResponseWrapper(IBaseResponse newResponse)
        {
            response = newResponse;
        }
    }
}

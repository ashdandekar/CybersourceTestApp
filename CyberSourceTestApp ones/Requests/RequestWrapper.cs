using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSourceTestApp_ones.Requests
{
    public class RequestWrapper
    { 
        [JsonProperty(PropertyName ="request")]
        public IBaseRequest request{get;set;}
    }
}

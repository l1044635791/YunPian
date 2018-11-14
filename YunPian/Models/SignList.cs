using System.Collections.Generic;
using Newtonsoft.Json;

namespace YunPian.Models {
    public class SignList {
        [JsonProperty ("total")]
        public int Total { set; get; }

        [JsonProperty ("sign")]
        public List<Sign> Sign { set; get; }
    }
}
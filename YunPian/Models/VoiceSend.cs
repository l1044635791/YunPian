using Newtonsoft.Json;

namespace YunPian.Models {
    public class VoiceSend {
        [JsonProperty ("sid")]
        public string Sid { set; get; }

        [JsonProperty ("count")]
        public int Count { set; get; }

        [JsonProperty ("fee")]
        public double Fee { set; get; }
    }
}
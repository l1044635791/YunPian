using Newtonsoft.Json;

namespace YunPian.Models {
    public class VoiceStatus : BaseStatus {
        [JsonProperty ("uid")]
        public string Uid { set; get; }

        [JsonProperty ("duration")]
        public double Duration { set; get; }
    }
}
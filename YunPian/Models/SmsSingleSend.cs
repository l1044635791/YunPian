using Newtonsoft.Json;

namespace YunPian.Models {
    public class SmsSingleSend {
        [JsonProperty ("code")]
        public int Code { set; get; }

        [JsonProperty ("msg")]
        public string Msg { set; get; }

        [JsonProperty ("count")]
        public int Count { set; get; }

        [JsonProperty ("fee")]
        public double Fee { set; get; }

        [JsonProperty ("unit")]
        public string Unit { set; get; }

        [JsonProperty ("mobile")]
        public string Mobile { set; get; }

        [JsonProperty ("sid")]
        public long Sid { set; get; }

        public override string ToString () {
            return $"短信编号:{Sid},成功给手机号 {Mobile} 发送{Count}条短信，花费{Fee}{Unit}".Replace ("RMB", "元");
        }
    }
}
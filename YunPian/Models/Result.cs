using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YunPian.Models {
    public class Result<T> {
        [JsonProperty ("code")]
        [DefaultValue (YunPianFields.Ok)]
        public int Code { get; set; }

        [JsonProperty ("msg")]
        public string Msg { get; set; }

        [JsonProperty ("detail")]
        public string Detail { get; set; }

        [JsonProperty ("data")]
        public T Data { get; set; }

        [JsonProperty ("e")]
        [JsonIgnore]
        public Exception Exception { get; set; }

        public bool IsSuccess () {
            return Code == YunPianFields.Ok;
        }

        public override string ToString () {
            return JsonConvert.SerializeObject (this);
        }
    }

}
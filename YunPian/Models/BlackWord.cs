using System.Collections.Generic;
using Newtonsoft.Json;

namespace YunPian.Models {
    public class BlackWord {
        [JsonProperty ("black_word")]
        public string BlackWordList { set; get; }
        public List<string> ToList () {
            return BlackWordList != null ? new List<string> (BlackWordList.Split (',')) : new List<string> ();
        }
    }
}
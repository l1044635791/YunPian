using Newtonsoft.Json;
using YunPian.Handlers;
using YunPian.Models;

namespace YunPian.Handlers {
    public class StdResultHandler<T> : ResultHandler<Result<T>, T> {
        public StdResultHandler (string version) : base (version) { }
        public override Result<T> Response (string response) {
            var result = JsonConvert.DeserializeObject<Result<T>> (response);
            this.Code = GetResultStatusCode (result);
            return result;
        }

        public override int GetResultStatusCode (Result<T> result) {
            return result.Code;
        }

        public override Result<T> Success (Result<T> result) {
            return result;
        }

        public override Result<T> Fail (Result<T> result) {
            return result;
        }
    }
}
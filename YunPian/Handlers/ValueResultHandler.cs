using System;
using Newtonsoft.Json;
using YunPian.Models;

namespace YunPian.Handlers {
    public class ValueResultHandler<T> : ResultHandler<Result<T>, T> {
        private readonly Func<string, T> _resultHandler;
        public ValueResultHandler (string version, Func<string, T> resultHandler) : base (version) {
            _resultHandler = resultHandler;
        }

        public override Result<T> Response (string response) {
            if (response != null && response[0] == '{') {
                var result = JsonConvert.DeserializeObject<Result<T>> (response);
                this.Code = GetResultStatusCode (result);
                return result;
            } else {
                var result = new Result<T> { Data = _resultHandler (response) };
                this.Code = GetResultStatusCode (result);
                return result;
            }
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
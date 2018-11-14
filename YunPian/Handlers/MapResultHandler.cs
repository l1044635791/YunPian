using System;
using Newtonsoft.Json.Linq;
using YunPian.Models;

namespace YunPian.Handlers {
    public class MapResultHandler<T> : ResultHandler<JObject, T> {
        private readonly Func<JObject, T> _resultHandler;
        public MapResultHandler (string version, Func<JObject, T> resultHandler) : base (version) {
            _resultHandler = resultHandler;
        }

        public override JObject Response (string response) {
            var result = response == null ? new JObject () : JObject.Parse (response);
            this.Code = GetResultStatusCode (result);
            return result;
        }

        public override Result<T> Success (JObject response) {
            return new Result<T> () {
                Code = Code,
                    Data = _resultHandler (response)
            };
        }

        /**
         * 错误流程 v1和v2返回格式一致
         */
        public override Result<T> Fail (JObject response) {
            return new Result<T> () {
                Code = Code,
                    Msg = response[YunPianFields.Msg]?.ToString (),
                    Detail = response[YunPianFields.Detail]?.ToString ()

            };
        }

        public override int GetResultStatusCode (JObject response) {
            var code = YunPianFields.UnknownException;
            if (response == null) return code;

            if (_version == null)
                _version = YunPianFields.VersionV2;

            switch (_version) {
                case YunPianFields.VersionV1:
                    code = response[YunPianFields.Code]?.ToObject<int> () ?? YunPianFields.UnknownException;
                    break;
                case YunPianFields.VersionV2:
                    code = response[YunPianFields.Code]?.ToObject<int> () ?? YunPianFields.Ok;
                    break;
            }

            return code;
        }
    }
}
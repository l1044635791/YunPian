using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using YunPian.Models;

namespace YunPian.Handlers {
    public class ListMapResultHandler<T> : ResultHandler<JContainer, List<T>> {
        private readonly Func<JContainer, List<T>> _resultHander;

        public ListMapResultHandler (string version, Func<JContainer, List<T>> resultHander) : base (version) {
            _resultHander = resultHander;
        }

        public override JContainer Response (string response) {
            if (response != null && response[0] == '[') {
                var result = JArray.Parse (response);
                this.Code = GetResultStatusCode (result);
                return result;
            } else {
                var result = JObject.Parse (response);
                this.Code = GetResultStatusCode (result);
                return result;
            }
        }

        public override Result<List<T>> Success (JContainer response) {
            return new Result<List<T>> () {
                Code = Code,
                    Data = _resultHander (response)
            };
        }

        /**
         * 错误流程 v1和v2返回格式一致
         */
        public override Result<List<T>> Fail (JContainer response) {
            if (response is JObject jObject)
                return new Result<List<T>> () {
                    Code = Code,
                        Msg = jObject[YunPianFields.Msg]?.ToString (),
                        Detail = jObject[YunPianFields.Detail]?.ToString (),
                };

            return new Result<List<T>> () {
                Code = Code
            };
        }

        public override int GetResultStatusCode (JContainer response) {
            if (response is JObject jObject) {
                var code = YunPianFields.UnknownException;
                if (jObject == null) return code;

                if (_version == null)
                    _version = YunPianFields.VersionV2;

                switch (_version) {
                    case YunPianFields.VersionV1:
                        code = jObject[YunPianFields.Code]?.ToObject<int> () ?? YunPianFields.UnknownException;
                        break;
                    case YunPianFields.VersionV2:
                        code = jObject[YunPianFields.Code]?.ToObject<int> () ?? YunPianFields.Ok;
                        break;
                }
                return code;
            } else {
                return YunPianFields.Ok;
            }
        }
    }

    public class SimpleListResultHandler<T> : ListMapResultHandler<T> {
        public SimpleListResultHandler (string version) : base (version, response => response.ToObject<List<T>> ()) { }
    }
}
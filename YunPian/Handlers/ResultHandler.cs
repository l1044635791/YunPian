using System;
using System.Collections;
using Newtonsoft.Json.Linq;
using YunPian.Models;

namespace YunPian.Handlers {
    public abstract class ResultHandler<TR, T> : IResultHandler<TR, T> {
        protected string _version;
        public ResultHandler (string version) {
            _version = version;
        }
        public int Code { get; set; }
        public abstract TR Response (string response);
        public abstract Result<T> Success (TR response);
        public abstract Result<T> Fail (TR response);
        public abstract int GetResultStatusCode (TR response);

        public Result<T> CatchException (Exception ex) {
            return new Result<T> () {
                Code = YunPianFields.UnknownException,
                    Exception = ex,
            };
        }
    }
}
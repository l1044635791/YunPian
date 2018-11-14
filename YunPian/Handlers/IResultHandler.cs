using System;
using Newtonsoft.Json.Linq;
using YunPian.Models;

namespace YunPian.Handlers {
    public interface IResultHandler<TR, T> {
        int Code { get; set; }
        int GetResultStatusCode (TR response);
        TR Response (string response);
        Result<T> Success (TR response);

        Result<T> Fail (TR response);

        Result<T> CatchException (Exception ex);
    }
}
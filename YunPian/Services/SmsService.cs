using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using YunPian.Handlers;
using YunPian.Models;

namespace YunPian.Services {
    public class SmsService : BaseService, ISmsService {
        public SmsService (IHttpClientFactory httpClientFactory, IOptions<YunPianOptions> options) : base (options) {
            _httpClient = httpClientFactory.CreateClient (nameof (YunPianOptions._smsHost));
        }

        /// <summary>
        /// 发送短信(单条)
        /// </summary>
        /// <param name="text">发送的文本内容</param>
        /// <param name="mobile">手机号 示例：单号码：13100000001</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public async Task<Result<SmsSingleSend>> SingleSendAsync (string text, string mobile, string charset = null) {
            var data = new Dictionary<string, string> ();

            data.Add (YunPianFields.Mobile, mobile);
            data.Add (YunPianFields.Text, text);

            // 设置对Result<SmsSingleSend>进行处理的方法
            var resultHandler = new MapResultHandler<SmsSingleSend> (Options.Version,
                response => {
                    return Options.Version == YunPianFields.VersionV2 ? response.ToObject<SmsSingleSend> () : null;
                });

            return await PostAsync (data, resultHandler, Options.SingleSendSms, charset);
        }

        /// <summary>
        /// 发送短信(批量)
        /// </summary>
        /// <param name="text">发送的文本内容</param>
        /// <param name="mobile">手机号(批量) 中间以逗号（‘,’）隔开，一次不要超过1000个 示例：单号码：13100000001 /多号码：13100000001,13100000002</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public async Task<Result<SmsBatchSend>> BatchSendAsync (string text, string mobile, string charset = null) {
            var data = new Dictionary<string, string> ();

            data.Add (YunPianFields.Mobile, mobile);
            data.Add (YunPianFields.Text, text);

            // 设置对Result<SmsBatchSend>进行处理的方法
            var resultHandler = new MapResultHandler<SmsBatchSend> (Options.Version, response => {
                return Options.Version == YunPianFields.VersionV2 ? new SmsBatchSend {
                    TotalCount = response[YunPianFields.TotalCount].ToObject<int> (),
                        TotalFee = response[YunPianFields.TotalFee].ToObject<double> (),
                        Data = response[YunPianFields.Data].ToObject<List<SmsSingleSend>> ()
                } : null;
            });

            return await PostAsync (data, resultHandler, Options.BatchSendSms, charset);
        }

        /// <summary>
        /// 发送短信(个性化发送)
        /// </summary>
        /// <param name="text">发送的文本内容 , 多条文本中间以逗号（‘,’）隔开，文本数量和手机号数量保持一致</param>
        /// <param name="mobile">手机号(批量) 中间以逗号（‘,’）隔开，一次不要超过1000个 示例：单号码：13100000001 /多号码：13100000001,13100000002</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public async Task<Result<SmsBatchSend>> MultiSendAsync (string text, string mobile, string charset = null) {
            var data = new Dictionary<string, string> ();

            data.Add (YunPianFields.Mobile, mobile);
            data.Add (YunPianFields.Text, TextUrlEncode (",", text));

            // 设置对Result<SmsBatchSend>进行处理的方法
            var resultHandler = new MapResultHandler<SmsBatchSend> (Options.Version, response => {
                return Options.Version == YunPianFields.VersionV2 ? new SmsBatchSend {
                    TotalCount = response[YunPianFields.TotalCount].ToObject<int> (),
                        TotalFee = response[YunPianFields.TotalFee].ToObject<double> (),
                        Data = response[YunPianFields.Data].ToObject<List<SmsSingleSend>> ()
                } : null;
            });

            return await PostAsync (data, resultHandler, Options.MultiSendSms, charset);
        }

        /// <summary>
        /// 发送短信V1版本(个性化发送)
        /// </summary>
        /// <param name="text">发送的文本内容,文本数量和手机号数量保持一致</param>
        /// <param name="mobile">手机号(批量) 中间以逗号（‘,’）隔开，一次不要超过1000个 示例：单号码：13100000001 /多号码：13100000001,13100000002</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public async Task<Result<List<SmsSingleSend>>> MultiSendAsync_V1 (string mobile, string charset = null, params string[] text) {
            var data = new Dictionary<string, string> ();

            data.Add (YunPianFields.Mobile, mobile);
            data.Add (YunPianFields.Text, TextUrlEncode (",", charset, text));

            // 设置对Result<List<SmsSingleSend>>进行处理的方法
            var resultHandler = new SimpleListResultHandler<SmsSingleSend> (YunPianFields.VersionV1);

            return await PostAsync (data, resultHandler, Options.MultiSendSms_V1, charset);
        }

        /// <summary>
        /// 获取短信发送状态
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量(默认20，最大数量100)</param>
        /// <returns></returns>
        public async Task<Result<List<SmsStatus>>> PullSmsStatusAsync (int pageIndex, int pageSize) {
            var data = new Dictionary<string, string> ();

            data.Add (YunPianFields.PageNum, pageIndex.ToString ());
            data.Add (YunPianFields.PageSize, pageSize.ToString ());

            // 设置对Result<List<SmsStatus>>进行处理的方法
            var resultHandler = new ListMapResultHandler<SmsStatus> (Options.Version, response => {
                switch (Options.Version) {
                    case YunPianFields.VersionV1:
                        return response is JObject jObject ? jObject[YunPianFields.SmsStatus].ToObject<List<SmsStatus>> () : new List<SmsStatus> ();
                    case YunPianFields.VersionV2:
                        return response.ToObject<List<SmsStatus>> ();
                    default:
                        return new List<SmsStatus> ();
                }
            });

            return await PostAsync (data, resultHandler, Options.PullSmsStatus);
        }

        /// <summary>
        /// 获取回复短信信息
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量(默认20，最大数量100)</param>
        /// <returns></returns>
        public async Task<Result<List<SmsReply>>> PullSmsReplyAsync (int pageIndex, int pageSize) {
            var data = new Dictionary<string, string> ();

            data.Add (YunPianFields.PageNum, pageIndex.ToString ());
            data.Add (YunPianFields.PageSize, pageSize.ToString ());

            // 设置对Result<List<SmsReply>>进行处理的方法
            var resultHandler = new ListMapResultHandler<SmsReply> (Options.Version, response => {
                switch (Options.Version) {
                    case YunPianFields.VersionV1:
                        return response is JObject jObject ? jObject[YunPianFields.SmsReply].ToObject<List<SmsReply>> () : new List<SmsReply> ();
                    case YunPianFields.VersionV2:
                        return response.ToObject<List<SmsReply>> ();
                    default:
                        return new List<SmsReply> ();
                }
            });

            return await PostAsync (data, resultHandler, Options.PullSmsReply);
        }

        /// <summary>
        /// 查看回复的短信
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量(默认20，最大数量100)</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="mobile">手机号</param>
        /// <returns></returns>
        public async Task<Result<List<SmsReply>>> GetSmsReplyAsync (int pageIndex, int pageSize, DateTime? start, DateTime? end, string mobile) {
            var data = new Dictionary<string, string> ();

            data.Add (YunPianFields.PageNum, pageIndex.ToString ());
            data.Add (YunPianFields.PageSize, pageSize.ToString ());
            if (start.HasValue)
                data.Add (YunPianFields.StartTime, start.Value.ToString ("yyyy-MM-dd HH:mm:ss"));
            if (end.HasValue)
                data.Add (YunPianFields.EndTime, end.Value.ToString ("yyyy-MM-dd HH:mm:ss"));

            // 存在手机号查看该手机号的信息，不存在手机号返回所有信息
            if (!string.IsNullOrWhiteSpace (mobile))
                data.Add (YunPianFields.Mobile, mobile);

            // 设置对Result<List<SmsReply>>进行处理的方法
            var resultHandler = new ListMapResultHandler<SmsReply> (Options.Version, response => {
                switch (Options.Version) {
                    case YunPianFields.VersionV1:
                        return response is JObject jObject ? jObject[YunPianFields.SmsReply].ToObject<List<SmsReply>> () : new List<SmsReply> ();
                    case YunPianFields.VersionV2:
                        return response.ToObject<List<SmsReply>> ();
                    default:
                        return new List<SmsReply> ();
                }
            });

            return await PostAsync (data, resultHandler, Options.GetSmsReply);
        }

        /// <summary>
        /// 查看文本屏蔽词
        /// </summary>
        /// <param name="text">文本内容</param>
        /// <returns>返回文本屏蔽词</returns>
        public async Task<Result<List<string>>> GetBlackWordsAsync (string text) {
            var data = new Dictionary<string, string> ();

            data.Add (YunPianFields.Text, text);

            // 设置对Result<List<string>>进行处理的方法
            var resultHandler = new ListMapResultHandler<string> (Options.Version, response => {
                switch (Options.Version) {
                    case YunPianFields.VersionV1:
                        return response is JObject jObject ? jObject[YunPianFields.Result].ToObject<BlackWord> ().ToList () : new List<string> ();
                    case YunPianFields.VersionV2:
                        return response.ToObject<List<string>> ();
                    default:
                        return new List<string> ();
                }
            });

            return await PostAsync (data, resultHandler, Options.GetBlackWords);
        }

        /// <summary>
        /// 查看短信记录
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量(默认20，最大数量100)</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="mobile">手机号</param>
        /// <returns>返回短信记录</returns>
        public async Task<Result<List<SmsRecord>>> GetRecordAsync (int pageIndex, int pageSize, DateTime? start, DateTime? end, string mobile) {
            var data = new Dictionary<string, string> ();

            data.Add (YunPianFields.PageNum, pageIndex.ToString ());
            data.Add (YunPianFields.PageSize, pageSize.ToString ());
            if (start.HasValue)
                data.Add (YunPianFields.StartTime, start.Value.ToString ("yyyy-MM-dd HH:mm:ss"));
            if (end.HasValue)
                data.Add (YunPianFields.EndTime, end.Value.ToString ("yyyy-MM-dd HH:mm:ss"));

            // 存在手机号查看该手机号的信息，不存在手机号返回所有信息
            if (!string.IsNullOrWhiteSpace (mobile))
                data.Add (YunPianFields.Mobile, mobile);

            // 设置对Result<List<SmsRecord>>进行处理的方法
            var resultHandler = new ListMapResultHandler<SmsRecord> (Options.Version, response => {
                switch (Options.Version) {
                    case YunPianFields.VersionV1:
                        return response is JObject jObject ? jObject[YunPianFields.Sms].ToObject<List<SmsRecord>> () : new List<SmsRecord> ();
                    case YunPianFields.VersionV2:
                        return response.ToObject<List<SmsRecord>> ();
                    default:
                        return new List<SmsRecord> ();
                }
            });

            return await PostAsync (data, resultHandler, Options.GetSmsRecord);
        }

        /// <summary>
        /// 统计短信发送条数
        /// </summary>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="mobile">手机号</param>
        /// <returns>返回短信发送条数</returns>
        public async Task<Result<int>> GetSmsCountAsync (DateTime? start, DateTime? end, string mobile) {
            var data = new Dictionary<string, string> ();

            if (start.HasValue)
                data.Add (YunPianFields.StartTime, start.Value.ToString ("yyyy-MM-dd HH:mm:ss"));
            if (end.HasValue)
                data.Add (YunPianFields.EndTime, end.Value.ToString ("yyyy-MM-dd HH:mm:ss"));
            // 存在手机号统计该手机号的短信数量，不存在手机号返回所有短信数量
            if (!string.IsNullOrWhiteSpace (mobile))
                data.Add (YunPianFields.Mobile, mobile);

            // 设置对Result<int>进行处理的方法
            var resultHandler = new ValueResultHandler<int> (Options.Version, int.Parse);

            return await PostAsync (data, resultHandler, Options.GetSmsCount);
        }

        /// <summary>
        /// 指定模板发送 V1
        /// </summary>
        /// <param name="tplId">模板ID</param>
        /// <param name="tplValue">模板内容 String
        /// 注：变量名和变量值都不能为空  
        /// 模板:【#company#】您的验证码是#code#。  
        /// 最终发送结果:【云片网】您的验证码是1234。  
        /// tplValue=urlencode("#code#") + "=" + urlencode("1234") + "&amp;" +urlencode("#company#") + "=" + urlencode("云片网");  
        /// 直接发送报文请求则使用下面的形式  
        /// tplValue=urlencode(urlencode("#code#") + "=" + urlencode("1234") +"&amp;" + urlencode("#company#") + "=" + urlencode("云片网"));  
        /// </param>
        /// <param name="mobile">发送手机号</param>
        /// <param name="charset">编码格式</param>
        /// <returns>返回短信发送状态信息</returns>
        public async Task<Result<SmsSingleSend>> TplSingleSendAsync_V1 (string tplId, string tplValue, string mobile, string charset = null) {
            var data = new Dictionary<string, string> ();

            data.Add (YunPianFields.Mobile, mobile);
            data.Add (YunPianFields.TplId, tplId);
            data.Add (YunPianFields.TplValue, tplValue);

            // 设置对Result<SmsSingleSend>进行处理的方法
            var resultHandler = new MapResultHandler<SmsSingleSend> (YunPianFields.VersionV1,
                response => {
                    return response[YunPianFields.Result].ToObject<SmsSingleSend> ();
                });

            return await PostAsync (data, resultHandler, Options.TplSingleSendSms_V1, charset);
        }

        /// <summary>
        /// 指定模板发送 V2
        /// </summary>
        /// <param name="tplId">模板ID</param>
        /// <param name="tplValue">模板内容 String
        /// 注：变量名和变量值都不能为空  
        /// 模板:【#company#】您的验证码是#code#。  
        /// 最终发送结果:【云片网】您的验证码是1234。  
        /// tplValue=urlencode("#code#") + "=" + urlencode("1234") + "&amp;" +urlencode("#company#") + "=" + urlencode("云片网");   
        /// 直接发送报文请求则使用下面的形式  
        /// tplValue=urlencode(urlencode("#code#") + "=" + urlencode("1234") +"&amp;" + urlencode("#company#") + "=" + urlencode("云片网"));  
        /// </param>
        /// <param name="mobile">发送手机号</param>
        /// <param name="charset">编码格式</param>
        /// <returns>返回短信发送状态信息</returns>
        public async Task<Result<SmsSingleSend>> TplSingleSendAsync (string tplId, string tplValue, string mobile, string charset = null) {
            var data = new Dictionary<string, string> ();

            data.Add (YunPianFields.Mobile, mobile);
            data.Add (YunPianFields.TplId, tplId);
            data.Add (YunPianFields.TplValue, tplValue);

            // 设置对Result<SmsSingleSend>进行处理的方法
            var resultHandler = new MapResultHandler<SmsSingleSend> (Options.Version,
                response => {
                    return Options.Version == YunPianFields.VersionV2 ? response.ToObject<SmsSingleSend> () : null;
                });

            return await PostAsync (data, resultHandler, Options.TplSingleSendSms, charset);
        }

        /// <summary>
        /// 指定模板发送 V2
        /// </summary>
        /// <param name="tplId">模板ID</param>
        /// <param name="tplValue">模板内容 String
        /// 注：变量名和变量值都不能为空  
        /// 模板:【#company#】您的验证码是#code#。  
        /// 最终发送结果： 【云片网】您的验证码是1234。  
        /// tplValue=urlencode("#code#") + "=" + urlencode("1234") + "&amp;" +urlencode("#company#") + "=" + urlencode("云片网");  
        /// 直接发送报文请求则使用下面的形式  
        /// tplValue=urlencode(urlencode("#code#") + "=" + urlencode("1234") +"&amp;" + urlencode("#company#") + "=" + urlencode("云片网"));  
        /// </param>
        /// <param name="mobile">发送手机号  
        /// 注:针对国际短信, mobile 参数会自动格式化到E.164格式  
        /// 可能会造成传入 mobile 参数跟后续的状态报告中的号码不一致。  
        /// E.164格式说明，参见：https://en.wikipedia.org/wiki/E.164  
        /// </param>
        /// <param name="charset">编码格式</param>
        /// <returns>返回短信发送状态信息</returns>
        public async Task<Result<SmsBatchSend>> TplBatchSendAsync (string tplId, string tplValue, string mobile, string charset = null) {
            var data = new Dictionary<string, string> ();

            data.Add (YunPianFields.Mobile, mobile);
            data.Add (YunPianFields.TplId, tplId);
            data.Add (YunPianFields.TplValue, tplValue);

            var resultHandler = new MapResultHandler<SmsBatchSend> (Options.Version,
                response => {
                    return Options.Version == YunPianFields.VersionV2 ? response.ToObject<SmsBatchSend> () : null;
                });

            return await PostAsync (data, resultHandler, Options.TplBatchSendSms, charset);
        }

        /// <summary>
        /// 注册成功回调 V2
        /// </summary>
        /// <param name="mobile">手机号
        /// 注册成功的手机号，请和调用接口的手机号一致
        /// </param>
        /// <param name="time">注册成功的时间,格式: yyyy-MM-dd HH:mm:ss    
        /// 可以是一天前，超过时间无法记录，默认当前时间  
        /// </param>
        /// <returns></returns>
        public async Task<Result<object>> RegisterSucceedCallBack (string mobile, DateTime? time) {
            var data = new Dictionary<string, string> ();

            data.Add (YunPianFields.Mobile, mobile);
            if (time.HasValue)
                data.Add (YunPianFields.Time, time.Value.ToString ("yyyy-MM-dd HH:mm:ss"));

            var resultHandler = new MapResultHandler<object> (Options.Version, rsp => null);
            return await PostAsync (data, resultHandler, Options.RegisterSucceedCallBack);
        }
    }
}
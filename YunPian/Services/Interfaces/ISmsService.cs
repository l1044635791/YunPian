using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YunPian.Models;

namespace YunPian.Services {
    public interface ISmsService {
        /// <summary>
        /// 发送短信(单条)
        /// </summary>
        /// <param name="text">发送的文本内容</param>
        /// <param name="mobile">手机号 示例：单号码：13100000001</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        Task<Result<SmsSingleSend>> SingleSendAsync (string text, string mobile, string charset = null);

        /// <summary>
        /// 发送短信(批量)
        /// </summary>
        /// <param name="text">发送的文本内容</param>
        /// <param name="mobile">手机号(批量) 中间以逗号（‘,’）隔开，一次不要超过1000个 示例：单号码：13100000001 /多号码：13100000001,13100000002</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        Task<Result<SmsBatchSend>> BatchSendAsync (string text, string mobile, string charset = null);

        /// <summary>
        /// 发送短信(个性化发送)
        /// </summary>
        /// <param name="text">发送的文本内容 , 多条文本中间以逗号（‘,’）隔开，文本数量和手机号数量保持一致</param>
        /// <param name="mobile">手机号(批量) 中间以逗号（‘,’）隔开，一次不要超过1000个 示例：单号码：13100000001 /多号码：13100000001,13100000002</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        Task<Result<SmsBatchSend>> MultiSendAsync (string text, string mobile, string charset = null);

        /// <summary>
        /// 发送短信V1版本(个性化发送)
        /// </summary>
        /// <param name="text">发送的文本内容,文本数量和手机号数量保持一致</param>
        /// <param name="mobile">手机号(批量) 中间以逗号（‘,’）隔开，一次不要超过1000个 示例：单号码：13100000001 /多号码：13100000001,13100000002</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        Task<Result<List<SmsSingleSend>>> MultiSendAsync_V1 (string mobile, string charset = null, params string[] text);
        /// <summary>
        /// 获取短信发送状态
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量(默认20，最大数量100)</param>
        /// <returns></returns>
        Task<Result<List<SmsStatus>>> PullSmsStatusAsync (int pageIndex, int pageSize);

        /// <summary>
        /// 获取回复短信信息
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量(默认20，最大数量100)</param>
        /// <returns></returns>
        Task<Result<List<SmsReply>>> PullSmsReplyAsync (int pageIndex, int pageSize);

        /// <summary>
        /// 查看回复的短信
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量(默认20，最大数量100)</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="mobile">手机号</param>
        /// <returns></returns>
        Task<Result<List<SmsReply>>> GetSmsReplyAsync (int pageIndex, int pageSize, DateTime? start, DateTime? end, string mobile);

        /// <summary>
        /// 查看文本屏蔽词
        /// </summary>
        /// <param name="text">文本内容</param>
        /// <returns>返回文本屏蔽词</returns>
        Task<Result<List<string>>> GetBlackWordsAsync (string text);

        /// <summary>
        /// 查看短信记录
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量(默认20，最大数量100)</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="mobile">手机号</param>
        /// <returns>返回短信记录</returns>
        Task<Result<List<SmsRecord>>> GetRecordAsync (int pageIndex, int pageSize, DateTime? start, DateTime? end, string mobile);

        /// <summary>
        /// 统计短信发送条数
        /// </summary>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="mobile">手机号</param>
        /// <returns>返回短信发送条数</returns>
        Task<Result<int>> GetSmsCountAsync (DateTime? start, DateTime? end, string mobile);

        /// <summary>
        /// 指定模板发送 V1
        /// </summary>
        /// <param name="tplId">模板ID</param>
        /// <param name="tplValue">模板内容 String
        /// 注：变量名和变量值都不能为空  
        /// 模板:【#company#】您的验证码是#code#。   
        /// 最终发送结果： 【云片网】您的验证码是1234。  
        /// tplValue=urlencode("#code#") + "=" + urlencode("1234") + "&amp;" +urlencode("#company#") + "=" + urlencode("云片网");   
        /// 直接发送报文请求则使用下面的形式:  
        /// tplValue=urlencode(urlencode("#code#") + "=" + urlencode("1234") +"&amp;" + urlencode("#company#") + "=" + urlencode("云片网"));  
        /// </param>
        /// <param name="mobile">发送手机号</param>
        /// <param name="charset">编码格式</param>
        /// <returns>返回短信发送状态信息</returns>
        Task<Result<SmsSingleSend>> TplSingleSendAsync_V1 (string tplId, string tplValue, string mobile, string charset = null);

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
        /// <param name="mobile">发送手机号</param>
        /// <param name="charset">编码格式</param>
        /// <returns>返回短信发送状态信息</returns>
        Task<Result<SmsSingleSend>> TplSingleSendAsync (string tplId, string tplValue, string mobile, string charset = null);

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
        Task<Result<SmsBatchSend>> TplBatchSendAsync (string tplId, string tplValue, string mobile, string charset = null);

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
        Task<Result<object>> RegisterSucceedCallBack (string mobile, DateTime? time);
    }
}
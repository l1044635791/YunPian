using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using YunPian.Handlers;
using YunPian.Models;

namespace YunPian.Services {
    public class VoiceService : BaseService, IVoiceService {
        public VoiceService (IHttpClientFactory httpClientFactory, IOptions<YunPianOptions> options) : base (options) {
            _httpClient = httpClientFactory.CreateClient (nameof (YunPianOptions._voiceHost));
        }

        /// <summary>
        /// 发送语音验证码
        /// </summary>
        /// <param name="code">验证码，支持4~6位阿拉伯数字</param>
        /// <param name="mobile">接收的手机号、固话（需加区号） 13140000001 01088880000</param>
        /// <param name="callback_url">本条语音验证码状态报告推送地址 http://your_receive_url_address</param>
        /// <returns></returns>
        public async Task<Result<VoiceSend>> VoiceSendAsync (string code, string mobile, string callback_url = null) {
            Params.Add (YunPianFields.Mobile, mobile);
            Params.Add (YunPianFields.Code, code);
            if (!string.IsNullOrWhiteSpace (callback_url))
                Params.Add (YunPianFields.CallbackUrl, callback_url);

            Uri = Options.VoiceSend;

            // 设置对Result<VoiceSend>进行处理的方法
            var resultHandler = new MapResultHandler<VoiceSend> (Options.Version, response => {
                switch (Options.Version) {
                    case YunPianFields.VersionV1:
                        {
                            return response[YunPianFields.Result].ToObject<VoiceSend> ();
                        }
                    case YunPianFields.VersionV2:
                        {
                            return response.ToObject<VoiceSend> ();
                        }
                    default:
                        return null;
                }
            });

            return await PostAsync (resultHandler);
        }

        /// <summary>
        /// 发送语音通知
        /// </summary>
        /// <param name="tpl_id">审核通过的模版ID</param>
        /// <param name="tpl_value">模版的变量值
        /// 示例:模版内容&quot;课程#name#在#time#开始&quot;,  
        /// 设置值为&quot;name=计算机&amp;time=17点&quot;,  
        /// 注:若出现特殊字符(例如&#39;=&#39;,&#39;&amp;&#39;),需要URLEncode
        /// </param>
        /// <param name="mobile">接收的手机号、固话（需加区号） 13140000001 01088880000</param>
        /// <returns></returns>
        public async Task<Result<VoiceSend>> TplNotifySendAsync (string tpl_id, string tpl_value, string mobile) {
            Params.Add (YunPianFields.Mobile, mobile);
            Params.Add (YunPianFields.TplId, tpl_id);
            Params.Add (YunPianFields.TplValue, tpl_value);

            Uri = Options.TplNotifyVoiceSend;

            // 设置对Result<VoiceSend>进行处理的方法
            var resultHandler = new MapResultHandler<VoiceSend> (Options.Version, response => {
                return Options.Version == YunPianFields.VersionV2 ? response.ToObject<VoiceSend> () : null;
            });

            return await PostAsync (resultHandler);
        }

        /// <summary>
        /// 获取语音状态报告
        /// </summary>
        /// <param name="type">状态类型，1语音验证码 2语音通知，默认为1</param>
        /// <param name="page_index">页码</param>
        /// <param name="page_size">每页个数，最大100个，默认20个</param>
        /// <returns></returns>
        public async Task<Result<List<VoiceStatus>>> PullVoiceStatusAsync (int type, int page_index, int page_size) {
            Params.Add (YunPianFields.Type, type.ToString ());
            Params.Add (YunPianFields.PageNum, page_index.ToString ());
            Params.Add (YunPianFields.PageSize, page_size.ToString ());

            Uri = Options.TplNotifyVoiceSend;

            // 设置对Result<VoiceStatus>进行处理的方法
            var resultHandler = new ListMapResultHandler<VoiceStatus> (Options.Version, response => {
                switch (Options.Version) {
                    case YunPianFields.VersionV1:
                        {
                            return response is JObject jObject ?
                                jObject[YunPianFields.VoiceStatus].ToObject<List<VoiceStatus>> () : new List<VoiceStatus> ();
                        }
                    case YunPianFields.VersionV2:
                        {
                            return response.ToObject<List<VoiceStatus>> ();
                        }
                    default:
                        return new List<VoiceStatus> ();
                }
            });

            return await PostAsync (resultHandler);
        }
    }
}
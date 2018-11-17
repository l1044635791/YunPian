using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using YunPian.Handlers;
using YunPian.Models;

namespace YunPian.Services {
    public class TempletService : BaseService, ITempletService {
        public TempletService (IHttpClientFactory httpClientFactory, IOptions<YunPianOptions> options) : base (options) {
            _httpClient = httpClientFactory.CreateClient (nameof (YunPianOptions._smsHost));
        }

        /// <summary>
        /// 获取默认模板
        /// </summary>
        /// <param name="tplId">模板ID 为空返回所有模板信息</param>
        /// <returns></returns>
        public async Task<Result<List<Template>>> GetDefaultTempletAsync (string tplId) {
            var data = new Dictionary<string, string> ();

            if (!string.IsNullOrWhiteSpace (tplId))
                data.Add (YunPianFields.TplId, tplId);

            var resultHandler = new SimpleListResultHandler<Template> (Options.Version);
            return await PostAsync (data, resultHandler, Options.GetDefaultTemplet);

        }

        /// <summary>
        /// 获取模板
        /// </summary>
        /// <param name="tplId">模板ID 为空返回所有模板信息</param>
        /// <returns></returns>
        public async Task<Result<List<Template>>> GetTempletAsync (string tplId) {
            var data = new Dictionary<string, string> ();

            if (!string.IsNullOrWhiteSpace (tplId))
                data.Add (YunPianFields.TplId, tplId);

            var resultHandler = new ListMapResultHandler<Template> (Options.Version, response => {
                if (response != null) {
                    switch (Options.Version) {
                        case YunPianFields.VersionV1:
                            {
                                if (response is JObject jObject) {
                                    var tpl = jObject[YunPianFields.Template];
                                    return tpl.GetType ().IsArray ?
                                        tpl.ToObject<List<Template>> () :
                                        new List<Template> { tpl.ToObject<Template> () };
                                }
                                break;
                            }
                        case YunPianFields.VersionV2:
                            return new List<Template> { response.ToObject<Template> () };
                        default:
                            return new List<Template> ();
                    }
                }
                return new List<Template> ();
            });

            return await PostAsync (data, resultHandler, Options.GetTemplet);
        }

        /// <summary>
        /// 添加模板
        /// </summary>
        /// <param name="tplContent">模板内容，必须以带符号【】的签名开头 示例:【云片网】您的验证码是#code#</param>
        /// <param name="notifyType">审核结果短信通知的方式: 0表示需要通知(默认); 1表示仅审核不通过时通知; 2表示仅审核通过时通知; 3表示不需要通知</param>
        /// <param name="lang">模板语言:简体中文zh_cn; 英文en; 繁体中文 zh_tw; 韩文ko,日文 ja</param>
        /// <returns></returns>
        public async Task<Result<Template>> AddTempletAsync (string tplContent, string notifyType = null, string lang = null) {
            var data = new Dictionary<string, string> ();

            data.Add (YunPianFields.TplContent, tplContent);
            if (!string.IsNullOrWhiteSpace (notifyType))
                data.Add (YunPianFields.NotifyType, notifyType);
            if (!string.IsNullOrWhiteSpace (lang))
                data.Add (YunPianFields.Lang, lang);

            var resultHandler = new MapResultHandler<Template> (Options.Version, response => {
                switch (Options.Version) {
                    case YunPianFields.VersionV1:
                        return response[YunPianFields.Template].ToObject<Template> ();
                    case YunPianFields.VersionV2:
                        return response.ToObject<Template> ();
                    default:
                        return null;
                }
            });
            return await PostAsync (data, resultHandler, Options.AddTemplet);
        }

        /// <summary>
        /// 删除模板
        /// </summary>
        /// <param name="tplId">模板ID</param>
        /// <returns></returns>
        public async Task<Result<Template>> DeleteTempletAsync (string tplId) {
            var data = new Dictionary<string, string> ();

            data.Add (YunPianFields.TplId, tplId);

            var resultHandler = new MapResultHandler<Template> (Options.Version, response => {
                return Options.Version == YunPianFields.VersionV2 ? response.ToObject<Template> () : null;
            });

            return await PostAsync (data, resultHandler, Options.DeleteTemplet);
        }

        /// <summary>
        /// 修改模板信息
        /// </summary>
        /// <param name="tplId">模板ID</param>
        /// <param name="tplContent">必须以带符号【】的签名开头. 示例:【云片网】您的验证码是#code#</param>
        /// <returns></returns>
        public async Task<Result<Template>> UpdateTempletAsync (string tplId, string tplContent) {
            var data = new Dictionary<string, string> ();

            data.Add (YunPianFields.TplId, tplId);
            data.Add (YunPianFields.TplContent, tplContent);

            var resultHandler = new MapResultHandler<Template> (Options.Version, response => {
                switch (Options.Version) {
                    case YunPianFields.VersionV1:
                        return response[YunPianFields.Template]?.ToObject<Template> ();
                    case YunPianFields.VersionV2:
                        return response[YunPianFields.Template] != null ? response[YunPianFields.Template].ToObject<Template> () : response.ToObject<Template> ();
                    default:
                        return null;
                }
            });

            return await PostAsync (data, resultHandler, Options.UpdateTemplet);
        }

        /// <summary>
        /// 添加语音通知模板
        /// </summary>
        /// <param name="tplContent">模板内容，必须以带符号【】的签名开头 示例:【云片网】您的验证码是#code#</param>
        /// <param name="notifyType">审核结果短信通知的方式: 0表示需要通知(默认); 1表示仅审核不通过时通知; 2表示仅审核通过时通知; 3表示不需要通知</param>
        public async Task<Result<Template>> AddVoiceNotifyTempletAsync (string tplContent, string notifyType = null) {
            var data = new Dictionary<string, string> ();

            data.Add (YunPianFields.TplContent, tplContent);
            if (!string.IsNullOrWhiteSpace (notifyType))
                data.Add (YunPianFields.NotifyType, notifyType);

            var resultHandler = new MapResultHandler<Template> (Options.Version, response => {
                return Options.Version == YunPianFields.VersionV2 ? response.ToObject<Template> () : null;
            });

            return await PostAsync (data, resultHandler, Options.AddVoiceNotifyTemplet);
        }

        /// <summary>
        /// 更新语音短信模板
        /// </summary>
        /// <param name="tplId">模板ID</param>
        /// <param name="tplContent">模板内容，必须以带符号【】的签名开头 示例:【云片网】您的验证码是#code#</param>
        /// <returns></returns>
        public async Task<Result<Template>> UpdateVoiceNotifyTempletAsync (string tplId, string tplContent) {
            var data = new Dictionary<string, string> ();

            data.Add (YunPianFields.TplId, tplId);
            data.Add (YunPianFields.TplContent, tplContent);

            var resultHandler = new MapResultHandler<Template> (Options.Version, response => {
                return Options.Version == YunPianFields.VersionV2 ? response[YunPianFields.Template] != null ?
                    response[YunPianFields.Template].ToObject<Template> () : response.ToObject<Template> () : null;
            });

            return await PostAsync (data, resultHandler, Options.UpdateVoiceNotifyTemplet);
        }
    }
}
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
        /// <param name="tpl_id">模板ID 为空返回所有模板信息</param>
        /// <returns></returns>
        public async Task<Result<List<Template>>> GetDefaultTempletAsync (string tpl_id) {
            if (!string.IsNullOrWhiteSpace (tpl_id))
                Params.Add (YunPianFields.TplId, tpl_id);

            Uri = Options.GetDefaultTemplet;

            var resultHandler = new SimpleListResultHandler<Template> (Options.Version);
            return await PostAsync (resultHandler);

        }

        /// <summary>
        /// 获取模板
        /// </summary>
        /// <param name="tpl_id">模板ID 为空返回所有模板信息</param>
        /// <returns></returns>
        public async Task<Result<List<Template>>> GetTempletAsync (string tpl_id) {
            if (!string.IsNullOrWhiteSpace (tpl_id))
                Params.Add (YunPianFields.TplId, tpl_id);

            Uri = Options.GetTemplet;            

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

            return await PostAsync (resultHandler);
        }

        /// <summary>
        /// 添加模板
        /// </summary>
        /// <param name="tpl_content">模板内容，必须以带符号【】的签名开头 示例:【云片网】您的验证码是#code#</param>
        /// <param name="notify_type">审核结果短信通知的方式: 0表示需要通知(默认); 1表示仅审核不通过时通知; 2表示仅审核通过时通知; 3表示不需要通知</param>
        /// <param name="lang">模板语言:简体中文zh_cn; 英文en; 繁体中文 zh_tw; 韩文ko,日文 ja</param>
        /// <returns></returns>
        public async Task<Result<Template>> AddTempletAsync (string tpl_content, string notify_type = null, string lang = null) {
            Params.Add (YunPianFields.TplContent, tpl_content);
            if (!string.IsNullOrWhiteSpace (notify_type))
                Params.Add (YunPianFields.NotifyType, notify_type);
            if (!string.IsNullOrWhiteSpace (lang))
                Params.Add (YunPianFields.Lang, lang);

            Uri = Options.AddTemplet;

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
            return await PostAsync (resultHandler);
        }

        /// <summary>
        /// 删除模板
        /// </summary>
        /// <param name="tpl_id">模板ID</param>
        /// <returns></returns>
        public async Task<Result<Template>> DeleteTempletAsync (string tpl_id) {
            Params.Add (YunPianFields.TplId, tpl_id);

            Uri = Options.DeleteTemplet;

            var resultHandler = new MapResultHandler<Template> (Options.Version, response => {
                return Options.Version == YunPianFields.VersionV2 ? response.ToObject<Template> () : null;
            });

            return await PostAsync (resultHandler);
        }

        /// <summary>
        /// 修改模板信息
        /// </summary>
        /// <param name="tpl_id">模板ID</param>
        /// <param name="tpl_content">必须以带符号【】的签名开头. 示例:【云片网】您的验证码是#code#</param>
        /// <returns></returns>
        public async Task<Result<Template>> UpdateTempletAsync (string tpl_id, string tpl_content) {
            Params.Add (YunPianFields.TplId, tpl_id);
            Params.Add (YunPianFields.TplContent, tpl_content);

            Uri = Options.UpdateTemplet;

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

            return await PostAsync (resultHandler);
        }

        /// <summary>
        /// 添加语音通知模板
        /// </summary>
        /// <param name="tpl_content">模板内容，必须以带符号【】的签名开头 示例:【云片网】您的验证码是#code#</param>
        /// <param name="notify_type">审核结果短信通知的方式: 0表示需要通知(默认); 1表示仅审核不通过时通知; 2表示仅审核通过时通知; 3表示不需要通知</param>
        public async Task<Result<Template>> AddVoiceNotifyTempletAsync (string tpl_content, string notify_type = null) {
            Params.Add (YunPianFields.TplContent, tpl_content);
            if (!string.IsNullOrWhiteSpace (notify_type))
                Params.Add (YunPianFields.NotifyType, notify_type);

            Uri = Options.AddVoiceNotifyTemplet;

            var resultHandler = new MapResultHandler<Template> (Options.Version, response => {
                return Options.Version == YunPianFields.VersionV2 ? response.ToObject<Template> () : null;
            });

            return await PostAsync (resultHandler);
        }

        /// <summary>
        /// 更新语音短信模板
        /// </summary>
        /// <param name="tpl_id">模板ID</param>
        /// <param name="tpl_content">模板内容，必须以带符号【】的签名开头 示例:【云片网】您的验证码是#code#</param>
        /// <returns></returns>
        public async Task<Result<Template>> UpdateVoiceNotifyTempletAsync (string tpl_id, string tpl_content) {
            Params.Add (YunPianFields.TplId, tpl_id);
            Params.Add (YunPianFields.TplContent, tpl_content);

            Uri = Options.UpdateVoiceNotifyTemplet;

            var resultHandler = new MapResultHandler<Template> (Options.Version, response => {
                return Options.Version == YunPianFields.VersionV2 ? response[YunPianFields.Template] != null ?
                    response[YunPianFields.Template].ToObject<Template> () : response.ToObject<Template> () : null;
            });

            return await PostAsync (resultHandler);
        }
    }
}
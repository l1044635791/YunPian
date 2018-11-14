using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using YunPian.Handlers;
using YunPian.Models;

namespace YunPian.Services {
    public class VideoService : BaseService, IVideoService {
        public VideoService (IHttpClientFactory httpClientFactory, IOptions<YunPianOptions> options) : base (options) {
            _httpClient = httpClientFactory.CreateClient (nameof (YunPianOptions._vsmsHost));
        }

        /// <summary>
        /// 添加视频短信模板
        /// </summary>
        /// <param name="param">需要包含_sign字段</param>
        /// <param name="layout"></param>
        /// <param name="material"></param>
        /// <returns></returns>
        public async Task<Result<Template>> AddVideoTemplateAsync (Dictionary<string, string> param, string layout, byte[] material) {
            var data = new MultipartFormDataContent ();
            foreach (var kv in param) {
                data.Add (new StringContent (kv.Value, Encoding.GetEncoding (Charset), "text/plain"), kv.Key);
            }

            data.Add (new StringContent (layout, Encoding.GetEncoding (Charset),
                "application/x-www-form-urlencoded"), YunPianFields.Layout);

            var httpContent = new ByteArrayContent (material);
            httpContent.Headers.Add ("Content-Type", $"application/octet-stream;charset={Charset}");
            data.Add (httpContent, YunPianFields.Material);

            Uri = Options.AddVideoTemplet;

            var resultHandler = new StdResultHandler<Template> (Options.Version);

            return await PostAsync (data, resultHandler);
        }

        /// <summary>
        /// 获取视频短信模板状态
        /// </summary>
        /// <param name="tpl_id">模板ID</param>
        /// <returns></returns>
        public async Task<Result<Template>> GetVideoTemplateAsync (string tpl_id) {
            Params.Add (YunPianFields.TplId, tpl_id);

            Uri = Options.GetVideoTemplet;

            var resultHandler = new StdResultHandler<Template> (Options.Version);

            return await PostAsync (resultHandler);
        }

        /// <summary>
        /// 批量发送视频短信
        /// </summary>
        /// <param name="tpl_id">模板ID</param>
        /// <param name="mobile">接收的手机号、固话（需加区号） 13140000001 01088880000</param>
        /// <returns></returns>
        public async Task<Result<SmsBatchSend>> TplBatchSendAsync (string tpl_id, string mobile) {
            Params.Add (YunPianFields.TplId, tpl_id);
            Params.Add (YunPianFields.Mobile, mobile);
            
            Uri = Options.TplBatchVideoSend;

            var resultHandler = new MapResultHandler<SmsBatchSend> (Options.Version, response => {
                return Options.Version == YunPianFields.VersionV2?response.ToObject<SmsBatchSend> () : null;
            });

            return await PostAsync (resultHandler);
        }
    }
}
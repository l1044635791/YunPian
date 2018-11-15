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
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public async Task<Result<Template>> AddVideoTemplateAsync (Dictionary<string, string> param, string layout, byte[] material, string charset = null) {
            var data = new MultipartFormDataContent ();
            foreach (var kv in param) {
                data.Add (new StringContent (kv.Value, Encoding.GetEncoding (charset??Options.Charset), "text/plain"), kv.Key);
            }

            data.Add (new StringContent (layout, Encoding.GetEncoding (charset??Options.Charset),
                "application/x-www-form-urlencoded"), YunPianFields.Layout);

            var httpContent = new ByteArrayContent (material);
            httpContent.Headers.Add ("Content-Type", $"application/octet-stream;charset={charset??Options.Charset}");

            data.Add (httpContent, YunPianFields.Material);

            var resultHandler = new StdResultHandler<Template> (Options.Version);

            return await PostAsync (data, resultHandler, Options.AddVideoTemplet);
        }

        /// <summary>
        /// 获取视频短信模板状态
        /// </summary>
        /// <param name="tpl_id">模板ID</param>
        /// <returns></returns>
        public async Task<Result<Template>> GetVideoTemplateAsync (string tpl_id) {
            var data = new Dictionary<string, string> ();

            data.Add (YunPianFields.TplId, tpl_id);

            var resultHandler = new StdResultHandler<Template> (Options.Version);

            return await PostAsync (data, resultHandler, Options.GetVideoTemplet);
        }

        /// <summary>
        /// 批量发送视频短信
        /// </summary>
        /// <param name="tpl_id">模板ID</param>
        /// <param name="mobile">接收的手机号、固话（需加区号） 13140000001 01088880000</param>
        /// <returns></returns>
        public async Task<Result<SmsBatchSend>> TplBatchSendAsync (string tpl_id, string mobile) {
            var data = new Dictionary<string, string> ();

            data.Add (YunPianFields.TplId, tpl_id);
            data.Add (YunPianFields.Mobile, mobile);

            var resultHandler = new MapResultHandler<SmsBatchSend> (Options.Version, response => {
                return Options.Version == YunPianFields.VersionV2?response.ToObject<SmsBatchSend> () : null;
            });

            return await PostAsync (data, resultHandler, Options.TplBatchVideoSend);
        }
    }
}
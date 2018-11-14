using System.Collections.Generic;
using System.Threading.Tasks;
using YunPian.Models;

namespace YunPian.Services {
    public interface IVideoService {
        /// <summary>
        /// 添加视频短信模板
        /// </summary>
        /// <param name="param">需要包含_sign字段</param>
        /// <param name="layout"></param>
        /// <param name="material"></param>
        /// <returns></returns>
        Task<Result<Template>> AddVideoTemplateAsync (Dictionary<string, string> param, string layout, byte[] material);

        /// <summary>
        /// 获取视频短信模板状态
        /// </summary>
        /// <param name="tpl_id">模板ID</param>
        /// <returns></returns>
        Task<Result<Template>> GetVideoTemplateAsync (string tpl_id);

        /// <summary>
        /// 批量发送视频短信
        /// </summary>
        /// <param name="tpl_id">模板ID</param>
        /// <param name="mobile">接收的手机号、固话（需加区号） 13140000001 01088880000</param>
        /// <returns></returns>
        Task<Result<SmsBatchSend>> TplBatchSendAsync (string tpl_id, string mobile);
    }
}
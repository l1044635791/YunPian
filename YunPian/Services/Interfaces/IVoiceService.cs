using System.Collections.Generic;
using System.Threading.Tasks;
using YunPian.Models;

namespace YunPian.Services {
    public interface IVoiceService {
        /// <summary>
        /// 发送语音验证码
        /// </summary>
        /// <param name="code">验证码，支持4~6位阿拉伯数字</param>
        /// <param name="mobile">接收的手机号、固话（需加区号） 13140000001 01088880000</param>
        /// <param name="callback_url">本条语音验证码状态报告推送地址 http://your_receive_url_address</param>
        /// <returns></returns>
        Task<Result<VoiceSend>> VoiceSendAsync (string code, string mobile, string callback_url = null);

        /// <summary>
        /// 发送语音通知
        /// </summary>
        /// <param name="tplId">审核通过的模版ID</param>
        /// <param name="tplValue">模版的变量值
        /// 示例:模版内容&quot;课程#name#在#time#开始&quot;,  
        /// 设置值为&quot;name=计算机&amp;time=17点&quot;,  
        /// 注:若出现特殊字符(例如&#39;=&#39;,&#39;&amp;&#39;),需要URLEncode
        /// </param>
        /// <param name="mobile">接收的手机号、固话（需加区号） 13140000001 01088880000</param>
        /// <returns></returns>
        Task<Result<VoiceSend>> TplNotifySendAsync (string tplId, string tplValue, string mobile);

        /// <summary>
        /// 获取语音状态报告
        /// </summary>
        /// <param name="type">状态类型，1语音验证码 2语音通知，默认为1</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页个数，最大100个，默认20个</param>
        /// <returns></returns>
        Task<Result<List<VoiceStatus>>> PullVoiceStatusAsync (int type, int pageIndex, int pageSize);
    }
}
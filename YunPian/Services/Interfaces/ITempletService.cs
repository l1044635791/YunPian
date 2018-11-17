using System.Collections.Generic;
using System.Threading.Tasks;
using YunPian.Models;

namespace YunPian.Services {
    public interface ITempletService {
        /// <summary>
        /// 获取默认模板
        /// </summary>
        /// <param name="tplId">模板ID 为空返回所有模板信息</param>
        /// <returns></returns>
        Task<Result<List<Template>>> GetDefaultTempletAsync (string tplId);

        /// <summary>
        /// 获取模板
        /// </summary>
        /// <param name="tplId">模板ID 为空返回所有模板信息</param>
        /// <returns></returns>
        Task<Result<List<Template>>> GetTempletAsync (string tplId);

        /// <summary>
        /// 添加模板
        /// </summary>
        /// <param name="tplContent">模板内容，必须以带符号【】的签名开头 示例:【云片网】您的验证码是#code#</param>
        /// <param name="notifyType">审核结果短信通知的方式: 0表示需要通知(默认); 1表示仅审核不通过时通知; 2表示仅审核通过时通知; 3表示不需要通知</param>
        /// <param name="lang">模板语言:简体中文zh_cn; 英文en; 繁体中文 zh_tw; 韩文ko,日文 ja</param>
        /// <returns></returns>
        Task<Result<Template>> AddTempletAsync (string tplContent, string notifyType = null, string lang = null);

        /// <summary>
        /// 删除模板
        /// </summary>
        /// <param name="tplId">模板ID</param>
        /// <returns></returns>
        Task<Result<Template>> DeleteTempletAsync (string tplId);

        /// <summary>
        /// 修改模板信息
        /// </summary>
        /// <param name="tplId">模板ID</param>
        /// <param name="tplContent">必须以带符号【】的签名开头. 示例:【云片网】您的验证码是#code#</param>
        /// <returns></returns>
        Task<Result<Template>> UpdateTempletAsync (string tplId, string tplContent);

        /// <summary>
        /// 添加语音通知模板
        /// </summary>
        /// <param name="tplContent">模板内容，必须以带符号【】的签名开头 示例:【云片网】您的验证码是#code#</param>
        /// <param name="notifyType">审核结果短信通知的方式: 0表示需要通知(默认); 1表示仅审核不通过时通知; 2表示仅审核通过时通知; 3表示不需要通知</param>
        Task<Result<Template>> AddVoiceNotifyTempletAsync (string tplContent, string notifyType = null);

        /// <summary>
        /// 更新语音短信模板
        /// </summary>
        /// <param name="tplId">模板ID</param>
        /// <param name="tplContent">模板内容，必须以带符号【】的签名开头 示例:【云片网】您的验证码是#code#</param>
        /// <returns></returns>
        Task<Result<Template>> UpdateVoiceNotifyTempletAsync (string tplId, string tplContent);
    }
}
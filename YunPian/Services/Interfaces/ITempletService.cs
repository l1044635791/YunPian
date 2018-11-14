using System.Collections.Generic;
using System.Threading.Tasks;
using YunPian.Models;

namespace YunPian.Services {
    public interface ITempletService {
        /// <summary>
        /// 获取默认模板
        /// </summary>
        /// <param name="tpl_id">模板ID 为空返回所有模板信息</param>
        /// <returns></returns>
        Task<Result<List<Template>>> GetDefaultTempletAsync (string tpl_id);

        /// <summary>
        /// 获取模板
        /// </summary>
        /// <param name="tpl_id">模板ID 为空返回所有模板信息</param>
        /// <returns></returns>
        Task<Result<List<Template>>> GetTempletAsync (string tpl_id);

        /// <summary>
        /// 添加模板
        /// </summary>
        /// <param name="tpl_content">模板内容，必须以带符号【】的签名开头 示例:【云片网】您的验证码是#code#</param>
        /// <param name="notify_type">审核结果短信通知的方式: 0表示需要通知(默认); 1表示仅审核不通过时通知; 2表示仅审核通过时通知; 3表示不需要通知</param>
        /// <param name="lang">模板语言:简体中文zh_cn; 英文en; 繁体中文 zh_tw; 韩文ko,日文 ja</param>
        /// <returns></returns>
        Task<Result<Template>> AddTempletAsync (string tpl_content, string notify_type = null, string lang = null);

        /// <summary>
        /// 删除模板
        /// </summary>
        /// <param name="tpl_id">模板ID</param>
        /// <returns></returns>
        Task<Result<Template>> DeleteTempletAsync (string tpl_id);

        /// <summary>
        /// 修改模板信息
        /// </summary>
        /// <param name="tpl_id">模板ID</param>
        /// <param name="tpl_content">必须以带符号【】的签名开头. 示例:【云片网】您的验证码是#code#</param>
        /// <returns></returns>
        Task<Result<Template>> UpdateTempletAsync (string tpl_id, string tpl_content);

        /// <summary>
        /// 添加语音通知模板
        /// </summary>
        /// <param name="tpl_content">模板内容，必须以带符号【】的签名开头 示例:【云片网】您的验证码是#code#</param>
        /// <param name="notify_type">审核结果短信通知的方式: 0表示需要通知(默认); 1表示仅审核不通过时通知; 2表示仅审核通过时通知; 3表示不需要通知</param>
        Task<Result<Template>> AddVoiceNotifyTempletAsync (string tpl_content, string notify_type = null);

        /// <summary>
        /// 更新语音短信模板
        /// </summary>
        /// <param name="tpl_id">模板ID</param>
        /// <param name="tpl_content">模板内容，必须以带符号【】的签名开头 示例:【云片网】您的验证码是#code#</param>
        /// <returns></returns>
        Task<Result<Template>> UpdateVoiceNotifyTempletAsync (string tpl_id, string tpl_content);
    }
}
using System.Threading.Tasks;
using YunPian.Models;

namespace YunPian.Services {
    public interface IUserService {
        /// <summary>
        /// 查询账户信息
        /// </summary>
        /// <returns></returns>
        Task<Result<User>> GetUserAsync ();

        /// <summary>
        /// 设置账户信息
        /// </summary>
        /// <param name="emergencyContact">紧急联系人姓名</param>
        /// <param name="emergencyMobile">紧急联系人手机号</param>
        /// <param name="alarmBalance">短信余额提醒阈值</param>
        /// <returns></returns>
        Task<Result<User>> SetUserAsync (string emergencyContact = null, string emergencyMobile = null, string alarmBalance = null);
    }
}
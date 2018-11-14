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
        /// <param name="emergency_contact">紧急联系人姓名</param>
        /// <param name="emergency_mobile">紧急联系人手机号</param>
        /// <param name="alarm_balance">短信余额提醒阈值</param>
        /// <returns></returns>
        Task<Result<User>> SetUserAsync (string emergency_contact = null, string emergency_mobile = null, string alarm_balance = null);
    }
}
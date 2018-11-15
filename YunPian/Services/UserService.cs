using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using YunPian.Handlers;
using YunPian.Models;

namespace YunPian.Services {
    public class UserService : BaseService, IUserService {
        public UserService (IHttpClientFactory httpClientFactory, IOptions<YunPianOptions> options) : base (options) {
            _httpClient = httpClientFactory.CreateClient (nameof (YunPianOptions._smsHost));
        }

        /// <summary>
        /// 查询账户信息
        /// </summary>
        /// <returns></returns>
        public async Task<Result<User>> GetUserAsync () {

            var resultHandler = new MapResultHandler<User> (Options.Version, response => {
                switch (Options.Version) {
                    case YunPianFields.VersionV1:
                        return response[YunPianFields.User].ToObject<User> ();
                    case YunPianFields.VersionV2:
                        return response.ToObject<User> ();
                    default:
                        return null;
                }
            });

            return await PostAsync (new Dictionary<string, string> (), resultHandler, Options.GetUser);
        }

        /// <summary>
        /// 设置账户信息
        /// </summary>
        /// <param name="emergency_contact">紧急联系人姓名</param>
        /// <param name="emergency_mobile">紧急联系人手机号</param>
        /// <param name="alarm_balance">短信余额提醒阈值</param>
        /// <returns></returns>
        public async Task<Result<User>> SetUserAsync (string emergency_contact = null, string emergency_mobile = null, string alarm_balance = null) {
            var data = new Dictionary<string, string> ();

            if (!string.IsNullOrEmpty (emergency_contact))
                data.Add (YunPianFields.EmergencyContact, emergency_contact);
            if (!string.IsNullOrEmpty (emergency_mobile))
                data.Add (YunPianFields.EmergencyMobile, emergency_mobile);
            if (!string.IsNullOrEmpty (alarm_balance))
                data.Add (YunPianFields.AlarmBalance, alarm_balance);

            var resultHandler = new MapResultHandler<User> (Options.Version, response => {
                return Options.Version == YunPianFields.VersionV2? response.ToObject<User> () : null;
            });

            return await PostAsync (data, resultHandler, Options.SetUser);
        }
    }
}
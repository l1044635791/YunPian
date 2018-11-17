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
        /// <param name="emergencyContact">紧急联系人姓名</param>
        /// <param name="emergencyMobile">紧急联系人手机号</param>
        /// <param name="alarmBalance">短信余额提醒阈值</param>
        /// <returns></returns>
        public async Task<Result<User>> SetUserAsync (string emergencyContact = null, string emergencyMobile = null, string alarmBalance = null) {
            var data = new Dictionary<string, string> ();

            if (!string.IsNullOrEmpty (emergencyContact))
                data.Add (YunPianFields.EmergencyContact, emergencyContact);
            if (!string.IsNullOrEmpty (emergencyMobile))
                data.Add (YunPianFields.EmergencyMobile, emergencyMobile);
            if (!string.IsNullOrEmpty (alarmBalance))
                data.Add (YunPianFields.AlarmBalance, alarmBalance);

            var resultHandler = new MapResultHandler<User> (Options.Version, response => {
                return Options.Version == YunPianFields.VersionV2? response.ToObject<User> () : null;
            });

            return await PostAsync (data, resultHandler, Options.SetUser);
        }
    }
}
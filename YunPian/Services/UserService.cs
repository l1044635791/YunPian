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
            Uri = Options.GetUser;

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

            return await PostAsync (resultHandler);
        }

        /// <summary>
        /// 设置账户信息
        /// </summary>
        /// <param name="emergency_contact">紧急联系人姓名</param>
        /// <param name="emergency_mobile">紧急联系人手机号</param>
        /// <param name="alarm_balance">短信余额提醒阈值</param>
        /// <returns></returns>
        public async Task<Result<User>> SetUserAsync (string emergency_contact = null, string emergency_mobile = null, string alarm_balance = null) {
            if (!string.IsNullOrEmpty (emergency_contact))
                Params.Add (YunPianFields.EmergencyContact, emergency_contact);
            if (!string.IsNullOrEmpty (emergency_mobile))
                Params.Add (YunPianFields.EmergencyMobile, emergency_mobile);
            if (!string.IsNullOrEmpty (alarm_balance))
                Params.Add (YunPianFields.AlarmBalance, alarm_balance);

            Uri = Options.SetUser;
            
            var resultHandler = new MapResultHandler<User> (Options.Version, response => {
                return Options.Version == YunPianFields.VersionV2? response.ToObject<User> () : null;
            });

            return await PostAsync (resultHandler);
        }
    }
}
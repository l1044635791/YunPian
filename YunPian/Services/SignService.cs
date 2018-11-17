using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using YunPian.Handlers;
using YunPian.Models;

namespace YunPian.Services {
    public class SignService : BaseService, ISignService {
        public SignService (IHttpClientFactory httpClientFactory, IOptions<YunPianOptions> options) : base (options) {
            _httpClient = httpClientFactory.CreateClient (nameof (YunPianOptions._smsHost));
        }

        /// <summary>
        /// 添加签名
        /// </summary>
        /// <param name="sign">签名内容</param>
        /// <param name="notify">是否短信通知审核结果 默认true</param>
        /// <param name="applyVip">是否申请专用通道，默认false</param>
        /// <param name="isOnlyGlobal">是否仅发国际短信，默认false</param>
        /// <param name="industryType">所属行业 默认 其他
        /// 其他值:  
        /// - 1. 游戏 2. 移动应用 3. 视频 4. 教育  5. IT/通信/电子服务 6. 电子商务  
        /// - 7. 金融 8. 网站  9. 商业服务 10. 房地产/建筑 11. 零售/租赁/贸易  
        /// - 12. 生产/加工/制造 13. 交通/物流 14. 文化传媒 15. 能源/电气   
        /// - 16. 政府企业 17. 农业 18. 物联网 19. 其它  
        /// </param>
        /// <returns></returns>
        public async Task<Result<Sign>> AddSignAsync (string sign, bool notify = true, bool applyVip = false, bool isOnlyGlobal = false, string industryType = "其他") {
            var data = new Dictionary<string, string> ();

            data.Add (YunPianFields.Sign, sign);
            data.Add (YunPianFields.Notify, notify.ToString ());
            data.Add (YunPianFields.ApplyVip, applyVip.ToString ());
            data.Add (YunPianFields.IsOnlyGlobal, isOnlyGlobal.ToString ());
            data.Add (YunPianFields.IndustryType, industryType);

            var resultHandler = new MapResultHandler<Sign> (Options.Version, response => {
                return Options.Version == YunPianFields.VersionV2 ? response[YunPianFields.Sign].ToObject<Sign> () : null;
            });
            return await PostAsync (data, resultHandler, Options.AddSign);
        }

        /// <summary>
        /// 修改签名
        /// </summary>
        /// <param name="sign">签名内容</param>
        /// <param name="oldSign">旧版签名内容</param>
        /// <param name="notify">是否短信通知审核结果 默认true</param>
        /// <param name="applyVip">是否申请专用通道，默认false</param>
        /// <param name="isOnlyGlobal">是否仅发国际短信，默认false</param>
        /// <param name="industryType">所属行业 默认 其他
        /// 其他值:  
        /// - 1. 游戏 2. 移动应用 3. 视频 4. 教育  5. IT/通信/电子服务 6. 电子商务  
        /// - 7. 金融 8. 网站  9. 商业服务 10. 房地产/建筑 11. 零售/租赁/贸易  
        /// - 12. 生产/加工/制造 13. 交通/物流 14. 文化传媒 15. 能源/电气   
        /// - 16. 政府企业 17. 农业 18. 物联网 19. 其它  
        /// </param>
        /// <returns></returns>
        public async Task<Result<Sign>> UpdateSignAsync (string sign, string oldSign, bool notify = true, bool applyVip = false, bool isOnlyGlobal = false, string industryType = "其他") {
            var data = new Dictionary<string, string> ();

            data.Add (YunPianFields.Sign, sign);
            data.Add (YunPianFields.OldSign, oldSign);
            data.Add (YunPianFields.Notify, notify.ToString ());
            data.Add (YunPianFields.ApplyVip, applyVip.ToString ());
            data.Add (YunPianFields.IsOnlyGlobal, isOnlyGlobal.ToString ());
            data.Add (YunPianFields.IndustryType, industryType);

            var resultHandler = new MapResultHandler<Sign> (Options.Version, response => {
                return Options.Version == YunPianFields.VersionV2 ? response[YunPianFields.Sign].ToObject<Sign> () : null;
            });

            return await PostAsync (data, resultHandler, Options.UpdateSign);
        }

        /// <summary>
        /// 获取签名
        /// </summary>
        /// <param name="pageIndex">页码 默认为1，值为空返回全部</param>
        /// <param name="pageSize">数量 默认20，值为空返回全部</param>
        /// <param name="sign">签名内容</param>
        /// <returns></returns>
        public async Task<Result<SignList>> GetSignAsync (int? pageIndex, int? pageSize, string sign = null) {
            var data = new Dictionary<string, string> ();

            if (!string.IsNullOrWhiteSpace (sign))
                data.Add (YunPianFields.Sign, sign);
            if (pageIndex != null)
                data.Add (YunPianFields.PageNum, pageIndex.ToString ());
            if (pageSize != null)
                data.Add (YunPianFields.PageSize, pageSize.ToString ());

            var resultHandler = new MapResultHandler<SignList> (Options.Version, response => {
                return Options.Version == YunPianFields.VersionV2? new SignList {
                    Total = response[YunPianFields.Total].ToObject<int> (),
                        Sign = response[YunPianFields.Sign].ToObject<List<Sign>> ()
                }: null;
            });

            return await PostAsync (data, resultHandler, Options.GetSign);
        }
    }
}
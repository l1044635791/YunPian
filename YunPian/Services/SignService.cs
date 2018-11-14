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
        /// <param name="apply_vip">是否申请专用通道，默认false</param>
        /// <param name="is_only_global">是否仅发国际短信，默认false</param>
        /// <param name="industry_type">所属行业 默认 其他
        /// 其他值:  
        /// - 1. 游戏 2. 移动应用 3. 视频 4. 教育  5. IT/通信/电子服务 6. 电子商务  
        /// - 7. 金融 8. 网站  9. 商业服务 10. 房地产/建筑 11. 零售/租赁/贸易  
        /// - 12. 生产/加工/制造 13. 交通/物流 14. 文化传媒 15. 能源/电气   
        /// - 16. 政府企业 17. 农业 18. 物联网 19. 其它  
        /// </param>
        /// <returns></returns>
        public async Task<Result<Sign>> AddSignAsync (string sign, bool notify = true, bool apply_vip = false, bool is_only_global = false, string industry_type = "其他") {
            Params.Add (YunPianFields.Sign, sign);
            Params.Add (YunPianFields.Notify, notify.ToString ());
            Params.Add (YunPianFields.ApplyVip, apply_vip.ToString ());
            Params.Add (YunPianFields.IsOnlyGlobal, is_only_global.ToString ());
            Params.Add (YunPianFields.IndustryType, industry_type);

            Uri = Options.AddSign;

            var resultHandler = new MapResultHandler<Sign> (Options.Version, response => {
                return Options.Version == YunPianFields.VersionV2 ? response[YunPianFields.Sign].ToObject<Sign> () : null;
            });
            return await PostAsync (resultHandler);
        }

        /// <summary>
        /// 修改签名
        /// </summary>
        /// <param name="sign">签名内容</param>
        /// <param name="old_sign">旧版签名内容</param>
        /// <param name="notify">是否短信通知审核结果 默认true</param>
        /// <param name="apply_vip">是否申请专用通道，默认false</param>
        /// <param name="is_only_global">是否仅发国际短信，默认false</param>
        /// <param name="industry_type">所属行业 默认 其他
        /// 其他值:  
        /// - 1. 游戏 2. 移动应用 3. 视频 4. 教育  5. IT/通信/电子服务 6. 电子商务  
        /// - 7. 金融 8. 网站  9. 商业服务 10. 房地产/建筑 11. 零售/租赁/贸易  
        /// - 12. 生产/加工/制造 13. 交通/物流 14. 文化传媒 15. 能源/电气   
        /// - 16. 政府企业 17. 农业 18. 物联网 19. 其它  
        /// </param>
        /// <returns></returns>
        public async Task<Result<Sign>> UpdateSignAsync (string sign, string old_sign, bool notify = true, bool apply_vip = false, bool is_only_global = false, string industry_type = "其他") {
            Params.Add (YunPianFields.Sign, sign);
            Params.Add (YunPianFields.OldSign, old_sign);
            Params.Add (YunPianFields.Notify, notify.ToString ());
            Params.Add (YunPianFields.ApplyVip, apply_vip.ToString ());
            Params.Add (YunPianFields.IsOnlyGlobal, is_only_global.ToString ());
            Params.Add (YunPianFields.IndustryType, industry_type);

            Uri = Options.UpdateSign;

            var resultHandler = new MapResultHandler<Sign> (Options.Version, response => {
                return Options.Version == YunPianFields.VersionV2 ? response[YunPianFields.Sign].ToObject<Sign> () : null;
            });

            return await PostAsync (resultHandler);
        }

        /// <summary>
        /// 获取签名
        /// </summary>
        /// <param name="page_index">页码 默认为1，值为空返回全部</param>
        /// <param name="page_size">数量 默认20，值为空返回全部</param>
        /// <param name="sign">签名内容</param>
        /// <returns></returns>
        public async Task<Result<SignList>> GetSignAsync (int? page_index, int? page_size, string sign = null) {
            if (!string.IsNullOrWhiteSpace (sign))
                Params.Add (YunPianFields.Sign, sign);
            if (page_index != null)
                Params.Add (YunPianFields.PageNum, page_index.ToString ());
            if (page_size != null)
                Params.Add (YunPianFields.PageSize, page_size.ToString ());

            Uri = Options.GetSign;

            var resultHandler = new MapResultHandler<SignList> (Options.Version, response => {
                return Options.Version == YunPianFields.VersionV2? new SignList {
                    Total = response[YunPianFields.Total].ToObject<int> (),
                        Sign = response[YunPianFields.Sign].ToObject<List<Sign>> ()
                }: null;
            });

            return await PostAsync (resultHandler);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YunPian.Models;

namespace YunPian.Services {
    public interface IShortenService {
        /// <summary>
        /// 生成短链接 V2
        /// </summary>
        /// <param name="name">名称，默认为“MM-dd HH:mm生成的短链接”</param>
        /// <param name="long_url">待转换的长网址，必须http://或https://开头 示例:https://www.yunpian.com</param>
        /// <param name="stat_duration">点击量统计持续时间（天），过期后不再统计，区间[0,30]，0表示不统计，默认30</param>
        /// <param name="provider">生成的链接的域名，传入1是yp2.cn，2是t.cn，默认1</param>
        /// <returns></returns>
        Task<Result<ShortUrl>> CreateShortenAsync (string name, string long_url, int stat_duration = 30, int provider = 1);

        /// <summary>
        /// 获取短链接统计 V2
        /// </summary>
        /// <param name="sid">短链接唯一标识 ckAclC</param>
        /// <param name="start">开始时间，默认一个小时前 2018-11-15 11:30:00</param>
        /// <param name="end">结束时间，默认当前时间 2018-11-15 12:30:00</param>
        /// <returns></returns>
        Task<Result<Dictionary<string, long>>> GetGetShortenAsync (string sid, DateTime? start, DateTime? end);
    }
}
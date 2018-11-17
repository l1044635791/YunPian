using System;
using Polly;
using YunPian;
using YunPian.Services;

namespace Microsoft.Extensions.DependencyInjection {
    public static class YunPianExtensions {
        /// <summary>
        /// 云片服务扩展方法
        /// </summary>
        /// <param name="service">应用程序服务</param>
        /// <param name="smsHost">短信服务主机地址</param>
        /// <param name="voiceHost">语音服务主机地址</param>
        /// <param name="flowHost">流服务主机地址</param>
        /// <param name="vsmsHost">视频服务主机地址</param>
        /// <param name="timeOut">主机超时时长</param>
        /// <param name="setupAction">云片网参数配置</param>
        public static void AddYunPianService (this IServiceCollection service, string smsHost, string voiceHost, string flowHost, string vsmsHost, int timeOut, Action<YunPianOptions> setupAction) {
            service.AddHttpClient (nameof (YunPianOptions._smsHost), c => {
                c.BaseAddress = new Uri (smsHost);
                c.Timeout = TimeSpan.FromSeconds (timeOut);
            }).AddTransientHttpErrorPolicy (builder => builder.WaitAndRetryAsync (new [] {
                TimeSpan.FromSeconds (1),
                    TimeSpan.FromSeconds (5),
                    TimeSpan.FromSeconds (10)
            }));
            service.AddHttpClient (nameof (YunPianOptions._voiceHost), c => {
                c.BaseAddress = new Uri (voiceHost);
                c.Timeout = TimeSpan.FromSeconds (timeOut);
            }).AddTransientHttpErrorPolicy (builder => builder.WaitAndRetryAsync (new [] {
                TimeSpan.FromSeconds (1),
                    TimeSpan.FromSeconds (5),
                    TimeSpan.FromSeconds (10)
            }));
            service.AddHttpClient (nameof (YunPianOptions._flowHost), c => {
                c.BaseAddress = new Uri (flowHost);
                c.Timeout = TimeSpan.FromSeconds (timeOut);
            }).AddTransientHttpErrorPolicy (builder => builder.WaitAndRetryAsync (new [] {
                TimeSpan.FromSeconds (1),
                    TimeSpan.FromSeconds (5),
                    TimeSpan.FromSeconds (10)
            }));
            service.AddHttpClient (nameof (YunPianOptions._vsmsHost), c => {
                c.BaseAddress = new Uri (vsmsHost);
                c.Timeout = TimeSpan.FromSeconds (timeOut);
            }).AddTransientHttpErrorPolicy (builder => builder.WaitAndRetryAsync (new [] {
                TimeSpan.FromSeconds (1),
                    TimeSpan.FromSeconds (5),
                    TimeSpan.FromSeconds (10)
            }));

            service.AddSingleton<ISmsService, SmsService> ();
            service.AddSingleton<IVoiceService, VoiceService> ();
            service.AddSingleton<IVideoService, VideoService> ();
            service.AddSingleton<ITempletService, TempletService> ();
            service.AddSingleton<ISignService, SignService> ();
            service.AddSingleton<IShortenService, ShortenService> ();

            service.Configure (setupAction);
        }

        /// <summary>
        /// 云片服务扩展方法
        /// </summary>
        /// <param name="service">应用程序服务</param>
        /// <param name="smsHost">短信服务主机地址</param>
        /// <param name="voiceHost">语音服务主机地址</param>
        /// <param name="flowHost">流服务主机地址</param>
        /// <param name="vsmsHost">视频服务主机地址</param>
        /// <param name="setupAction">云片网参数配置方法</param>
        public static void AddYunPianService (this IServiceCollection service, string smsHost, string voiceHost, string flowHost, string vsmsHost, Action<YunPianOptions> setupAction) {
            service.AddYunPianService (smsHost, voiceHost, flowHost, vsmsHost, YunPianOptions._timeout, setupAction);
        }

        /// <summary>
        /// 云片短信服务扩展方法
        /// </summary>
        /// <param name="service">应用程序服务</param>
        /// <param name="setupAction">云片网参数配置方法</param>
        public static void AddYunPianService (this IServiceCollection service, Action<YunPianOptions> setupAction) {
            service.AddYunPianService (YunPianOptions._smsHost, YunPianOptions._voiceHost, YunPianOptions._flowHost, YunPianOptions._vsmsHost, setupAction);
        }
    }
}
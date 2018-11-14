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
        /// <param name="sms_host">短信服务主机地址</param>
        /// <param name="voice_host">语音服务主机地址</param>
        /// <param name="flow_host">流服务主机地址</param>
        /// <param name="vsms_host">视频服务主机地址</param>
        /// <param name="time_out">主机超时时长</param>
        /// <param name="setupAction">云片网参数配置</param>
        public static void AddYunPianService (this IServiceCollection service, string sms_host, string voice_host, string flow_host, string vsms_host, int time_out, Action<YunPianOptions> setupAction) {
            service.AddHttpClient (nameof (YunPianOptions._smsHost), c => {
                c.BaseAddress = new Uri (sms_host);
                c.Timeout = TimeSpan.FromSeconds (time_out);
            }).AddTransientHttpErrorPolicy (builder => builder.WaitAndRetryAsync (new [] {
                TimeSpan.FromSeconds (1),
                    TimeSpan.FromSeconds (5),
                    TimeSpan.FromSeconds (10)
            }));
            service.AddHttpClient (nameof (YunPianOptions._voiceHost), c => {
                c.BaseAddress = new Uri (voice_host);
                c.Timeout = TimeSpan.FromSeconds (time_out);
            }).AddTransientHttpErrorPolicy (builder => builder.WaitAndRetryAsync (new [] {
                TimeSpan.FromSeconds (1),
                    TimeSpan.FromSeconds (5),
                    TimeSpan.FromSeconds (10)
            }));
            service.AddHttpClient (nameof (YunPianOptions._flowHost), c => {
                c.BaseAddress = new Uri (flow_host);
                c.Timeout = TimeSpan.FromSeconds (time_out);
            }).AddTransientHttpErrorPolicy (builder => builder.WaitAndRetryAsync (new [] {
                TimeSpan.FromSeconds (1),
                    TimeSpan.FromSeconds (5),
                    TimeSpan.FromSeconds (10)
            }));
            service.AddHttpClient (nameof (YunPianOptions._vsmsHost), c => {
                c.BaseAddress = new Uri (vsms_host);
                c.Timeout = TimeSpan.FromSeconds (time_out);
            }).AddTransientHttpErrorPolicy (builder => builder.WaitAndRetryAsync (new [] {
                TimeSpan.FromSeconds (1),
                    TimeSpan.FromSeconds (5),
                    TimeSpan.FromSeconds (10)
            }));

            service.AddTransient<ISmsService, SmsService> ();
            service.AddTransient<IVoiceService, VoiceService> ();
            service.AddTransient<IVideoService, VideoService> ();
            service.AddTransient<ITempletService, TempletService> ();
            service.AddTransient<ISignService, SignService> ();
            service.AddTransient<IShortenService, ShortenService> ();

            service.Configure (setupAction);
        }

        /// <summary>
        /// 云片服务扩展方法
        /// </summary>
        /// <param name="service">应用程序服务</param>
        /// <param name="sms_host">短信服务主机地址</param>
        /// <param name="voice_host">语音服务主机地址</param>
        /// <param name="flow_host">流服务主机地址</param>
        /// <param name="vsms_host">视频服务主机地址</param>
        /// <param name="setupAction">云片网参数配置方法</param>
        public static void AddYunPianService (this IServiceCollection service, string sms_host, string voice_host, string flow_host, string vsms_host, Action<YunPianOptions> setupAction) {
            service.AddYunPianService (sms_host, voice_host, flow_host, vsms_host, YunPianOptions._timeout, setupAction);
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
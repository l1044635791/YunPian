namespace YunPian {
    public class YunPianOptions {
        public static string _smsHost = "https://sms.yunpian.com"; // 短信
        public static string _voiceHost = "https://voice.yunpian.com"; // 语音
        public static string _flowHost = "https://flow.yunpian.com"; // 流
        public static string _vsmsHost = "https://vsms.yunpian.com"; // 视频
        public static int _timeout = 30; // 单位：秒
        public string _version = "v2"; // Api版本
        public string _charset = "utf-8"; // 编码格式

        public YunPianOptions (string apikey, string apiSecret) {
            this.ApiKey = apikey;
            this.ApiSecret = apiSecret;
        }
        public YunPianOptions (string apikey) {
            this.ApiKey = apikey;
        }
        public YunPianOptions () { }

        // Api版本号
        public string Version {
            get {
                return _version;
            }
            set {
                _version = value;
            }
        }

        // 文本编码格式
        public string Charset {
            get {
                return _charset;
            }
            set {
                _charset = value;
            }
        }

        // ApiKey
        public string ApiKey { get; set; }
        // ApiSecret
        public string ApiSecret { get; set; }
        // 获取user信息
        public string GetUser => $"/{Version}/user/get.json";
        // 设置用户信息
        public string SetUser => $"/{Version}/user/set.json";
        // 发送单条短信
        public string SingleSendSms => $"/{Version}/sms/single_send.json";
        // 发送批量短信
        public string BatchSendSms => $"/{Version}/sms/batch_send.json";
        // 发送个性化短信
        public string MultiSendSms_V1 => $"/v1/sms/multi_send.json";
        public string MultiSendSms => $"/{Version}/sms/multi_send.json";
        // 发送指定模板短信
        public string TplSingleSendSms_V1 => $"/v1/sms/tpl_send.json";
        public string TplSingleSendSms => $"/{Version}/sms/tpl_single_send.json";
        public string TplBatchSendSms => $"/{Version}/sms/tpl_batch_send.json";

        // 注册成功回调地址
        public string RegisterSucceedCallBack => $"/{Version}/sms/reg_complete.json";
        // 获取状态报告
        public string PullSmsStatus => $"/{Version}/sms/pull_status.json";
        // 获取回复短信
        public string PullSmsReply => $"/{Version}/sms/pull_reply.json";
        // 查看回复短信
        public string GetSmsReply => $"/{Version}/sms/get_reply.json";
        // 查看短信发送记录
        public string GetSmsRecord => $"/{Version}/sms/get_record.json";
        // 统计短信条数
        public string GetSmsCount => $"/{Version}/sms/count.json";
        // 查看文本屏蔽词
        public string GetBlackWords => $"/{Version}/sms/get_black_word.json";
        // 添加签名
        public string AddSign => $"/{Version}/sign/add.json";
        // 获取签名
        public string GetSign => $"/{Version}/sign/get.json";
        // 修改签名
        public string UpdateSign => $"/{Version}/sign/update.json";

        // 获取默认短信模板
        public string GetDefaultTemplet => $"/{Version}/tpl/get_default.json";
        // 获取短信模板
        public string GetTemplet => $"/{Version}/tpl/get.json";
        // 增加短信模板
        public string AddTemplet => $"/{Version}/tpl/add.json";
        // 修改短信模板
        public string UpdateTemplet => $"/{Version}/tpl/upd.json";
        // 删除短信模板
        public string DeleteTemplet => $"/{Version}/tpl/del.json";
        // 添加语音短信模板
        public string AddVoiceNotifyTemplet => $"/{Version}/tpl/add_voice_notify.json";
        // 修改语音短信模板
        public string UpdateVoiceNotifyTemplet => $"/{Version}/tpl/update_voice_notify.json";
        // 发送语音短信
        public string VoiceSend => $"/{Version}/voice/send.json";
        // 发送指定语音短信
        public string TplNotifyVoiceSend => $"/{Version}/voice/tpl_notify.json";
        // 状态报告
        public string PullVoiceStatus => $"/{Version}/voice/pull_status.json";
        // 查询流量包
        public string GetFlowPackage => $"/{Version}/flow/get_package.json";
        // 流量包充值
        public string RechargeFlow => $"/{Version}/flow/recharge.json";
        // 状态报告
        public string PullFlowStatus => $"/{Version}/flow/pull_status.json";
        // 视频
        public string AddVideoTemplet => $"/{Version}/vsms/add_tpl.json";
        public string GetVideoTemplet => $"/{Version}/vsms/get_tpl.json";
        public string TplBatchVideoSend => $"/{Version}/vsms/tpl_batch_send.json";
        //短链接
        public string CreateShorten => $"/{Version}/short_url/shorten.json";
        public string GetShorten => $"/{Version}/short_url/stat.json";
    }
}
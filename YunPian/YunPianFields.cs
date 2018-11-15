namespace YunPian {
    /// <summary>
    /// 云片网接口字段
    /// </summary>
    public static class YunPianFields {
        public const int Ok = 0;
        public const int ArgumentMissing = 1;
        public const int Error = 2;
        public const int UnknownException = -50;
        public const string VersionV1 = "v1";
        public const string VersionV2 = "v2";
        public const string ApiKey = "apikey";
        public const string Code = "code";
        public const string Msg = "msg";
        public const string Detail = "detail";
        public const string Data = "data";
        public const string User = "user";
        public const string Balance = "balance";
        // 紧急联系人电话
        public const string EmergencyMobile = "emergency_mobile";
        public const string EmergencyContact = "emergency_contact";
        // 余额告警阈值
        public const string AlarmBalance = "alarm_balance";
        public const string IpWhitelist = "ip_whitelist";
        public const string Email = "email";
        public const string GmtCreated = "gmt_created";
        public const string ApiVersion = "api_version";

        // 用户登录
        public const string Sign = "sign";
        public const string Notify = "notify";
        public const string ApplyVip = "apply_vip";
        public const string IsOnlyGlobal = "is_only_global";
        public const string IndustryType = "industry_type";
        public const string OldSign = "old_sign";
        public const string LicenseUrl = "license_url";
        // 模板id
        public const string TplId = "tpl_id";
        // 模板值
        public const string TplValue = "tpl_value";
        // 模板内容
        public const string TplContent = "tpl_content";
        public const string CheckStatus = "check_status";
        public const string Reason = "reason";
        public const string Template = "template";
        public const string Layout = "layout";
        public const string Material = "material";
        // 模板语言
        public const string Lang = "lang";
        public const string CountryCode = "country_code";
        public const string NotifyType = "notify_type";
        // 呼叫
        public const string From = "from";
        public const string To = "to";
        public const string Duration = "duration";
        public const string AreaCode = "area_code";
        public const string MessageId = "message_id";
        public const string AnonymousNumber = "anonymous_number";
        public const string PageSize = "page_size";
        // 流
        public const string Carrier = "carrier";
        public const string FlowPackage = "flow_package";
        public const string _Sign = "_sign";
        public const string CallbackUrl = "callback_url";
        public const string Result = "result";
        public const string FlowStatus = "flow_status";
        // 语音
        public const string DisplayNum = "display_num";
        public const string VoiceStatus = "voice_status";
        public const string Type = "type";
        // 短信
        public const string Extend = "extend";
        public const string SmsStatus = "sms_status";
        public const string SmsReply = "sms_reply";
        public const string Sms = "sms";
        public const string Total = "total";
        public const string Nick = "nick";
        public const string Uid = "uid";
        public const string Text = "text";
        public const string Mobile = "mobile";
        public const string StartTime = "start_time";
        public const string EndTime = "end_time";
        public const string PageNum = "page_num";
        public const string Time = "time";
        public const string Register = "register";
        public const string MobileStat = "mobile_stat";
        // short_url
        public const string ShortUrl = "short_url";
        public const string LongUrl = "long_url";
        public const string StatDuration = "stat_duration";
        public const string Provider = "provider";
        public const string Name = "name";
        public const string Stat = "stat";
        // 流量充值参数
        public const string Sn = "sn";
        public const string Count = "count";
        public const string Fee = "fee";
        public const string Unit = "unit";
        public const string Sid = "sid";
        // batch_send 接口 增添的返回值名
        public const string TotalCount = "total_count";
        public const string TotalFee = "total_fee";
        public const string SeperatorComma = ",";
        public const string RecordId = "record_id";
    }
}
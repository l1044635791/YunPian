using System;
using Xunit;
using YunPian.Services;

namespace YunPian.Test {
    public class SmsServiceTest {
        private readonly SmsService _smsService;
        public SmsServiceTest()
        {
            var httpClientFactoryMock = Substitute.For<IHttpClientFactory>();
            _smsService = new SmsService();
        }
        [Fact]
        public void SingleSendAsyncTest () {

        }
    }
}
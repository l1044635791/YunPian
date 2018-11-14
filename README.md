# YunPian.NetCore

[![NuGet](https://img.shields.io/nuget/v/YunPian.NetCore.svg)](https://www.nuget.org/packages/YunPian.NetCore/)
[![Build Status](https://dev.azure.com/1044635791/YunPian/_apis/build/status/l1044635791.YunPian)](https://dev.azure.com/1044635791/YunPian/_build/latest?definitionId=5)

基于云片网短信服务SDK的.NetCore版本实现

## 快速开始

- NuGet获取[YunPian.NetCore](https://www.nuget.org/packages/YunPian.NetCore/)

- 示例

```json
appsettings.json

{
    "YunPianApiKey":"你的云片ApiKey",
}
```

```csharp
Startup.cs

public void ConfigureServices (IServiceCollection services) {  
    //添加 YunPian 服务
    services.AddYunPianService (options =>  
        options.ApiKey = Configuration["YunPianApiKey"];  
    );
}
```

```csharp
AccountController

public readonly ISmsService _smsService;

// 通过构造函数依赖注入的方式获取服务实例
public AccountController (ISmsService smsService) {
    _smsService = smsService;
}

[HttpGet ("send-verifycode/{mobile}")]
public async Task<IActionResult> SendVerifyCode ([FromRoute] string mobile) {
    var message = $"【云片网】您的验证码是123456。如非本人操作，请忽略本短信";
    // 发送单条短信
    var result = await _smsService.SingleSendAsync (message, mobile);
    return result;
}
```

## 源码说明

- 工程源码

- YunPian 云片源码工程 `namespace=YunPian`
  - Services 云片服务接口源码
  - Models 模型对象定义
  - Handlers 响应结果处理方法
  - YunPianExtensions IServiceCollection 扩展方法
  - YunPianOptions 客户端配置数据
  - YunPianFields 云片网接口字段

## 环境要求

### Windows

- 需要`.NET 2.0` 及以上.
- 需要 `Visual Studio 2010`及以上.

### Linux/Mac

- 需要 `Mono 3.12` 及以上.

## 参考文档

- [API文档](https://www.yunpian.com/api2.0/guide.html)

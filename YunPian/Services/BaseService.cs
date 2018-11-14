using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Options;
using YunPian.Handlers;
using YunPian.Models;

namespace YunPian.Services
{
    public class BaseService {
        protected HttpClient _httpClient;
        private readonly YunPianOptions _options;
        public BaseService (IOptions<YunPianOptions> options) {

            _options = options.Value;

            this.Params = new Dictionary<string, string> {
                [YunPianFields.ApiKey] = _options.ApiKey
            };
        }

        protected string Uri { get; set; }
        protected string Charset { get; set; }
        protected YunPianOptions Options => _options;
        protected Dictionary<string, string> Params { get; set; }

        /// <summary>
        /// Post请求处理
        /// </summary>
        /// <param name="resultHandler">返回的响应处理方法</param>
        /// <typeparam name="TR">接收的类型</typeparam>
        /// <typeparam name="T">转换类型</typeparam>
        /// <returns>转换后的结果类型</returns>
        public async Task<Result<T>> PostAsync<TR, T> (IResultHandler<TR, T> resultHandler) {
            // 指定编码格式
            var encoding = Encoding.GetEncoding (Charset??_options.Charset);

            // 对返回的结果进行处理
            var response = resultHandler.Response (await (await _httpClient.PostAsync (Uri,
                new StringContent (UrlEncode (Params, encoding), encoding, "application/x-www-form-urlencoded")
            )).Content.ReadAsStringAsync ());

            // 对发送成功/失败/异常的返回结果进行处理
            try {
                return resultHandler.Code == YunPianFields.Ok ? resultHandler.Success (response) : resultHandler.Fail (response);
            } catch (Exception ex) {
                return resultHandler.CatchException (ex);
            }
        }

        /// <summary>
        /// Post请求处理
        /// </summary>
        /// <param name="content">Http内容</param>
        /// <param name="resultHandler">返回的响应处理方法</param>
        /// <typeparam name="TR">接收的类型</typeparam>
        /// <typeparam name="T">转换类型</typeparam>
        /// <returns>转换后的结果类型</returns>
        public async Task<Result<T>> PostAsync<TR, T> (HttpContent content, IResultHandler<TR, T> resultHandler) {

            // 对返回的结果进行处理
            var response = resultHandler.Response (await (await _httpClient.PostAsync (Uri, content)).Content.ReadAsStringAsync ());

            // 对发送成功/失败/异常的返回结果进行处理
            try {
                return resultHandler.Code == YunPianFields.Ok ? resultHandler.Success (response) : resultHandler.Fail (response);
            } catch (Exception ex) {
                return resultHandler.CatchException (ex);
            }
        }

        /// <summary>
        /// 参数UrlEncode
        /// </summary>
        /// <param name="@params">参数</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>返回编码后的string字符串</returns>
        protected string UrlEncode (Dictionary<string, string> @params, Encoding encoding = null) {
            return string.Join ("&",
                @params.Select (kv => string.Format ("{0}={1}", HttpUtility.UrlEncode (kv.Key, encoding??Encoding.GetEncoding (Charset??_options.Charset)),
                    HttpUtility.UrlEncode (kv.Value, encoding))));
        }

        /// <summary>
        /// 文本UrlEncode
        /// </summary>
        /// <param name="@params">参数</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>返回编码后的string字符串</returns>
        protected string TextUrlEncode (string seperator, string text) {
            return string.Join (seperator, text.Split (',').Select (m => $"{HttpUtility.UrlEncode (m, Encoding.GetEncoding (Charset??_options.Charset))}"));
        }

        /// <summary>
        /// 文本UrlEncode
        /// </summary>
        /// <param name="@params">参数</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>返回编码后的string字符串</returns>
        protected string TextUrlEncode (string seperator, params string[] text) {
            return string.Join (seperator, text.Select (m => $"{HttpUtility.UrlEncode (m, Encoding.GetEncoding (Charset??_options.Charset))}"));
        }
    }
}
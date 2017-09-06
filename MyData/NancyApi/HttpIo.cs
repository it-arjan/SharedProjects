using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using NLogWrapper;
using EasyHttp.Infrastructure;

namespace MyData.NancyApi
{
    public class HttpIo : IHttpIo
    {
        private string _apiToken; // to access the data api
        private string _socketServerAccessToken; // secure access to the socketserver
        private string _socketToken; // to identify which socket
        private static readonly NLogWrapper.ILogger _logger = LogManager.CreateLogger(typeof(NancyApiDb));

        // static httpclient reduces overhead
        private static HttpClient _staticHttpClient = new HttpClient();

        public HttpIo(string apiToken, string socketServerAccessToken, string socketFeedId)
        {
            _apiToken = apiToken;
            _socketServerAccessToken = socketServerAccessToken;
            _socketToken = socketFeedId;
            ResetSessionSpecificHeaders(_staticHttpClient);
        }

        public void ResetSessionSpecificHeaders(HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiToken);

            httpClient.DefaultRequestHeaders.Remove("X-socketServerAccessToken");
            httpClient.DefaultRequestHeaders.Add("X-socketServerAccessToken", _socketServerAccessToken);

            httpClient.DefaultRequestHeaders.Remove("X-socketFeedId");
            httpClient.DefaultRequestHeaders.Add("X-socketFeedId", _socketToken);
        }

        public string SyncRequest(System.Net.Http.HttpMethod method, string url, string json, string contentType)
        {
            HttpResponseMessage httpResponseMsg = null;

            var accept = new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(contentType);
            if (!_staticHttpClient.DefaultRequestHeaders.Accept.Contains(accept))
                _staticHttpClient.DefaultRequestHeaders.Accept.Add(accept);

            var ReponseMsg = string.Empty;

            var resultStatus = System.Net.HttpStatusCode.Ambiguous;
            try
            {
                if (method == System.Net.Http.HttpMethod.Get)
                {
                    httpResponseMsg = _staticHttpClient.GetAsync(url).Result;
                }
                else if (method == System.Net.Http.HttpMethod.Post)
                {
                    httpResponseMsg = _staticHttpClient.PostAsync(url, new StringContent(json)).Result;
                }
                else if (method == System.Net.Http.HttpMethod.Delete)
                {
                    httpResponseMsg = _staticHttpClient.DeleteAsync(url).Result;
                }
                else
                {
                    throw new Exception("unkown http method");
                }

                resultStatus = httpResponseMsg.StatusCode;
                var logMsg = string.Format("Log msg: {0} returned {1}", url, resultStatus);
                _logger.Info(logMsg);

                ReponseMsg = httpResponseMsg.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                ReponseMsg = ex.Message;
                _logger.Error(ReponseMsg);
            }

            return ReponseMsg;
        }

        public async Task<string> AsyncRequest(System.Net.Http.HttpMethod method, string url, string json, string contentType)
        {
            HttpResponseMessage httpResponseMsg = null;

            _staticHttpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(contentType)
                );
 
            var ReponseMsg = string.Empty;

            var resultStatus = System.Net.HttpStatusCode.Ambiguous;
            try
            {
                if (method == System.Net.Http.HttpMethod.Get)
                {
                    httpResponseMsg = await _staticHttpClient.GetAsync(url);
                }
                else if (method == System.Net.Http.HttpMethod.Post)
                {
                    httpResponseMsg = await _staticHttpClient.PostAsync(url, new StringContent(json));
                }
                else if (method == System.Net.Http.HttpMethod.Delete)
                {
                    httpResponseMsg = await _staticHttpClient.DeleteAsync(url);
                }
                else
                {
                    throw new Exception("unkown method");
                }

                resultStatus = httpResponseMsg.StatusCode;
                var logMsg = string.Format("Log msg: {0} returned {1}", url, resultStatus);
                _logger.Info(logMsg);

                ReponseMsg = await httpResponseMsg.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                ReponseMsg = ex.Message;
                _logger.Error(ReponseMsg);
            }

            return ReponseMsg;
        }
    }
}

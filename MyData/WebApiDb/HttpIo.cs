using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using NLogWrapper;
using EasyHttp.Infrastructure;
using System.Diagnostics;

namespace MyData.NancyApi
{
    public class HttpIo : IHttpIo
    {
        private string _apiToken; // to access the data api
        private string _socketAccessToken; // secure access to the socketserver
        private string _apiFeedId; // to identify which socket
        private static readonly NLogWrapper.ILogger _logger = LogManager.CreateLogger(typeof(WebApiDb));

        // static httpclient reduces overhead, separate one for each logged on user
        private static Dictionary<string, HttpClient> _staticHttpClientMap = new Dictionary<string, HttpClient>();

        public HttpIo(string apiToken, string socketServerAccessToken, string apiFeedId)
        {
            _apiToken = apiToken;
            _socketAccessToken = socketServerAccessToken;
            _apiFeedId = apiFeedId;
            if (!_staticHttpClientMap.ContainsKey(_socketAccessToken))
            {
                // first request of new user
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiToken);
                client.DefaultRequestHeaders.Remove("X-socketServerAccessToken");
                client.DefaultRequestHeaders.Add("X-socketServerAccessToken", _socketAccessToken);

                client.DefaultRequestHeaders.Remove("X-socketFeedId");
                client.DefaultRequestHeaders.Add("X-socketFeedId", _apiFeedId);

                _staticHttpClientMap.Add(_socketAccessToken, client);
            }
        }

        public string SyncRequest(System.Net.Http.HttpMethod method, string url, string json, string contentType)
        {
            HttpResponseMessage httpResponseMsg = null;

            var accept = new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(contentType);
            if (!StaticClient().DefaultRequestHeaders.Accept.Contains(accept))
                StaticClient().DefaultRequestHeaders.Accept.Add(accept);

            var ReponseMsg = string.Empty;

            var resultStatus = System.Net.HttpStatusCode.Ambiguous;
            try
            {
                if (method == System.Net.Http.HttpMethod.Get)
                {
                    httpResponseMsg = StaticClient().GetAsync(url).Result;
                }
                else if (method == System.Net.Http.HttpMethod.Post)
                {
                    httpResponseMsg = StaticClient().PostAsync(url, new StringContent(json)).Result;
                }
                else if (method == System.Net.Http.HttpMethod.Delete)
                {
                    httpResponseMsg = StaticClient().DeleteAsync(url).Result;
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

            StaticClient().DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(contentType)
                );
 
            var ReponseMsg = string.Empty;

            var resultStatus = System.Net.HttpStatusCode.Ambiguous;
            try
            {
                if (method == System.Net.Http.HttpMethod.Get)
                {
                    httpResponseMsg = await StaticClient().GetAsync(url);
                }
                else if (method == System.Net.Http.HttpMethod.Post)
                {
                    httpResponseMsg = await StaticClient().PostAsync(url, new StringContent(json));
                }
                else if (method == System.Net.Http.HttpMethod.Delete)
                {
                    httpResponseMsg = await StaticClient().DeleteAsync(url);
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

        private HttpClient StaticClient()
        {
            Debug.Assert(_staticHttpClientMap != null && _staticHttpClientMap.ContainsKey(_socketAccessToken));
            return _staticHttpClientMap[_socketAccessToken];
        }
    }
}

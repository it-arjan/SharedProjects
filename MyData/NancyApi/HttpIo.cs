using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using EasyHttp.Http;
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

        // static httpclients improve performance
        private static System.Net.Http.HttpClient _httpClient = new System.Net.Http.HttpClient();
        private static EasyHttp.Http.HttpClient _eHttp = new EasyHttp.Http.HttpClient();

        public HttpIo(string apiToken, string socketServerAccessToken, string socketFeedId)
        {
            _apiToken = apiToken;
            _socketServerAccessToken = socketServerAccessToken;
            _socketToken = socketFeedId;

        }
        public string SyncRequest(System.Net.Http.HttpMethod method, string url, string json, string contentType)
        {
            string result = string.Empty;

            _eHttp.Request.AddExtraHeader("Authorization", string.Format("bearer {0}", _apiToken));
            _eHttp.Request.AddExtraHeader("X-socketServerAccessToken", _socketServerAccessToken);
            _eHttp.Request.AddExtraHeader("X-socketFeedId", _socketToken);
            _eHttp.Request.Accept = contentType;
            bool error = false;
            if (method == System.Net.Http.HttpMethod.Get)
            {
                _eHttp.Get(url);
                error = _eHttp.Response.StatusCode != System.Net.HttpStatusCode.OK;
                if (!error)
                    result = _eHttp.Response.RawText;
            }
            else if (method == System.Net.Http.HttpMethod.Post)
            {
                _eHttp.Post(url, json, contentType);
                error = _eHttp.Response.StatusCode != System.Net.HttpStatusCode.OK;
            }
            else if (method == System.Net.Http.HttpMethod.Delete)
            {
                _eHttp.Delete(url, contentType);
                error = _eHttp.Response.StatusCode != System.Net.HttpStatusCode.NoContent;
            }
            else
            {
                throw new HttpException(_eHttp.Response.StatusCode, _eHttp.Response.StatusDescription);
            }

            if (error)
            {
                _logger.Error(" {0} request returned {1}", method, _eHttp.Response.StatusCode);
                throw new HttpException(_eHttp.Response.StatusCode, _eHttp.Response.StatusDescription);
            }

            return result;
        }

        public async Task<string> AsyncRequest(System.Net.Http.HttpMethod method, string url, string json, string contentType)
        {
            HttpResponseMessage httpResponseMsg = null;

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiToken);
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(contentType)
                );
            _httpClient.DefaultRequestHeaders.Add("X-socketServerAccessToken", _socketServerAccessToken);
            _httpClient.DefaultRequestHeaders.Add("X-socketFeedId", _socketToken);

            var ReponseMsg = string.Empty;

            var resultStatus = System.Net.HttpStatusCode.Ambiguous;
            try
            {
                if (method == System.Net.Http.HttpMethod.Get)
                {
                    httpResponseMsg = await _httpClient.GetAsync(url);
                }
                else if (method == System.Net.Http.HttpMethod.Post)
                {
                    httpResponseMsg = await _httpClient.PostAsync(url, new StringContent(json));
                }
                else if (method == System.Net.Http.HttpMethod.Delete)
                {
                    httpResponseMsg = await _httpClient.DeleteAsync(url);
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

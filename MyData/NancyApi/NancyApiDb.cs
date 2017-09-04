using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyData.Models;
using EasyHttp;
using Newtonsoft.Json;
using EasyHttp.Infrastructure;
using EasyHttp.Http;
using System.Net.Http;
using NLogWrapper;

namespace MyData.NancyApi
{
    public class NancyApiDb : IData
    {
        private static readonly NLogWrapper.ILogger _logger = LogManager.CreateLogger(typeof(NancyApiDb));
        private string _apiBaseUrl;
        IHttpIo _dataSourceIo;

        public NancyApiDb(string url, IHttpIo dataSourceIo)
        {
            _dataSourceIo = dataSourceIo;

            _apiBaseUrl = url;
        }

 
        public void AddPostback(MyData.Models.PostbackData pbd)
        {
            string url = string.Format("{0}/postback", _apiBaseUrl);
            _dataSourceIo.SyncRequest(System.Net.Http.HttpMethod.Post, url, JsonConvert.SerializeObject(pbd), HttpContentTypes.ApplicationJson);
        }

        public void AddRequestlog(MyData.Models.RequestLogEntry re)
        {
            string url = string.Format("{0}/requestlog", _apiBaseUrl);
            _dataSourceIo.SyncRequest(System.Net.Http.HttpMethod.Post, url, JsonConvert.SerializeObject(re), HttpContentTypes.ApplicationJson);
        }

        public void RemovePostback(int id)
        {
            string url = string.Format("{0}/postback/{1}", _apiBaseUrl, id);
            _dataSourceIo.SyncRequest(System.Net.Http.HttpMethod.Delete, url, json:null, contentType: "");
        }

        public void RemoveRequestlog(int id)
        {
            string url = string.Format("{0}/requestlog/{1}", _apiBaseUrl, id);
            _dataSourceIo.SyncRequest(System.Net.Http.HttpMethod.Delete, url, json: null, contentType: "");
        }

        public void Commit()
        {
            //not needed
        }

        public void Dispose()
        {
            //not needed
        }

        public PostbackData FindPostback(int id)
        {
            string url = string.Format("{0}/postback/{1}", _apiBaseUrl, id);
            var json = _dataSourceIo.SyncRequest(System.Net.Http.HttpMethod.Get, url, json: null, contentType: HttpContentTypes.ApplicationJson);
            return JsonConvert.DeserializeObject<PostbackData>(json);
        }

        public RequestLogEntry FindRequestLog(int id)
        {
            string url = string.Format("{0}/requestlog/{1}", _apiBaseUrl, id);
            var json = _dataSourceIo.SyncRequest(System.Net.Http.HttpMethod.Get, url, json: null, contentType: HttpContentTypes.ApplicationJson);
            return JsonConvert.DeserializeObject<RequestLogEntry>(json);
        }

        public List<PostbackData> GetPostbacksFromToday()
        {
            string url = string.Format("{0}/postback/today", _apiBaseUrl);
            var json = _dataSourceIo.SyncRequest(System.Net.Http.HttpMethod.Get, url, json: null, contentType: HttpContentTypes.ApplicationJson);
            return JsonConvert.DeserializeObject<List<PostbackData>>(json);
        }

        public List<PostbackData> GetRecentPostbacks(int nr)
        {
            string url = string.Format("{0}/postback/recent/take/{1}", _apiBaseUrl, nr);
            var json = _dataSourceIo.SyncRequest(System.Net.Http.HttpMethod.Get, url, json: null, contentType: HttpContentTypes.ApplicationJson);
            return JsonConvert.DeserializeObject<List<PostbackData>>(json);
        }

        public List<PostbackData> GetRecentPostbacks(int nr, string aspSessionId)
        {
            string url = string.Format("{0}/postback/{1}/recent/take/{2}", _apiBaseUrl, aspSessionId, nr);
            var json = _dataSourceIo.SyncRequest(System.Net.Http.HttpMethod.Get, url, json: null, contentType: HttpContentTypes.ApplicationJson);
            return JsonConvert.DeserializeObject<List<PostbackData>>(json);
        }

        public List<RequestLogEntry> GetRecentRequestLogs(int nr)
        {
            string url = string.Format("{0}/requestlog/recent/take/{1}", _apiBaseUrl, nr);
            var json = _dataSourceIo.SyncRequest(System.Net.Http.HttpMethod.Get, url, json: null, contentType: HttpContentTypes.ApplicationJson);
            return JsonConvert.DeserializeObject<List<RequestLogEntry>>(json);
        }

        public List<RequestLogEntry> GetRecentRequestLogs(int nr, string aspSessionId)
        {
            string url = string.Format("{0}/requestlog/{1}/recent/take/{2}", _apiBaseUrl, aspSessionId, nr);
            var json = _dataSourceIo.SyncRequest(System.Net.Http.HttpMethod.Get, url, json: null, contentType: HttpContentTypes.ApplicationJson);
            return JsonConvert.DeserializeObject<List<RequestLogEntry>>(json);
        }

        public async Task<PostbackData> FindPostbackAsync(int id)
        {
            var json = await _dataSourceIo.AsyncRequest(
                System.Net.Http.HttpMethod.Get,
                string.Format("{0}/postback/{1}", _apiBaseUrl, id),
                json: null,
                contentType: HttpContentTypes.ApplicationJson
                );

            return JsonConvert.DeserializeObject<PostbackData>(json);
        }

        public async Task AddPostbackAsync(PostbackData pbd)
        {
            string url = string.Format("{0}/postback", _apiBaseUrl);
            await _dataSourceIo.AsyncRequest(
                System.Net.Http.HttpMethod.Post,
                url, 
                JsonConvert.SerializeObject(pbd), 
                "application/json"
                );

        }

        public async Task AddRequestlogAsync(RequestLogEntry re)
        {
            string url = string.Format("{0}/requestlog", _apiBaseUrl);
            await _dataSourceIo.AsyncRequest(
                System.Net.Http.HttpMethod.Post,
                url,
                JsonConvert.SerializeObject(re),
                "application/json"
                );
        }

        public async Task RemovePostbackAsync(int id)
        {
            string url = string.Format("{0}/postback/{1}", _apiBaseUrl, id);
            await _dataSourceIo.AsyncRequest(
                 System.Net.Http.HttpMethod.Delete,
                 url,
                 json:null,
                 contentType: "application/json"
                 );
        }

        public async Task RemoveRequestlogAsync(int id)
        {
            string url = string.Format("{0}/requestlog/{1}", _apiBaseUrl, id);
            await _dataSourceIo.AsyncRequest(
                 System.Net.Http.HttpMethod.Delete,
                 url,
                 json: null,
                 contentType: "application/json"
                 );
        }

        public async Task<List<RequestLogEntry>> GetRecentRequestLogsAsync(int nr)
        {
            var json = await _dataSourceIo.AsyncRequest(
                System.Net.Http.HttpMethod.Get, 
                string.Format("{0}/requestlog/recent/take/{1}", _apiBaseUrl, nr),
                json: null,
                contentType: HttpContentTypes.ApplicationJson
                );
            return JsonConvert.DeserializeObject<List<RequestLogEntry>>(json);
        }

        public async Task<List<RequestLogEntry>> GetRecentRequestLogsAsync(int nr, string aspSessionId)
        {
            var json = await _dataSourceIo.AsyncRequest(
                System.Net.Http.HttpMethod.Get, 
                string.Format("{0}/requestlog/{1}/recent/take/{2}", _apiBaseUrl, aspSessionId, nr),
                json: null,
                contentType: HttpContentTypes.ApplicationJson
                );
            return JsonConvert.DeserializeObject<List<RequestLogEntry>>(json);
        }

        public async Task<RequestLogEntry> FindRequestLogAsync(int id)
        {
            var json = await _dataSourceIo.AsyncRequest(
                System.Net.Http.HttpMethod.Get, 
                string.Format("{0}/requestlog/{1}", _apiBaseUrl, id),
                json: null,
                contentType: HttpContentTypes.ApplicationJson
                );
            return JsonConvert.DeserializeObject<RequestLogEntry>(json);

        }

        public async Task<List<PostbackData>> GetRecentPostbacksAsync(int nr)
        {
            var json = await _dataSourceIo.AsyncRequest(
                System.Net.Http.HttpMethod.Get, 
                string.Format("{0}/postback/recent/take/{1}", _apiBaseUrl, nr),
                json: null,
                contentType: HttpContentTypes.ApplicationJson
                );
            return JsonConvert.DeserializeObject<List<PostbackData>>(json);
        }

        public async Task<List<PostbackData>> GetRecentPostbacksAsync(int nr, string aspSessionId)
        {
            var json = await _dataSourceIo.AsyncRequest(
                System.Net.Http.HttpMethod.Get, 
                string.Format("{0}/postback/{1}/recent/take/{2}", _apiBaseUrl, aspSessionId, nr),
                json: null,
                contentType: HttpContentTypes.ApplicationJson
                );
            return JsonConvert.DeserializeObject<List<PostbackData>>(json);

        }

        public async Task<List<PostbackData>> GetPostbacksFromTodayAsync()
        {
            var json = await _dataSourceIo.AsyncRequest(
                System.Net.Http.HttpMethod.Get, 
                string.Format("{0}/postback/today", _apiBaseUrl), 
                json: null,
                contentType: HttpContentTypes.ApplicationJson
                );
            return JsonConvert.DeserializeObject<List<PostbackData>>(json);

        }



    }
}

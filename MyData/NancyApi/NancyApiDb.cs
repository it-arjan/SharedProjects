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

namespace MyData.NancyApi
{
    public class NancyApiDb : IData
    {
        private string _apiToken;
        private string _socketToken;
        private string _apiBaseUrl;
        public NancyApiDb(string url, string oauthToken, string socketToken)
        {
            if (TokenExpired(oauthToken)) throw new Exception("SetApiToken: token expired");
            _apiToken = oauthToken;
            _socketToken = socketToken;
            //var text = GetRequest(string.Format("{0}postback", url), HttpContentTypes.TextPlain);
            // the public get has returned 200 OK
            _apiBaseUrl = url;

        }
        public void Add<T>(T data)
        {
            bool typeFound = false;

            if (typeof(T) == typeof(MyData.Models.RequestLogEntry))
            {
                var x = data as MyData.Models.RequestLogEntry;
                string url = string.Format("{0}/requestlog", _apiBaseUrl);
                Post(url, JsonConvert.SerializeObject(x));
                typeFound = true;
            }
            if (typeof(T) == typeof(MyData.Models.PostbackData))
            {
                var x = data as MyData.Models.PostbackData;
                string url = string.Format("{0}/postback", _apiBaseUrl);
                Post(url, JsonConvert.SerializeObject(x));
                typeFound = true;
            }

            if (!typeFound) throw new Exception("You need to add type " + typeof(T).Name + " in this generic Add function");
        }

        private string GetRequest(string url, string contentType)
        {
            var eHttp = new EasyHttp.Http.HttpClient();
            eHttp.Request.AddExtraHeader("Authorization", string.Format("bearer {0}", _apiToken));
            eHttp.Request.AddExtraHeader("X-socketToken", _socketToken);
            eHttp.Request.Accept = contentType;
            eHttp.Get(url);
            if (eHttp.Response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new HttpException(eHttp.Response.StatusCode, eHttp.Response.StatusDescription);

            return eHttp.Response.RawText;
        }

        private void Post(string url, string json)
        {
            var eHttp = new EasyHttp.Http.HttpClient();
            eHttp.Request.AddExtraHeader("Authorization", string.Format("bearer {0}", _apiToken));
            eHttp.Request.AddExtraHeader("X-socketToken", _socketToken);
            eHttp.Post(url, json, HttpContentTypes.ApplicationJson);
            if (eHttp.Response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new HttpException(eHttp.Response.StatusCode, eHttp.Response.StatusDescription);
        }

        private void Delete(string url)
        {
            var eHttp = new EasyHttp.Http.HttpClient();
            eHttp.Request.AddExtraHeader("Authorization", string.Format("bearer {0}", _apiToken));
            eHttp.Request.AddExtraHeader("X-socketToken", _socketToken);
            eHttp.Delete(url, HttpContentTypes.ApplicationJson);
            if (eHttp.Response.StatusCode != System.Net.HttpStatusCode.NoContent)
                throw new HttpException(eHttp.Response.StatusCode, eHttp.Response.StatusDescription);
        }

        public void RemovePostback(int id)
        {
            string url = string.Format("{0}/postback/{1}", _apiBaseUrl, id);
            Delete(url);
        }

        public void RemoveRequestlog(int id)
        {
            string url = string.Format("{0}/requestlog/{1}", _apiBaseUrl, id);
            Delete(url);
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
            var json = GetRequest(string.Format("{0}/postback/{1}", _apiBaseUrl, id), HttpContentTypes.ApplicationJson);
            return JsonConvert.DeserializeObject<PostbackData>(json);
        }

        public RequestLogEntry FindRequestLog(int id)
        {
            var json = GetRequest(string.Format("{0}/requestlog/{1}", _apiBaseUrl, id), HttpContentTypes.ApplicationJson);
            return JsonConvert.DeserializeObject<RequestLogEntry>(json);
        }

        public List<PostbackData> GetPostbacksFromToday()
        {
            var json = GetRequest(string.Format("{0}/postback/today", _apiBaseUrl), HttpContentTypes.ApplicationJson);
            return JsonConvert.DeserializeObject<List<PostbackData>>(json);
        }

        public List<PostbackData> GetRecentPostbacks(int nr)
        {
            var json = GetRequest(string.Format("{0}/postback/recent/take/{1}", _apiBaseUrl, nr), HttpContentTypes.ApplicationJson);
            return JsonConvert.DeserializeObject<List<PostbackData>>(json);
        }

        public List<PostbackData> GetRecentPostbacks(int nr, string aspSessionId)
        {
            var json = GetRequest(string.Format("{0}/postback/{1}/recent/take/{2}", _apiBaseUrl, aspSessionId, nr), HttpContentTypes.ApplicationJson);
            return JsonConvert.DeserializeObject<List<PostbackData>>(json);
        }

        public List<RequestLogEntry> GetRecentRequestLogs(int nr)
        {
            var json = GetRequest(string.Format("{0}/requestlog/recent/take/{1}", _apiBaseUrl, nr), HttpContentTypes.ApplicationJson);
            return JsonConvert.DeserializeObject<List<RequestLogEntry>>(json);
        }

        public List<RequestLogEntry> GetRecentRequestLogs(int nr, string aspSessionId)
        {
            var json = GetRequest(string.Format("{0}/requestlog/{1}/recent/take/{2}", _apiBaseUrl, aspSessionId, nr), HttpContentTypes.ApplicationJson);
            return JsonConvert.DeserializeObject<List<RequestLogEntry>>(json);
        }

        private void SetApiToken(string token)
        {
            if (TokenExpired(token))
                throw new Exception("SetApiToken: token expired");
            _apiToken = token;
        }

        private void SetBaseApiUrl(string url)
        {
            //var text = GetRequest(string.Format("{0}/postback", url), HttpContentTypes.TextPlain);
            // public get has returned OK
            _apiBaseUrl = url;
        }

        private bool TokenExpired(string jwt)
        {
            //_logger.Debug("Checking expiration of token({1}) {0}", jwt, scope);
            // #PastedCode
            //
            //=> Retrieve the 2nd part of the JWT token (this the JWT payload)
            var payloadBytes = jwt.Split('.')[1];

            //=> Padding the raw payload with "=" chars to reach a length that is multiple of 4
            var mod4 = payloadBytes.Length % 4;
            if (mod4 > 0) payloadBytes += new string('=', 4 - mod4);

            //=> Decoding the base64 string
            var payloadBytesDecoded = Convert.FromBase64String(payloadBytes);

            //=> Retrieve the "exp" property of the payload's JSON
            var payloadStr = Encoding.UTF8.GetString(payloadBytesDecoded, 0, payloadBytesDecoded.Length);
            var payload = JsonConvert.DeserializeAnonymousType(payloadStr, new { Exp = 0UL });


            var date1970CET = new DateTime(1970, 1, 1, 0, 0, 0).AddHours(1);
            //_logger.Debug("Expired Check: the token({1}) is valid until {0}.", date1970CET.AddSeconds(payload.Exp), scope);

            //=> Get the current timestamp
            var currentTimestamp = (ulong)(DateTime.UtcNow.AddHours(1) - date1970CET).TotalSeconds;
            // Compare
            var isExpired = currentTimestamp + 10 > payload.Exp; // 10 sec = margin
            //var logMsg = isExpired ? string.Format("Expired Check: token({0}) is expired.", scope)
            //                        : string.Format("Expired Check: token({0}) still valid.", scope);
            //_logger.Info(logMsg);

            return isExpired;
        }

 
    }
}

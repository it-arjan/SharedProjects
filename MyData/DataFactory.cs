using MyData.Fake;
using MyData.NancyApi;
using Newtonsoft.Json;
using NLogWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyData
{
    public enum MyDbType { EtfDb, ApiDbNancy, ApiDbWebApi, FakeDb };
    public class DataFactory
    {
        private static readonly NLogWrapper.ILogger _logger = LogManager.CreateLogger(typeof(DataFactory));
        private MyDbType _dbType;
        public DataFactory(MyDbType type)
        {
            _dbType = type;
        }

        public IData Db(string baseUrl= "forNancyApi", string oauthToken = "forNancyApi", string socketServerAccessToken = "SocketServerAccessToken", string socketFeedId= "SocketIdToken")
        {
            if (_dbType == MyDbType.EtfDb)
            {
                return new Etf.EntityFrameworkDb();
            }
            else if (_dbType == MyDbType.ApiDbNancy)
            {
                if (OauthTokenExpired(oauthToken))
                {
                    var msg = "Db: oauth token expired";
                    _logger.Error(msg);
                    throw new Exception(msg); // return null better??
                }
                return new NancyApi.NancyApiDb(baseUrl, new HttpIo(oauthToken, socketServerAccessToken, socketFeedId));
            }
            else if (_dbType == MyDbType.FakeDb)
            {
                return new FakeDb();
            }

            var msg2 = "DataFactory:: Unkown Db type!";
            _logger.Error(msg2);
            throw new Exception(msg2); // return null better??
        }
            private bool OauthTokenExpired(string jwt)
        {
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


        public IDataSetup DbSetup()
        {
            return new Etf.EtfSetup();
        }
    }
}

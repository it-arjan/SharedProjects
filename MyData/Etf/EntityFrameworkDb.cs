using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyData;
using MyData.Models;
using MyData.Etf.Models;

namespace MyData.Etf
{
    public class EntityFrameworkDb : IData
    {
        public FrontendDbContext _etfDb;
        public EntityFrameworkDb()
        {
            _etfDb = new FrontendDbContext();
        }

        public void Add<T>(T data)
        {
            bool typeFound = false;
            if(typeof(T) == typeof(MyData.Models.RequestLogEntry))
            {
                var x = data as MyData.Models.RequestLogEntry;
                var y = new Etf.Models.RequestLogEntry(x);
                _etfDb.RequestLogEntries.Add(y);
                typeFound = true;
            }
            if (typeof(T) == typeof(MyData.Models.PostbackData))
            {
                var x = data as MyData.Models.PostbackData;
                var y = new Etf.Models.PostbackData(x);
                _etfDb.Postbacks.Add(y);
                typeFound = true;
            }
            if (typeof(T) == typeof(MyData.Models.IpSessionId))
            {
                var x = data as MyData.Models.IpSessionId;
                var y = new Etf.Models.IpSessionId(x);
                _etfDb.IpSessionIds.Add(y);
                typeFound = true;
            }
            if (!typeFound) throw new Exception("You need to add type " + typeof(T).Name + " in this generic Add function" );
        }

        public void RemovePostback(int id)
        {
            var x = _etfDb.Postbacks.Find(id);
            _etfDb.Postbacks.Remove(x);
        }

        public void RemoveRequestlog(int id)
        {
            var x = _etfDb.RequestLogEntries.Find(id);
            _etfDb.RequestLogEntries.Remove(x);
        }

        public void RemoveIpSessionid(int id)
        {
            var x = _etfDb.IpSessionIds.Find(id);
            _etfDb.IpSessionIds.Remove(x);
        }

        public bool IpSessionIdExists(string sessionId, string ip)
        {
            return _etfDb.IpSessionIds.Where(I => I.SessionID == sessionId && I.Ip == ip).Any();
        }

        public List<MyData.Models.RequestLogEntry> GetRecentRequestLogs(int nr)
        {
            var result=new List < MyData.Models.RequestLogEntry >  ();
            var etfTransfers = _etfDb.RequestLogEntries.OrderByDescending(rq => rq.Timestamp).Take(nr).ToList();
            etfTransfers.ForEach(transfer => result.Add(new MyData.Models.RequestLogEntry(transfer)));
            return result;

        }

        public List<MyData.Models.RequestLogEntry> GetRecentRequestLogs(int nr, string SessionId)
        {
            var result = new List<MyData.Models.RequestLogEntry>();
            var etfTransfers = _etfDb.RequestLogEntries.Where(rq => rq.AspSessionId == SessionId).OrderByDescending(rq => rq.Timestamp).ToList();
            etfTransfers.ForEach(transfer => result.Add(new MyData.Models.RequestLogEntry(transfer)));
            return result;
        }

        public List<MyData.Models.PostbackData> GetPostbacksFromToday()
        {
            var result = new List<MyData.Models.PostbackData>();
            var etfTransfers = _etfDb.Postbacks.Where(pb => System.Data.Entity.DbFunctions.DiffDays(pb.End, DateTime.Now) < 1).ToList();
            etfTransfers.ForEach(transfer => result.Add(new MyData.Models.PostbackData(transfer)));
            return result;
        }

        public bool SessionIdExists(string aspSessionId)
        {
            return _etfDb.IpSessionIds.Where(ips => ips.SessionID == aspSessionId).Any();
        }

        public MyData.Models.RequestLogEntry FindRequestLog(int id)
        {
            return new MyData.Models.RequestLogEntry(_etfDb.RequestLogEntries.Find(id));
        }

        public MyData.Models.PostbackData FindPostback(int id)
        {
            return new MyData.Models.PostbackData(_etfDb.Postbacks.Find(id));
        }

        public MyData.Models.IpSessionId FindIpSessionId(int id)
        {
            return new MyData.Models.IpSessionId(_etfDb.IpSessionIds.Find(id));
        }

        public List<MyData.Models.PostbackData> GetRecentPostbacks(int nr)
        {
            var result = new List<MyData.Models.PostbackData>();
            var etfTransfers = _etfDb.Postbacks.OrderByDescending(c => c.End).Take(nr).ToList();
            etfTransfers.ForEach(transfer => result.Add(new MyData.Models.PostbackData(transfer)));
            return result;
        }

        public List<MyData.Models.PostbackData> GetRecentPostbacks(int nr, string SessionId)
        {
            var result = new List<MyData.Models.PostbackData>();
            var etfTransfers = _etfDb.Postbacks.Where(pb => pb.AspSessionId==SessionId).OrderByDescending(c => c.End).Take(nr).ToList();
            etfTransfers.ForEach(transfer => result.Add(new MyData.Models.PostbackData(transfer)));
            return result;
        }

        public void Commit()
        {
            _etfDb.SaveChanges();
        }

        public void Dispose()
        {
            _etfDb.Dispose();
        }

        public void SetBaseApiUrl(string url)
        {
            throw new NotImplementedException();
        }

        public void SetApiToken(string token)
        {
            throw new NotImplementedException();
        }


    }
}

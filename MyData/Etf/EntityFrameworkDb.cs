﻿using System;
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

        public void AddPostback(MyData.Models.PostbackData pbd)
        {
            var y = new Etf.Models.PostbackData(pbd);
            _etfDb.Postbacks.Add(y);
        }

        public void AddRequestlog(MyData.Models.RequestLogEntry re)
        {
            var y = new Etf.Models.RequestLogEntry(re);
            _etfDb.RequestLogEntries.Add(y);
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
            var etfTransfers = _etfDb.RequestLogEntries.Where(rq => rq.AspSessionId == SessionId).OrderByDescending(rq => rq.Timestamp).Take(nr).ToList();
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

        public MyData.Models.RequestLogEntry FindRequestLog(int id)
        {
            return new MyData.Models.RequestLogEntry(_etfDb.RequestLogEntries.Find(id));
        }

        public MyData.Models.PostbackData FindPostback(int id)
        {
            return new MyData.Models.PostbackData(_etfDb.Postbacks.Find(id));
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

        public Task AddPostbackAsync(MyData.Models.PostbackData pbd)
        {
            // TODO
            throw new NotImplementedException();
        }

        public Task AddRequestlogAsync(MyData.Models.RequestLogEntry re)
        {
            // TODO
            throw new NotImplementedException();
        }

        public Task RemovePostbackAsync(int id)
        {
            // TODO
            throw new NotImplementedException();
        }

        public Task RemoveRequestlogAsync(int id)
        {
            // TODO
            throw new NotImplementedException();
        }

        public Task<List<MyData.Models.RequestLogEntry>> GetRecentRequestLogsAsync(int nr)
        {
            // TODO
            throw new NotImplementedException();
        }

        public Task<List<MyData.Models.RequestLogEntry>> GetRecentRequestLogsAsync(int nr, string SessionId)
        {
            // TODO
            throw new NotImplementedException();
        }

        public Task<MyData.Models.RequestLogEntry> FindRequestLogAsync(int id)
        {
            // TODO
            throw new NotImplementedException();
        }

        public Task<MyData.Models.PostbackData> FindPostbackAsync(int id)
        {
            // TODO
            throw new NotImplementedException();
        }

        public Task<List<MyData.Models.PostbackData>> GetRecentPostbacksAsync(int nr)
        {
            // TODO
            throw new NotImplementedException();
        }

        public Task<List<MyData.Models.PostbackData>> GetRecentPostbacksAsync(int nr, string SessionId)
        {
            // TODO
            throw new NotImplementedException();
        }

        public Task<List<MyData.Models.PostbackData>> GetPostbacksFromTodayAsync()
        {
            // TODO
            throw new NotImplementedException();
        }
    }
}

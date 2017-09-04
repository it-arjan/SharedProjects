using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyData.Models;

namespace MyData.Fake
{
    public class FakeDb : IData
    {
        public void AddPostback(PostbackData pbd)
        {
        }

        public async Task AddPostbackAsync(PostbackData pbd)
        {
            await Task.Delay(1);
        }

        public void AddRequestlog(RequestLogEntry re)
        {
        }

        public async Task AddRequestlogAsync(RequestLogEntry re)
        {
            await Task.Delay(1);
        }

        public void Commit()
        {
        }

        public void Dispose()
        {
        }

        public PostbackData FindPostback(int id)
        {
            return new PostbackData { Id = id };
        }

        public async Task<PostbackData> FindPostbackAsync(int id)
        {
            await Task.Delay(1);
            return new PostbackData { Id = id };
        }

        public RequestLogEntry FindRequestLog(int id)
        {
            return new RequestLogEntry { Id = id };
        }

        public async Task<RequestLogEntry> FindRequestLogAsync(int id)
        {
            await Task.Delay(1);
            return new RequestLogEntry { Id = id };
        }

        public List<PostbackData> GetPostbacksFromToday()
        {
            return new List<PostbackData>();
        }

        public async Task<List<PostbackData>> GetPostbacksFromTodayAsync()
        {
            await Task.Delay(1);
            return new List<PostbackData>();
        }

        public List<PostbackData> GetRecentPostbacks(int nr)
        {
            return new List<PostbackData>();
        }

        public List<PostbackData> GetRecentPostbacks(int nr, string aspSessionId)
        {
            return new List<PostbackData>();
        }

        public async Task<List<PostbackData>> GetRecentPostbacksAsync(int nr)
        {
            await Task.Delay(1);
            return new List<PostbackData>();
        }

        public async Task<List<PostbackData>> GetRecentPostbacksAsync(int nr, string aspSessionId)
        {
            await Task.Delay(1);
            return new List<PostbackData>();
        }

        public List<RequestLogEntry> GetRecentRequestLogs(int nr)
        {
            return new List<RequestLogEntry>();
        }

        public List<RequestLogEntry> GetRecentRequestLogs(int nr, string aspSessionId)
        {
            return new List<RequestLogEntry>();
        }

        public async Task<List<RequestLogEntry>> GetRecentRequestLogsAsync(int nr)
        {
            await Task.Delay(1);
            return new List<RequestLogEntry>();
        }

        public async Task<List<RequestLogEntry>> GetRecentRequestLogsAsync(int nr, string aspSessionId)
        {
            await Task.Delay(1);
            return new List<RequestLogEntry>();
        }

        public void RemovePostback(int id)
        {
            
        }

        public async Task RemovePostbackAsync(int id)
        {
            await Task.Delay(1);
            
        }

        public void RemoveRequestlog(int id)
        {
            
        }

        public async Task RemoveRequestlogAsync(int id)
        {
            await Task.Delay(1);
        }
    }
}

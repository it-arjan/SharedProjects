using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyData.Models;
using MyData;

namespace MyData
{
    public interface IData: IDisposable
    {
        void AddPostback(PostbackData pbd);
        void AddRequestlog(RequestLogEntry re);

        Task AddPostbackAsync(PostbackData pbd);
        Task AddRequestlogAsync(RequestLogEntry re);

        void RemovePostback(int id);
        void RemoveRequestlog(int id);

        Task RemovePostbackAsync(int id);
        Task RemoveRequestlogAsync(int id);

        List<RequestLogEntry> GetRecentRequestLogs(int nr);
        List<RequestLogEntry> GetRecentRequestLogs(int nr, string aspSessionId);
        RequestLogEntry FindRequestLog(int id);

        Task<List<RequestLogEntry>> GetRecentRequestLogsAsync(int nr);
        Task<List<RequestLogEntry>> GetRecentRequestLogsAsync(int nr, string aspSessionId);
        Task<RequestLogEntry> FindRequestLogAsync(int id);

        PostbackData FindPostback(int id);
        List<PostbackData> GetRecentPostbacks(int nr);
        List<PostbackData> GetRecentPostbacks(int nr, string aspSessionId);
        List<PostbackData> GetPostbacksFromToday();

        Task<PostbackData> FindPostbackAsync(int id);
        Task<List<PostbackData>> GetRecentPostbacksAsync(int nr);
        Task<List<PostbackData>> GetRecentPostbacksAsync(int nr, string aspSessionId);
        Task<List<PostbackData>> GetPostbacksFromTodayAsync();

        //void SetBaseApiUrl(string url);
        //void SetApiToken(string token);
        void Commit();
    }
}

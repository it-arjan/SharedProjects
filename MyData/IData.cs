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
        void        Add<T>(T data);

        void RemovePostback(int id);
        void RemoveRequestlog(int id);
        void RemoveIpSessionid(int id);

        IpSessionId FindIpSessionId(int id);
        bool        IpSessionIdExists(string sessionId, string ip);
        bool            SessionIdExists(string aspSessionId);

        List<RequestLogEntry> GetRecentRequestLogs    (int nr);
        List<RequestLogEntry> GetRecentRequestLogs     (int nr, string SessionId);
        RequestLogEntry     FindRequestLog  (int id);

        PostbackData        FindPostback(int id);
        List<PostbackData>      GetRecentPostbacks(int nr);
        List<PostbackData> GetRecentPostbacks(int nr, string SessionId);
        List<PostbackData>      GetPostbacksFromToday();

        //void SetBaseApiUrl(string url);
        //void SetApiToken(string token);
        void Commit();
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyData.Models
{
    public class RequestLogEntry
    {
        public RequestLogEntry(MyData.Etf.Models.RequestLogEntry transfer)
        {
            Id = transfer.Id;
            User = transfer.User;
            Ip = transfer.Ip;
            AspSessionId = transfer.AspSessionId;
            Method = transfer.Method;
            ContentType = transfer.ContentType;
            RecentContributions = transfer.RecentContributions;
            Path = transfer.Path;
            Timestamp = transfer.Timestamp;
        }
        public RequestLogEntry(System.Data.DataRow dRow)
        {
            int.TryParse(dRow["Id"].ToString(), out int intId);
            int.TryParse(dRow["RecentContributions"].ToString(), out int intRecentContributions);

            Id = intId;
            User = dRow["User"].ToString(); ;
            Ip = dRow["Ip"].ToString();
            AspSessionId = dRow["AspSessionId"].ToString();
            Path = dRow["Path"].ToString();
            Method = dRow["Method"].ToString(); ;
            ContentType = dRow["ContentType"].ToString(); ;

            RecentContributions = intRecentContributions;
        }

        public RequestLogEntry()
        {
            Timestamp = DateTime.Now;
        }
        public int Id { get; set; }
        public string User { get; set; }
        public string Ip { get; set; }
        public string AspSessionId { get; set; }
        public string Path { get; set; }
        public string Method { get; set; }
        public string ContentType { get; set; }
        public int    RecentContributions { get; set; }

        [DisplayFormat(DataFormatString = "{0:ddd MMMM dd, H:mm}")]
        public DateTime Timestamp { get; set; }

    }
}
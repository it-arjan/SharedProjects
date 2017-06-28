using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using MyData.Models;

namespace MyData.Etf.Models
{
    public class RequestLogEntry
    {

        public RequestLogEntry()
        {
            Timestamp = DateTime.Now;
        }

        public RequestLogEntry(MyData.Models.RequestLogEntry transfer)
        {
            User = transfer.User;
            Ip = transfer.Ip;
            AspSessionId = transfer.AspSessionId;
            Method = transfer.Method;
            ContentType = transfer.ContentType;
            RecentContributions = transfer.RecentContributions;
            Timestamp = transfer.Timestamp;
            Path = transfer.Path;
        }

         [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
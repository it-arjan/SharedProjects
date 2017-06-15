using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyData.Models
{
    public class IpSessionId
    {
        public IpSessionId()
        {

        }
        public IpSessionId(MyData.Etf.Models.IpSessionId transfer)
        {
            Id = transfer.Id;
            SessionID = transfer.SessionID;
            Ip = transfer.Ip;
        }
        public int Id { get; set; }
        public string SessionID { get; set; }
        public string Ip { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyData.Models;

namespace MyData.Etf.Models
{
    public class IpSessionId
    {

        public IpSessionId(MyData.Models.IpSessionId transfer)
        {
            Id = transfer.Id;
            SessionID = transfer.SessionID;
            Ip = transfer.Ip;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string SessionID { get; set; }
        [Required]
        public string Ip { get; set; }
    }
}

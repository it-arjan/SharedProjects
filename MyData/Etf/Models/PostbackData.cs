﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using MyData.Models;

namespace MyData.Etf.Models
{
    public class PostbackData
    {

        public PostbackData(PostbackData toClone)
        {
            MessageId = toClone.MessageId;
            UserName = toClone.UserName;
            Start = toClone.Start;
            End = toClone.End;
            Duration = toClone.Duration;
            Content = toClone.Content;
        }

        public PostbackData()
        {
            Start = DateTime.Now;
            End = DateTime.Now;
        }

        public PostbackData(MyData.Models.PostbackData transfer)
        {
            AspSessionId = transfer.AspSessionId;
            MessageId = transfer.MessageId;
            UserName = transfer.UserName;
            Start = transfer.Start;
            End = transfer.End;
            Duration = transfer.Duration;
            Content = transfer.Content;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string MessageId { get; set; }
        public string UserName { get; set; }

        public DateTime Start { get; set; }
        [DisplayFormat(DataFormatString = "{0:ddd MMMM dd, H:mm}")]
        public DateTime End { get; set; }

        [DisplayFormat( DataFormatString = "{0:0.00}")]
        public decimal Duration { get; set; }
        public string AspSessionId { get; set; }

        public string Content { get; set; }
    }
}
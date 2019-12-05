using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TitleLeader.Models.ErrorModels
{
    public class Error
    {
        public long Id { get; set; }
        public DateTime ErrorDate { get; set; }
        public string  ErrorMessage { get; set; }
        public string StackTrace { get; set; }
        public string Source { get; set; }
        public string TragetSite { get; set; }
        public string UserId { get; set; }
    }
}
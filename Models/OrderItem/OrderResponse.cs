using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TitleLeader.Custom;

namespace TitleLeader.Models.OrderItem
{
    public class OrderResponse
    {
        public APICredentialInfo Credentials { get; set; }
        public int OrderNumber { get; set; }
        public int ItemNumber { get; set; }
        public string AQUADecision { get; set; }
        public string Response { get; set; }
        public int ResponseCode { get; set; }
        public int ResWareFileID { get; set; }
        public string ResWareFileNumber { get; set; }
        public string ResponseTime { get; set; }
        public List<documentPA> documents { get; set; }
    }

    public class documentPA
    {
        public string fileName { get; set; }
        public string format { get; set; }
        public string version { get; set; }
        public string binary { get; set; }
    }
}
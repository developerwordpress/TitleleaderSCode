using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TitleLeader.Models.Dataupload
{
    public class OpenOrderModels
    {
        public string OrderNo { get; set; }
        public string ItemNo { get; set; }
        public int CompletedSearch { get; set; }
        public int Search { get; set; }
        public string OrderDate { get; set; }
    }
}
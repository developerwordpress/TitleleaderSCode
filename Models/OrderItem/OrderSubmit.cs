using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TitleLeader.Models.OrderItem
{
    public class OrderSubmit
    {
        public int ID { get; set; }
        public int order_no { get; set; }
        public int item_no { get; set; }
        public string search_type { get; set; }
        public string house_no { get; set; }
        public string house_unit { get; set; }
        public string street_no { get; set; }
        public string street_addr { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string county { get; set; }
        public string zip_code { get; set; }
        public string aditional_notes { get; set; }
        public string order_type { get; set; }
    }
}
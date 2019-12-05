using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TitleLeader.Models.OrderItem
{
    public class ItemSubmit
    {
        public int item_no { get; set; }
        public string owner_first_name { get; set; }
        public string owner_last_name { get; set; }
    }
}
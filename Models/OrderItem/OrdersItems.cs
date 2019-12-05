using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TitleLeader.Models.OrderItem
{
    public class OrdersItems
    {
        public OrderSubmit Orders { get; set; }
        public List<ItemSubmit> Items { get; set; }
    }
}
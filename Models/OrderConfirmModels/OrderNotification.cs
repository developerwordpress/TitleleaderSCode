using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TitleLeader.Models.OrderConfirmModels
{
    public class OrderNotification
    {
        public string OrderNumber { get; set; }
        public int ItemNumber { get; set; }
        public string ResWareFileNumber { get; set; }
        public int ResponseCode { get; set; }
         public int DisplayStatus { get; set; }
        public string created_by { get; set; }
        public string created_on { get; set; }
       
    }
}
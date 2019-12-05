using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;


namespace TitleLeader.Models.Invoice
{
    public class Invoice
    {
 public string inv_no { get; set; }
 public string order_no { get; set; }
 public string order_date { get; set; }
 public string amount_unit { get; set; }
 public double total_amount { get; set; }
 public double amount_paid { get; set; }
 public double amount_due { get; set; }
 public string status { get; set; }
 public string created_by { get; set; }
 public string created_on { get; set; }
 public string updated_by { get; set; }
 public string updated_on { get; set; }
 public string search { get; set; }
 public string search_type { get; set; }
 public string rate { get; set; }
 public string gap_rate { get; set; }
 public string txtsearchesprice { get; set; }
 public string txtnoofsearches { get; set; }
 public string txttotalamonut { get; set; }
    }

    public class InvoiceCompleteOrders
    {
        public int ID { get; set; }
        public string order_no { get; set; }
        public string item_no { get; set; }
        public string search_type { get; set; }
        public string house_no { get; set; }
        public string house_unit { get; set; }
        public string street_no { get; set; }
        public string street_add { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string county { get; set; }
        public string zip_code { get; set; }
        public string country { get; set; }
        public string price { get; set; }
        public string status { get; set; }
        public string aditional_notes { get; set; }
        public string order_type { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime order_date { get; set; }
        public string owner_first_name { get; set; }
        public string owner_last_name { get; set; }
        public string order_status { get; set; }
        public string created_by { get; set; }
    }
}
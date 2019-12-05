using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TitleLeader.Models.OrderItemEdit
{
    public class OrderItemInfo
    {
        public string OrderNo { get; set; }
        public string ItemNo { get; set; }
        [Required]
        public string HouseNumber { get; set; }
        [Required]
        public string Unit { get; set; }
        [Required]
        public string StreetName { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string County { get; set; }
        [Required]
        public string ZipCode { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}
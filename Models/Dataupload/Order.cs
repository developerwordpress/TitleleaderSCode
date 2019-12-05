using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TitleLeader.Models.DataUpload
{
    public class Order
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Search Type is required.")]
        public string search_type { get; set; }
        [Required(ErrorMessage = "House no is required.")]
        public string house_no { get; set; }
        [Required(ErrorMessage = "House unit is required.")]
        public string house_unit { get; set; }
        [Required(ErrorMessage = "Street No is required.")]
        public string street_no { get; set; }
        [Required(ErrorMessage = "Address is required.")]
        public string street_addr { get; set; }

        [Required(ErrorMessage = "City is required.")]
        public string city { get; set; }
        [Required(ErrorMessage = "State is required.")]
        public string state { get; set; }
        [Required(ErrorMessage = "County is required.")]
        public string county { get; set; }
        [Required(ErrorMessage = "Zip is required.")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Zip must be 5 digits")]
        public string zip_code { get; set; }
        [Required(ErrorMessage = "Aditional is required.")]
        public string aditional_notes { get; set; }
        public string order_type { get; set; }
    }
}
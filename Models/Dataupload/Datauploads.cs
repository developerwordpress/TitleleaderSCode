using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TitleLeader.Models.DataUpload;

namespace TitleLeader.Models.Dataupload
{
    public class Datauploads
    {
        public Order Orders { get; set; }
        public Owner Owners { get; set; }
    }
    public class DatauploadSubmit
    {
        public int ID { get; set; }
        [Required]
        public string search_type { get; set; }
        [Required]
        public string house_no { get; set; }
       
        public string house_unit { get; set; }
      
        public string street_no { get; set; }
        [Required]
        public string street_addr { get; set; }
        [Required]
        public string city { get; set; }
        
        [Required]
        public string state { get; set; }
        [Required]
        public string county { get; set; }
        [Required]
        [StringLength(5, MinimumLength= 5, ErrorMessage = "Zip must be 5 digits")]
        public string zip_code { get; set; }
        [Required]
        public string aditional_notes { get; set; }
        [Required]
        public string order_type { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime order_date { get; set; }
        [Required]
        public List<Owner> Owners { get; set; }
    }
    public class DatauploadSave
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
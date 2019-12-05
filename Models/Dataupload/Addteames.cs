using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace TitleLeader.Models.Dataupload
{
    public class Addteames
    {
        [Required(ErrorMessage = "Please choose file.")]
        public HttpPostedFileBase fupload { get; set; }
        public byte[] Photo { get; set; }
        public string strusername { get; set; }
        public string strname { get; set; }
        public string stremail { get; set; }
        public string strpassword { get; set; }
        public bool Status { get; set; }
        public int id { get; set; }
        public string created_by { get; set; }
        public string create_on { get; set; }
        public string updated_by { get; set; }
        public string updated_on { get; set; }


    }
}
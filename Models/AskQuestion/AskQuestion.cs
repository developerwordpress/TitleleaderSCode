using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TitleLeader.Models.AskQuestion
{
    public class AskQuestion
    {
        public string OrderNo { get; set; }
        public string ItemNo { get; set; }
        [Required]
        public string Question { get; set; }
       
    }
}
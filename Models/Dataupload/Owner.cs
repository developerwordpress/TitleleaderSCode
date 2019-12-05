using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TitleLeader.Models.Dataupload
{
    public class Owner
    {
        [Required(ErrorMessage = "Owner First Name is required.")]
        public string owner_first_name { get; set; }
        [Required(ErrorMessage = "Owner Last Name is required.")]
        public string owner_last_name { get; set; }
    }
}
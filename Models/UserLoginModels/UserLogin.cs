using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TitleLeader.Models.UserLoginModels
{
    public class UserLogin
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Username is requird")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is requird")]
        public string Password { get; set; }
        public bool Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime LastLogin { get; set; }
    }
}
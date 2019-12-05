using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TitleLeader.Models.UserLoginModels
{
    public class ProfileImage
    {
        public byte[] Photo { get; set; }
        public int UserId { get; set; }
    }
}
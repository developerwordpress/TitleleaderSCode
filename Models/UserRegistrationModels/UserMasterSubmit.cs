using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TitleLeader.Models.UserRegistrationModels
{
    public class UserMasterSubmit
    {
        public int Id { get; set; }
        public string Username { get; set; }      
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string CompanyName { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }        
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string state { get; set; }
        public string Zip { get; set; }
    }
}
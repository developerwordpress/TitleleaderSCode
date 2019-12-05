using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace TitleLeader.Models.UserRegistrationModels
{
    public class UserMaster
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Username is required")]
        [Remote("CheckUsername", "Home", ErrorMessage = "Username already exists")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [MembershipPassword(MinRequiredNonAlphanumericCharacters = 1, MinNonAlphanumericCharactersError = "Your password needs to contain at least one symbol (!, @, #, etc).", ErrorMessage = "Your password must be 6 characters long and contain at least one symbol (!, @, #, etc).", MinRequiredPasswordLength = 6)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm password is required")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Password and confirmation password must match")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "First name is required")]
        public string Firstname { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        public string Lastname { get; set; }
        [Required(ErrorMessage = "Company name is required")]
        public string CompanyName { get; set; }
        public string Title { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email address is not valid")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Confirm email is required")]
        [System.ComponentModel.DataAnnotations.Compare("Email", ErrorMessage = "Email and confirmation email must match")]
        [EmailAddress(ErrorMessage = "Email address is not valid")]
        public string ConfirmEmail { get; set; }
        [Required(ErrorMessage = "Phone number is required")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }
        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }
        [Required(ErrorMessage = "State is required")]
        public int StateId { get; set; }
        public string State { get; set; }
        [Required(ErrorMessage = "Zip Code is required")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Zip must be 5 digits")]
        public string Zip { get; set; }
        public bool BulkOrderTalk { get; set; }
        [Required(ErrorMessage = "Please check")]
        public bool TermsCondition { get; set; }
        [Required(ErrorMessage = "Please check")]
        public bool UserAgreement { get; set; }
        public bool AutomaticPayment { get; set; }
        public bool Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime LastLogin { get; set; }
    }
}
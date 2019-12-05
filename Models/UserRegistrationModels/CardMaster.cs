using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TitleLeader.Models.UserRegistrationModels
{
    public class CardMaster
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [Required(ErrorMessage = "Card number is required")]
        public string CardNumber { get; set; }
        [Required(ErrorMessage = "Expiry month and year is required")]
        public string ExpMonthYear { get; set; }
        [Required(ErrorMessage = "Name on card is required")]
        public string NameOnCard { get; set; }
        [Required(ErrorMessage = "CCV is required")]
        public string CCV { get; set; }
        [Required(ErrorMessage = "Billing address is required")]
        public string BillingAddress { get; set; }
        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }
        [Required(ErrorMessage = "State is required")]
        public string StateId { get; set; }
        public string State { get; set; }
        [Required(ErrorMessage = "Zip Code is required")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Zip must be 5 digits")]
        public string Zip { get; set; }
        public bool AutomaticPayment { get; set; }
        public bool Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
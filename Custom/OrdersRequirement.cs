using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TitleLeader.OrderPlacementWebService;

namespace TitleLeader.Custom
{
    public class OrdersRequirement
    {
        public List<OrderPlacementServicePropertyAddress> OrderAddress  { get; set; }
        public List<OrderPlacementServiceBuyerSeller> ByerAddress  { get; set; }
        public List<OrderPlacementServiceBuyerSeller> SellerAddress { get; set; }
        public APICredentialInfo Credentials { get; set; }
        public int OrderNumber { get; set; }
        public int ItemNumber { get; set; }
        public int ClientId { get; set; }
        public int OfficeId { get; set; }
        public string FileNumber { get; set; }
        public int ClientsClientID { get; set; }
        public int TransactionTypeID { get; set; }
        public int ProductID { get; set; }
        public int UnderwriterID { get; set; }
        public int PrimaryContactID { get; set; }
    }

    public class APICredentialInfo
    {
        public string api_key { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }

    public class OrderResponsePA
    {
        public string status { get; set; }
        public string orderId { get; set; }
        public string clientFileNumber { get; set; }
        public string timestamp { get; set; }

    }
}
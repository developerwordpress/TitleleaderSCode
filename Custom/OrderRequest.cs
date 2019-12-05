using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TitleLeader.OrderPlacementWebService;

namespace TitleLeader.Custom
{
    public class OrderRequest : IOrderPlacementService
    {
        public PlaceOrderResponse PlaceOrder(int ClientID, int OfficeID, string FileNumber, OrderPlacementServicePropertyAddress PropertyAddress, int ClientsClientID, int TransactionTypeID, int ProductID, int UnderwriterID, int PrimaryContactID, DateTime? EstimatedSettlementDate, decimal SalesPrice, decimal LoanAmount, string LoanNumber, decimal CashOut, string[] PayoffMortgagees, int[] OptionalActionGroupIDs, OrderPlacementServicePartner Lender, bool IsLender, OrderPlacementServiceBuyerSeller[] Buyers, OrderPlacementServiceBuyerSeller[] Sellers, OrderPlacementServicePartner[] AdditionalPartners, OrderPlacementServicePartner ClientsClient, string Notes, bool RequestAQUADecision, decimal OriginalDebtAmount, decimal UnpaidPrincipalAmount)
        {
            OrderPlacementServiceClient client = new OrderPlacementServiceClient();
            PlaceOrderResponse response = client.PlaceOrder(ClientID, OfficeID, FileNumber, PropertyAddress,
                ClientsClientID, TransactionTypeID, ProductID, UnderwriterID,
                PrimaryContactID, EstimatedSettlementDate, SalesPrice, LoanAmount, LoanNumber, CashOut, PayoffMortgagees, OptionalActionGroupIDs, Lender, IsLender, Buyers, Sellers, AdditionalPartners, ClientsClient, Notes, RequestAQUADecision, OriginalDebtAmount, UnpaidPrincipalAmount);
            return response;
        }

        public Task<PlaceOrderResponse> PlaceOrderAsync(int ClientID, int OfficeID, string FileNumber, OrderPlacementServicePropertyAddress PropertyAddress, int ClientsClientID, int TransactionTypeID, int ProductID, int UnderwriterID, int PrimaryContactID, DateTime? EstimatedSettlementDate, decimal SalesPrice, decimal LoanAmount, string LoanNumber, decimal CashOut, string[] PayoffMortgagees, int[] OptionalActionGroupIDs, OrderPlacementServicePartner Lender, bool IsLender, OrderPlacementServiceBuyerSeller[] Buyers, OrderPlacementServiceBuyerSeller[] Sellers, OrderPlacementServicePartner[] AdditionalPartners, OrderPlacementServicePartner ClientsClient, string Notes, bool RequestAQUADecision, decimal OriginalDebtAmount, decimal UnpaidPrincipalAmount)
        {
            throw new NotImplementedException();
        }
    }
}
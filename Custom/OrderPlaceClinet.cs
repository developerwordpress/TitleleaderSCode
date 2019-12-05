using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using TitleLeader.OrderPlacementWebService;

namespace TitleLeader.Custom
{
    public class OrderPlaceClinet
    {
        public PlaceOrderResponse PlaceOrders(OrdersRequirement ordersRequirement)
        {
            OrderPlacementServiceClient orderPlacementServiceClient = new OrderPlacementServiceClient();
            // orderPlacementServiceClient.ClientCredentials.UserName.UserName = "WLTIXML";
            //orderPlacementServiceClient.ClientCredentials.UserName.Password = "FNA$xml2018wlti";
            orderPlacementServiceClient.ClientCredentials.UserName.UserName = "WWL_XML";
            orderPlacementServiceClient.ClientCredentials.UserName.Password = "Wwl_2019";

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            orderPlacementServiceClient.Open();
            OrderPlacementServiceBuyerSeller[] orderPlacementServiceBuyer = new OrderPlacementServiceBuyerSeller[ordersRequirement.ByerAddress.Count()];
            OrderPlacementServiceBuyerSeller[] orderPlacementServiceSellers = new OrderPlacementServiceBuyerSeller[ordersRequirement.SellerAddress.Count()];
            for (int i=0; i< ordersRequirement.ByerAddress.Count();i++)
            {
                orderPlacementServiceBuyer[i] = ordersRequirement.ByerAddress[i];
            }

            for (int i = 0; i < ordersRequirement.SellerAddress.Count(); i++)
            {
                orderPlacementServiceSellers[i] = ordersRequirement.SellerAddress[i];
            }

            var response = orderPlacementServiceClient.PlaceOrder(
                ordersRequirement.ClientId,
                ordersRequirement.OfficeId,
                ordersRequirement.FileNumber,
                ordersRequirement.OrderAddress[0],
                ordersRequirement.ClientsClientID,
                ordersRequirement.TransactionTypeID,
                ordersRequirement.ProductID,
                ordersRequirement.UnderwriterID,
                ordersRequirement.PrimaryContactID,
                null,
                0,
                0,
                "",
                0,
                null,
                null,
                null,
                false,
                orderPlacementServiceBuyer,
                orderPlacementServiceSellers,
                null,
                null,
                "test",
                false,
                0,
                0
            );
            return response;
        }

        
    }
}
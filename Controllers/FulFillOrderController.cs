using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TitleLeader.OrderPlacementWebService;
using TitleLeader.Custom;
using TitleLeader.Models.OrderItem;
using TitleLeader.Models.OrderConfirmModels;

using System.IO;
using System.Web;
using System.Web.Mvc;


namespace TitleLeader.Controllers
{
    public class FulFillOrderController : ApiController
    {
        // POST api/<controller>
        public string Post(OrderResponse response)
        {
            string ret_str;

            try
            {
                TitleLeaderDataAccess DataAccess = new TitleLeaderDataAccess();

                if (response.Credentials.api_key == "224008f3-e8d7-4600-9108-3bca03e00277" && response.Credentials.username == "punctual" && response.Credentials.password == "4b5tr4ct!")
                {
                    List<OrderResponse> orderResponses = new List<OrderResponse>();
                    DateTime DateTime = DateTime.Now;

                    string[] order_info = response.ResWareFileNumber.Split('-');
                    // order_info[0] : UserID, order_info[1] : OrderNo, order_info[2] : ItemNo

                    orderResponses.Add(new OrderResponse
                    {
                        OrderNumber = Int32.Parse(order_info[1]),
                        ItemNumber = Int32.Parse(order_info[2]),
                        AQUADecision = response.AQUADecision,
                        Response = response.Response,
                        ResponseCode = response.ResponseCode,
                        ResponseTime = DateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        ResWareFileID = response.OrderNumber,
                        ResWareFileNumber = response.ResWareFileNumber
                    });

                    DataAccess.InsertOrderResponse(orderResponses);
                    //DataAccess.UpdateOrderNo(response.OrderNumber, Int32.Parse(order_info[1]), order_info[0], response.ResponseCode);
                                        
                    OrderNotification orderNotification = new OrderNotification()
                    {
                        OrderNumber = order_info[1],
                        ItemNumber = Int32.Parse(order_info[2]),
                        ResWareFileNumber = response.ResWareFileNumber,
                        DisplayStatus = 0,
                        ResponseCode = 1,
                        created_by = order_info[0]
                    };
                    
                    DataAccess.InsertNewNotification(orderNotification);

                    List<documentPA> docs = response.documents;

                    foreach (var doc in docs)
                    {
                        byte[] buffer = Convert.FromBase64String(doc.binary.Replace(@"\", ""));
                        File.WriteAllBytes(@"C:\Inetpub\vhosts\titleleader.com\app.titleleader.com\pdf\" + doc.fileName, buffer);
                        //File.WriteAllBytes(Server.MapPath("~/pdf/") + doc.fileName, buffer);
                        DataAccess.InsertOrderDoc(order_info[1], order_info[2], order_info[0], doc.fileName);
                    }

                    ret_str = "Success";
                }
                else
                {
                    ret_str = "Certificate error!";
                }
            } catch (Exception ex)
            {
                //ret_str = "Failed!";
                ret_str = ex.Message;
            }

            return ret_str;
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "test1", "test2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}
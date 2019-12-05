using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using TitleLeader.Custom;
using TitleLeader.Models.AskQuestion;
using TitleLeader.Models.Dataupload;
using TitleLeader.Models.DocumentRecive;
using TitleLeader.Models.Invoice;
using TitleLeader.Models.OrderItem;
using TitleLeader.Models.OrderItemEdit;
using TitleLeader.Models.OrderConfirmModels;
using TitleLeader.OrderPlacementWebService;
using TitleLeader.Models.StripeChargeModel;
using Stripe.Infrastructure;
namespace TitleLeader.Controllers
{
    using System.Threading.Tasks;

   
    [Authorize, TitleLeaderExceptionFilter]
    public class SiteController : Controller
    {
        TitleLeaderDataAccess DataAccess = new TitleLeaderDataAccess();

        public ActionResult Manualuploads()
        {
            List<SelectListItem> States = new List<SelectListItem>();
            foreach (var item in DataAccess.GetStates())
            {
                SelectListItem selectItem = new SelectListItem() { Text = Convert.ToString(item.DISPLAY_NAME), Value = Convert.ToString(item.NAME) };
                States.Add(selectItem);
            }
            ViewBag.States = States;

            List<SelectListItem> County = new List<SelectListItem>();
            //foreach (var item in DataAccess.GetCounty())
            //{
            //    SelectListItem selectItem = new SelectListItem() { Text = Convert.ToString(item.County), Value = Convert.ToString(item.County) };
            //    County.Add(selectItem);
            //}
            ViewBag.County = County;

            List<SelectListItem> SearchType = new List<SelectListItem>();
            foreach (var item in DataAccess.GetSearchType())
            {
                SelectListItem selectItem = new SelectListItem() { Text = Convert.ToString(item.DISPLAY_NAME), Value = Convert.ToString(item.ID) };
                SearchType.Add(selectItem);
            }
            ViewBag.SearchTypes = SearchType;
            return View();
        }
        public class County
        {

            public string State { get; set; }
            public string CountyName { get; set; }
        }
        public List<County> GetAllCounty()
        {
            List<County> objCounty = new List<County>();
            foreach (var item in DataAccess.GetCounty())
            {
                objCounty.Add(new County { State = item.DISPLAY_NAME, CountyName = item.County });
            }
           return objCounty;
        }
        [HttpPost]
        public ActionResult GetCountyByStateId(string state)
        {
            List<County> objCounty = new List<County>();
            objCounty = GetAllCounty().Where(m => m.State == state).ToList();
            SelectList obgCounty = new SelectList(objCounty, "CountyName", "CountyName", 0);
            return Json(obgCounty);
        }
        [HttpPost]
        public ActionResult Manualuploads(List<DatauploadSubmit> dataUploadSubmit)
        {
            if (ModelState.IsValid)
            {
                string orderno = DataAccess.InsertOrders(dataUploadSubmit);
                return Json(orderno, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Orderconfirm(string ordno)
        {
            ViewBag.msgdelete = "";
            Session["ordno"] = ordno;
            var orders = DataAccess.GetSaveOrders(ordno);
            ViewBag.OrderCount = DataAccess.CountOrder(ordno);
            return View(orders);
        }
        //public ActionResult RefreshOrder()
        //{
        //    DataAccess.GetResponseDetails();
        //    return Json("success",JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        public ActionResult DeleteOrderItem(List<int> OrderItemNos)
        {
            foreach (int item in OrderItemNos)
            {
                DataAccess.DeleteOrder(Session["ordno"].ToString(), item.ToString());
            }
            Dictionary<string, string> jsondata = new Dictionary<string, string>();
            int count = DataAccess.CountOrder(Session["ordno"].ToString());
            jsondata["count"] = count.ToString();
            jsondata["orderno"] = Session["ordno"].ToString();

            return Json(jsondata, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveforLatterUpdate(List<int> OrderItemNos)
        {
            foreach (int item in OrderItemNos)
            {
                DataAccess.UpdateOrderDraft(Session["ordno"].ToString(), item.ToString());
            }
            Dictionary<string, string> jsondata = new Dictionary<string, string>();
            int count = DataAccess.CountOrder(Session["ordno"].ToString());
            jsondata["count"] = count.ToString();
            jsondata["orderno"] = Session["ordno"].ToString();

            return Json(jsondata, JsonRequestBehavior.AllowGet);
        }
        public ActionResult OpenOrders()
        {
            var openOrders = DataAccess.GetOpenOrders();
            return View(openOrders);
        }
        [HttpPost]
        public ActionResult OpenOrders(string orderid, string fillter)
        {
            ViewBag.SearchOption = orderid;
            ViewBag.FillterOption = fillter;
            var openOrders = DataAccess.SearchOpenOrders(orderid, fillter);
            ViewBag.search = orderid;
            return View(openOrders);
        }
        public ActionResult PaymentCompleteSearch(string ordno, string invno)
        {
            ViewBag.msg = "";
            Session["orderno1"] = ordno;
            var orders = DataAccess.GetInvoiceCompleteOrders(ordno);
            ViewBag.Invno = invno;
            ViewBag.OrderCount = DataAccess.CountOrder(ordno);
            ViewBag.OrderCompleteSearchCount = DataAccess.CountCompleteOrderSearch(ordno);
            ViewBag.UserDetails = DataAccess.UserMasterDetails(Convert.ToInt32(HttpContext.User.Identity.Name));
            return View(orders);
        }
        public ActionResult OpenSearch(string ordno)
        {
            ViewBag.msg = "";
            Session["orderno"] = ordno;
            var orders = DataAccess.GetSaveOrders(ordno);

            ViewBag.OrderCount = DataAccess.CountOrder(ordno);
            ViewBag.OrderOpenSearchCount = DataAccess.CountOpenOrderSearch(ordno);
            ViewBag.UserDetails = DataAccess.UserMasterDetails(Convert.ToInt32(HttpContext.User.Identity.Name));
            return View(orders);
        }
        public ActionResult CompleteSearch(string ordno)
        {
            ViewBag.msg = "";
            Session["orderno1"] = ordno;
            var orders = DataAccess.GetCompleteOrders(ordno);
           
            ViewBag.OrderCount = DataAccess.CountOrder(ordno);
            ViewBag.OrderCompleteSearchCount = DataAccess.CountCompleteOrderSearch(ordno);
            ViewBag.UserDetails = DataAccess.UserMasterDetails(Convert.ToInt32(HttpContext.User.Identity.Name));
            return View(orders);
        }
        [HttpPost]
        public ActionResult CompleteSearch(string orderitemno, string hdnval, string fillter)
        {
            ViewBag.OrderCount = DataAccess.CountOrder(Session["orderno1"].ToString());
            ViewBag.OrderCountSearch = DataAccess.CountCompleteOrderSearch(Session["orderno1"].ToString());
            ViewBag.UserDetails = DataAccess.UserMasterDetails(Convert.ToInt32(HttpContext.User.Identity.Name));
            var orders = DataAccess.GetOrdersItemno(Session["orderno1"].ToString(), orderitemno, fillter);
            if (orders.Count > 0)
            {
                //do nothings
                ViewBag.msg = "";
            }
            else
            {
                ViewBag.msg = "Data not found!!";
                orders = DataAccess.GetCompleteOrders(Session["orderno1"].ToString());
            }
            ViewBag.SearchOption = hdnval;
            ViewBag.FillterOption = fillter;
            ViewBag.search = orderitemno;
            // ViewBag.OrderCount = DataAccess.CountOrder(ordno);
            return View(orders);
        }
        [HttpPost]
        public ActionResult OpenSearch(string orderitemno, string hdnval, string fillter)
        {
            ViewBag.OrderCount = DataAccess.CountOrder(Session["orderno"].ToString());
            ViewBag.OrderCountSearch = DataAccess.CountOpenOrderSearch(Session["orderno"].ToString());
            ViewBag.UserDetails = DataAccess.UserMasterDetails(Convert.ToInt32(HttpContext.User.Identity.Name));
            var orders = DataAccess.GetOrdersItemno(Session["orderno"].ToString(), orderitemno, fillter);
            if (orders.Count > 0)
            {
                //do nothings
                ViewBag.msg = "";
            }
            else
            {
                ViewBag.msg = "Data not found!!";
                orders = DataAccess.GetSaveOrders(Session["orderno"].ToString());
            }
            ViewBag.SearchOption = hdnval;
            ViewBag.FillterOption = fillter;
            ViewBag.orderitem = orderitemno;
            // ViewBag.OrderCount = DataAccess.CountOrder(ordno);
            return View(orders);
        }
        [HttpPost]


        public ActionResult PlaceOrder(string orderNo)
        {
            var orders = DataAccess.GetSaveOrders(orderNo.ToString());
            int count = 0;
            foreach (var ord in orders)
            {
                var result = DataAccess.PlaceOrderForClient(ord);
                if (result)
                {
                    count++;
                }
            }
            if (count == orders.Count)
            {
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(orders.Count - count, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SaveForLater()
        {
            var savedOrders = DataAccess.GetSavedLaterOrder();

            // int OrderCount = Convert.ToInt32(openOrders.Where(q => q.ItemNo.ToString()=="1").Count());
            int OrderCount = savedOrders.Count();
            int OrderITemNo = savedOrders.Sum(q => q.Search);
            Session["savedorder"] = OrderCount;
            Session["saveditemno"] = OrderITemNo;



            ViewBag.OrderCount = OrderCount.ToString();
            ViewBag.OrderITemNo = OrderITemNo.ToString();
            return View(savedOrders);


        }
        [HttpPost]
        public ActionResult SaveForLater(string orderid, string fillter)
        {
            // var savedOrders = DataAccess.SearchOpenOrders(orderid);
            ViewBag.OrderCount = Session["savedorder"];
            ViewBag.OrderITemNo = Session["saveditemno"];
            var savedOrders = DataAccess.GetSavedLaterOrder(orderid, fillter);
            ViewBag.SearchOption = orderid;
            ViewBag.FillterOption = fillter;
            ViewBag.search = orderid;
            return View(savedOrders);
        }
        public ActionResult ActionRequired()
        {
            var ReturOders = DataAccess.GetReturnedOrder();
            return View(ReturOders);

        }
        [HttpPost]
        public ActionResult ActionRequired(string txtkey, string fillter)
        {
            var ReturOders = DataAccess.SearchReturnedOrder(txtkey, fillter);
            ViewBag.SearchOption = txtkey;
            ViewBag.FillterOption = fillter;
            ViewBag.search = txtkey;
            return View(ReturOders);

        }

        public ActionResult CompleteOrder()
        {
            var completeOrder = DataAccess.GetCompleteOrder();

            return View(completeOrder);
        }
        [HttpPost]
        public ActionResult CompleteOrder(string orderid, string fillter)
        {
            var completeOrder = DataAccess.GetCompleteOrder(orderid, fillter);
            ViewBag.SearchOption = orderid;
            ViewBag.FillterOption = fillter;
            return View(completeOrder);
        }


        public ActionResult NotificationOrder()
        {
            var completeOrder = DataAccess.GetCompleteOrder();
          
             foreach (OpenOrderModels completeOrders in completeOrder)
                {
                    List<OrderNotification> orderNotifications = new List<OrderNotification>();
                    OrderNotification orderNotification = new OrderNotification()
                    {

                        OrderNumber = completeOrders.OrderNo,
                        DisplayStatus = 1,
                        ResponseCode = 1
                    };
                    orderNotifications.Add(orderNotification);
                    DataAccess.UpdateNotification(orderNotifications);
                }
                
          
            return RedirectToAction("CompleteOrder");
            
        }
        public ActionResult NotificationCompleteOrder()
        {
           
            var countComplateOrder = DataAccess.GetOrderNotification();
            return PartialView(countComplateOrder);
        }
        public ActionResult ExcelFileUpload()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult ExcelFileUpload(ExcelUpload PostedFile)
        {
            var result = DataAccess.ReadExcelFiles(PostedFile.UploadFile);
            if (result != "0")
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DownloadExcelFormat()
        {
            return File(Server.MapPath("~/Content/ExcelFormat/TitleReaderExcelFormat.xls"), "application/vnd.ms-excel");
        }
        
        //public ActionResult Payment(string stripeEmail, string stripeToken)
        //{
        //    var customers = new StripeCustomerService();
        //    var charges = new StripeChargeService();

        //    var customer = customers.Create(new StripeCustomerCreateOptions
        //    {
        //        Email = stripeEmail
        //    });

        //    var charge = charges.Create(new StripeChargeCreateOptions
        //    {
        //        Amount = 500,//charge in cents
        //        Description = "Sample Charge",
        //        Currency = "usd",
        //        CustomerId = customer.Id
        //    });

        //    // further application specific code goes here

        //    return View();
        //}
        public ActionResult Invoice()
        {
            var invoice = DataAccess.GetInvoice("", "");
            InvoiceOverdue(10, 15, 20, 30);
           // ViewBag.search = invoice.Count();
            return View(invoice);
        }
        [HttpPost]
        public ActionResult Invoice(string orderitemno, string fillter)
        {
            var invoice = DataAccess.GetInvoice(orderitemno, fillter);
             InvoiceOverdue(10, 15, 20, 30);
            ViewBag.SearchOption = orderitemno;
            ViewBag.FillterOption = fillter;
            ViewBag.search = orderitemno;
            return View(invoice);
        }
        public void InvoiceOverdue(int noofday1, int noofday2, int noofday3, int noofday4)
        {
            double invsum = 0;
            int totaldue = 0;
            int overdue1days = 0;
            int overdue2days = 0;
            int overdue3days = 0;
            int overdue4days = 0;

            double overdue1sum = 0;
            double overdue2sum = 0;
            double overdue3sum = 0;
            double overdue4sum = 0;

            foreach (var m in DataAccess.GetInvoice("", ""))
            {
                invsum = Convert.ToDouble(invsum) + (Convert.ToDouble(m.total_amount));
                totaldue = totaldue + 1;
                if ((Convert.ToDateTime(DateTime.Now) - Convert.ToDateTime(m.created_on)).Days <= noofday1)
                {
                    overdue1days = overdue1days + 1;
                    overdue1sum = overdue1sum + (Convert.ToDouble(m.total_amount));

                }

                if ((Convert.ToDateTime(DateTime.Now) - Convert.ToDateTime(m.created_on)).Days > noofday1 && (Convert.ToDateTime(DateTime.Now) - Convert.ToDateTime(m.created_on)).Days <= noofday2)
                {
                    overdue2days = overdue2days + 1;
                    overdue2sum = overdue2sum + (Convert.ToDouble(m.total_amount));
                }

                if ((Convert.ToDateTime(DateTime.Now) - Convert.ToDateTime(m.created_on)).Days > noofday2 && (Convert.ToDateTime(DateTime.Now) - Convert.ToDateTime(m.created_on)).Days <= noofday3)
                {
                    overdue3days = overdue3days + 1;
                    overdue3sum = overdue3sum + (Convert.ToDouble(m.total_amount));
                }

                if ((Convert.ToDateTime(DateTime.Now) - Convert.ToDateTime(m.created_on)).Days > noofday3 && (Convert.ToDateTime(DateTime.Now) - Convert.ToDateTime(m.created_on)).Days <= noofday4)
                {
                    overdue4days = overdue4days + 1;
                    overdue4sum = overdue4sum + (Convert.ToDouble(m.total_amount));
                }
            }
            double overdue1per = 0;
            double overdue2per = 0;
            double overdue3per = 0;
            double overdue4per = 0;
            if (totaldue > 0)
            {
                ViewBag.invsum = invsum;
                ViewBag.totaldue = totaldue;
                ViewBag.overdue1sum = overdue1sum;
                ViewBag.overdue2sum = overdue2sum;
                ViewBag.overdue3sum = overdue3sum;
                ViewBag.overdue4sum = overdue4sum;
                ViewBag.overdue1per = Math.Round(Convert.ToDouble((overdue1days * 100) / totaldue));
                ViewBag.overdue2per = Math.Round(Convert.ToDouble((overdue2days * 100) / totaldue));
                ViewBag.overdue3per = Math.Round(Convert.ToDouble((overdue3days * 100) / totaldue));
                ViewBag.overdue4per = Math.Round(Convert.ToDouble((overdue4days * 100) / totaldue));
            }
            else
            {
                ViewBag.invsum = 0;
                ViewBag.totaldue = 0;
                ViewBag.overdue1sum = 0;
                ViewBag.overdue2sum = 0;
                ViewBag.overdue3sum = 0;
                ViewBag.overdue4sum = 0;
                ViewBag.overdue1per = 0;
                ViewBag.overdue2per = 0;
                ViewBag.overdue3per = 0;
                ViewBag.overdue4per = 0;
            }

        }
         public ActionResult Charge(string stripeToken, string invno, string orderno, string noofsearch, string totalamount,string search)
               {
          
            try
            {
                string apiKey = "sk_test_kOJGWbZILZucg7cQ984vI2iC00jhEgfZQh";
                var stripeClient = new Stripe.StripeClient(apiKey);
                ViewBag.UserDetails = DataAccess.UserMasterDetails(Convert.ToInt32(HttpContext.User.Identity.Name));
                var userdetails = new TitleLeader.Models.UserRegistrationModels.UserMaster();
                userdetails = (TitleLeader.Models.UserRegistrationModels.UserMaster)ViewBag.UserDetails;
                var stripeEmail = @userdetails.Email;
                dynamic response = stripeClient.CreateChargeWithToken(Convert.ToDecimal(totalamount), stripeToken, "GBP", stripeEmail);

                if (response.IsError == false && response.Paid)
                {
                    // success
                    //DataAccess.InsertPayments(invno, orderno, noofsearch, totalamount, search);
                }
               
            }
            catch (Exception ex)
            {
                
                
            }
               DataAccess.InsertPayments(invno, orderno, noofsearch, totalamount, search);
               var invoice = DataAccess.GetInvoice("", "");
               InvoiceOverdue(10, 15, 20, 30);
                return View("Invoice", invoice);

        }


         public ActionResult InvoiceCharge(string stripeToken, string invno, string totalamount)
         {
            
             try
             {
                 string apiKey = "sk_test_kOJGWbZILZucg7cQ984vI2iC00jhEgfZQh";
                 var stripeClient = new Stripe.StripeClient(apiKey);
                 ViewBag.UserDetails = DataAccess.UserMasterDetails(Convert.ToInt32(HttpContext.User.Identity.Name));
                 var userdetails = new TitleLeader.Models.UserRegistrationModels.UserMaster();
                 userdetails = (TitleLeader.Models.UserRegistrationModels.UserMaster)ViewBag.UserDetails;
                 var stripeEmail = @userdetails.Email;
                 dynamic response = stripeClient.CreateChargeWithToken(Convert.ToDecimal(totalamount), stripeToken, "GBP", stripeEmail);

                 if (response.IsError == false && response.Paid)
                 {
                     // success
                     //DataAccess.InsertPayments(invno, orderno, noofsearch, totalamount, search);
                 }

             }
             catch (Exception ex)
             {


             }
             DataAccess.InsertInvoicePayments(invno,  totalamount);
             var invoice = DataAccess.GetInvoice("", "");
             InvoiceOverdue(10, 15, 20, 30);
             return View("Invoice", invoice);

         }
        public ActionResult PlaceOrders()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PlaceOrders(string orderno, List<string> items)
        {
            var orderResponses = DataAccess.OrderConfirmation(Convert.ToInt32(orderno), items);
            if (orderResponses != null)
            {
                return PartialView(orderResponses);
            }
            else
            {
                return PartialView();
            }
        }

        public ActionResult GetDetailsSaveForLater(string orderno)
        {
            var orders = DataAccess.GetDetailsSaveForLater(orderno);
            return Json(orders, JsonRequestBehavior.AllowGet);
        }


        public ActionResult DeleteOrder(string strOrderNo, string strItemno)
        {
            int c = 0;
            int r = 0;
            if (strItemno == "")
            {
                List<OrderItems> items = new List<OrderItems>();
                items = DataAccess.GetItemNo(strOrderNo);
                foreach (var item in items)
                {
                    r = DataAccess.DeleteOrder(strOrderNo, item.orderitem.ToString());
                    c = c + r;
                }
            }
            else
            {
                r = DataAccess.DeleteOrder(strOrderNo, strItemno);
                c = c + r;
            }

            if (c > 0)
            {
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult EditSaveForLatter(string orderno, string itemno)
        {
            var data = DataAccess.GetOrderItemDetails(orderno, itemno);
            List<SelectListItem> States = new List<SelectListItem>();
            foreach (var item in DataAccess.GetStates())
            {
                SelectListItem selectItem = new SelectListItem() { Text = Convert.ToString(item.DISPLAY_NAME), Value = Convert.ToString(item.NAME), Selected = Convert.ToString(item.NAME) == data.State ? true : false };
                States.Add(selectItem);
            }
            ViewBag.States = States;
            List<SelectListItem> County = new List<SelectListItem>();
            foreach (var item in DataAccess.GetCounty())
            {
                SelectListItem selectItem = new SelectListItem() { Text = Convert.ToString(item.County), Value = Convert.ToString(item.County), Selected = Convert.ToString(item.NAME) == data.State ? true : false };
                County.Add(selectItem);
            }
            ViewBag.County = County;
            return PartialView(data);
        }
        [HttpPost]
        public ActionResult EditSaveForLatter(OrderItemInfo orderItemInfo)
        {
            if (ModelState.IsValid)
            {
                var result = DataAccess.UpdateOrderItemDetails(orderItemInfo);
                if (result)
                {
                    return Json(orderItemInfo.OrderNo, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("error", JsonRequestBehavior.AllowGet);
                }
            }
            return Json("error", JsonRequestBehavior.AllowGet);
        }
        public ActionResult AskQuestion(string orderno, string itemno)
        {
            AskQuestion obj = new AskQuestion();
            obj.OrderNo = orderno;
            obj.ItemNo = itemno;
            return PartialView(obj);
        }
        [HttpPost]
        public ActionResult AskQuestion(AskQuestion obj)
        {
            using (MailMessage mm = new MailMessage("rupeshonlineforyou@gmail.com", "rupeshonlineforyou@gmail.com"))
            {
                //devtestkr123@gmail.com
                // mm.Subject = "Ask Question by, Order No:"+obj.OrderNo+",Search No:"+obj.ItemNo;
                // mm.Body = obj.Question;
                // //if (fuAttachment.HasFile)
                // //{
                // //    string FileName = Path.GetFileName(fuAttachment.PostedFile.FileName);
                // //    mm.Attachments.Add(new Attachment(fuAttachment.PostedFile.InputStream, FileName));
                // //}
                // mm.IsBodyHtml = false;
                // SmtpClient smtp = new SmtpClient();
                // smtp.Host = "smtp.gmail.com";
                // smtp.EnableSsl = true;
                // NetworkCredential NetworkCred = new NetworkCredential("devtestkr123@gmail.com", "test123#");
                // smtp.UseDefaultCredentials = true;
                // smtp.Credentials = NetworkCred;
                // smtp.Port = 587;
                // smtp.Send(mm);
                //// ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Email sent.');", true);

                try
                {
                    MailMessage msg = new MailMessage();
                    msg.From = new MailAddress("devtestkr123@gmail.com", obj.OrderNo);
                    msg.To.Add("devtestkr123@gmail.com");
                    msg.Subject = "Ask Question by Order No:" + obj.OrderNo + ",Search No:" + obj.ItemNo; ;
                    msg.Body = "Q.> " + obj.Question;
                    msg.IsBodyHtml = true;
                    SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 465);
                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = new System.Net.NetworkCredential("devtestkr123@gmail.com", "admin");
                    smtpClient.Send(msg);
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {

                    return Json("error", JsonRequestBehavior.AllowGet);
                }

            }

        }

        public ActionResult SaveForLatterSubmit(string strOrderNo, string strOrderItem)
        {
            List<OrderItems> items = new List<OrderItems>();
            List<string> itemno = new List<string>();
            if (strOrderItem == "")
            {
                items = DataAccess.GetItemNo(strOrderNo);
                foreach (var item in items)
                {
                    itemno.Add(item.orderitem.ToString());
                }
            }
            else
            {
                itemno.Add(strOrderItem);
            }


            var orderResponses = DataAccess.OrderConfirmation(Convert.ToInt32(strOrderNo), itemno);
            if (orderResponses != null)
            {
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Documentids(string orderno)
        {
            List<ResDocument> resDocuments = new List<ResDocument>();
            using (TitleLeaderDataAccess dataAccess = new TitleLeaderDataAccess())
            {
                var docs = dataAccess.GetOrderDoc(orderno);
                foreach (string doc in docs)
                {
                    ResDocument resDocument = new ResDocument()
                    {
                        DocumentId = "",
                        DocumentName = doc
                    };

                    resDocuments.Add(resDocument);
                }

                /*var item = dataAccess.GetOrderResponses(orderno);
                if (item != null)
                {
                    var m_strFilePath = "https://www.fnas.com/API/files/" + item.ResWareFileID + "/documents?format=xml";
                    string xmlStr;
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    using (var wc = new WebClient())
                    {
                        wc.UseDefaultCredentials = true;
                        wc.Credentials = new NetworkCredential("WWL_XML", "Wwl_2019");
                        xmlStr = wc.DownloadString(m_strFilePath);
                    }
                    var xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(xmlStr);

                   
                    foreach (XmlNode RootNode in xmlDoc.DocumentElement.SelectNodes("/DocumentsResponse/Documents/Document"))
                    {
                        //DocumentID = RootNode.SelectSingleNode("DocumentID").InnerText;
                        ResDocument resDocument = new ResDocument()
                        {
                            DocumentId = RootNode.SelectSingleNode("DocumentID").InnerText,
                            DocumentName = RootNode.SelectSingleNode("DocumentName").InnerText
                        };

                        resDocuments.Add(resDocument);
                    }
                   
                }*/
                return Json(resDocuments, JsonRequestBehavior.AllowGet);
            }
        }

        public void CheckDocument()
        {

            //Start - Change done on 15-11-2019
            using (TitleLeaderDataAccess dataAccess = new TitleLeaderDataAccess())
            {
                var OpenOrders = DataAccess.GetOrders();
                foreach (OpenOrderModels OpenOrder in OpenOrders)
                {
                    var OrderSearch = dataAccess.GetOrderSearch(OpenOrder.OrderNo);
                    if (OrderSearch != null)
                    {
                        foreach (OrderResponse OrderSearchitem in OrderSearch)
                        {
                            var docs = dataAccess.GetOrderDoc(OpenOrder.OrderNo, OrderSearchitem.ItemNumber.ToString());
                            if (docs != null)
                            {
                                foreach (string doc in docs)
                                {
                                    try
                                    {
                                        var url = "http://app.titleleader.com/pdf/" + doc + "";
                                        
                                        System.Net.WebRequest webRequest = System.Net.WebRequest.Create(url);
                                        webRequest.Method = "HEAD";

                                        using (System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)webRequest.GetResponse())
                                        {



                                            if (response.StatusCode.ToString() == "OK")
                                            {

                                                List<OrderNotification> orderNotifications = new List<OrderNotification>();
                                                OrderNotification orderNotification = new OrderNotification()
                                                {
                                                    OrderNumber = OpenOrder.OrderNo,
                                                    ItemNumber = OrderSearchitem.ItemNumber,
                                                    ResWareFileNumber = OrderSearchitem.ResWareFileNumber,
                                                    DisplayStatus = 0,
                                                    ResponseCode = 1
                                                };
                                                orderNotifications.Add(orderNotification);
                                                dataAccess.InsertNotification(orderNotifications);
                                            }

                                        }
                                    }
                                    catch (WebException)
                                    {
                                    }
                                }
                            }
                        }
                    }

                }
            }
            //End - Change done on 15-11-2019
        }

        public ActionResult DownloadDocument(string docid, string docname)
        {
            var m_strFilePath = "https://www.fnas.com/API/Documents/" + docid + "?format=xml";
            string xmlStr;
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            using (var wc = new WebClient())
            {
                wc.UseDefaultCredentials = true;
                wc.Credentials = new NetworkCredential("WWL_XML", "Wwl_2019");
                xmlStr = wc.DownloadString(m_strFilePath);
            }
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlStr);
            string DocumentBody = "";
            foreach (XmlNode RootNode in xmlDoc.DocumentElement.SelectNodes("/DocumentResponse/Document"))
            {
                DocumentBody = RootNode.SelectSingleNode("DocumentBody").InnerText;
            }

            byte[] docBytes = Convert.FromBase64String(DocumentBody);
            string filetype = "";
            if (Path.GetExtension(docname) == ".pdf")
            {
                filetype = "application/pdf";
            }
            else if (Path.GetExtension(docname) == ".doc")
            {
                filetype = "application/msword";
            }
            else
            {
                filetype = "";
            }
            return File(docBytes, filetype, docname);
        }

        public ActionResult Documentidsitem(string orderno,string itemno)
        {
            List<ResDocument> resDocuments = new List<ResDocument>();
            using (TitleLeaderDataAccess dataAccess = new TitleLeaderDataAccess())
            {
                var docs = dataAccess.GetOrderDoc(orderno, itemno);
                foreach (string doc in docs)
                {
                    ResDocument resDocument = new ResDocument()
                    {
                        DocumentId = "",
                        DocumentName = doc
                    };

                    resDocuments.Add(resDocument);
                }

                /*var item = dataAccess.GetOrderResponses(orderno, itemno);
                var m_strFilePath = "https://www.fnas.com/API/files/" + item.ResWareFileID + "/documents?format=xml";
                string xmlStr;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                using (var wc = new WebClient())
                {
                    wc.UseDefaultCredentials = true;
                    wc.Credentials = new NetworkCredential("WWL_XML", "Wwl_2019");
                    xmlStr = wc.DownloadString(m_strFilePath);
                }
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlStr);

                List<ResDocument> resDocuments = new List<ResDocument>();
                foreach (XmlNode RootNode in xmlDoc.DocumentElement.SelectNodes("/DocumentsResponse/Documents/Document"))
                {
                    //DocumentID = RootNode.SelectSingleNode("DocumentID").InnerText;
                    ResDocument resDocument = new ResDocument()
                    {
                        DocumentId = RootNode.SelectSingleNode("DocumentID").InnerText,
                        DocumentName = RootNode.SelectSingleNode("DocumentName").InnerText
                    };

                    resDocuments.Add(resDocument);
                }*/
                return Json(resDocuments, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
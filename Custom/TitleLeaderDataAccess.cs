using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TitleLeader.Models.ErrorModels;
using TitleLeader.Models.UserLoginModels;
using TitleLeader.Models.UserRegistrationModels;
using TitleLeader.Models.Dataupload;
using TitleLeader.Models.DataUpload;
using TitleLeader.Models.OrderConfirmModels;
using TitleLeader.OrderPlacementWebService;
using System.Configuration;
using System.Data.OleDb;
using System.IO;
using TitleLeader.Models.Invoice;
using TitleLeader.Models.OrderItem;
using TitleLeader.Models.OrderItemEdit;
using System.Web.Security;

using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace TitleLeader.Custom
{
    
    public class TitleLeaderDataAccess : OrderPlaceClinet, IDisposable
    {
        
        public long AddError(Error error)
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string query = "INSERT INTO ERRORLOG(ERROR_DATE,MESSAGE,STACK_TRACE,SOURCE,TARGET_SITE,USER_ID)" +
                           "VALUES(@ErrorDate,@ErrorMessage,@StackTrace,@Source,@TragetSite,@UserId);" +
                           "SELECT CAST(SCOPE_IDENTITY() as int)";
                var returnId = db.Query<int>(query, error).SingleOrDefault();
                return returnId;
            }
        }
        [TitleLeaderExceptionFilter]
        public UserLogin UserAuthentication(string userName, string password)
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string query = "SELECT T1.Id,T1.CreateDate,T1.LastLogin,T1.Password,T1.Status,T1.UpdateDate," +
                    "T1.Username FROM UserLoginMaster T1 " +
                    "WHERE T1.Username=@Username  COLLATE SQL_Latin1_General_CP1_CS_AS AND T1.Password=@Password AND T1.Status=@Status";
                var userLogin = db.QueryFirstOrDefault<UserLogin>(query, new { Username = userName, Password = password, Status = true });
                return userLogin;
            }
        }
        [TitleLeaderExceptionFilter]
        public UserLogin TeamAuthentication(string userName)
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string query = "SELECT T1.Id,T1.CreateDate,T1.LastLogin,T1.Password,T1.Status,T1.UpdateDate," +
                    "T1.Username FROM UserLoginMaster T1 " +
                    "WHERE T1.ID=@ID  COLLATE SQL_Latin1_General_CP1_CS_AS  AND T1.Status=@Status";
                var userLogin = db.QueryFirstOrDefault<UserLogin>(query, new { ID = userName,  Status = true });
                return userLogin;
            }
        }
        [TitleLeaderExceptionFilter]
        public UserLogin TeamUserAuthentication(string userName, string password)
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string query = "SELECT T1.created_by AS Username FROM t_team T1 " +
                    "WHERE T1.stremail=@stremail  COLLATE SQL_Latin1_General_CP1_CS_AS AND T1.strpassword=@strpassword AND T1.Status=@Status";
                var userLogin = db.QueryFirstOrDefault<UserLogin>(query, new { stremail = userName, strpassword = password, Status = true });
                return userLogin;
            }
        }
        [TitleLeaderExceptionFilter]
        public dynamic GetStates()
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string query = "SELECT distinct ID,DISPLAY_NAME FROM Options WHERE OPTION_ID=@OPTION_ID AND DELETE_FLG=@DELETE_FLG";
                var states = db.Query(query, new { OPTION_ID = 1, DELETE_FLG = false });
                return states;
            }
        }
        [TitleLeaderExceptionFilter]
        public dynamic GetCounty()
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string query = "SELECT distinct DISPLAY_NAME,County FROM Options WHERE OPTION_ID=@OPTION_ID and County is not null ";
                var county = db.Query(query, new { OPTION_ID = 1 });
                return county;
            }
        }


        [TitleLeaderExceptionFilter]
        public UserLogin UserRegistration(UserMaster userMaster, List<CardMaster> cardMasters)
        {
            string query1 = "INSERT INTO UserLoginMaster(Username,Password,Status,CreateDate,UpdateDate,LastLogin) " +
                "VALUES(@Username,@Password,@Status,@CreateDate,@UpdateDate,@LastLogin);" +
                "SELECT @@IDENTITY AS IDNT";
            string query2 = "INSERT INTO UserMaster(Id,Firstname,Lastname,CompanyName,Email,Address,City,StateId,State,Zip,BulkOrderTalk,TermsCondition,UserAgreement,AutomaticPayment,Status,CreateDate,UpdateDate) " +
                "VALUES(@Id,@Firstname,@Lastname,@CompanyName,@Email,@Address,@City,@StateId,@State,@Zip,@BulkOrderTalk,@TermsCondition,@UserAgreement,@AutomaticPayment,@Status,@CreateDate,@UpdateDate)";
            string query3 = "INSERT INTO CardMaster(UserId,CardNumber,ExpMonthYear,NameOnCard,CCV,BillingAddress,City,StateId,State,Zip,Status,CreateDate,UpdateDate) " +
                "VALUES(@UserId,@CardNumber,@ExpMonthYear,@NameOnCard,@CCV,@BillingAddress,@City,@StateId,@State,@Zip,@Status,@CreateDate,@UpdateDate)";
            bool saveFlg = false;
            UserLogin userLogin = new UserLogin();
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var returnId = db.QueryFirstOrDefault<int>(query1, new
                        {
                            Username = userMaster.Username,
                            Password = userMaster.Password,
                            Status = true,
                            CreateDate = DateTime.Now,
                            UpdateDate = DateTime.Now,
                            LastLogin = DateTime.Now
                        }, transaction: transaction);

                        var rowaffectedRows = db.QueryFirstOrDefault(query2, new
                        {
                            Id = returnId,
                            Firstname = userMaster.Firstname,
                            Lastname = userMaster.Lastname,
                            CompanyName = userMaster.CompanyName,
                            Email = userMaster.Email,
                            Address = userMaster.Address,
                            City = userMaster.City,
                            StateId = userMaster.StateId,
                            State = userMaster.State,
                            Zip = userMaster.Zip,
                            BulkOrderTalk = userMaster.BulkOrderTalk,
                            TermsCondition = userMaster.TermsCondition,
                            UserAgreement = userMaster.UserAgreement,
                            AutomaticPayment = userMaster.AutomaticPayment,
                            Status = true,
                            CreateDate = DateTime.Now,
                            UpdateDate = DateTime.Now,
                        }, transaction: transaction);

                        foreach (CardMaster cardMaster in cardMasters)
                        {
                            var rows = db.QueryFirstOrDefault(query3, new
                            {
                                UserId = returnId,
                                CardNumber = cardMaster.CardNumber,
                                ExpMonthYear = cardMaster.ExpMonthYear,
                                NameOnCard = cardMaster.NameOnCard,
                                CCV = cardMaster.CCV,
                                BillingAddress = cardMaster.BillingAddress,
                                City = cardMaster.City,
                                StateId = cardMaster.StateId,
                                State = cardMaster.State,
                                Zip = cardMaster.Zip,
                                Status = true,
                                CreateDate = DateTime.Now,
                                UpdateDate = DateTime.Now
                            }, transaction: transaction);
                        }

                        transaction.Commit();
                        string query4 = "SELECT T1.Id,T1.CreateDate,T1.LastLogin,T1.Password,T1.Status,T1.UpdateDate," +
                       "T1.Username FROM UserLoginMaster T1 " +
                       "WHERE T1.Id=@Id AND T1.Status=@Status";
                        userLogin = db.QueryFirstOrDefault<UserLogin>(query4, new { Id = returnId, Status = true });
                        saveFlg = true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
                    if (saveFlg)
                    {
                        return userLogin;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
        [TitleLeaderExceptionFilter]
        public UserMaster UserMasterDetails(int id)
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string query = "SELECT * FROM UserMaster WHERE Id=@Id AND Status=@Status";
                var userMaster = db.QueryFirstOrDefault<UserMaster>(query, new { Id = id, Status = true });
                return userMaster;
            }
        }
        [TitleLeaderExceptionFilter]
        public bool CheckUsername(string username)
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string query = "SELECT COUNT(*) FROM UserLoginMaster WHERE Username=@Username;";
                int userCount = db.QueryFirstOrDefault<int>(query, new { Username = username });
                if (userCount > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        [TitleLeaderExceptionFilter]
        public dynamic GetSearchType()
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string query = "SELECT ID,DISPLAY_NAME FROM Options WHERE OPTION_ID=@OPTION_ID AND DELETE_FLG=@DELETE_FLG";
                var states = db.Query(query, new { OPTION_ID = 2, DELETE_FLG = false });
                return states;
            }
        }
        [TitleLeaderExceptionFilter]
        public string InsertOrders(List<DatauploadSubmit> dataUploadSubmit)
        {
            string query1 = "INSERT INTO order_details( order_no, order_item_no,order_type, search_type, house_no, house_unit,street_no, street_addr, city, state,county, zip_code, created_by, created_on, status,remarks)" +
                "VALUES( @order_no, @order_item_no,@order_type, @search_type, @house_no, @house_unit, @street_no,@street_addr, @city, @state,@county, @zip_code, @created_by, @created_on, @status,@remarks)";
            string query2 = "INSERT INTO order_owner_details(order_no, order_item_no, owner_first_name, owner_last_name)" +
                "VALUES(@order_no, @order_item_no,@owner_first_name, @owner_last_name)";
            string strOrderno = get_order_no();
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                db.Open();
                foreach (var orderitem in dataUploadSubmit)
                {
                    using (var transaction = db.BeginTransaction())
                    {
                        try
                        {
                            string strOrderItemNo = get_order_item_no(strOrderno);
                            var returnvalue = db.QueryFirstOrDefault<int>(query1, new
                            {
                                order_no = strOrderno,
                                order_item_no = strOrderItemNo,
                                order_type = orderitem.order_type,
                                search_type = orderitem.search_type,
                                house_no = orderitem.house_no,
                                house_unit = orderitem.house_unit,
                                street_addr = orderitem.street_addr,
                                city = orderitem.city,
                                state = orderitem.state,
                                county = orderitem.county,
                                zip_code = orderitem.zip_code,
                                created_by = HttpContext.Current.User.Identity.Name.ToString(),
                                created_on = DateTime.Now,
                                status = "O",
                                remarks = orderitem.aditional_notes,
                                street_no = orderitem.street_no
                            }, transaction: transaction);

                            foreach (var item in orderitem.Owners)
                            {
                                var row = db.QueryFirstOrDefault(query2, new
                                {
                                    order_no = strOrderno,
                                    order_item_no = strOrderItemNo,
                                    owner_first_name = item.owner_first_name,
                                    owner_last_name = item.owner_last_name
                                }, transaction: transaction);
                            }

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                        }
                    }
                }
            }
            return strOrderno;
        }
        [TitleLeaderExceptionFilter]
        public List<OpenOrderModels> GetOpenOrders()
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {

                string query = "SELECT o.order_no as OrderNo,count(*) search,CONVERT(date, o.created_on) as OrderDate FROM order_details O  where O.order_no NOT IN (select distinct OrderNumber from OrderNotification) and o.status='O' and o.created_by=" + HttpContext.Current.User.Identity.Name + " GROUP BY o.order_no ,CONVERT(date, o.created_on) order by o.order_no desc;";
               // string query = "SELECT o.order_no as OrderNo,count(*) search,CONVERT(date, o.created_on) as OrderDate FROM order_details O where o.status='O' and o.created_by=" + HttpContext.Current.User.Identity.Name + " GROUP BY o.order_no,CONVERT(date, o.created_on);";
                var openOrder = db.Query<OpenOrderModels>(query).ToList();
                return openOrder;
            }
        }
        [TitleLeaderExceptionFilter]
        public List<OpenOrderModels> GetOrders()
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {

                string query = "SELECT o.order_no as OrderNo,count(*) search,CONVERT(date, o.created_on) as OrderDate FROM order_details O  where  o.created_by=" + HttpContext.Current.User.Identity.Name + " GROUP BY o.order_no ,CONVERT(date, o.created_on) order by o.order_no desc;";
                // string query = "SELECT o.order_no as OrderNo,count(*) search,CONVERT(date, o.created_on) as OrderDate FROM order_details O where o.status='O' and o.created_by=" + HttpContext.Current.User.Identity.Name + " GROUP BY o.order_no,CONVERT(date, o.created_on);";
                var openOrder = db.Query<OpenOrderModels>(query).ToList();
                return openOrder;
            }
        }
        public List<OpenOrderModels> GetRecentORder()
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string query = "SELECT top 6 O.order_no as OrderNo,(select count(*) from OrderResponse inner join OrderNotification  on OrderResponse.ResWareFileNumber=OrderNotification.ResWareFileNumber and OrderNotification.OrderNumber=O.order_no  ) CompletedSearch,(select count(*) from order_details where order_details.order_no=O.order_no ) Search,CONVERT(date, o.created_on) as OrderDate FROM order_details O where O.status='O' and O.created_by=" + HttpContext.Current.User.Identity.Name + " GROUP BY O.order_no,CONVERT(date, O.created_on) order by OrderDate desc";

                var openOrder = db.Query<OpenOrderModels>(query).ToList();
                return openOrder;
            }
        }
        public List<Invoice> GetRecentInvoice()
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string query = "SELECT inv_no, SUM(total_amount) total_amount FROM Invoice_Detail where created_by=" + HttpContext.Current.User.Identity.Name + " group by inv_no";

                var inv = db.Query<Invoice>(query).ToList();
                return inv;
            }
        }

        [TitleLeaderExceptionFilter]
        public List<DatauploadSave> GetSaveOrders(string orderNo)
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string query = "select owners.owner_first_name as owner_first_name,owners.owner_last_name as owner_last_name, orders.ID as ID,orders.city as city,orders.created_on as order_date,orders.house_no as house_no,orders.house_unit as house_unit,orders.order_item_no as item_no,orders.order_no as order_no,orders.order_type as order_type,orders.remarks as aditional_notes,opt1.DISPLAY_NAME as search_type,state,orders.street_addr as street_add,orders.zip_code as zip_code,orders.status as order_status,orders.county as county from order_details orders left join order_owner_details owners on orders.order_no=owners.order_no and orders.order_item_no=owners.order_item_no left join  Options opt1 on opt1.ID=orders.search_type where orders.order_no=@order_no and orders.status='O'";
                var saveOrder = db.Query<DatauploadSave>(query, new { order_no = orderNo }).ToList();
                return saveOrder;
            }
        }
        public List<DatauploadSave> GetCompleteOrders(string orderNo)
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                //string query = "select owners.owner_first_name as owner_first_name,owners.owner_last_name as owner_last_name, orders.ID as ID,orders.city as city,orders.created_on as order_date,orders.house_no as house_no,orders.house_unit as house_unit,orders.order_item_no as item_no,orders.order_no as order_no,orders.order_type as order_type,orders.remarks as aditional_notes,opt1.DISPLAY_NAME as search_type,state,orders.street_addr as street_add,orders.zip_code as zip_code,orders.status as order_status from order_details orders left join order_owner_details owners on orders.order_no=owners.order_no and orders.order_item_no=owners.order_item_no left join  Options opt1 on opt1.ID=orders.search_type where orders.order_no=@order_no and orders.status='C'";

                string query = "select owners.owner_first_name as owner_first_name,owners.owner_last_name as owner_last_name, orders.ID as ID,orders.city as city,orders.created_on as order_date,orders.house_no as house_no,orders.house_unit as house_unit,orders.order_item_no as item_no,orders.order_no as order_no,orders.order_type as order_type,orders.remarks as aditional_notes,opt1.DISPLAY_NAME as search_type,state,orders.street_addr as street_add,orders.zip_code as zip_code,orders.status as order_status,orders.county as county  from order_details orders left join order_owner_details owners on orders.order_no=owners.order_no and orders.order_item_no=owners.order_item_no left join  Options opt1 on opt1.ID=orders.search_type where orders.order_no=@order_no ";
                var saveOrder = db.Query<DatauploadSave>(query, new { order_no = orderNo }).ToList();
                return saveOrder;
            }
        }

        public List<InvoiceCompleteOrders> GetInvoiceCompleteOrders(string orderNo)
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                //string query = "select owners.owner_first_name as owner_first_name,owners.owner_last_name as owner_last_name, orders.ID as ID,orders.city as city,orders.created_on as order_date,orders.house_no as house_no,orders.house_unit as house_unit,orders.order_item_no as item_no,orders.order_no as order_no,orders.order_type as order_type,orders.remarks as aditional_notes,opt1.DISPLAY_NAME as search_type,state,orders.street_addr as street_add,orders.zip_code as zip_code,orders.status as order_status from order_details orders left join order_owner_details owners on orders.order_no=owners.order_no and orders.order_item_no=owners.order_item_no left join  Options opt1 on opt1.ID=orders.search_type where orders.order_no=@order_no and orders.status='C'";

                string query = "select owners.owner_first_name as owner_first_name,owners.owner_last_name as owner_last_name, orders.ID as ID,orders.city as city,orders.created_on as order_date,orders.house_no as house_no,orders.house_unit as house_unit,orders.order_item_no as item_no,orders.order_no as order_no,orders.order_type as order_type,orders.remarks as aditional_notes,opt1.DISPLAY_NAME as search_type,state,orders.street_addr as street_add,orders.zip_code as zip_code,orders.status as order_status,Invoice_Detail.amount_unit as price,Invoice_Detail.status,orders.county as county  from order_details orders left join order_owner_details owners on orders.order_no=owners.order_no and orders.order_item_no=owners.order_item_no left join  Options opt1 on opt1.ID=orders.search_type join Invoice_Detail on  orders.order_no=Invoice_Detail.order_no and orders.order_item_no=Invoice_Detail.order_item_no where orders.order_no=@order_no ";
                var saveOrder = db.Query<InvoiceCompleteOrders>(query, new { order_no = orderNo }).ToList();
                return saveOrder;
            }
        }
        public List<DatauploadSave> GetCompleteSerach(string orderNo)
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                // string query = "select owners.owner_first_name as owner_first_name,owners.owner_last_name as owner_last_name, orders.ID as ID,orders.city as city,orders.created_on as order_date,orders.house_no as house_no,orders.house_unit as house_unit,orders.order_item_no as item_no,orders.order_no as order_no,orders.order_type as order_type,orders.remarks as aditional_notes,opt1.DISPLAY_NAME as search_type,state,orders.street_addr as street_add,orders.zip_code as zip_code,orders.status as order_status from order_details orders left join order_owner_details owners on orders.order_no=owners.order_no and orders.order_item_no=owners.order_item_no left join  Options opt1 on opt1.ID=orders.search_type where orders.order_no=@order_no and orders.status='C'";
                string query = "select owners.owner_first_name as owner_first_name,owners.owner_last_name as owner_last_name, orders.ID as ID,orders.city as city,orders.created_on as order_date,orders.house_no as house_no,orders.house_unit as house_unit,orders.order_item_no as item_no,orders.order_no as order_no,orders.order_type as order_type,orders.remarks as aditional_notes,opt1.DISPLAY_NAME as search_type,state,orders.street_addr as street_add,orders.zip_code as zip_code,orders.status as order_status,orders.county as county from order_details orders left join order_owner_details owners on orders.order_no=owners.order_no and orders.order_item_no=owners.order_item_no left join  Options opt1 on opt1.ID=orders.search_type where orders.order_no=@order_no ";
                var saveOrder = db.Query<DatauploadSave>(query, new { order_no = orderNo }).ToList();
                return saveOrder;
            }
        }
        [TitleLeaderExceptionFilter]
        public List<DatauploadSave> GetOrdersItemno(string orderNo, string orderItemno, string fillter)
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {

                string itemfillter = "";
                switch (fillter)
                {
                    case "0":
                        itemfillter = "FORMAT(CONVERT(date,created_on,110),'MM-dd-yyyy') like'%" + orderItemno + "%'";
                        break;
                    case "1":
                        itemfillter = "orders.order_no like'%" + orderNo + "%'";
                        break;
                    case "2":
                        itemfillter = "orders.order_item_no like'%" + orderItemno + "%'";
                        break;
                    case "3":
                        itemfillter = "UPPER(owner_first_name) like'%" + orderItemno + "%' or UPPER(owner_last_name) like '%" + orderItemno + "%'";
                        break;
                    case "4":
                        itemfillter = "UPPER(city) like'%" + orderItemno + "%' or UPPER(house_no) like '%" + orderItemno + "%' or UPPER(house_unit) like '%" + orderItemno + "%' or  UPPER(state)  like '%" + orderItemno + "%' or UPPER(street_addr)  like '%" + orderItemno + "%' or UPPER(zip_code) like '%" + orderItemno + "%' or UPPER(county) like '%" + orderItemno + "%'";
                        break;
                }
                string query = "";
                query = " select owners.owner_first_name as owner_first_name,owners.owner_last_name as owner_last_name, orders.ID as ID,orders.city as city,orders.created_on as order_date,orders.house_no as house_no,orders.house_unit as house_unit,orders.order_item_no as item_no,orders.order_no as order_no,orders.order_type as order_type,orders.remarks as aditional_notes,opt1.DISPLAY_NAME as search_type,state,orders.street_addr as street_add,orders.zip_code as zip_code,orders.county as county from order_details orders left join order_owner_details owners on orders.order_no = owners.order_no and orders.order_item_no = owners.order_item_no  left join  Options opt1 on opt1.ID = orders.search_type " +
                    "where (orders.order_no=@orderno) and " + itemfillter + ";";
                var saveOrder = db.Query<DatauploadSave>(query, new { orderno = orderNo }).ToList();
                return saveOrder;
            }
        }
        public int CountOrder(string orderNo)
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string query = "select count(*) from order_details orders where orders.order_no=@order_no";
                int Count = db.QueryFirstOrDefault<int>(query, new { order_no = orderNo });
                return Count;
            }
        }
        public int CountOpenOrderSearch(string orderNo)
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string query = "select count(*) from order_details orders where orders.order_no=@order_no AND status='O'";
                int Count = db.QueryFirstOrDefault<int>(query, new { order_no = orderNo });
                return Count;
            }
        }
        public int CountOpenOrder()
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string query = "select count(*) from order_details orders where  status='O' and created_by=" + HttpContext.Current.User.Identity.Name + "";
                int Count = db.QueryFirstOrDefault<int>(query);
                return Count;
            }
        }
        public int CountCompleteOrderSearch(string orderNo)
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string query = "select (select count(*) from OrderResponse inner join OrderNotification  on OrderResponse.ResWareFileNumber=OrderNotification.ResWareFileNumber and OrderNotification.OrderNumber=orders.order_no  ) CompletedSearch from order_details orders where orders.order_no=@order_no";
                int Count = db.QueryFirstOrDefault<int>(query, new { order_no = orderNo });
                return Count;
            }
        }
        [TitleLeaderExceptionFilter]
        public string get_order_no()
        {                       
            string strOrderNo = "1";


            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string strQuery = "";
                strQuery = "select max(CAST(order_no as bigint) + 1) from order_details where created_by=" + HttpContext.Current.User.Identity.Name + "";
                string data = db.Query<string>(strQuery).Single();
                if (data != null)
                {
                    strOrderNo = data;
                }
            }
            return strOrderNo;
        }

        [TitleLeaderExceptionFilter]
        public string get_transaction_no()
        {
            string strTransactionNo = "100";


            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string strQuery = "";
                strQuery = "select max(CAST(Transaction_No as bigint) + 1) from Payment_Detail ";
                string data = db.Query<string>(strQuery).Single();
                if (data != null)
                {
                    strTransactionNo = data;
                }
            }
            return strTransactionNo;
        }

        [TitleLeaderExceptionFilter]
        public string get_filenumber(string order_no, string item_no)
        {
            string file_number = "";
            file_number = HttpContext.Current.User.Identity.Name.ToString() + "-" + order_no + "-" + item_no + "-" + DateTime.Now.ToString("MMddyy");         
            return file_number;
        }

        [TitleLeaderExceptionFilter]
        public string get_order_item_no(string strOrderno)
        {
            string strOrderItemNo = "1";
            if (strOrderno == "1")
            {
                strOrderItemNo = "1";
            }
            else
            {
                using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
                {
                    string strQuery = "";
                    strQuery = " select ISNULL(max(order_item_no)+1,1) from order_details where order_no = " + strOrderno + "";
                    string data = db.Query<string>(strQuery).Single();
                    strOrderItemNo = data;
                }
            }
            return strOrderItemNo;
        }
        [TitleLeaderExceptionFilter]
        public List<OpenOrderModels> SearchOpenOrders(string orderno, string fillter)
        {
            string search = "";
            switch (fillter)
            {
                case "0":
                    search = "FORMAT(CONVERT(date,created_on,110),'MM-dd-yyyy') like'%" + orderno + "%'";
                    break;
                case "1":
                    search = "o.order_no like '%" + orderno + "%'";
                    break;
                case "2":
                    search = "o.order_item_no like '%" + orderno + "%'";
                    break;
                case "3":
                    search = "UPPER(w.owner_first_name) like'%" + orderno.ToUpper() + "%' or UPPER(w.owner_last_name) like'%" + orderno.ToUpper() + "%'";
                    break;
                case "4":
                    search = "UPPER(o.house_no) like'%" + orderno.ToUpper() + "%' or UPPER(o.house_unit) like'%" + orderno.ToUpper() + "%' or UPPER(o.street_addr) like'%" + orderno.ToUpper() + "%' or UPPER(o.city) like'%" + orderno.ToUpper() + "%' or UPPER(o.state) like'%" + orderno.ToUpper() + "%' or UPPER(o.zip_code) like'%" + orderno.ToUpper() + "%' or UPPER(o.county) like'%" + orderno.ToUpper() + "%'";
                    break;
                default:
                    search = "FORMAT(CONVERT(date,created_on,110),'MM-dd-yyyy') like'%" + orderno + "%' or " +
                             "o.order_no like '%" + orderno + "%' or " +
                             "o.order_item_no like '%" + orderno + "%' or " +
                             "UPPER(w.owner_first_name) like'%" + orderno.ToUpper() + "%' or UPPER(w.owner_last_name) like'%" + orderno.ToUpper() + "%' or " +
                                "UPPER(o.house_no) like'%" + orderno.ToUpper() + "%' or UPPER(o.house_unit) like'%" + orderno.ToUpper() + "%' or UPPER(o.street_addr) like'%" + orderno.ToUpper() + "%' or UPPER(o.city) like'%" + orderno.ToUpper() + "%' or UPPER(o.state) like'%" + orderno.ToUpper() + "%' or UPPER(o.zip_code) like'%" + orderno.ToUpper() + "%' or UPPER(o.county) like'%" + orderno.ToUpper() + "%'";
                    break;
            }
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                //string query = "SELECT o.order_no as OrderNo,count(*) search,CONVERT(date, o.created_on) as OrderDate FROM order_details O left join order_owner_details w on o.order_no=w.order_no where o.created_by=" + HttpContext.Current.User.Identity.Name + " and " + search + " GROUP BY o.order_no,CONVERT(date, o.created_on);";

                string query = "SELECT o.order_no as OrderNo,count(*) search,CONVERT(date, o.created_on) as OrderDate FROM order_details O left join order_owner_details w on o.order_no=w.order_no  where O.order_no NOT IN (select distinct OrderNumber from OrderNotification) and  o.created_by=" + HttpContext.Current.User.Identity.Name + " and " + search + " GROUP BY o.order_no,CONVERT(date, o.created_on) order by o.order_no desc;";
                var openOrder = db.Query<OpenOrderModels>(query).ToList();
                return openOrder;
            }
        }
        [TitleLeaderExceptionFilter]
        public int CountOrderNo(int id)
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string query = "SELECT COUNT(*) FROM order_details WHERE order_item_no='1' and created_by=@Id";
                int ordcount = db.QueryFirstOrDefault<int>(query, new { Id = id, Status = true });
                return ordcount;
            }
        }
        public List<OrderItems> GetItemNo(string strOrderno)
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                db.Open();
                string query = "SELECT order_item_no  FROM order_details WHERE  order_no=@order_no AND status='D'";
                var orderITem = db.Query<OrderItems>(query, new { order_no = strOrderno }).ToList();
                return orderITem;
            }
        }
        public int DeleteOrder(string strOrderno, string strOrderItemno)
        {
            string query1 = "DELETE FROM order_details WHERE order_no=@order_no and order_item_no=@order_item_no";
            string query2 = "DELETE FROM order_owner_details WHERE order_no=@order_no and order_item_no=@order_item_no";
            int cnt = 0;
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                db.Open();

                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var rowseffects1 = db.Execute(query1, new { order_no = strOrderno, order_item_no = strOrderItemno }, transaction: transaction);
                        var rowseffects2 = db.Execute(query2, new { order_no = strOrderno, order_item_no = strOrderItemno }, transaction: transaction);

                        transaction.Commit();
                        cnt = 1;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
                }

            }
            return cnt;

        }


        public int UpdateOrderDraft(string strOrderno, string strOrderItemno)
        {
            string query1 = "UPDATE order_details SET STATUS='D' WHERE order_no=@order_no and order_item_no=@order_item_no";

            int cnt = 0;
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                db.Open();

                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var rowseffects1 = db.Execute(query1, new { order_no = strOrderno, order_item_no = strOrderItemno }, transaction: transaction);
                        transaction.Commit();
                        cnt = 1;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
                }

            }
            return cnt;

        }
        public bool PlaceOrderForClient(DatauploadSave uploadOrder)
        {
            OrderRequest orderRequest = new OrderRequest();
            OrderPlacementServicePropertyAddress propertydetails = new OrderPlacementServicePropertyAddress();
            propertydetails.AddressStreetInfo = "";
            propertydetails.City = uploadOrder.city;
            //propertydetails.County = "USA";
            propertydetails.County = uploadOrder.county;
            propertydetails.Description = uploadOrder.aditional_notes;
            propertydetails.LegalDescription = "";
            propertydetails.State = uploadOrder.state;
            propertydetails.StreetName = uploadOrder.street_add;
            propertydetails.StreetNumber = uploadOrder.house_no;
            propertydetails.Unit = uploadOrder.house_unit;
            propertydetails.Zip = uploadOrder.zip_code;

            PlaceOrderResponse response = orderRequest.PlaceOrder(57830, 1, "", propertydetails, 4856243, 0, 0, 0, 2404165, DateTime.Now, 0, 0, "", 0, null, null, null, false, null, null, null
                 , null, uploadOrder.aditional_notes, false, 0, 0);
            if (response.ResponseCode == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<OpenOrderModels> GetSavedLaterOrder()
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string query = "SELECT o.order_no as OrderNo,count(*) search,CONVERT(VARCHAR(10), CONVERT(date, o.created_on), 101) as OrderDate FROM order_details O where o.status='D' and o.created_by=" + HttpContext.Current.User.Identity.Name + " GROUP BY o.order_no,CONVERT(date, o.created_on);";
                var openOrder = db.Query<OpenOrderModels>(query).ToList();
                return openOrder;
            }
        }
        public List<OpenOrderModels> GetSavedLaterOrder(string ordnos, string fillter)
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string search = "";
                switch (fillter)
                {
                    case "0":
                        search = "FORMAT(CONVERT(date,created_on,110),'MM-dd-yyyy') like'%" + ordnos + "%'";
                        break;
                    case "1":
                        search = "o.order_no like '%" + ordnos + "%'";
                        break;
                    case "2":
                        search = "o.order_item_no like '%" + ordnos + "%'";
                        break;
                    case "3":
                        search = "UPPER(w.owner_first_name) like'%" + ordnos.ToUpper() + "%' or UPPER(w.owner_last_name) like'%" + ordnos.ToUpper() + "%'";
                        break;
                    case "4":
                        search = "UPPER(o.house_no) like'%" + ordnos.ToUpper() + "%' or UPPER(o.house_unit) like'%" + ordnos.ToUpper() + "%' or UPPER(o.street_addr) like'%" + ordnos.ToUpper() + "%' or UPPER(o.city) like'%" + ordnos.ToUpper() + "%' or UPPER(o.state) like'%" + ordnos.ToUpper() + "%' or UPPER(o.zip_code) like'%" + ordnos.ToUpper() + "%' or UPPER(o.county) like'%" + ordnos.ToUpper() + "%'";
                        break;
                    default:
                        search = "FORMAT(CONVERT(date,created_on,110),'MM-dd-yyyy') like'%" + ordnos + "%' or " +
                            "o.order_no like '%" + ordnos + "%' or " +
                            "o.order_item_no like '%" + ordnos + "%' or " +
                            "UPPER(w.owner_first_name) like'%" + ordnos.ToUpper() + "%' or UPPER(w.owner_last_name) like'%" + ordnos.ToUpper() + "%' or " +
                            "UPPER(o.house_no) like'%" + ordnos.ToUpper() + "%' or UPPER(o.house_unit) like'%" + ordnos.ToUpper() + "%' or UPPER(o.street_addr) like'%" + ordnos.ToUpper() + "%' or UPPER(o.city) like'%" + ordnos.ToUpper() + "%' or UPPER(o.state) like'%" + ordnos.ToUpper() + "%' or UPPER(o.zip_code) like'%" + ordnos.ToUpper() + "%' or UPPER(o.county) like'%" + ordnos.ToUpper() + "%'";
                        break;
                }
                string query = "SELECT o.order_no as ordnos,count(*) search,CONVERT(date, o.created_on) as OrderDate FROM order_details O left join order_owner_details w on o.order_no=w.order_no where o.created_by=" + HttpContext.Current.User.Identity.Name + " and " + search + " GROUP BY o.order_no,CONVERT(date, o.created_on);";
                var openOrder = db.Query<OpenOrderModels>(query).ToList();
                return openOrder;
            }
        }
        public List<DatauploadSave> GetReturnedOrder()
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string query = "select   order_no, order_item_no as item_no, street_addr, house_unit, city, state,county, zip_code from order_details WHERE status='D' and created_by=" + HttpContext.Current.User.Identity.Name + "";
                var returnedOrder = db.Query<DatauploadSave>(query).ToList();
                return returnedOrder;
            }
        }
        public List<DatauploadSave> GetReturnedOrderDahsboard()
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string query = "select top 6 order_no, order_item_no, street_addr, house_unit, city, state,county, zip_code from order_details WHERE status='D' and created_by=" + HttpContext.Current.User.Identity.Name + " order by created_on desc";
                var returnedOrder = db.Query<DatauploadSave>(query).ToList();
                return returnedOrder;
            }
        }
        public List<DatauploadSave> GetSaveForLaterDahsboard()
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string query = "select top 6 order_no, order_item_no, street_addr, house_unit, city, state,county, zip_code from order_details WHERE status='D' and created_by=" + HttpContext.Current.User.Identity.Name + " order by created_on desc";
                var returnedOrder = db.Query<DatauploadSave>(query).ToList();
                return returnedOrder;
            }
        }
        //public List<OpenOrderModels> GetComplatedOrderDashboard()
        //{
        //    using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
        //    {
        //        string query = "SELECT top 6 o.order_no as OrderNo,count(*) search,CONVERT(date, o.created_on) as OrderDate FROM order_details O where o.status='C' and o.created_by=" + HttpContext.Current.User.Identity.Name + " GROUP BY o.order_no,CONVERT(date, o.created_on) order by OrderDate desc";

        //        var openOrder = db.Query<OpenOrderModels>(query).ToList();
        //        return openOrder;
        //    }
        //}
        public List<OpenOrderModels> GetComplatedOrderDashboard()
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string query = "SELECT top 6 O.order_no as OrderNo,(select count(*) from OrderResponse inner join OrderNotification  on OrderResponse.ResWareFileNumber=OrderNotification.ResWareFileNumber and OrderNotification.OrderNumber=O.order_no  ) CompletedSearch,(select count(*) from order_details where order_details.order_no=O.order_no ) Search,CONVERT(date, O.created_on) as OrderDate FROM order_details O inner join OrderNotification N on   O.order_no =N.OrderNumber where O.created_by=" + HttpContext.Current.User.Identity.Name + " GROUP BY O.order_no,CONVERT(date, O.created_on) order by OrderDate desc";

                var openOrder = db.Query<OpenOrderModels>(query).ToList();
                return openOrder;
            }
        }
        public List<DatauploadSave> SearchReturnedOrder(string searckey, string fillter)
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string itemfillter = "";
                switch (fillter)
                {
                    case "0":
                        itemfillter = "FORMAT(CONVERT(date,created_on,110),'MM-dd-yyyy') like'%" + searckey + "%'";
                        break;
                    case "1":
                        itemfillter = "orders.order_no like'%" + searckey + "%'";
                        break;
                    case "2":
                        itemfillter = "orders.order_item_no like'%" + searckey + "%'";
                        break;
                    case "3":
                        itemfillter = "UPPER(owner_first_name) like'%" + searckey + "%' or UPPER(owner_last_name) like '%" + searckey + "%'";
                        break;
                    case "4":
                        itemfillter = "UPPER(city) like'%" + searckey + "%' or UPPER(house_no) like '%" + searckey + "%' or UPPER(house_unit) like '%" + searckey + "%' or UPPER(state) like '%" + searckey + "%' or UPPER(street_addr) like '%" + searckey + "%' or UPPER(zip_code) like '%" + searckey + "%' or UPPER(county) like '%" + searckey + "%'";
                        break;
                }
                string query = "";
                query = " select owners.owner_first_name as owner_first_name,owners.owner_last_name as owner_last_name, orders.ID as ID,orders.city as city,orders.created_on as order_date,orders.house_no as house_no,orders.house_unit as house_unit,orders.order_item_no as item_no,orders.order_no as order_no,orders.order_type as order_type,orders.remarks as aditional_notes,opt1.DISPLAY_NAME as search_type,state,orders.street_addr as street_add,orders.zip_code as zip_code,orders.county as county from order_details orders left join order_owner_details owners on orders.order_no = owners.order_no and orders.order_item_no = owners.order_item_no  left join  Options opt1 on opt1.ID = orders.search_type " +
                    "where  created_by=@created_by and " + itemfillter + ";";

                var saveOrder = db.Query<DatauploadSave>(query, new
                {
                    created_by = HttpContext.Current.User.Identity.Name
                }).ToList();
                return saveOrder;
            }
        }
        //public List<OpenOrderModels> GetCompleteOrder()
        //{
        //    using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
        //    {
        //        string query = "SELECT o.order_no as OrderNo,count(*) search,CONVERT(VARCHAR(10), CONVERT(date, o.created_on), 101) as OrderDate FROM order_details O where o.status='C' and o.created_by=" + HttpContext.Current.User.Identity.Name + " GROUP BY o.order_no,CONVERT(date, o.created_on);";
        //        var openOrder = db.Query<OpenOrderModels>(query).ToList();
        //        return openOrder;
        //    }
        //}
        public List<OpenOrderModels> GetCompleteOrder()
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string query = "SELECT o.order_no as OrderNo,count(*) search,CONVERT(VARCHAR(10), CONVERT(date, o.created_on), 101) as OrderDate FROM order_details O inner join OrderNotification N on   O.order_no =N.OrderNumber  where o.created_by=" + HttpContext.Current.User.Identity.Name + " GROUP BY o.order_no,CONVERT(date, o.created_on) order by o.order_no desc;";
                var openOrder = db.Query<OpenOrderModels>(query).ToList();
                return openOrder;
            }
        }
        public List<OpenOrderModels> GetOrderNotification()
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string query = "SELECT o.order_no as OrderNo,count(*) search,CONVERT(VARCHAR(10), CONVERT(date, o.created_on), 101) as OrderDate FROM order_details O inner join OrderNotification N on   O.order_no =N.OrderNumber and N.DisplayStatus='0'  where o.created_by=" + HttpContext.Current.User.Identity.Name + " GROUP BY o.order_no,CONVERT(date, o.created_on);";
                var openOrder = db.Query<OpenOrderModels>(query).ToList();
                return openOrder;
            }
        }
        //public List<OpenOrderModels> GetCompleteOrder(string orderno, string fillter)
        //{
        //    using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
        //    {
        //        string search = "";
        //        switch (fillter)
        //        {
        //            case "0":
        //                search = "FORMAT(CONVERT(date,created_on,110),'MM-dd-yyyy') like'%" + orderno + "%'";
        //                break;
        //            case "1":
        //                search = "o.order_no like '%" + orderno + "%'";
        //                break;
        //            case "2":
        //                search = "o.order_item_no like '%" + orderno + "%'";
        //                break;
        //            case "3":
        //                search = "UPPER(w.owner_first_name) like'%" + orderno.ToUpper() + "%' or UPPER(w.owner_last_name) like'%" + orderno.ToUpper() + "%'";
        //                break;
        //            case "4":
        //                search = "UPPER(o.house_no) like'%" + orderno.ToUpper() + "%' or UPPER(o.house_unit) like'%" + orderno.ToUpper() + "%' or UPPER(o.street_addr) like'%" + orderno.ToUpper() + "%' or UPPER(o.city) like'%" + orderno.ToUpper() + "%' or UPPER(o.state) like'%" + orderno.ToUpper() + "%' or UPPER(o.zip_code) like'%" + orderno.ToUpper() + "%'";
        //                break;
        //        }
        //        string query = "SELECT o.order_no as orderno,count(*) search,CONVERT(date, o.created_on) as OrderDate FROM order_details O left join order_owner_details w on o.order_no=w.order_no where o.created_by=" + HttpContext.Current.User.Identity.Name + " and " + search + " GROUP BY o.order_no,CONVERT(date, o.created_on);";
        //        var openOrder = db.Query<OpenOrderModels>(query).ToList();
        //        return openOrder;
        //    }
        //}
        public List<OpenOrderModels> GetCompleteOrder(string orderno, string fillter)
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string search = "";
                switch (fillter)
                {
                    case "0":
                        search = "FORMAT(CONVERT(date,created_on,110),'MM-dd-yyyy') like'%" + orderno + "%'";
                        break;
                    case "1":
                        search = "o.order_no like '%" + orderno + "%'";
                        break;
                    case "2":
                        search = "o.order_item_no like '%" + orderno + "%'";
                        break;
                    case "3":
                        search = "UPPER(w.owner_first_name) like'%" + orderno.ToUpper() + "%' or UPPER(w.owner_last_name) like'%" + orderno.ToUpper() + "%'";
                        break;
                    case "4":
                        search = "UPPER(o.house_no) like'%" + orderno.ToUpper() + "%' or UPPER(o.house_unit) like'%" + orderno.ToUpper() + "%' or UPPER(o.street_addr) like'%" + orderno.ToUpper() + "%' or UPPER(o.city) like'%" + orderno.ToUpper() + "%' or UPPER(o.state) like'%" + orderno.ToUpper() + "%' or UPPER(o.zip_code) like'%" + orderno.ToUpper() + "%' or UPPER(o.county) like'%" + orderno.ToUpper() + "%'";
                        break;
                }
                string query = "SELECT o.order_no as orderno,count(*) search,CONVERT(date, o.created_on) as OrderDate FROM order_details O  left join order_owner_details w on o.order_no=w.order_no inner join OrderNotification N on   O.order_no =N.OrderNumber where o.created_by=" + HttpContext.Current.User.Identity.Name + " and " + search + " GROUP BY o.order_no,CONVERT(date, o.created_on) order by o.order_no desc;";
                var openOrder = db.Query<OpenOrderModels>(query).ToList();
                return openOrder;
            }
        }
        public string ReadExcelFiles(HttpPostedFileBase uploadFile)
        {
            string filePath = string.Empty;
            if (uploadFile != null)
            {
                string path = HttpContext.Current.Server.MapPath("~/Content/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                filePath = path + Path.GetFileName(uploadFile.FileName);
                string extension = Path.GetExtension(uploadFile.FileName);
                uploadFile.SaveAs(filePath);

                string conString = string.Empty;
                switch (extension)
                {
                    case ".xls": //Excel 97-03.
                        conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                        break;
                    case ".xlsx": //Excel 07 and above.
                        conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                        break;
                }

                conString = string.Format(conString, filePath);
                DataTable dtProperties = new DataTable();
               // DataTable dtOwners = new DataTable();
                using (OleDbConnection connExcel = new OleDbConnection(conString))
                {
                    using (OleDbCommand cmdExcel = new OleDbCommand())
                    {
                        using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                        {
                            cmdExcel.Connection = connExcel;

                            //Get the name of First Sheet.
                            connExcel.Open();
                            DataTable dtExcelSchema;
                            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            string sheetName1 = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                           // string sheetName2 = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                            connExcel.Close();

                            //Read Data from First Sheet.
                            connExcel.Open();
                            cmdExcel.CommandText = "SELECT * From [" + sheetName1 + "]";
                            odaExcel.SelectCommand = cmdExcel;
                            odaExcel.Fill(dtProperties);
                            //cmdExcel.CommandText = "SELECT * From [" + sheetName2 + "]";
                            //odaExcel.SelectCommand = cmdExcel;
                            //odaExcel.Fill(dtOwners);
                            connExcel.Close();
                        }
                    }
                }
                List<DatauploadSubmit> dataUploadInsert = new List<DatauploadSubmit>();
                foreach (DataRow row in dtProperties.Rows)
                {
                    DatauploadSubmit dataUpload = new DatauploadSubmit();
                    dataUpload.aditional_notes = row["NOTES"].ToString();
                    dataUpload.city = row["CITY"].ToString();
                    dataUpload.house_no = row["HOUSE_NUMBER"].ToString();
                    dataUpload.house_unit = row["UNIT"].ToString();
                    dataUpload.order_date = DateTime.Now;
                    dataUpload.order_type = "EXCEL";
                    //dataUpload.search_type = row["SEARCH_TYPE"].ToString();
                    dataUpload.search_type = "3";
                    dataUpload.state = row["STATE"].ToString();
                    dataUpload.county = row["COUNTY"].ToString();
                    dataUpload.street_addr = row["STREET_NAME"].ToString();
                    dataUpload.zip_code = row["ZIP"].ToString();
                    List<Owner> Owners = new List<Owner>();
                    //foreach (DataRow crow in dtOwners.AsEnumerable().Where(q => q["PRO_ID"].ToString() == row["ID"].ToString()))
                    //{
                    string[] OWNERS_NAME = row["OWNERS_NAME"].ToString().Split(',');
                    foreach (string OWNER_NAME in OWNERS_NAME)
                    {
                        string[] OWNER_NAME_AR = OWNER_NAME.ToString().Split(' ');
                        string OWNER_FIRST_NAME="";
                        string OWNER_LAST_NAME = "";
                       
                         int ARCount = 1;
                        foreach (string NAME in OWNER_NAME_AR)
                        {
                            if (ARCount == 1)
                            {
                                OWNER_FIRST_NAME = OWNER_NAME_AR[0].ToString();
                            }
                            else if (ARCount == 2)
                            {
                                OWNER_LAST_NAME = OWNER_NAME_AR[1].ToString();
                            }
                            ARCount = ARCount + 1;
                        }
                        Owner owner = new Owner();
                        owner.owner_first_name = OWNER_FIRST_NAME;
                        owner.owner_last_name = OWNER_LAST_NAME;
                        Owners.Add(owner);
                    }
                        
                    //}
                    dataUpload.Owners = Owners;
                    dataUploadInsert.Add(dataUpload);
                }
                string orderno = InsertOrders(dataUploadInsert);
                if (string.IsNullOrEmpty(orderno))
                {
                    return orderno;
                }
                else
                {
                    return orderno;
                }
            }
            return "0";
        }

        public List<Invoice> GetInvoice(string strsearch, string fillter)
        {

            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
               
                    string search = "";
                    switch (fillter)
                    {
                        case "0":
                            search = "FORMAT(CONVERT(date,created_on,110),'MM-dd-yyyy') like'%" + strsearch + "%'";
                            break;
                        case "1":
                            search = "order_no like '%" + strsearch + "%'";
                            break;
                        case "2":
                            search = "inv_no like '%" + strsearch + "%'";
                            break;
                    }
                    string query = "";
                    if (search == "" && fillter == "")
                    {
                        query = "select (select count(*) from Invoice_Detail where Invoice_Detail.order_no=A.order_no  ) search,(select distinct created_on from order_details where order_details.order_no=A.order_no and order_details.created_by=" + HttpContext.Current.User.Identity.Name + " ) order_date,(select distinct created_on from order_details where order_details.order_no=A.order_no and order_details.created_by=" + HttpContext.Current.User.Identity.Name + " ) created_on,A.order_no,A.inv_no,SUM(A.total_amount) total_amount,(select SUM(total_amount) from Invoice_Detail where Invoice_Detail.order_no=A.order_no and Invoice_Detail.status<>'P' and Invoice_Detail.created_by=" + HttpContext.Current.User.Identity.Name + "  ) amount_due,(select SUM(total_amount) from Invoice_Detail where Invoice_Detail.order_no=A.order_no and Invoice_Detail.status='P' and Invoice_Detail.created_by=" + HttpContext.Current.User.Identity.Name + " ) amount_paid,(select distinct status from Invoice_Detail where Invoice_Detail.order_no=A.order_no and Invoice_Detail.status<>'P' and Invoice_Detail.created_by=" + HttpContext.Current.User.Identity.Name + " ) status from Invoice_Detail A where   created_by=" + HttpContext.Current.User.Identity.Name + " GROUP BY A.inv_no,A.order_no;";
                    }
                    else
                    {
                        query = "select (select count(*) from Invoice_Detail where Invoice_Detail.order_no=A.order_no ) search,(select distinct created_on from order_details where order_details.order_no=A.order_no and  order_details.created_by=" + HttpContext.Current.User.Identity.Name + "  ) order_date,(select distinct created_on from order_details where order_details.order_no=A.order_no and order_details.created_by=" + HttpContext.Current.User.Identity.Name + " ) created_on,A.order_no,A.inv_no,SUM(A.total_amount) total_amount,(select SUM(total_amount) from Invoice_Detail where Invoice_Detail.order_no=A.order_no and Invoice_Detail.status<>'P' and Invoice_Detail.created_by=" + HttpContext.Current.User.Identity.Name + " ) amount_due,(select SUM(total_amount) from Invoice_Detail where Invoice_Detail.order_no=A.order_no and Invoice_Detail.status='P' and Invoice_Detail.created_by=" + HttpContext.Current.User.Identity.Name + " ) amount_paid,(select distinct status from Invoice_Detail where Invoice_Detail.order_no=A.order_no and Invoice_Detail.status<>'P' and Invoice_Detail.created_by=" + HttpContext.Current.User.Identity.Name + " ) status from Invoice_Detail A where  " + search + " and created_by=" + HttpContext.Current.User.Identity.Name + " GROUP BY A.inv_no,A.order_no;";
                    }
                    var invoice = db.Query<Invoice>(query).ToList();
                    return invoice;
                }
            
        }

        public List<Invoice> GetInvoicePrice(string strsearch, int strsearchitemnumber, string fillter)
        {

            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string query1 = "SELECT COUNT(*) FROM user_override_search_rate WHERE user_id=@user_id";
                int userCount = db.QueryFirstOrDefault<int>(query1, new { user_id = Convert.ToInt32(HttpContext.Current.User.Identity.Name) });
                if (userCount > 0)
                {
                    string search = "";
                    switch (fillter)
                    {
                        
                        case "1":
                          search = "order_no like '%" + strsearch + "%' and order_item_no like '%" + strsearchitemnumber + "%'";
                            break;
                       
                    }
                    string query = "";

                    query = "select  rate,gap_rate  from order_details,user_override_search_rate where  " + search + " and user_override_search_rate.state=order_details.state and user_override_search_rate.county=order_details.county and user_override_search_rate.search_type=order_details.search_type  and order_details.created_by=" + HttpContext.Current.User.Identity.Name + ";";
                   
                    var invoice = db.Query<Invoice>(query).ToList();
                    return invoice;
                }
                else
                {
                    string search = "";
                    switch (fillter)
                    {
                       
                        case "1":
                            search = "order_no like '%" + strsearch + "%' and order_item_no like '%" + strsearchitemnumber + "%'";
                            break;
                       
                    }
                    string query = "";

                    query = "select  rate,gap_rate  from order_details,search_rate where  " + search + " and search_rate.state=order_details.state and search_rate.county=order_details.county and search_rate.search_type=order_details.search_type  and order_details.created_by=" + HttpContext.Current.User.Identity.Name + ";";
                   
                    var invoice = db.Query<Invoice>(query).ToList();
                    return invoice;
                }
            }
        }

        
        public List<CardMaster> GetCardDetails()
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string query = " select Id, CardNumber, ExpMonthYear, NameOnCard, CCV,Zip  from cardmaster where UserId ='" + HttpContext.Current.User.Identity.Name + "'";
                var carddetails = db.Query<CardMaster>(query).ToList();
                return carddetails;
              
            }
        }
        public List<UserMaster> GetUserDetails()
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string query = " select * from UserMaster where Id ='" + HttpContext.Current.User.Identity.Name + "'";
                var userdetails = db.Query<UserMaster>(query).ToList();
                return userdetails;
            }
        }

        public bool ChangeProfilePhoto(HttpPostedFileBase ProfilePhoto)
        {
            if (ProfilePhoto != null)
            {
                byte[] data = null;
                using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
                {
                    using (MemoryStream target = new MemoryStream())
                    {
                        ProfilePhoto.InputStream.CopyTo(target);
                        data = target.ToArray();
                    }
                    string query = "UPDATE UserMaster SET PHOTO=@PHOTO WHERE Id=@Id";

                    var result = db.Execute(query, new
                    {
                        PHOTO = data,
                        Id = HttpContext.Current.User.Identity.Name
                    });

                    if (result > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        public ProfileImage GetProfileImage(string userid)
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string query = "SELECT Id,PHOTO FROM UserMaster WHERE Id=@id";
                var reuslt = db.QueryFirstOrDefault<ProfileImage>(query, new
                {
                    id = userid
                });
                return reuslt;
            }
        }
        public int PwdExists(string strOldPwd)
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string query = "SELECT COUNT(*) FROM UserLoginMaster WHERE Id=@Id and Password=@Password";
                var cnt = db.QueryFirstOrDefault<int>(query, new { Id = HttpContext.Current.User.Identity.Name, Password = strOldPwd });
                return cnt;
            }
        }
        public int ResetPassword(string strNewpwd)
        {
            string query1 = "UPDATE UserLoginMaster SET Password=@Password WHERE Id=@Id";

            int cnt = 0;
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                db.Open();

                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var rowseffects1 = db.Execute(query1, new { Password = strNewpwd, Id = HttpContext.Current.User.Identity.Name }, transaction: transaction);


                        transaction.Commit();
                        cnt = 1;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
                }

            }
            return cnt;

        }

        public int UpdateProfile(string strName, string strCompany, string strTitle, string strPhone, string strEmail, string strAddress)
        {
            string[] words = strName.Split(' ');
            string fname = "";
            string lname = "";
            if (words.Length == 1)
            {
                fname = words[0].ToString();
            }
            else if (words.Length == 2)
            {
                fname = words[0].ToString();
                lname = words[1].ToString();
            }
            else
            {
                fname = words[0].ToString();
                lname = words[1].ToString() + words[1].ToString();

            }
            string query1 = "update UserMaster SET firstname=@firstname,lastname=@lastname,companyname=@companyname,title=@title,phonenumber=@phonenumber,email=@email,address=@address where id=@id";

            int cnt = 0;
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                db.Open();

                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var rowseffects1 = db.Execute(query1, new { firstname = fname, lastname = lname, companyname = strCompany, title = strTitle, phonenumber = strPhone, email = strEmail, address = strAddress, Id = HttpContext.Current.User.Identity.Name }, transaction: transaction);


                        transaction.Commit();
                        cnt = 1;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
                }

            }
            return cnt;

        }

        public int UpdateCard(string strCardNo, string strNameoncard, string strExpiary, string strZipcode)
        {
            //int id = get_cardid();

            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string query = "INSERT INTO  cardmaster(CardNumber,NameOnCard,ExpMonthYear,Zip,UserId,CreateDate,UpdateDate,CCV,BillingAddress,City,StateId,Status) Values(@CardNumber,@NameOnCard,@ExpMonthYear,@Zip,@UserId,@CreateDate,@UpdateDate,@CCV,@BillingAddress,@City,@StateId,@Status)";
                var returnId = db.Query<int>(query, new { CardNumber = strCardNo, NameOnCard = strNameoncard, ExpMonthYear = strExpiary, Zip = strZipcode, UserId = HttpContext.Current.User.Identity.Name, CreateDate = DateTime.Now, UpdateDate = DateTime.Now, CCV = "DUMMY", BillingAddress = "DUMMY", City = "DUMMY", StateId = "1", Status = "1" }).SingleOrDefault();
                return returnId;
            }
        }
        public int DeleteCardDetails(string itemno)
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string query = "DELETE FROM CardMaster WHERE Id=@Id AND UserId = '" + HttpContext.Current.User.Identity.Name + "'";
                var states = db.Execute(query, new { Id = itemno });
                return states;
            }
        }
        public int InsertTeam(Addteames obj)
        {
            int returnId = 0;
            if (obj.fupload != null)
            {
                byte[] data = null;
                using (MemoryStream target = new MemoryStream())
                {
                    obj.fupload.InputStream.CopyTo(target);
                    data = target.ToArray();
                }

                string password = FormsAuthentication.HashPasswordForStoringInConfigFile(obj.strpassword.Trim(), "MD5");
                using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
                {
                    string query = "INSERT INTO t_team(strname,stremail,strpassword,status,fupload,created_by,create_on) values(@strname,@stremail,@strpassword,@status,@fupload,@created_by,@create_on)";
                    returnId = db.Query<int>(query, new { strname = obj.strname, stremail = obj.stremail, strpassword = password, status=true, fupload = data, created_by = HttpContext.Current.User.Identity.Name, create_on = DateTime.Now }).SingleOrDefault();
                    return returnId;
                }
            }
            return returnId;
        }
        public List<Addteames> GetTeamDetails()
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string query = " select id,strname,fupload Photo from t_team WHERE created_by ='" + HttpContext.Current.User.Identity.Name + "'";
                var teamdetails = db.Query<Addteames>(query).ToList();
                return teamdetails;
            }
        }
        public int get_cardid()
        {
            int data = 0;
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string strQuery = "";
                strQuery = " select ISNULL(max(ID)+1,1) from cardmaster where UserId = " + HttpContext.Current.User.Identity.Name + "";
                data = db.Query<int>(strQuery).Single();
            }
            return data;
        }

        public List<OrderResponse> OrderConfirmation(int orderno, List<string> item_nos)
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {

                string in_query = item_nos.Aggregate((x, y) => x + "," + y).TrimEnd(new char[] { ',' });

                string query1 = "SELECT T1.ID,T1.order_no AS order_no,t1.order_item_no as item_no,T1.order_type as order_type,T1.search_type as search_type,T1.house_no as house_no,T1.house_unit as house_unit,T1.street_addr as street_addr,T1.city as city,T1.state as state,T1.county as county,T1.zip_code as zip_code ,T1.remarks as aditional_notes,t1.street_no as street_no FROM ORDER_DETAILS T1 where t1.order_no=@order_no and t1.order_item_no in(" + in_query + ");";
                string query2 = "SELECT t2.order_item_no as item_no, T2.owner_first_name,T2.owner_last_name FROM ORDER_OWNER_DETAILS T2 WHERE T2.order_no=@order_no and t2.order_item_no=@order_item_no";
                string query3 = "select t1.Username as Username,t2.Firstname as Firstname,t2.Lastname as Lastname,t2.CompanyName as CompanyName,t2.PhoneNumber as PhoneNumber," +
                    "t2.Email as Email,t2.Address as Address,t2.City as City,t2.State as state,t2.Zip as Zip from usermaster t2 inner join " +
                    "UserLoginMaster t1 on t1.id=t2.id where t1.id=@id and t1.status=@status";
                var orders = db.Query<OrderSubmit>(query1, new { order_no = orderno }).ToList();
                var userdetails = db.QueryFirstOrDefault<UserMasterSubmit>(query3, new { id = HttpContext.Current.User.Identity.Name, status = true });
                List<OrdersItems> ordersItemslist = new List<OrdersItems>();
                foreach (var item in orders)
                {
                    OrdersItems ordersItems = new OrdersItems();
                    var items = db.Query<ItemSubmit>(query2, new { order_no = item.order_no, order_item_no = item.item_no }).ToList();
                    ordersItems.Orders = item;
                    ordersItems.Items = items;
                    ordersItemslist.Add(ordersItems);
                }
                List<OrderResponse> orderResponses = new List<OrderResponse>();
                OrdersRequirement ordersRequirement = new OrdersRequirement();
                foreach (var prow in ordersItemslist)
                {
                    ordersRequirement = new OrdersRequirement();
                    ordersRequirement.OrderNumber = prow.Orders.order_no;
                    ordersRequirement.ItemNumber = prow.Orders.item_no;
                    ordersRequirement.OrderAddress = new List<OrderPlacementServicePropertyAddress>();
                    ordersRequirement.ByerAddress = new List<OrderPlacementServiceBuyerSeller>();
                    ordersRequirement.SellerAddress = new List<OrderPlacementServiceBuyerSeller>();
                    ordersRequirement.OrderAddress.Add(new OrderPlacementServicePropertyAddress
                    {
                        AddressStreetInfo = prow.Orders.street_addr,
                        City = prow.Orders.city,
                        State = prow.Orders.state,
                     //   County = "woodford",
                        County = prow.Orders.county,
                        Description = prow.Orders.aditional_notes,
                        LegalDescription = "",
                        StreetName = prow.Orders.street_addr,
                        StreetNumber = prow.Orders.street_no,
                        StreetSuffix = "",
                        Unit = prow.Orders.house_unit,
                        Zip = prow.Orders.zip_code
                    });

                    ordersRequirement.ByerAddress.Add(new OrderPlacementServiceBuyerSeller
                    {
                        Address = new OrderPlacementServiceAddress
                        {
                            City = userdetails.City,
                            AddressStreetInfo = userdetails.Address,
                            State = userdetails.state,
                            Zip = userdetails.Zip
                        },
                        Email = userdetails.Email,
                        FirstName = userdetails.Firstname,
                        LastName = userdetails.Lastname,
                        MaritalStatus = "",
                        Phone = userdetails.PhoneNumber,
                    });

                    string item_no = "";

                    foreach (var item1 in prow.Items)
                    {

                        ordersRequirement.SellerAddress.Add(new OrderPlacementServiceBuyerSeller
                        {
                            Address = new OrderPlacementServiceAddress
                            {
                                City = prow.Orders.city,
                                AddressStreetInfo = prow.Orders.street_addr,
                                State = prow.Orders.state,
                                Zip = prow.Orders.zip_code
                            },
                            Email = "",
                            FirstName = item1.owner_first_name,
                            LastName = item1.owner_last_name,
                            MaritalStatus = "",
                            Phone = ""
                        });

                        item_no = item1.item_no.ToString();
                    }
                    ordersRequirement.FileNumber = get_filenumber(prow.Orders.order_no.ToString(), item_no);
                    ordersRequirement.ClientId = 4757066;
                    ordersRequirement.ClientsClientID = 1838;                    
                    ordersRequirement.OfficeId = 1;
                    ordersRequirement.PrimaryContactID = 4757407;
                    ordersRequirement.ProductID = Convert.ToInt32(prow.Orders.search_type);
                    switch (ordersRequirement.ProductID)
                    {
                        case 1193:
                            ordersRequirement.ProductID = 4;
                            break;

                        case 1209:
                            ordersRequirement.ProductID = 1;
                            break;

                        case 1385:
                            ordersRequirement.ProductID = 14;
                            break;

                        case 1850:
                            ordersRequirement.ProductID = 2;
                            break;

                        default:
                            ordersRequirement.ProductID = 3;
                            break;

                    }
                    ordersRequirement.TransactionTypeID = 24;

                    
                    var result = callPAAPI(ordersRequirement);

                    OrderResponse response = result.Result;
                    orderResponses.Add(response);

                    /*OrderPlaceClinet placeClinet = new OrderPlaceClinet();
                    PlaceOrderResponse response = placeClinet.PlaceOrders(ordersRequirement);                   

                    orderResponses.Add(new OrderResponse
                     {
                         AQUADecision = response.AQUADecision,
                         ItemNumber = prow.Orders.item_no,
                         Response = response.Response,
                         OrderNumber = prow.Orders.order_no,
                         ResponseCode = response.ResponseCode,
                         ResponseTime = response.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"),
                         ResWareFileID = response.ResWareFileID,
                         ResWareFileNumber = response.ResWareFileNumber
                     });*/

                }
              //  InsertOrderResponse(orderResponses);
                return orderResponses;
            }
        }

        public async Task<OrderResponse> callPAAPI(OrdersRequirement ordersRequirement)
        {
            string Baseurl = "https://pabdev.punctualabstract.com/titleleader";            

            OrderResponse return_response = new OrderResponse();

            APICredentialInfo temp_credential = new APICredentialInfo();
            temp_credential.api_key = "fd7a51d848b9e58a2d3d4c0f14c689bff500fabf";
            temp_credential.username = "title_leader";
            temp_credential.password = "12Ti?tle34";

            // For Live 
            /*Baseurl = "https://pab.punctualabstract.com/titleleader";
            temp_credential.api_key = "fd7a51d848b9e58a2d3d4c0f14c689bff500fabf";
            temp_credential.username = "titleleader";
            temp_credential.password = "12Ti?tle34";*/

            ordersRequirement.Credentials = temp_credential;

            var client = new HttpClient();

            try
            {
                var json = new JavaScriptSerializer().Serialize(ordersRequirement);

                var ret_value = client.PostAsJsonAsync(Baseurl, ordersRequirement).Result;
                
                OrderResponsePA response = ret_value.Content.ReadAsAsync<OrderResponsePA>().Result;

                return_response = new OrderResponse
                {
                    AQUADecision = "",
                    ItemNumber = ordersRequirement.ItemNumber,
                    Response = response.status,
                    OrderNumber = Int32.Parse(response.orderId),
                    ResponseCode = 1,
                    ResponseTime = response.timestamp.ToString(),
                    ResWareFileID = 0,
                    ResWareFileNumber = response.clientFileNumber
                };

                //InsertOrderResponse(orderResponses);

            }
            catch (Exception ex)
            {

            }

            return return_response;
        }

        public void InsertOrderResponse(List<OrderResponse> orderResponses)
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string query4 = "SELECT * FROM  OrderResponse WHERE OrderNumber =@OrderNumber and ItemNumber =@ItemNumber";

                string query = "INSERT INTO OrderResponse(AQUADecision,Response,ResponseCode,ResWareFileID,ResWareFileNumber,ResponseTime,OrderNumber,ItemNumber)VALUES(@AQUADecision,@Response,@ResponseCode,@ResWareFileID,@ResWareFileNumber,@ResponseTime,@OrderNumber,@ItemNumber)";
                string query1 = "UPDATE OrderResponse SET AQUADecision=@AQUADecision,Response=@Response,ResponseCode=@ResponseCode,ResWareFileID=@ResWareFileID,ResWareFileNumber=@ResWareFileNumber,ResponseTime=@ResponseTime,OrderNumber=@OrderNumber,ItemNumber=@ItemNumber WHERE OrderNumber =@OrderNumber and ItemNumber =@ItemNumber";
                string query2 = "UPDATE order_details SET STATUS = @STATUS WHERE order_no=@order_no AND order_item_no=@order_item_no";
                foreach (var item in orderResponses)
                {
                    var rows4 = db.QueryFirstOrDefault(query4, new { OrderNumber = item.OrderNumber, ItemNumber = item.ItemNumber });
                    if (rows4 == null)
                    {
                        var result = db.Execute(query, item);
                    } else
                    {
                        var result = db.Execute(query1, item);
                    }
                    
                    if (item.ResponseCode == 0)
                    {
                        var result1 = db.Execute(query2, new { STATUS = "O", order_no = item.OrderNumber, order_item_no = item.ItemNumber });
                    }
                    else
                    {
                        var result1 = db.Execute(query2, new { STATUS = "A", order_no = item.OrderNumber, order_item_no = item.ItemNumber });
                    }
                }
            }
        }
        public List<DatauploadSave> GetDetailsSaveForLater(string orderNo)
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string query = "select owners.owner_first_name as owner_first_name,owners.owner_last_name as owner_last_name, orders.ID as ID,orders.city as city,orders.created_on as order_date,orders.house_no as house_no,orders.house_unit as house_unit,orders.order_item_no as item_no,orders.order_no as order_no,orders.order_type as order_type,orders.remarks as aditional_notes,opt1.DISPLAY_NAME as search_type,state,orders.street_addr as street_add,orders.zip_code as zip_code,orders.status as order_status,orders.county as county from order_details orders left join order_owner_details owners on orders.order_no=owners.order_no and orders.order_item_no=owners.order_item_no left join  Options opt1 on opt1.ID=orders.search_type where orders.order_no=@order_no ";
                var saveOrder = db.Query<DatauploadSave>(query, new { order_no = orderNo }).ToList();
                return saveOrder;
            }
        }

        public OrderItemInfo GetOrderItemDetails(string orderno, string itemno)
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string query = "SELECT T1.order_no AS OrderNo,T1.order_item_no AS ItemNo,T1.house_no AS HouseNumber,T1.house_unit AS Unit,T1.street_addr AS StreetName,T1.city AS City,T1.state AS State,T1.county AS County,T1.zip_code AS ZipCode,T2.owner_first_name AS FirstName,T2.owner_last_name AS LastName FROM order_details T1 LEFT JOIN order_owner_details T2 ON (T1.order_no=T2.order_no AND T1.order_item_no=T2.order_item_no) " +
                    "WHERE T1.order_no=@OrderNo AND T1.order_item_no=@ItemNo";
                var result = db.QueryFirstOrDefault<OrderItemInfo>(query, new { OrderNo = orderno, ItemNo = itemno });
                return result;
            }
        }

        public bool UpdateOrderItemDetails(OrderItemInfo orderItemInfo)
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                db.Open();
                using (var trans = db.BeginTransaction())
                {
                    try
                    {
                        string query1 = "UPDATE ORDER_DETAILS SET house_no=@HouseNumber,house_unit=@Unit,street_addr=@StreetName,city=@City,state=@State,county=@County,zip_code=@ZipCode where order_no=@OrderNo and order_item_no=@ItemNo";
                        string query2 = "UPDATE ORDER_OWNER_DETAILS SET owner_first_name=@FirstName,owner_last_name=@LastName WHERE order_no=@OrderNo AND order_item_no=@ItemNo";

                        var result1 = db.Execute(query1, orderItemInfo, trans);
                        var result2 = db.Execute(query2, orderItemInfo, trans);

                        trans.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return false;
                    }
                }
            }
        }

        public bool UpdateOrderNo(int new_order_no, int order_no, string created_by, int responseCode)
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string query = "UPDATE ORDER_DETAILS SET order_no=@new_order_no WHERE order_no =@order_no AND created_by= @created_by";
                if (responseCode == 2)
                {
                    query = "UPDATE ORDER_DETAILS SET order_no=@new_order_no, status='D' WHERE order_no =@order_no AND created_by= @created_by";
                }
                    
                try
                {
                    var result = db.Execute(query, new { new_order_no = new_order_no, order_no = order_no, created_by = created_by });
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public void UpdateNotification(List<OrderNotification> orderNotifications)
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {

                string query = "UPDATE  OrderNotification SET DisplayStatus=@DisplayStatus WHERE OrderNumber =@OrderNumber";

                foreach (var item in orderNotifications)
                {
                    
                        var result = db.Execute(query, item);
                    
                }

            }

        }

        public void InsertNewNotification(OrderNotification orderNotification)
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string get_query = "SELECT * FROM  OrderNotification WHERE OrderNumber =@OrderNumber AND ResWareFileNumber =@ResWareFileNumber";
                
                var get_notifications = db.QueryFirstOrDefault(get_query, new { OrderNumber = orderNotification.OrderNumber, ResWareFileNumber = orderNotification.ResWareFileNumber });
                if (get_notifications == null)
                {
                    string query = "INSERT INTO OrderNotification(OrderNumber,ItemNumber,ResWareFileNumber,ResponseCode,DisplayStatus,created_by,created_on)VALUES(@OrderNumber,@ItemNumber,@ResWareFileNumber,@ResponseCode,@DisplayStatus,@created_by,@created_on)";
                    var result = db.Execute(query, new { OrderNumber = orderNotification.OrderNumber, ItemNumber = orderNotification.ItemNumber, ResWareFileNumber = orderNotification.ResWareFileNumber, ResponseCode = orderNotification.ResponseCode, DisplayStatus = orderNotification.DisplayStatus, created_by = orderNotification.created_by, created_on = DateTime.Now });
                } else
                {
                    string query2 = "UPDATE OrderNotification SET OrderNumber=@OrderNumber,ItemNumber=@ItemNumber,ResWareFileNumber=@ResWareFileNumber,ResponseCode=@ResponseCode,DisplayStatus=@DisplayStatus,created_by=@created_by,created_on=@created_on WHERE OrderNumber =@OrderNumber and OrderNumber =@ResWareFileNumber";
                    var result2 = db.QueryFirstOrDefault(query2, new { OrderNumber = orderNotification.OrderNumber, ItemNumber = orderNotification.ItemNumber, ResWareFileNumber = orderNotification.ResWareFileNumber, ResponseCode = orderNotification.ResponseCode, DisplayStatus = orderNotification.DisplayStatus, created_by = orderNotification.created_by, created_on = DateTime.Now });
                }

                string query4 = "SELECT * FROM  Invoice_Detail T1 WHERE T1.order_no =@order_no and T1.order_item_no =@order_item_no";

                var rows4 = db.QueryFirstOrDefault(query4, new { order_no = Convert.ToInt32(orderNotification.OrderNumber), order_item_no = orderNotification.ItemNumber });
                if (rows4 == null)
                {
                    string query2 = "INSERT INTO Invoice_Detail(inv_no,order_no,order_item_no,amount_unit,total_amount,status,created_by,created_on)VALUES(@inv_no,@order_no,@order_item_no,@amount_unit,@total_amount,@status,@created_by,@created_on)";
                    var result1 = db.Execute(query2, new { inv_no = "INV" + "-" + Convert.ToInt32(orderNotification.OrderNumber), order_no = orderNotification.OrderNumber, order_item_no = orderNotification.ItemNumber.ToString(), amount_unit = 0, total_amount = 0, status = 'D', created_by = orderNotification.created_by, created_on = DateTime.Now });
                }

            }
        }

        public void InsertNotification(List<OrderNotification> orderNotifications)
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {

                string query1 = "INSERT INTO OrderNotification(OrderNumber,ItemNumber,ResWareFileNumber,ResponseCode,DisplayStatus,created_by,created_on)VALUES(@OrderNumber,@ItemNumber,@ResWareFileNumber,@ResponseCode,@DisplayStatus,@created_by,@created_on)";

                string query3 = "UPDATE order_details SET STATUS = @STATUS WHERE order_no=@order_no AND order_item_no=@order_item_no";
                foreach (var item in orderNotifications)
                {
                    string query = "SELECT * FROM  OrderNotification T1 WHERE T1.OrderNumber =@OrderNumber and T1.ItemNumber =@ItemNumber";

                    var rows = db.QueryFirstOrDefault(query, new { OrderNumber = Convert.ToInt32(item.OrderNumber), ItemNumber = Convert.ToInt32(item.ItemNumber) });
                    if (rows == null)
                    {

                        var result = db.Execute(query1, new { OrderNumber = item.OrderNumber, ItemNumber = item.ItemNumber, ResWareFileNumber = item.ResWareFileNumber, ResponseCode = item.ResponseCode, DisplayStatus = item.DisplayStatus, created_by = HttpContext.Current.User.Identity.Name.ToString(), created_on = DateTime.Now });

                        var result2 = db.Execute(query3, new { STATUS = "C", order_no = item.OrderNumber, order_item_no = item.ItemNumber });



                        string query4 = "SELECT * FROM  Invoice_Detail T1 WHERE T1.order_no =@order_no and T1.order_item_no =@order_item_no";

                        var rows4 = db.QueryFirstOrDefault(query4, new { order_no = Convert.ToInt32(item.OrderNumber), order_item_no = Convert.ToInt32(item.ItemNumber) });
                        if (rows4 == null)
                        {
                            //Start - Change done on 15-11-2019
                            string query2 = "INSERT INTO Invoice_Detail(inv_no,order_no,order_item_no,amount_unit,total_amount,status,created_by,created_on)VALUES(@inv_no,@order_no,@order_item_no,@amount_unit,@total_amount,@status,@created_by,@created_on)";
                            var result1 = db.Execute(query2, new { inv_no = "INV" + "-" + Convert.ToInt32(item.OrderNumber), order_no = item.OrderNumber, order_item_no = item.ItemNumber, amount_unit = 0, total_amount = 0, status = 'D', created_by = HttpContext.Current.User.Identity.Name.ToString(), created_on = DateTime.Now });
                            //End - Change done on 15-11-2019
                        }

                        double Price = 0;
                        foreach (var m in GetInvoicePrice(item.OrderNumber, item.ItemNumber, "1"))
                        {

                            Price = Convert.ToDouble(m.rate) + Convert.ToDouble(m.gap_rate);
                        }
                        var NoOfCompletedSearch = CountCompleteOrderSearch(item.OrderNumber);
                        string query5 = "UPDATE  Invoice_Detail SET amount_unit=@amount_unit,total_amount=@total_amount WHERE order_no =@order_no and order_item_no=@order_item_no";
                        var result5 = db.Execute(query5, new { amount_unit = Price, total_amount = (Price), order_no = item.OrderNumber, order_item_no = item.ItemNumber });


                    }
                    else
                    {
                        var result2 = db.Execute(query3, new { STATUS = "C", order_no = item.OrderNumber, order_item_no = item.ItemNumber });



                        string query4 = "SELECT * FROM  Invoice_Detail T1 WHERE T1.order_no =@order_no and T1.order_item_no =@order_item_no";

                        var rows4 = db.QueryFirstOrDefault(query4, new { order_no = Convert.ToInt32(item.OrderNumber), order_item_no = Convert.ToInt32(item.ItemNumber) });
                        if (rows4 == null)
                        {
                            //Start - Change done on 15-11-2019
                            string query2 = "INSERT INTO Invoice_Detail(inv_no,order_no,order_item_no,amount_unit,total_amount,status,created_by,created_on)VALUES(@inv_no,@order_no,@order_item_no,@amount_unit,@total_amount,@status,@created_by,@created_on)";
                            var result1 = db.Execute(query2, new { inv_no = "INV" + "-" + Convert.ToInt32(item.OrderNumber), order_no = item.OrderNumber, order_item_no = item.ItemNumber, amount_unit = 0, total_amount = 0, status = 'D', created_by = HttpContext.Current.User.Identity.Name.ToString(), created_on = DateTime.Now });
                            //End - Change done on 15-11-2019
                        }

                        double Price = 0;
                        foreach (var m in GetInvoicePrice(item.OrderNumber, item.ItemNumber, "1"))
                        {

                            Price = Convert.ToDouble(m.rate) + Convert.ToDouble(m.gap_rate);
                        }
                        var NoOfCompletedSearch = CountCompleteOrderSearch(item.OrderNumber);
                        string query5 = "UPDATE  Invoice_Detail SET amount_unit=@amount_unit,total_amount=@total_amount WHERE order_no =@order_no and order_item_no=@order_item_no";
                        var result5 = db.Execute(query5, new { amount_unit = Price, total_amount = (Price), order_no = item.OrderNumber, order_item_no = item.ItemNumber });


                    }
                }

            }

        }



        public string InsertPayments(string invno, string orderno, string noofsearch, string totalamount, string search)
        {
            string query1 = "INSERT INTO Payment_Detail( invoice_No, order_no,transaction_No, amount, created_by, created_on)" +
                "VALUES( @invoice_No, @order_no,@transaction_No, @amount, @created_by, @created_on)";
            string query2 = "UPDATE  Invoice_Detail SET status=@status WHERE inv_no=@inv_no and order_no =@order_no and order_item_no in (@order_item_no)";
            string strTransaction_no = get_transaction_no();
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                db.Open();
                
                    using (var transaction = db.BeginTransaction())
                    {
                        try
                        {
                           
                        var row = db.QueryFirstOrDefault(query1, new
                        {
                            invoice_No = invno,
                            order_no = orderno,
                            transaction_No = strTransaction_no,
                            amount = totalamount,
                                created_by = HttpContext.Current.User.Identity.Name.ToString(),
                                created_on = DateTime.Now
                                
                            }, transaction: transaction);

                        string[] search_items = search.ToString().Split(',');
                        foreach (string search_item in search_items)
                        {
                            var row1 = db.QueryFirstOrDefault(query2, new
                            {
                                status = "P",
                                inv_no = invno,
                                order_no = orderno,
                                order_item_no =Convert.ToInt16( search_item)
                            }, transaction: transaction);

                        }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                        }
                    }
               
            }
            return orderno;
        }


        public string InsertInvoicePayments(string invno,  string totalamount)
        {
            string query1 = "INSERT INTO Payment_Detail( invoice_No, order_no,transaction_No, amount, created_by, created_on)" +
                "VALUES( @invoice_No, @order_no,@transaction_No, @amount, @created_by, @created_on)";
            string query2 = "UPDATE  Invoice_Detail SET status=@status WHERE inv_no=@inv_no ";
            string strTransaction_no = get_transaction_no();
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                db.Open();

                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        string[] Invoice_Nos = invno.ToString().Split(',');
                        foreach (string Invoice_No in Invoice_Nos)
                          {
                              string strQuery = "";
                              strQuery = " select order_no from Invoice_Detail where inv_no = '" + Invoice_No + "'";
                             // string data = db.Query<string>(strQuery).Single();
                              string data = db.QueryFirstOrDefault<string>(strQuery, new
                              {
                              }, transaction: transaction);
                              string orderno = data;

                              var row = db.QueryFirstOrDefault(query1, new
                              {
                                  invoice_No = Invoice_No,
                                  order_no = orderno,
                                  transaction_No = strTransaction_no,
                                  amount = totalamount,
                                  created_by = HttpContext.Current.User.Identity.Name.ToString(),
                                  created_on = DateTime.Now

                              }, transaction: transaction);


                              var row1 = db.QueryFirstOrDefault(query2, new
                              {
                                  status = "P",
                                  inv_no = Invoice_No,

                              }, transaction: transaction);


                              
                          }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
                }

            }
            return invno;
        }
        public OrderResponse GetOrderResponses(string orderno)
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string query = "SELECT * FROM  OrderResponse T1 WHERE T1.OrderNumber =@order_no";
                var result = db.QueryFirstOrDefault<OrderResponse>(query, new { order_no = Convert.ToInt32(orderno) });
                return result;
            }
        }
       
        public List<OrderResponse> GetOrderSearch(string orderno)
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string query = "SELECT * FROM  OrderResponse T1 WHERE T1.OrderNumber =@order_no";
                var orderResponse = db.Query<OrderResponse>(query, new { order_no = Convert.ToInt32(orderno) }).ToList();
                return orderResponse;
            }
        }
        public OrderResponse GetOrderResponses(string orderno,string itemno)
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string query = "SELECT * FROM  OrderResponse T1 WHERE T1.OrderNumber =@order_no AND T1.ItemNumber=@ItemNo";
                var result = db.QueryFirstOrDefault<OrderResponse>(query, new { order_no = Convert.ToInt32(orderno), ItemNo= Convert.ToInt32(itemno) });
                return result;
            }
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public void InsertOrderDoc(string order_no, string item_no, string created_by, string doc_name)
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string query = "INSERT INTO OrderDocs(order_no,item_no,created_by,doc_name)VALUES(@order_no, @item_no, @created_by, @doc_name)";
                var result = db.Execute(query, new { order_no = order_no, item_no = item_no, created_by = created_by, doc_name = doc_name });

            }
        }
        public List<string> GetOrderDoc(string order_no)
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string query = "SELECT doc_name FROM  OrderDocs WHERE order_no =@order_no";
                var result = db.Query<string>(query, new { order_no = order_no }).ToList();
                return result;
            }
        }
        public List<string> GetOrderDoc(string order_no, string item_no)
        {
            using (IDbConnection db = new TitleLeaderDatabaseConnection().GetDBConnection)
            {
                string query = "SELECT doc_name FROM  OrderDocs WHERE order_no =@order_no AND item_no=@item_no";
                var result = db.Query<string>(query, new { order_no = order_no, item_no = item_no }).ToList();
                return result;
            }
        }
    }
}
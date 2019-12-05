using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TitleLeader.Custom;

using TitleLeader.Models.Dataupload;
using TitleLeader.Models.UserLoginModels;
using TitleLeader.Models.UserRegistrationModels;

namespace TitleLeader.Controllers
{
    using System.Threading.Tasks;
  
    using Stripe;
    public class HomeController : Controller
    {
        // GET: Home
        
        TitleLeaderDataAccess DataAccess = new TitleLeaderDataAccess();
        [Authorize, TitleLeaderExceptionFilter, TitleLeaderActionFilter]
        public ActionResult Index()
      {
            int userid = int.Parse(HttpContext.User.Identity.Name);
            ViewBag.OrderCount = DataAccess.CountOrderNo(userid);
            var countComplateOrder = DataAccess.GetCompleteOrder().Count();
            ViewBag.CountOrder = countComplateOrder;
            var openOrders = DataAccess.GetRecentORder();
            var ReturOders = DataAccess.GetReturnedOrderDahsboard();
            var SaveForLater = DataAccess.GetSaveForLaterDahsboard();
            var RecentInvoice = DataAccess.GetRecentInvoice();
            var CompletedOrder = DataAccess.GetComplatedOrderDashboard();
            var ReturOders1 = DataAccess.GetReturnedOrder();
            ViewBag.ReturOders1 = ReturOders1.Count.ToString();

            //Start - Change done on 15-11-2019
            SiteController sc = new SiteController();
            sc.CheckDocument();
            //End - Change done on 15-11-2019


           // var invoice = DataAccess.GetInvoice("", "");
             dynamic displaydata = new ExpandoObject();
            displaydata.OpenOrderModels = openOrders;
            displaydata.DatauploadSave = ReturOders;
            displaydata.RecentInvouce = RecentInvoice;
            displaydata.CompletedOrder = CompletedOrder;
           // displaydata.OrderInvoice = invoice;
          
            InvoiceOverdue(30,60,90,120);
            InvoiceTurnaround(3, 5, 7, 15);
            Session.Timeout = 60;  
            return View(displaydata);
          
        }
        public void InvoiceTurnaround(int noofday1, int noofday2, int noofday3, int noofday4)
        {
            double invsum = 0;
            int totaldue = 0;
            int Turnaround1days = 0;
            int Turnaround2days = 0;
            int Turnaround3days = 0;
            int Turnaround4days = 0;

            double Turnaround1sum = 0;
            double Turnaround2sum = 0;
            double Turnaround3sum = 0;
            double Turnaround4sum = 0;

            foreach (var m in DataAccess.GetInvoice("", ""))
            {
                invsum = Convert.ToDouble(invsum)  +(Convert.ToDouble(m.total_amount));
                totaldue = totaldue + 1;
                if ((Convert.ToDateTime(DateTime.Now) - Convert.ToDateTime(m.created_on)).Days <= noofday1 )
                {
                    Turnaround1days = Turnaround1days + 1;
                    Turnaround1sum = Turnaround1sum + (Convert.ToDouble(m.total_amount));

                }

                if ((Convert.ToDateTime(DateTime.Now) - Convert.ToDateTime(m.created_on)).Days <= noofday2 && (Convert.ToDateTime(DateTime.Now) - Convert.ToDateTime(m.created_on)).Days > noofday1)
                {
                    Turnaround2days = Turnaround2days + 1;
                    Turnaround2sum = Turnaround2sum + (Convert.ToDouble(m.total_amount));
                }

                if ((Convert.ToDateTime(DateTime.Now) - Convert.ToDateTime(m.created_on)).Days <= noofday3 && (Convert.ToDateTime(DateTime.Now) - Convert.ToDateTime(m.created_on)).Days > noofday2)
                {
                    Turnaround3days = Turnaround3days + 1;
                    Turnaround3sum = Turnaround3sum + (Convert.ToDouble(m.total_amount));
                }

                if ((Convert.ToDateTime(DateTime.Now) - Convert.ToDateTime(m.created_on)).Days > noofday3)
                {
                    Turnaround4days = Turnaround4days + 1;
                    Turnaround4sum = Turnaround4sum + (Convert.ToDouble(m.total_amount));
                }
            }
            double Turnaround1per = 0;
            double Turnaround2per = 0;
            double Turnaround3per = 0;
            double Turnaround4per = 0;
            if (totaldue > 0)
            {
                int countopenorder = DataAccess.CountOpenOrder();
                //ViewBag.invsum = invsum;
                //ViewBag.totaldue = totaldue;
                ViewBag.Turnaround1days = Turnaround1days;
                ViewBag.Turnaround2days = Turnaround2days;
                ViewBag.Turnaround3days = Turnaround3days;
                ViewBag.Turnaround4days = Turnaround4days;
                ViewBag.Turnaround1per = Math.Round(Convert.ToDouble((Turnaround1days * 100) / totaldue));
                ViewBag.Turnaround2per = Math.Round(Convert.ToDouble((Turnaround2days * 100) / totaldue));
                ViewBag.Turnaround3per = Math.Round(Convert.ToDouble((Turnaround3days * 100) / totaldue));
                ViewBag.Turnaround4per = Math.Round(Convert.ToDouble((Turnaround4days * 100) / totaldue));


            }
            else
            {
                ViewBag.Turnaround1days = 0;
                ViewBag.Turnaround2days = 0;
                ViewBag.Turnaround3days = 0;
                ViewBag.Turnaround4days = 0;
                ViewBag.Turnaround1per = 0;
                ViewBag.Turnaround2per = 0;
                ViewBag.Turnaround3per = 0;
                ViewBag.Turnaround4per = 0;
            }

        }


        public void InvoiceOverdue(int noofday1,int noofday2,int noofday3,int noofday4)
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
                invsum = Convert.ToDouble(invsum) + (Convert.ToDouble(m.total_amount) );
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
        public ActionResult Accounts()
        {
            dynamic displaydata = new ExpandoObject();
            displaydata.CarDetails = DataAccess.GetCardDetails();
            displaydata.UserDetails = DataAccess.GetUserDetails();
            displaydata.TeamDetails = DataAccess.GetTeamDetails();
            
            return View(displaydata);
        }

        [HttpPost]
        public ActionResult ChangeProfileImage(HttpPostedFileBase ProfilePhoto)
        {
            var result = DataAccess.ChangeProfilePhoto(ProfilePhoto);
            if (result)
            {
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetProfilePhoto()
        {
            var result = DataAccess.GetProfileImage(HttpContext.User.Identity.Name);
            if (result.Photo != null)
            {
                return File(result.Photo, "image/jpg");
            }
            else
            {
                return File(HttpContext.Server.MapPath("~/Content/images/avatar-2.png"), "image/jpg");
            }
        }

        [AllowAnonymous, TitleLeaderExceptionFilter]
        public ActionResult Registration1()
        {
            List<SelectListItem> States = new List<SelectListItem>();
            foreach (var item in DataAccess.GetStates())
            {
                SelectListItem selectItem = new SelectListItem() { Text = Convert.ToString(item.DISPLAY_NAME), Value = Convert.ToString(item.ID) };
                States.Add(selectItem);
            }
            ViewBag.States = States;
            UserMaster userMaster = new UserMaster();
            return View("Registration1", userMaster);
        }
        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken, TitleLeaderExceptionFilter]
        public ActionResult Registration1(UserMaster userMaster)
        {
            if (ModelState.IsValid)
            {
                Session["USERMASTER"] = userMaster;
                return RedirectToAction("Registration2");
            }
            else
            {
                return View();
            }
        }
       
        [AllowAnonymous, TitleLeaderExceptionFilter]
        public ActionResult Registration2()
        {
            if (Session["USERMASTER"] != null)
            {
                List<SelectListItem> States = new List<SelectListItem>();
                foreach (var item in DataAccess.GetStates())
                {
                    SelectListItem selectItem = new SelectListItem() { Text = Convert.ToString(item.DISPLAY_NAME), Value = Convert.ToString(item.ID) };
                    States.Add(selectItem);
                }
                ViewBag.States = States;
                CardMaster cardMaster = new CardMaster();
                cardMaster.AutomaticPayment = true;
                return View(cardMaster);
            }
            else
            {
                return RedirectToAction("Registration1");
            }
        }

        [HttpPost, AllowAnonymous, TitleLeaderExceptionFilter]
        public ActionResult Registration2(List<CardMaster> cardMasters)
        {
            if (ModelState.IsValid)
            {
                if (Session["USERMASTER"] != null)
                {
                    UserMaster userMaster = Session["USERMASTER"] as UserMaster;
                    foreach (CardMaster cardMaster in cardMasters)
                    {
                       // userMaster.AutomaticPayment = cardMaster.AutomaticPayment;
                        userMaster.AutomaticPayment =true;
                    }
                    string password = FormsAuthentication.HashPasswordForStoringInConfigFile(userMaster.Password.Trim(), "MD5");
                    userMaster.Password = password;
                    UserLogin userLogin = DataAccess.UserRegistration(userMaster, cardMasters);

                    var authTicketUserPassword = new FormsAuthenticationTicket(1, userLogin.Username, DateTime.Now, DateTime.Now.AddMinutes(10), false, userMaster.Password, FormsAuthentication.FormsCookiePath);
                    HttpCookie authcookie = new HttpCookie("LoginInfo");
                    authcookie.Values.Add("USER_ID", userLogin.Id.ToString());
                    authcookie.Values.Add("USER_NAME", userLogin.Username);
                    authcookie.Values.Add("PASSWORD", FormsAuthentication.Encrypt(authTicketUserPassword));
                    authcookie.Expires = authTicketUserPassword.Expiration;
                    Response.Cookies.Add(authcookie);
                    FormsAuthentication.SetAuthCookie(userLogin.Id.ToString(), true);
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("error", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken, TitleLeaderExceptionFilter]
        public ActionResult Login(UserLogin userLogin)
        {
            if (ModelState.IsValid)
            {
                string password = FormsAuthentication.HashPasswordForStoringInConfigFile(userLogin.Password.Trim(), "MD5");
                UserLogin userLoginobj = DataAccess.UserAuthentication(userLogin.Username, password);
                if (userLoginobj != null)
                {
                    var authTicketUserPassword = new FormsAuthenticationTicket(1, "Password", DateTime.Now, DateTime.Now.AddMinutes(10), false, userLogin.Password, FormsAuthentication.FormsCookiePath);
                    HttpCookie authcookie = new HttpCookie("LoginInfo");
                    authcookie.Values.Add("USER_ID", userLoginobj.Id.ToString());
                    authcookie.Values.Add("USER_NAME", userLoginobj.Username);
                    authcookie.Values.Add("PASSWORD", FormsAuthentication.Encrypt(authTicketUserPassword));
                    authcookie.Expires = authTicketUserPassword.Expiration;
                    Response.Cookies.Add(authcookie);
                    FormsAuthentication.SetAuthCookie(userLoginobj.Id.ToString(), true);
                    return RedirectToAction("Index");
                }
                else
                {
                    string teamuserpassword = FormsAuthentication.HashPasswordForStoringInConfigFile(userLogin.Password.Trim(), "MD5");
                    UserLogin teamuserLoginobj = DataAccess.TeamUserAuthentication(userLogin.Username, teamuserpassword);
                    if (teamuserLoginobj != null)
                    {
                        UserLogin teamLoginobj = DataAccess.TeamAuthentication(teamuserLoginobj.Username);
                        if (teamLoginobj != null)
                        {
                            var authTicketUserPassword = new FormsAuthenticationTicket(1, "Password", DateTime.Now, DateTime.Now.AddMinutes(10), false, userLogin.Password, FormsAuthentication.FormsCookiePath);
                            HttpCookie authcookie = new HttpCookie("LoginInfo");
                            authcookie.Values.Add("USER_ID", teamLoginobj.Id.ToString());
                            authcookie.Values.Add("USER_NAME", teamLoginobj.Username);
                            authcookie.Values.Add("PASSWORD", FormsAuthentication.Encrypt(authTicketUserPassword));
                            authcookie.Expires = authTicketUserPassword.Expiration;
                            Response.Cookies.Add(authcookie);
                            FormsAuthentication.SetAuthCookie(teamLoginobj.Id.ToString(), true);
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ViewBag.Error = "0";
                            return View();
                        }
                    }
                    else
                    {
                        ViewBag.Error = "0";
                        return View();
                    }
                    
                }
            }
            else
            {
                ViewBag.Error = "0";
                return View();
            }
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login");
        }

        public ActionResult Error()
        {
            return View();
        }

        [Authorize, TitleLeaderExceptionFilter]
        public ActionResult GetUserDetails()
        {
            int userid = int.Parse(HttpContext.User.Identity.Name);
            UserMaster userMaster = DataAccess.UserMasterDetails(userid);
            return PartialView(userMaster);
        }

        [AllowAnonymous, TitleLeaderExceptionFilter]
        public ActionResult CheckUsername(string username)
        {
            return Json(DataAccess.CheckUsername(username), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Termsandcondition()
        {
            return View();
        }
        public ActionResult BulkOrderDiscounts()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(string strOldPwd,string strNewpwd,string strConfpwd)
        {
             strOldPwd = FormsAuthentication.HashPasswordForStoringInConfigFile(strOldPwd.Trim(), "MD5");
            strNewpwd = FormsAuthentication.HashPasswordForStoringInConfigFile(strNewpwd.Trim(), "MD5");
            strConfpwd = FormsAuthentication.HashPasswordForStoringInConfigFile(strConfpwd.Trim(), "MD5");
            if (strOldPwd=="" || strNewpwd=="")
            {
                return Json("Invalid", JsonRequestBehavior.AllowGet);
            }
            else if(strNewpwd!= strConfpwd)
            {
                return Json("notmatchcon", JsonRequestBehavior.AllowGet);
            }
            else if(DataAccess.PwdExists(strOldPwd)>0)
            {
               int c= DataAccess.ResetPassword(strNewpwd);
                if(c>0)
                {
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("error", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("notmatch", JsonRequestBehavior.AllowGet);
            }
           

        }

        [HttpPost]
        public ActionResult UpdateProfile(string strName,string strCompany,string strTitle,string strPhone,string strEmail,string strAddress)
        {
          int c=DataAccess.UpdateProfile( strName,  strCompany,  strTitle,  strPhone,  strEmail,  strAddress);
            if(c>0)
            {
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("success", JsonRequestBehavior.AllowGet);
            }
           
        }
        [HttpPost]
        public ActionResult DeleteCard(string carditem)
        {
            int c= DataAccess.DeleteCardDetails(carditem);

            if (c > 0)
            {
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("success", JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public ActionResult AddPyamentcard(string strCardNo,string strNameoncard,string strExpiary,string strZipcode)
        {
            if(strCardNo==""||strNameoncard==""||strExpiary==""||strZipcode=="")
            {
                return Json("Invalid", JsonRequestBehavior.AllowGet);
            }
            else
            {
                int cnt = DataAccess.UpdateCard(strCardNo, strNameoncard, strExpiary, strZipcode);
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            
            
        }
        [HttpPost]
        public ActionResult Addteame(Addteames obj)
        {
            int c = DataAccess.InsertTeam(obj);
            if (c > 0)
            {
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
           
        }
    }
}
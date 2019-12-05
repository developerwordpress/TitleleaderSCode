using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TitleLeader.Models.ErrorModels;

namespace TitleLeader.Custom
{
    public class TitleLeaderExceptionFilter : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            TitleLeaderDataAccess tileLeaderDataAccess = new TitleLeaderDataAccess();
            Error error = new Error
            {
                ErrorDate = DateTime.Now,
                ErrorMessage = filterContext.Exception.Message,
                Source = filterContext.Exception.Source,
                StackTrace = filterContext.Exception.StackTrace,
                TragetSite = filterContext.Exception.TargetSite.Name,
                UserId = HttpContext.Current.User.Identity.Name != null ? HttpContext.Current.User.Identity.Name : ""
            };
            tileLeaderDataAccess.AddError(error);
            filterContext.ExceptionHandled = true;
        }
    }
}
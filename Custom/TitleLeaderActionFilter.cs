using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TitleLeader.Models.ErrorModels;
using System.Web.Routing;
using TitleLeader.Controllers;
namespace TitleLeader.Custom
{
    public class TitleLeaderActionFilter : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            ActionCheckDocument("OnResultExecuted", filterContext.RouteData);
        }
        private void ActionCheckDocument(string methodName, RouteData routeData)
        {
            //SiteController sc = new SiteController();
            //sc.CheckDocument();
        }
    }
}
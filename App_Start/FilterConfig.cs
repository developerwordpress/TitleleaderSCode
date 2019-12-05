using System.Web;
using System.Web.Mvc;
using TitleLeader.Custom;

namespace TitleLeader
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new TitleLeaderExceptionFilter());
        }
    }
}

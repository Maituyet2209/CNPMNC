using System.Web;
using System.Web.Mvc;

namespace He_thong_ban_DT_online
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}

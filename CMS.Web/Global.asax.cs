using CMS.Models;
using CMS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CMS.Web
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        LuotTruyCapService luotTruyCapService = new LuotTruyCapService();
        protected void Application_Start()
        {
            Application["VisitorOnline"] = 0;
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            MvcHandler.DisableMvcResponseHeader = true;
            AntiForgeryConfig.SuppressXFrameOptionsHeader = true;
        }
        protected void Session_Start()
        {
            Application.Lock();
            Application["VisitorOnline"] = (int)Application["VisitorOnline"] + 1;
            LuotTruyCap luotTruyCap = new LuotTruyCap();
            luotTruyCap.IP = Request.UserHostAddress;
            luotTruyCap.ThietBi = Request.UserAgent;
            luotTruyCap.ThoiGian = DateTime.Now;
            luotTruyCap.TrangTruyCapDauTien = Request.Url.OriginalString.Length > 250? Request.Url.OriginalString.Substring(0,250):  Request.Url.OriginalString;
            luotTruyCapService.ThemLuotTruyCap(luotTruyCap);
            Application.UnLock();
        }
        protected void Session_End(Object sender, EventArgs e)
        {
            Application["VisitorOnline"] = (int)Application["VisitorOnline"] - 1;
        }
        protected void Application_PreSendRequestHeaders()
        {
            Response.Headers.Remove("Server");           //Remove Server Header  
            Response.Headers.Remove("X-AspNet-Version"); //Remove X-AspNet-Version Header
            Response.AddHeader("x-frame-options", "DENY");
        }
    }
}

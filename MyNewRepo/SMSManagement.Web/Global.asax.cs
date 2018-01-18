﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace SMSManagement.Web
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            #region 增加后台任务支持



            #endregion
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        //protected void Application_BeginRequest(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string session_param_name = "ASPSESSID";
        //        string session_cookie_name = "ASP.NET_SESSIONID";

        //        if (HttpContext.Current.Request.Form[session_param_name] != null)
        //        {
        //            UpdateCookie(session_cookie_name, HttpContext.Current.Request.Form[session_param_name]);
        //        }
        //        else if (HttpContext.Current.Request.QueryString[session_param_name] != null)
        //        {
        //            UpdateCookie(session_cookie_name, HttpContext.Current.Request.QueryString[session_param_name]);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }

        //    try
        //    {
        //        string auth_param_name = "AUTHID";
        //        string auth_cookie_name = FormsAuthentication.FormsCookieName;

        //        if (HttpContext.Current.Request.Form[auth_param_name] != null)
        //        {
        //            UpdateCookie(auth_cookie_name, HttpContext.Current.Request.Form[auth_param_name]);
        //        }
        //        else if (HttpContext.Current.Request.QueryString[auth_param_name] != null)
        //        {
        //            UpdateCookie(auth_cookie_name, HttpContext.Current.Request.QueryString[auth_param_name]);
        //        }

        //    }
        //    catch (Exception)
        //    {
        //    }

        //    try
        //    {
        //        string[] tmp = HttpContext.Current.Request.Url.Segments.Clone() as string[];
        //        if (tmp.Length < 4)
        //            return;
        //        string lang = tmp[2];
        //        tmp[1] = "";
        //        tmp[2] = "";

        //        string qs = HttpContext.Current.Request.QueryString.ToString();
        //        if (qs.Length == 0)
        //        {
        //            qs = "lang=" + lang.Substring(0, lang.Length - 1);
        //        }
        //        else
        //        {
        //            qs += "&lang=" + lang.Substring(0, lang.Length - 1);
        //        }

        //        string ttt = "~" + string.Join("", tmp) + "?" + qs;
        //        HttpContext.Current.RewritePath("~" + string.Join("", tmp) + "?" + qs);
        //    }
        //    catch (Exception)
        //    {

        //        //throw;
        //    }
        //}

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        void UpdateCookie(string cookie_name, string cookie_value)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(cookie_name);

            if (cookie == null)
            {
                HttpCookie cookie1 = new HttpCookie(cookie_name, cookie_value);
                Response.Cookies.Add(cookie1);
            }
            else
            {
                cookie.Value = cookie_value;
                HttpContext.Current.Request.Cookies.Set(cookie);
            }
        }
    }
}
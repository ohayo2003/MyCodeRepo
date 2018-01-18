using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace SMSManagement.Web.Common
{
    public class IPHelper
    {

        #region 获取IP
        /// <summary>
        /// 获取用户IP
        /// </summary>
        /// <returns></returns>
        public static string GetClientIP()
        {
            string ip = GetIPNew2();
            if (!String.IsNullOrEmpty(ip))
            {
                return ip;
            }
            else
            {
                return GetWebUserIP();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetIPNew2()
        {
            string sIpout = string.Empty;
            try
            {
                string user_IP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] == null ? ""
                    : HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                if (!string.IsNullOrEmpty(user_IP))
                {
                    string[] arIps = user_IP.Split(',');

                    if (arIps.Length > 1)
                    {
                        sIpout = arIps[arIps.Length - 1].Trim();
                    }

                }

                if (sIpout.Length > 0)
                {
                    List<string> ipList = FilterLocalIP(sIpout);
                    if (ipList != null && ipList.Count > 0)
                    {
                        sIpout = ipList[0];
                    }
                }
            }
            catch
            {
                sIpout = string.Empty;
            }

            return sIpout;

        }

        /// <summary>
        /// 得到用户外网IP
        /// 默认取第一个IP
        /// </summary>
        /// <returns></returns>
        public static string GetWebUserIP()
        {
            string user_IP = GetWebUserIPs();
            List<string> ipList = FilterLocalIP(user_IP);
            if (ipList != null && ipList.Count > 0)
            {
                user_IP = ipList[0];
            }
            return user_IP;
        }

        /// <summary>
        /// 过滤内网ip
        /// </summary>
        /// <param name="ips"></param>
        /// <returns></returns>
        public static List<string> FilterLocalIP(string ips)
        {
            if (string.IsNullOrEmpty(ips))
            {
                return null;
            }
            string[] ipArray = ips.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> ipList = new List<string>(ipArray);
            foreach (string s in ipArray)
            {
                if (GetIPType(s) != 2)
                {
                    ipList.Remove(s);
                }
            }
            return ipList;
        }

        public static string GetWebUserIPs()
        {
            string user_IP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] == null ? "" : HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            if (string.IsNullOrEmpty(user_IP))
            {
                user_IP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] == null ? "" : HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();

                user_IP = user_IP.Replace("::1", "");

                if (string.IsNullOrEmpty(user_IP))
                {
                    user_IP = HttpContext.Current.Request.UserHostAddress;

                    user_IP = user_IP.Replace("::1", "");

                    if (string.IsNullOrEmpty(user_IP))//在服务器上不会执行到此代码，主要是方便本地本机调试使用，获取IP
                    {
                        string strHostName = System.Net.Dns.GetHostName();
                        System.Net.IPAddress[] clientIPAddress = System.Net.Dns.GetHostAddresses(strHostName);

                        foreach (System.Net.IPAddress temp in clientIPAddress)
                        {
                            if (temp.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            {
                                user_IP = temp.ToString();
                                break;
                            }
                        }
                    }
                }
            }
            return user_IP;
        }

        //common.GetIPType
        public static int GetIPType(string ipAddress)
        {
            //ABC类外的IP地址为广域网IP
            //A类:10.0.0.0~10.255.255.255
            //B类:172.16.0.0~172.31.255.255
            //C类:192.168.0.0~192.168.255.255

            string[] ipAddressList = ipAddress.Split('.');
            int ipAddressTemp;

            //检查IP地址是否有效
            if (ipAddressList.Length != 4)
            {
                return 0;
            }

            if (!(int.TryParse(ipAddressList[0], out ipAddressTemp) && int.TryParse(ipAddressList[1], out ipAddressTemp)
                && int.TryParse(ipAddressList[2], out ipAddressTemp) && int.TryParse(ipAddressList[3], out ipAddressTemp)))
            {
                return 0;
            }

            if (!(int.Parse(ipAddressList[0]) >= 0 && int.Parse(ipAddressList[0]) <= 255
                    && int.Parse(ipAddressList[1]) >= 0 && int.Parse(ipAddressList[1]) <= 255
                    && int.Parse(ipAddressList[2]) >= 0 && int.Parse(ipAddressList[2]) <= 255
                    && int.Parse(ipAddressList[3]) >= 0 && int.Parse(ipAddressList[3]) <= 255))
            {
                return 0;
            }

            //局域网IP
            if (ipAddress == "127.0.0.1")
            {
                return 1;
            }

            if (int.Parse(ipAddressList[0]) == 10
                    || (int.Parse(ipAddressList[0]) == 172 && int.Parse(ipAddressList[1]) >= 16 && int.Parse(ipAddressList[1]) <= 31)
                    || (int.Parse(ipAddressList[0]) == 192 && int.Parse(ipAddressList[1]) == 168))
            {
                return 1;
            }


            return 2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetALLIP()
        {
            string result = String.Empty;

            try
            {
                string temp1 = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                string temp2 = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                string temp3 = HttpContext.Current.Request.UserHostAddress;

                string temp = "";

                if (!string.IsNullOrEmpty(temp1))
                {
                    temp = temp1;
                }
                if (!string.IsNullOrEmpty(temp2))
                {
                    temp += ",[2]" + temp2;
                }
                if (!string.IsNullOrEmpty(temp3))
                {
                    temp += ",[3]" + temp3;
                }

                result = temp.Trim(',');
            }
            catch
            {
                result = "nocatch";
            }

            return result;
        }

        public static string GetServerIP()
        {
            try
            {
                System.Net.IPHostEntry ips = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());

                return FliterIP(ips.AddressList);
            }
            catch
            {
                return "";
            }
        }

        public static string FliterIP(System.Net.IPAddress[] iplist)
        {
            foreach (System.Net.IPAddress temp in iplist)
            {
                string temp_ip = temp.ToString();

                if (GetIPType(temp_ip) == 1)
                {
                    return temp_ip;
                }
            }

            return "";
        }

        /// <summary>
        /// 判断是否是IP地址格式 0.0.0.0
        /// </summary>
        /// <param name="str1">待判断的IP地址</param>
        /// <returns>true or false</returns>
        public static bool IsIPAddress(string str1)
        {
            if (str1 == null || str1 == string.Empty || str1.Length < 7 || str1.Length > 15) return false;

            string regformat = @"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}$";

            Regex regex = new Regex(regformat, RegexOptions.IgnoreCase);
            return regex.IsMatch(str1);
        }

        #endregion

    }
}

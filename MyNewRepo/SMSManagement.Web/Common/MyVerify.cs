using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;
using System.Net;


namespace SMSManagement.Web.Common
{
    /// <summary>
    /// 验证方法
    /// </summary>
    public class MyVerify
    {
        /// <summary>
        /// 请求参数是否为空
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool RequestIsNull(string name)
        {
            if (HttpContext.Current.Request[name] == null)
            {
                return true;
            }

            return false;
        }
        /// <summary>
        /// 判断请求URL和当前URL是否出自同一地址（域名）
        /// </summary>
        /// <returns></returns>
        public static bool IsSameUrl()
        {
            try
            {
                if (HttpContext.Current.Request.Url.DnsSafeHost.ToLower().Equals(HttpContext.Current.Request.UrlReferrer.DnsSafeHost.ToLower()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 验证字符串是否为min与max之间的数字
        /// </summary>
        /// <param name="number">要验证的字符串</param>
        /// <param name="min">可以取到的最小值</param>
        /// <param name="max">可以取到的最大值</param>
        /// <returns></returns>
        public static bool IsInRange(string number, int min, int max)
        {
            if (IsNumber(number))
            {
                int temp = Convert.ToInt32(number);

                if (temp < min || temp > max)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// 验证字符串是否为正数
        /// </summary>
        /// <param name="number">要验证的字符串</param>
        /// <returns></returns>
        public static bool IsPNumber(string number)
        {
            if (IsNumber(number))
            {
                int temp = Convert.ToInt32(number);

                if (temp > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }
        /// <summary>
        /// 验证字符串是否为正数
        /// </summary>
        /// <param name="number">要验证的字符串</param>
        /// <param name="rs">要返回的正整数</param>
        /// <returns></returns>
        public static bool IsPNumber(string number, out int rs)
        {
            rs = 0;

            if (IsNumber(number))
            {
                int temp = Convert.ToInt32(number);

                if (temp > 0)
                {
                    rs = temp;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }
        /// <summary>
        /// 验证字符串是否为数字
        /// </summary>
        /// <param name="number">要验证的字符串</param>
        /// <returns></returns>
        public static bool IsNumber(string number)
        {
            int temp = 0;

            return int.TryParse(number, out temp);
        }
        /// <summary>
        /// 验证字符串是否为数字
        /// </summary>
        /// <param name="number">要验证的字符串</param>
        /// <param name="rs">要返回的整数</param>
        /// <returns></returns>
        public static bool IsNumber(string number, out int rs)
        {
            return int.TryParse(number, out rs);
        }
        /// <summary>
        /// 判断输入字符串是否为空。（过滤“'”和空格符）
        /// </summary>
        /// <param name="s">源字符串</param>
        /// <param name="o">过滤后的字符串</param>
        /// <returns></returns>
        public static bool IsEmpty(string s, out string o)
        {
            o = s.Replace("'", "").Replace(" ", "");

            if (s.Equals(""))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 验证日期
        /// </summary>
        /// <param name="str_input"></param>
        /// <param name="IsStartDate"></param>
        /// <returns></returns>
        public static string VerDate(string str_input, bool IsStartDate)
        {
            string str_out = "";
            if (IsStartDate)
            {
                str_out = "1900-01-01";
            }
            else
            {
                str_out = DateTime.Now.ToString("yyyy-MM-dd");
            }

            try
            {
                //DateTime dt = DateTime.Parse(str_input);
                IFormatProvider cul = new System.Globalization.CultureInfo("zh-CN", true);
                string[] formats = new string[] { "yyyy", "yyyy-MM-dd", "yyyy-MM", "yyyy-M-d", "yyyy-M-dd", "yyyy-MM-d", "yyyy-MM", "yyyy-M" };
                DateTime dt = DateTime.ParseExact(str_input, formats, cul, System.Globalization.DateTimeStyles.AllowInnerWhite);

                str_out = dt.ToString("yyy-MM-dd");
            }
            catch
            {

            }

            return str_out;
        }
        /// <summary>
        /// 验证数据表格
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool HaveData(object dt)
        {
            if (dt != null)
            {
                DataSet temp = (DataSet)dt;

                if (temp.Tables.Count > 0 && temp.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// 验证字符是否为字母和数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsCharAndNumber(string str)
        {
            if (str == null || str == "")
                return false;
            return System.Text.RegularExpressions.Regex.IsMatch(str, "^[A-Za-z0-9]+$");
        }
        /// <summary>
        /// 验证字符是否为数字
        /// </summary>
        /// <param name="str">参数</param>
        /// <returns>Bool</returns>
        //public static bool IsNumber(string str)
        //{
        //    if (str == null || str == "") 
        //        return false;
        //    return System.Text.RegularExpressions.Regex.IsMatch(str, @"^[-]?[0-9]\d*[.]?\d*$");
        //}
        /// <summary>
        /// 验证字符是否为整数
        /// </summary>
        /// <param name="str">参数</param>
        /// <returns>Bool</returns>
        public static bool IsInterger(string str)
        {
            if (str == null || str == "")
                return false;
            return !System.Text.RegularExpressions.Regex.IsMatch(str, @"[^0-9]");
        }
        /// <summary>
        /// 验证字符是否为日期格式
        /// </summary>
        /// <param name="str">参数</param>
        /// <returns>Bool</returns>
        public static bool IsDate(string str)
        {
            if (str == null || str == "")
                return false;
            return System.Text.RegularExpressions.Regex.IsMatch(str, @"^(?ni:(?=\d)((?'year'((1[6-9])|([2-9]\d))\d\d)(?'sep'[/.-])(?'month'0?[1-9]|1[012])\2(?'day'((?<!(\2((0?[2469])|11)\2))31)|(?<!\2(0?2)\2)(29|30)|((?<=((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(16|[2468][048]|[3579][26])00)\2\3\2)29)|((0?[1-9])|(1\d)|(2[0-8])))(?:(?=\x20\d)\x20|$))?((?<time>((0?[1-9]|1[012])(:[0-5]\d){0,2}(\x20[AP]M))|([01]\d|2[0-3])(:[0-5]\d){1,2}))?)$");
        }
        /// <summary>
        /// 验证是否为正整数
        /// </summary>
        /// <param name="str">参数</param>
        /// <returns>Bool</returns>
        public static bool IsPositveDecimal(string str)
        {
            if (str == null || str == "")
                return false;
            return System.Text.RegularExpressions.Regex.IsMatch(str, @"^[0-9]\d*[.]?\d*$");
        }
        /// <summary>
        /// 验证是否为有效Email地址
        /// </summary>
        /// <param name="str">参数</param>
        /// <returns>Bool</returns>        
        public static bool IsEmail(string str)
        {
            if (str == null || str == "")
                return false;
            return System.Text.RegularExpressions.Regex.IsMatch(str, @"^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$");
        }
        /// <summary>
        /// 验证是否有有效的电话
        /// </summary>
        /// <param name="str">参数</param>
        /// <returns>Bool</returns>
        public static bool IsPhone(string str)
        {
            if (String.IsNullOrEmpty(str))
                return false;
            //return System.Text.RegularExpressions.Regex.IsMatch(str, @"^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$");
            return System.Text.RegularExpressions.Regex.IsMatch(str, @"(^[0-9]{3,4}\-[0-9]{3,8}$)|(^[0-9]{3,8}$)|(^\([0-9]{3,4}\)[0-9]{3,8}$)|(^0{0,1}[0-9]{11}$)");
        }
        /// <summary>
        /// 验证是否为有效的分数(规则为整数或者1位小数)
        /// </summary>
        /// <param name="score">分数</param>
        /// <returns></returns>
        public static bool IsVaildScore(string score)
        {
            if (score == null || score.Length == 0)
                return false;
            var pattern = "[^0-9|^.]";
            if (System.Text.RegularExpressions.Regex.IsMatch(score, pattern))
                return false;
            pattern = "((^[1-9][0-9]{0,2})(.[0-9])?)$|^0$|^0.0|^(0.[0-9]{1,1})$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(score, pattern))
                return false;
            double value = -1.0;
            if (!double.TryParse(score, out  value))
            {
                value = -1.0;
            }
            return value >= 0.0 && value <= 100.0;
        }

        #region IP范围
        /// <summary>
        /// 验证IP范围
        /// </summary>
        /// <param name="ip">用户</param>
        /// <param name="iprange">ip范围（192.168.25.197-192.168.100.217,192.168.100.217-192.168.100.218）</param>
        /// <returns></returns>
        public static bool TheIpIsRange(string ip, string iprange)
        {
            string[] rangipaddresslist = iprange.Split(',');

            return IpIsRange(ip, rangipaddresslist);
        }

        //接口函数 参数分别是你要判断的IP  和 你允许的IP范围
        //（已经重载）
        //（允许同时指定多个数组）
        static bool IpIsRange(string userip, params string[] ranges)
        {
            bool tmpRes = false;
            foreach (var item in ranges)
            {
                if (IsRange(userip, item))
                {
                    tmpRes = true; break;
                }
            }

            return tmpRes;
        }

        /// <summary>
        /// 判断指定的IP是否在指定的IP范围内   这里只能指定一个范围
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="ranges"></param>
        /// <returns></returns>
        static bool IsRange(string ip, string ranges)
        {
            bool result = false;
            int count;
            string start_ip, end_ip;
            //检测指定的IP范围 是否合法
            if (TryParseRanges(ranges, out count, out start_ip, out end_ip))//检测ip范围格式是否有效
            {
                if (ip == "::1") ip = "127.0.0.1";

                try
                {
                    IPAddress.Parse(ip);//判断指定要判断的IP是否合法
                }
                catch (Exception ex)
                {
                    //KYCX.Logging.Logger.DefaultLogger.ErrorFormat("要检测的IP地址无效", ex);

                    return result;
                }

                if (count == 1 && ip == start_ip) result = true;//如果指定的IP范围就是一个IP，那么直接匹配看是否相等
                else if (count == 2)//如果指定IP范围 是一个起始IP范围区间
                {

                    bool tmpRes = false;
                    if (MyTransform.IPToInt64(start_ip) <= MyTransform.IPToInt64(ip) && MyTransform.IPToInt64(ip) <= MyTransform.IPToInt64(end_ip))
                    {
                        tmpRes = true;
                    }

                    result = tmpRes;
                }
            }

            return result;
        }

        //尝试解析IP范围  并获取闭区间的 起始IP   (包含)
        private static bool TryParseRanges(string ranges, out int count, out string start_ip, out string end_ip)
        {
            string[] _r = ranges.Split('-');
            count = _r.Count();

            start_ip = _r[0];
            end_ip = "";

            if (!(_r.Count() == 2 || _r.Count() == 1))
            {
                //KYCX.Logging.Logger.DefaultLogger.ErrorFormat("TryParseRanges(),IP范围指定格式不正确，可以指定一个IP，如果是一个范围请用“-”分隔", "");
                return false;
            }

            try
            {
                IPAddress.Parse(_r[0]);
                if (_r.Count() == 2)
                {
                    end_ip = _r[1];
                    IPAddress.Parse(_r[1]);
                }
                return true;
            }
            catch (Exception)
            {
                //KYCX.Logging.Logger.DefaultLogger.ErrorFormat("TryParseRanges(),IP地址无效", "");
                return false;
            }
        }

        #endregion
    }
}

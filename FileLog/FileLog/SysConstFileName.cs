using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileLog
{
    /// <summary>
    /// 系统常量性文件名规范
    /// </summary>
    public static class SysConstFileName
    {
        /// <summary>
        /// 系统日志文件名
        /// </summary>
        public static string SysLogFileName(string MerchantId)
        {

            return System.AppDomain.CurrentDomain.BaseDirectory + "logs\\sys" + MerchantId + DateTime.Now.ToString("yyyy-MM-dd") + ".log";

        }
        /// <summary>
        /// 系统跟踪日志名
        /// </summary>
        public static string TraceLogFileName(string MerchantId)
        {

            return System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "logs\\tra" + MerchantId + DateTime.Now.ToString("yyyy-MM-dd") + ".log";

        }
        /// <summary>
        /// 系统事件日志文件名
        /// </summary>
        public static string EventLogFileName(string MerchantId)
        {

            return System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "logs\\evt" + MerchantId + DateTime.Now.ToString("yyyy-MM-dd") + ".log";

        }
        /// <summary>
        /// 系统事件日志文件名
        /// </summary>
        public static string SysOptLogFileName(string MerchantId)
        {

            return System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "logs\\opt" + MerchantId + DateTime.Now.ToString("yyyy-MM-dd") + ".log";

        }
        /// <summary>
        /// 系统登录日志文件名
        /// </summary>
        public static string SysLoginFileName(string MerchantId)
        {

            return System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "logs\\login" + MerchantId + DateTime.Now.ToString("yyyy-MM-dd") + ".log";

        }
    }
}

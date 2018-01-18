using SMSManagement.Web.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMSManagement.Web.SP
{
    /// <summary>
    /// 系统结构
    /// </summary>
    public enum ArchitectureMode
    {
        BS,
        CS
    }
    public class ep
    {
        public static ArchitectureMode ArchMode = ArchitectureMode.BS;//系统结构

        public static LoginInfo Lif;

        //User
        public static string id_main { get; set; }
        public static string pw_main { get; set; }

        //Server
        public static string id { get; set; }
        public static string pw { get; set; }

        public static readonly int PageSize = 10;
        /// <summary>
        /// 用户登录信息
        /// </summary>
        public static LoginInfo UserInfo
        {
            get
            {
                if (ArchMode == ArchitectureMode.BS)
                {
                    if (System.Web.HttpContext.Current.Session["UserInfo"] != null)
                    {
                        return (LoginInfo)System.Web.HttpContext.Current.Session["UserInfo"];
                    }
                }
                else
                {
                    return Lif;
                }

                return null;
            }
            set
            {
                Lif = value;
                System.Web.HttpContext.Current.Session["UserInfo"] = Lif;
            }
        }

        public static string UserServerIP
        {
            get
            {
                if (ArchMode == ArchitectureMode.BS)
                {

                    return System.Configuration.ConfigurationManager.AppSettings["UserServerIP"];
                }
                else
                {
                    return XMLConfig.GetString(XmlFileName.DataConfiger, "UserServerIP");
                }

            }
        }
        /// <summary>
        /// 用户信息数据库名称
        /// </summary>
        public static string UserDBName
        {
            get
            {
                if (ArchMode == ArchitectureMode.BS)
                {

                    return System.Configuration.ConfigurationManager.AppSettings["UserDBName"];

                }
                else
                {
                    return XMLConfig.GetString(XmlFileName.DataConfiger, "UserDataBase");
                }
            }
        }



        private static void SetInfo_Main()
        {
            DllImports dip = new DllImports();

            string userid = string.Empty;
            string userpwd = string.Empty;

            dip.SetInfo_Main(ref userid, ref userpwd);

            id_main = userid;
            pw_main = userpwd;
        }

        public static string UserServerName
        {
            get
            {
                if (id_main == null)
                {
                    SetInfo_Main();
                }

                return id_main;
            }
        }
        /// <summary>
        /// 用户信息服务器的密码
        /// </summary>
        public static string UserServerPassword
        {
            get
            {
                if (pw_main == null)
                {
                    SetInfo_Main();
                }

                return pw_main;
            }
        }

        public static string ServerIP
        {
            get
            {
                if (ArchMode == ArchitectureMode.BS)
                {
                    return System.Configuration.ConfigurationManager.AppSettings["ServerIP" + UserInfo.ServerID].Split('|')[0];
                }
                else
                {

                    return XMLConfig.GetString(XmlFileName.DataConfiger, "DbHost");

                }
            }
        }

        public static string DBName
        {
            get
            {
                if (ArchMode == ArchitectureMode.BS)
                {

                    return System.Configuration.ConfigurationManager.AppSettings["ServerIP" + UserInfo.ServerID].Split('|')[1];

                }
                else
                {

                    return XMLConfig.GetString(XmlFileName.DataConfiger, "DataBase");

                }
            }
        }

        private static void SetInfo()
        {
            DllImports dip = new DllImports();

            string serverid = string.Empty;
            string serverpwd = string.Empty;

            dip.SetInfo(ref serverid, ref serverpwd);

            id = serverid;
            pw = serverpwd;

        }

        public static string ServerSQLName
        {
            get
            {
                if (id == null)
                {
                    SetInfo();
                }

                return id;
            }
        }
        /// <summary>
        /// 分配给用户使用的SQL数据库密码
        /// </summary>
        public static string ServerSQLPassword
        {
            get
            {
                if (pw == null)
                {
                    SetInfo();
                }

                return pw;
            }
        }

        /// <summary>
        /// 上传文件保存位置
        /// </summary>
        public static string SaveFolder
        {
            get
            {
                if (ArchMode == ArchitectureMode.BS)
                {
                    return System.Configuration.ConfigurationManager.AppSettings["UNAME"];
                }
                else
                {
                    return XMLConfig.GetString(XmlFileName.DataConfiger, "UNAME");
                }
            }
        }

        /// <summary>
        /// 本地盘符
        /// </summary>
        public static string LocalDisk
        {
            get
            {
                if (ArchMode == ArchitectureMode.BS)
                {
                    return System.Configuration.ConfigurationManager.AppSettings["LocalDisk"];
                }
                else
                {
                    return XMLConfig.GetString(XmlFileName.DataConfiger, "LocalDisk");
                }

            }
        }



        public static string BaseCheckServiceInfo
        {
            get
            {
                if (ArchMode == ArchitectureMode.BS)
                {
                    return System.Configuration.ConfigurationManager.AppSettings["BaseCheckServiceInfo"];
                }
                else
                {
                    return XMLConfig.GetString(XmlFileName.DataConfiger, "BaseCheckServiceInfo");
                }

            }
        }

        /// <summary>
        /// FTP连接字符串
        /// </summary>
        public static string FtpConnectString
        {
            get
            {
                if (ArchMode == ArchitectureMode.CS)
                {
                    return XMLConfig.GetString(XmlFileName.DataConfiger, "FtpConnectString");
                }
                else
                {
                    return System.Configuration.ConfigurationManager.AppSettings["FtpConnectString"];
                }

            }
        }

        public static string CheckSystemType
        {
            get
            {
                if (ArchMode == ArchitectureMode.CS)
                {
                    return XMLConfig.GetString(XmlFileName.DataConfiger, "CheckSystemType");
                }
                else
                {
                    return System.Configuration.ConfigurationManager.AppSettings["CheckSystemType"];
                }

            }
        }

        public static string ReportServerMappingDir
        {
            get
            {
                if (ArchMode == ArchitectureMode.CS)
                {
                    return XMLConfig.GetString(XmlFileName.DataConfiger, "ReportServerMappingDir");
                }
                else
                {
                    return System.Configuration.ConfigurationManager.AppSettings["ReportServerMappingDir"];
                }

            }
        }

        /// <summary>
        /// 报告单下载连接，例http://192.168.100.66:8093
        /// </summary>
        public static string ReportDownLoadLink
        {
            get
            {
                if (ArchMode == ArchitectureMode.CS)
                {
                    return XMLConfig.GetString(XmlFileName.DataConfiger, "ReportDownLoadLink");
                }
                else
                {
                    return System.Configuration.ConfigurationManager.AppSettings["ReportDownLoadLink"];
                }

            }
        }

        public static string KbaseIndexViewServerIP
        {
            get
            {
                if (ArchMode == ArchitectureMode.CS)
                {
                    return XMLConfig.GetString(XmlFileName.DataConfiger, "KbaseIndexViewServerIP");
                }
                else
                {
                    return System.Configuration.ConfigurationManager.AppSettings["KbaseIndexViewServerIP"];
                }

            }
        }

        public static string ReportTypeList
        {
            get
            {
                return "1,2,3,4";
            }
        }

        /// <summary>
        /// 导出excel条件
        /// </summary>
        public static string ExportExcelInfo
        {
            get
            {

                if (System.Web.HttpContext.Current.Session["ExportExcelInfo"] != null)
                {
                    return (string)System.Web.HttpContext.Current.Session["ExportExcelInfo"];
                }
                return "";

            }
            set
            {
                System.Web.HttpContext.Current.Session["ExportExcelInfo"] = value;
            }
        }

        /// <summary>
        /// 获取IP范围
        /// </summary>
        public static string IPRange
        {
            get
            {

                string value = "";
                if (System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains("IPRange"))
                {
                    value = System.Configuration.ConfigurationManager.AppSettings["IPRange"];

                }
                return value;
            }
        }

        public static string AesKey
        {
            get
            {
                //return System.Configuration.ConfigurationManager.AppSettings["AesKey"];
                string aesKey = "";
                if (System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains("AesKey"))
                {
                    aesKey = System.Configuration.ConfigurationManager.AppSettings["AesKey"];
                    if (aesKey.Length > 32)
                    {
                        aesKey = aesKey.Substring(0, 32);
                    }
                }
                return aesKey;
            }
        }

        /// <summary>
        /// 本地盘符
        /// </summary>
        public static string ErrorPath
        {
            get
            {
                if (ArchMode == ArchitectureMode.BS)
                {
                    return System.Configuration.ConfigurationManager.AppSettings["ErrorPath"];
                }
                else
                {
                    return XMLConfig.GetString(XmlFileName.DataConfiger, "ErrorPath");
                }

            }
        }

        public static Customerlink GetSQLLink(DataConnectType dct)
        {
            Customerlink cl = null;

            try
            {
                if (dct == DataConnectType.UserDBDataService)
                {
                    cl = new Customerlink(UserServerIP, UserDBName, UserServerName, UserServerPassword);

                }
                else if (dct == DataConnectType.ServerDBDataService)
                {
                    cl = new Customerlink(ServerIP, DBName, ServerSQLName, ServerSQLPassword);
                }
                else
                {
                    cl = new Customerlink(CustomServerIP, CustomServerDBName, ServerSQLName, ServerSQLPassword);
                }


            }
            catch (Exception ex)
            {
                cl = null;
            }
            return cl;
        }

        public static string CustomServerIP
        {
            get
            {
                if (ArchMode == ArchitectureMode.BS)
                {

                    return System.Configuration.ConfigurationManager.AppSettings["SMSServerIP"];
                }
                else
                {
                    return XMLConfig.GetString(XmlFileName.DataConfiger, "SMSServerIP");
                }

            }
        }
        /// <summary>
        /// 用户信息数据库名称
        /// </summary>
        public static string CustomServerDBName
        {
            get
            {
                if (ArchMode == ArchitectureMode.BS)
                {
                    return System.Configuration.ConfigurationManager.AppSettings["SMSServerDBName"];

                }
                else
                {
                    return XMLConfig.GetString(XmlFileName.DataConfiger, "SMSServerDBName");
                }
            }
        }

        public static string SendBlockDueTime
        {
            get
            {
                if (ArchMode == ArchitectureMode.BS)
                {
                    return System.Configuration.ConfigurationManager.AppSettings["SendBlockDueTime"];
                }
                else
                {
                    return "";
                }
            }
        }
        public static string SendBlockBatchMaxNum
        {
            get
            {
                if (ArchMode == ArchitectureMode.BS)
                {
                    return System.Configuration.ConfigurationManager.AppSettings["SendBlockBatchMaxNum"];
                }
                else
                {
                    return "";
                }
            }
        }
        public static string DBBlockDueTime
        {
            get
            {
                if (ArchMode == ArchitectureMode.BS)
                {
                    return System.Configuration.ConfigurationManager.AppSettings["DBBlockDueTime"];
                }
                else
                {
                    return "";
                }
            }
        }
        public static string DBBlockBatchMaxNum
        {
            get
            {
                if (ArchMode == ArchitectureMode.BS)
                {
                    return System.Configuration.ConfigurationManager.AppSettings["DBBlockBatchMaxNum"];
                }
                else
                {
                    return "";
                }
            }
        }
    }
}

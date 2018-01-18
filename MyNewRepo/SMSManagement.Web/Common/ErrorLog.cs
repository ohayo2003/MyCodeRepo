using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.Serialization;

namespace SMSManagement.Web.Common
{   
	/// <summary>
	/// 定义异常类
	/// </summary>
	public class ConnectionException:System.Exception
	{
		/// <summary>
		/// 记录错误日志
		/// </summary>
		/// <param name="msg">日志标题</param>
		/// <param name="ex">内容</param>
		public ConnectionException(string msg,Exception ex)
		{
			Errors.WriteLog(msg,ex);
		}
        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="msg"></param>
		public ConnectionException(string msg):base("数据库连接超时,数据不存在或尚未启动!")
		{
			this.Source=msg;
		}
	}
    public class DataAccessExcption : System.Exception
    {
        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="msg">日志标题</param>
        /// <param name="ex">内容</param>
        public DataAccessExcption(string msg, Exception ex)
        {
            Errors.WriteLog(msg, ex);
        }
        public DataAccessExcption(string msg): base("数据访问失败,字段名错误或参数错误!")
        {
            this.Source = msg;
        }
    }
    /// <summary>
    /// 功能描述:记录系统错误日志
    ///          读取系统错误日志
    ///          清除系统错误日志
    ///
    /// </summary>
    public class Errors
    {
        /// <summary>
        /// 写错误日志
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="Ex">异常信息</param>
        public static void WriteLog(string msg, Exception Ex)
        {

            ErrorsData Log = new ErrorsData();
            try
            {
                if (!System.IO.Directory.Exists(System.Configuration.ConfigurationSettings.AppSettings["ErrorPath"]))
                    System.IO.Directory.CreateDirectory(System.Configuration.ConfigurationSettings.AppSettings["ErrorPath"]);

                if (System.IO.File.Exists(System.Configuration.ConfigurationSettings.AppSettings["ErrorPath"] + "\\Errors.xml"))
                {
                    Log = ReadLog();
                }
                DataRow row = Log.Tables[0].NewRow();
                row[ErrorsData.DESCRIPTION_Field] = msg;
                row[ErrorsData.DETAIL_Field] = Ex.ToString();
                row[ErrorsData.DATE__Field] = DateTime.Now;
                Log.Tables[0].Rows.Add(row);
                Log.WriteXml(System.Configuration.ConfigurationSettings.AppSettings["ErrorPath"] + "\\Errors.xml");
            }
            catch
            {
                return;
            }

        }

        /// <summary>
        /// 读取错误日志信息
        /// </summary>
        /// <returns></returns>
        public static ErrorsData ReadLog()
        {
            string fullname = System.Configuration.ConfigurationSettings.AppSettings["ErrorPath"] +"\\Errors.xml";
            if (System.IO.File.Exists(fullname))
                return ReadLog(System.Configuration.ConfigurationSettings.AppSettings["ErrorPath"] + "\\Errors.xml");
            else
                return new ErrorsData();
        }
        /// <summary>
        /// 读取错误日志信息
        /// </summary>
        /// <param name="path">错误日志信息文件</param>
        /// <returns>错误日志数据实体</returns>
        public static ErrorsData ReadLog(string path)
        {
            try
            {
                ErrorsData Log = new ErrorsData();
                Log.ReadXml(path, System.Data.XmlReadMode.IgnoreSchema);
                return Log;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 清除错误日志
        /// </summary>
        public static void ClearLog()
        {
            try
            {
                if (System.IO.File.Exists(System.Configuration.ConfigurationSettings.AppSettings["ErrorPath"] + "\\Errors.xml"))
                {
                    System.IO.File.Delete(System.Configuration.ConfigurationSettings.AppSettings["ErrorPath"] + "\\Errors.xml");
                }

            }
            catch
            {
                return;
            }
        }
        /// <summary>
        /// ErrorsData 的摘要说明。
        /// </summary>
        [System.ComponentModel.DesignerCategory("+Code")]
        [SerializableAttribute]
        public class ErrorsData : DataSet
        {
            public const string ERRORS_Table = "Errors";
            public const string ID_Field = "ID";
            public const string DESCRIPTION_Field = "Description";
            public const string DETAIL_Field = "Detail";
            public const string DATE__Field = "Date_";

            public ErrorsData(SerializationInfo info, StreamingContext context): base(info, context)
            {
            }
            public ErrorsData()
            {
                BuildeErrorLog();
            }
            private void BuildeErrorLog()
            {

                DataTable table = new DataTable(ERRORS_Table);
                DataColumnCollection columns = table.Columns;
                columns.Add(ID_Field, typeof(System.Int64));
                columns.Add(DESCRIPTION_Field, typeof(System.String));
                columns.Add(DETAIL_Field, typeof(System.String));
                columns.Add(DATE__Field, typeof(System.DateTime));

                this.Tables.Add(table);
            }
        }
    }
}

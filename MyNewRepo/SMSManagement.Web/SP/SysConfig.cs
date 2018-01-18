using System;
using System.Web;
using System.Windows;
using System.Configuration;
using System.Reflection;

namespace SMSManagement.Web.SP
{
    public class SysConfig
    {
        private static string m_configpath;//配置文件目录
		private static string m_errorpath;//错误日志目录

		public static ArchitectureMode ArchMode =ArchitectureMode.BS;//系统结构
		
		/// <summary>
		/// 配置文件目录
		/// </summary>
		public static string ConfigPath
		{
			get
			{
				if(m_configpath==null)
				{
					if(ArchMode==ArchitectureMode.BS)
					{
						m_configpath=System.Configuration.ConfigurationSettings.AppSettings["ConfigPath"];
					}
					else
					{
                        //m_configpath=System.Windows.Forms.Application.StartupPath;
					}
				}
				return m_configpath;
			}
		}
		/// <summary>
		/// 系统错误日志目录
		/// </summary>
		public static string ErrorPath
		{
			get
			{
				if(m_errorpath==null)
				{
					if(ArchMode==ArchitectureMode.BS)
					{
						m_errorpath=System.Configuration.ConfigurationSettings.AppSettings["ErrorPath"];
					}
					else
					{
						m_errorpath=AppDomain.CurrentDomain.BaseDirectory+"Log\\";
					}
				}
				return m_errorpath;
			}
		}
		/// <summary>
		/// 版本
		/// 0\国家版
		/// 1\多服务器(省市版)
		/// 2\多服务器(地市版)
		/// 3\单服务器版
		/// </summary>
		public static int Edition
		{
			get
			{
				return 1;
			}

		}
		/// <summary>
		/// 当前用户的最高等级
		/// </summary>
		public static int Grade
		{
			get
			{
				return 1;
			}
		}
		/// <summary>
		/// 版本名称号
		/// </summary>
		public static string Version
		{
			get
			{
				return "Beta版";
			}
		}

    }

    /// <summary>
    /// 配置文件的类型
    /// 1、数据库配置文件
    /// </summary>
    public enum XmlFileName
    {
        /// <summary>
        /// 数据库配置参数
        /// </summary>
        DataConfiger,//数据库配置参数
        /// <summary>
        /// 分系统配置参数
        /// </summary>
        SystemConfiger
    }
    /// <summary>
    /// 配置文件定义
    /// </summary>
    public class XmlFile
    {
        /// <summary>
        /// 配置文件保存路径
        /// </summary>
        public string Path;
        /// <summary>
        /// 配置初始文件
        /// </summary>
        public string InitXml;
        /// <summary>
        /// 文件索引串
        /// </summary>
        public string IndexStr;
    }
}

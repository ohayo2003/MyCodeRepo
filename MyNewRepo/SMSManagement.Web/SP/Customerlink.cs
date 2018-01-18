using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMSManagement.Web.SP
{
    public class Customerlink
    {
        /// <summary>
        /// 数据库IP
        /// </summary>
        public string m_ServerIP { set; get; }

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string m_DbName { set; get; }

        /// <summary>
        /// 登录用户ID
        /// </summary>
        public string m_ServerID { set; get; }

        /// <summary>
        /// 登录用户密码
        /// </summary>
        public string m_PassWord { set; get; }


        public Customerlink()
        {
            m_ServerIP = "";

            m_DbName = "";

            m_ServerID = "";

            m_PassWord = "";

        }
        /// <summary>
        /// 初始化用户指定连接参数
        /// </summary>
        /// <param name="ServerIP">数据库IP</param>
        /// <param name="DbName">数据库名称</param>
        /// <param name="ServerID">登录用户ID</param>
        /// <param name="PassWord">登录用户密码</param>
        public Customerlink(string ServerIP, string DbName, string ServerID, string PassWord)
        {
            m_ServerIP = ServerIP;

            m_DbName = DbName;

            m_ServerID = ServerID;

            m_PassWord = PassWord;

        }

    }

    public enum DataConnectType
    {
        /// <summary>
        /// 用户服务器
        /// </summary>
        UserDBDataService = 1,
        /// <summary>
        /// 数据服务器
        /// </summary>
        ServerDBDataService = 2,
        /// <summary>
        /// 指定服务器
        /// </summary>
        CustomDBDataService = 3


    }


}

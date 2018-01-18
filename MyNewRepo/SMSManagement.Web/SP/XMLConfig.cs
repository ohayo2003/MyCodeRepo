using System;
using System.Xml;

namespace SMSManagement.Web.SP
{
    public class XMLConfig
    {

        /// <summary>
        /// 初始化xml串，返回指定的Xml文件串
        /// </summary>
        /// <param name="filename">文件名称</param>
        /// <returns>xml文件串</returns>
        private static XmlFile InitXml(XmlFileName filename)
        {
            XmlFile file = new XmlFile();
            switch (filename)
            {
                case XmlFileName.DataConfiger://配置环境
                    file.InitXml = "<?xml version='1.0' encoding='UTF-8'?><Config><Servers >"
                        + "<Server id='1'>"
                        + "<DbHost>localhost</DbHost>"
                        + "<DataBase>THESISCHECK</DataBase>"
                        + "<UserDataBase>USERINFO</UserDataBase>"
                        + "<ChildTables>4</ChildTables>"
                        + "<Interval>5</Interval>"
                        + "<Top>200</Top>"
                        + "<UserServerIP>localhost</UserServerIP>"
                        + "<StartTime>1</StartTime>"
                        + "<StopTime>23</StopTime>"
                        + "<SaveDisk>D</SaveDisk>"
                        + "<FileServerDisk>Z</FileServerDisk>"
                        + "<ServerNumber>5</ServerNumber>"
                        + "<IsDelete>!true</IsDelete>"
                        + "<Kprefix>Thesis_</Kprefix>"
                        + "<KBaseIndexIP>127.0.0.1</KBaseIndexIP>"
                        + "<FtpConnectString>127.0.0.1;report;cnki123</FtpConnectString>"
                        + "<ReportServerType>1</ReportServerType>"
                        + "<UserPwdTrans_Type>1103</UserPwdTrans_Type>"
                        + "<ServerIndex>1</ServerIndex>"
                        + "<DupCheckServerIP>192.168.100.218</DupCheckServerIP>"
                        + "<DupCheckDbName>DUPCHECK</DupCheckDbName>"
                        + "<CurSystemType>PMLC</CurSystemType>"
                        + "<IssueIP>192.168.100.152</IssueIP>"
                        + "<IssueCollectionViewName>CJFDTOTAL</IssueCollectionViewName>"
                        + "<ServerList>192.168.100.218,AMLC_DB;192.168.100.218,SMLC_DB</ServerList>"
                        + "<UserServerList>192.168.22.120,AMLC_USERINFO;192.168.22.120,SMLC_USERINFO</UserServerList>"
                        + "</Server>"
                        + "</Servers></Config>";
                    file.Path = SysConfig.ConfigPath + "\\config.xml";
                    file.IndexStr = "Config/Servers/Server[@id='1']/";
                    return file;

                case XmlFileName.SystemConfiger://配置参数
                    file.InitXml = "<?xml version='1.0' encoding='UTF-8'?><SystemConfig><Systems>"
                        + "<System type=''>"
                        + "</System>"
                        + "</Systems></SystemConfig>";
                    file.Path = SysConfig.ConfigPath + "\\systemconfig.xml";
                    file.IndexStr = "SystemConfig/Systems/System";
                    return file;

                default:
                    return null;
            }
        }

        /// <summary>
        /// 创建xml配置文件
        /// </summary>
        /// <param name="file">文件类型</param>
        private static void Creat(XmlFile file)
        {
            XmlDocument obj = new XmlDocument();
            try
            {
                obj.LoadXml(file.InitXml);
                obj.Save(file.Path);
            }
            catch (Exception Ex)
            {
                //MessageBox.Show("创建系统配置文件失败,可能由以下原因引起:\n" + Ex.Message);

            }
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="filename">文件类型</param>
        /// <param name="name">项目名称</param>
        /// <returns>项目内容</returns>
        public static string GetString(XmlFileName filename, string name)
        {
            XmlFile file = InitXml(filename);
            if (file == null) return "";
            try
            {
                if (!System.IO.File.Exists(file.Path))
                    Creat(file);
                XmlDocument obj = new XmlDocument();
                obj.Load(file.Path);
                XmlNode node = obj.SelectSingleNode(file.IndexStr + name);
                if (node != null)
                {
                    return node.InnerText;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception Ex)
            {
                //MessageBox.Show("读取配置文件错误,可能由以下原因引起:\n" + name + "\n" + Ex.Message);
                return "";
            }
        }

        public static string GetString(XmlFileName filename, string systemtype, string name)
        {
            XmlFile file = InitXml(filename);
            if (file == null) return "";
            try
            {
                if (!System.IO.File.Exists(file.Path))
                    Creat(file);
                XmlDocument obj = new XmlDocument();
                obj.Load(file.Path);
                XmlNode node = obj.SelectSingleNode(file.IndexStr + "[@type='" + systemtype.Trim().ToLower() + "']/" + name);
                if (node != null)
                {
                    return node.InnerText;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception Ex)
            {
                //MessageBox.Show("读取配置文件错误,可能由以下原因引起:\n" + name + "\n" + Ex.Message);
                return "";
            }
        }

        /// <summary>
        /// 设置文件
        /// </summary>
        /// <param name="filename">文件类型</param>
        /// <param name="name">项目名称</param>
        /// <param name="values">项目值</param>
        public static void SetString(XmlFileName filename, string name, string values)
        {
            XmlFile file = InitXml(filename);
            if (file == null) return;
            try
            {
                if (!System.IO.File.Exists(file.Path))
                    Creat(file);
                XmlDocument obj = new XmlDocument();
                obj.Load(file.Path);
                XmlNode node = obj.SelectSingleNode(file.IndexStr + name);
                node.InnerText = values;
                obj.Save(file.Path);
            }
            catch (Exception Ex)
            {
                //MessageBox.Show("设置配置文件出错,可能由以下原因引起:\n" + Ex.Message);
            }
        }



        /// <summary>
        /// 读配置文件
        /// </summary>
        /// <param name="configKey">名称</param>
        public static string ReadXml(string configKey)
        {
            string str = string.Empty;
            try
            {
                XmlDocument doc = new XmlDocument();
                string strFileName = AppDomain.CurrentDomain.BaseDirectory.ToString() + "config.xml";
                doc.Load(strFileName);
                //找出名称为“add”的所有元素
                XmlNodeList nodes = doc.GetElementsByTagName("add");
                for (int i = 0; i < nodes.Count; i++)
                {
                    //获得将当前元素的key属性
                    XmlAttribute att = nodes[i].Attributes["key"];
                    //根据元素的第一个属性来判断当前的元素是不是目标元素
                    if (att.Value == configKey)
                    {
                        //对目标元素中的第二个属性赋值
                        att = nodes[i].Attributes["value"];
                        str = att.Value.ToString();
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
            return str;
        }

    }
}

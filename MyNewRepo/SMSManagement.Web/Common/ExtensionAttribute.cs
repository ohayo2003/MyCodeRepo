using SMSManagement.Web.SP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Protocols;

namespace SMSManagement.Web.Common
{

    /// <summary>
    /// 验证用户请求IP范围
    /// </summary>
    public class ExtensionAttribute : SoapExtensionAttribute
    {
        int _priority = 1;

        public override int Priority
        {
            get
            {
                return _priority;
            }
            set
            {
                _priority = value;
            }
        }

        public override Type ExtensionType
        {
            get { return typeof(MyExtension); }
        }
    }


    public class MyExtension : SoapExtension
    {

        public override object GetInitializer(Type serviceType)
        {
            return GetType();
        }

        public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute)
        {
            return null;
        }

        public override void Initialize(object initializer)
        {

        }

        //这个override的方法会被调用四次
        //分别是SoapMessageStage的BeforeSerialize,AfterSerialize,BeforeDeserialize,AfterDeserialize
        public override void ProcessMessage(SoapMessage message)
        {
            if (message.Stage == SoapMessageStage.AfterDeserialize) //反序列化后处理
            {
                if (!VerifyIP())
                {
                    string userip = IPHelper.GetClientIP();
                    string msg = "请求IP(" + userip + ")超出有效范围";
                    //KYCX.Logging.Logger.DefaultLogger.Error("ProcessMessage()," + msg);
                    throw new SoapHeaderException(msg, SoapException.ClientFaultCode);
                }       
            }
        }

        public bool VerifyIP()
        {
            bool r = false;
            string userip = IPHelper.GetClientIP();
            string rangeip = ep.IPRange;
            if (rangeip.Length > 0)
            {

                return MyVerify.TheIpIsRange(userip, rangeip);
            }
            return r;

        }
    }


}
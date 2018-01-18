using SMSManagement.Web.Common;
using SMSManagement.Web.Model;
using SMSManagement.Web.Work;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMSManagement.Web.SMSHandler
{
    public class CnkiSender
    {

        public static CnkiSender Instance { get; private set; }

        static CnkiSender()
        {
            Instance = new CnkiSender();
        }

        private CnkiSender()
        {
            url = "http://211.151.247.202/ttknoaws/sms.asmx";
        }

        private string url;

        public int SingleSend(SendMsgStruct msg)
        {
            int result = -1;

            try
            {
                object[] objArray = new object[10];
                objArray[0] = msg.TelNumber;
                objArray[1] = msg.Content;
                objArray[2] = "";
                objArray[3] = "g";
                objArray[4] = "1010340110101";
                objArray[5] = "科研诚信分公司";
                objArray[6] = "kycx01";
                objArray[7] = "科研诚信";
                objArray[8] = "";
                objArray[9] = true;

                result = (int)WebServiceHelper.InvokeWebService(url, "SMS_Add_ForAll", objArray);
            }
            catch (Exception ex)
            {
                AsyncHelper.RunSync<bool>(() => Manager.Instance.WriteLogFile("cnki发送msg出现问题", ex));
                result = -2;
            }

            return result;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SMSManagement.Web.Model
{
    [Serializable]
    public class ReceiveMsgStruct
    {
        /// <summary>
        /// datetime序列化要求只保留年月日
        /// </summary>
        //[JsonConverter(typeof(ChinaDateTimeConverter))]
        //public DateTime timeBegin { get; set; }

        //[JsonConverter(typeof(ChinaDateTimeConverter))]
        //public DateTime timeEnd { get; set; }

        public string CSShortName { get; set; }
        public string UserName { get; set; }

        public Guid UniqueID { get; set; }

        public long RequestTime { get; set; }
        public long ReceiveTime { get; set; }

        public string TelNumber { get; set; }
        public string Content { get; set; }



    }

    [Serializable]
    public class SendMsgStruct
    {
        public string CSShortName { get; set; }
        public string UserName { get; set; }

        public string HName { get; set; }
        public Guid UniqueID { get; set; }

        public long ReceiveTime { get; set; }
        public long SendTime { get; set; }

        public string TelNumber { get; set; }
        public string Content { get; set; }

        public Guid BatchID { get; set; }
        public int SendState { get; set; }
        public string ErrorMsg { get; set; }

        public SendMsgStruct()
        {
            CSShortName = "";
            UserName = "";

            HName = "";
            UniqueID = Guid.Empty;

            ReceiveTime = -1;
            SendTime = -1;


            TelNumber = "";
            Content = "";

            BatchID = Guid.Empty;
            SendState = -1;

            ErrorMsg = "";
        }


        public static SendMsgStruct Copy(SendMsgStruct sms)
        {
            SendMsgStruct tmp = new SendMsgStruct();

            tmp.CSShortName = string.Copy(sms.CSShortName);
            tmp.UserName = string.Copy(sms.UserName);

            tmp.HName = string.Copy(sms.HName);
            tmp.UniqueID = sms.UniqueID;

            tmp.ReceiveTime = sms.ReceiveTime;
            tmp.SendTime = sms.SendTime;


            tmp.TelNumber = string.Copy(sms.TelNumber);
            tmp.Content = string.Copy(sms.Content);

            tmp.BatchID = sms.BatchID;
            tmp.SendState = sms.SendState;

            tmp.ErrorMsg = string.Copy(sms.ErrorMsg);

            return tmp;
        }
    }

}

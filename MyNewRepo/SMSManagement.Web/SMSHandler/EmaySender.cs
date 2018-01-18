using Newtonsoft.Json;
using SMSManagement.Web.Common;
using SMSManagement.Web.Work;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SMSManagement.Web.SMSHandler
{
    public class SMSRequestEntity
    {
        public string mobile { get; set; }
        public string customSmsId { get; set; }
        public string content { get; set; }
    }

    public class SMSRequestBody
    {
        public List<SMSRequestEntity> smses { get; set; }
        public string timerTime { get; set; }
        public string extendedCode { get; set; }
        public long requestTime { get; set; }
        public int requestValidPeriod { get; set; }

        public SMSRequestBody()
        {
            smses = new List<SMSRequestEntity>();
            timerTime = "";
            extendedCode = "112";
            requestValidPeriod = 30;
        }
    }

    public class SMSResponseEntity
    {
        public string mobile { get; set; }
        public string smsId { get; set; }
        public string customSmsId { get; set; }
    }

    public class SMSResponseBody
    {
        public List<SMSResponseEntity> smses { get; set; }
        public string resultCode { get; set; }
        public bool flag { get; set; }

        public SMSResponseBody()
        {
            smses = new List<SMSResponseEntity>();
            resultCode = "";
            flag = false;
        }
    }


    public class EmaySender
    {
        public static EmaySender Instance { get; private set; }
        static EmaySender()
        {
            Instance = new EmaySender();
        }

        private EmaySender()
        {
            appId = "EUCP-EMY-SMS1-0DWDN";
            secretKey = "DE972004083DBA8A";
            host = "bjmtn.b2m.cn:80";
            isCompress = false;
        }

        // appId
        private String appId = "EUCP-EMY-SMS1-0DWDN";
        // 密钥
        private String secretKey = "DE972004083DBA8A";
        // 接口地址
        private String host = "bjmtn.b2m.cn:80";
        //参数压缩
        private bool isCompress = false;//是否压缩，默认否

        
        public SMSResponseBody BatchSend(SMSRequestBody body)
        {
            SMSResponseBody responseBody = new SMSResponseBody();

            string result = "";
            Hashtable headerhs = new Hashtable();
            Byte[] byteArray = null;
            string jsondata = "";
            string url = "http://" + host + "/inter/sendPersonalitySMS";

            headerhs.Add("appId", appId);
            //jsondata = "{\"smses\":[{\"mobile\":15500000000,\"customSmsId\":null,\"content\":\"【短信签名】短信内容\"},{\"mobile\":15500000001,\"customSmsId\":null,\"content\":\"【短信签名】短信内容\"}],\"timerTime\":\"\",\"extendedCode\":112,\"requestTime\":" + DateTime.Now.Ticks.ToString() + ",\"requestValidPeriod\":30}";
            jsondata = JsonConvertEx.ObjectToJson(body);

            if (isCompress)
            {
                headerhs.Add("gzip", "on");//先压缩成byte再加密
                //byteArray = HttpHelper.postdata(url, AESHelper.AESEncrypt(GzipHelper.GZipCompressString(jsondata), secretKey), headerhs, Encoding.UTF8, secretKey);
            }
            else
            {
                byteArray = HttpHelper.postdata(url, AESHelper.AESEncrypt(jsondata, secretKey), headerhs, Encoding.UTF8, secretKey);
            }

            if (byteArray != null)
            {
                if (isCompress)
                {
                    //result = GzipHelper.DecompressString(AESHelper.AESDecrypt(byteArray, secretKey));
                }
                else
                {
                    result = AESHelper.AESDecryptString(byteArray, secretKey);
                }

                if (result != "")
                {
                    if (result.IndexOf("ERROR") != -1)
                    {
                        responseBody.flag = false;
                        responseBody.resultCode = result;
                    }
                    else
                    {
                        responseBody.flag = true;
                        responseBody.resultCode = "";
                        responseBody.smses = JsonConvertEx.JsonToList<SMSResponseEntity>(result);
                    }

                    return responseBody;
                }
            }

            AsyncHelper.RunSync<bool>(() => Manager.Instance.WriteLogFile("亿美发送msg出现问题"));
            return null;
        }

    }
}
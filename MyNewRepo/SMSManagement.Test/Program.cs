using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSManagement.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string AesKey = "DE972004083DBA8A";
            string url = "http://localhost/SMSManagement.Web/SMSService.asmx";

            Task.Factory.StartNew(() => {
                for (int i = 0; i < 5; i++)
                {
                    ReceiveMsgStruct ms = new ReceiveMsgStruct()
                    {                        
                        CSShortName = "VIP",
                        UserName = "niut",
                        UniqueID = Guid.Empty,
                        RequestTime = DateTime.Now.Ticks,
                        TelNumber = "15110052640",
                        Content = "【AAAAAcnkiTest】发送次数 abcd" + i

                    };
                    byte[] data = AESHelper.AESEncrypt(JsonConvertEx.ObjectToJson(ms), AesKey);

                    object rrr = WebServiceHelper.InvokeWebService(url, "SMSService", "SendVCode", new object[] { data });
                    Console.WriteLine(rrr.ToString());
                }
            
            
            });

            Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    ReceiveMsgStruct ms = new ReceiveMsgStruct()
                    {
                        UserName = "niut",
                        CSShortName = "VIP",
                        RequestTime = DateTime.Now.Ticks,
                        TelNumber = "15110052640",
                        Content = "【BBBBBcnkiTest】发送次数 abcd" + i,
                        UniqueID = Guid.Empty
                    };
                    byte[] data = AESHelper.AESEncrypt(JsonConvertEx.ObjectToJson(ms), AesKey);

                    object rrr = WebServiceHelper.InvokeWebService(url, "SMSService", "SendVCode", new object[] { data });
                    Console.WriteLine(rrr.ToString());
                }


            });




            Console.ReadKey();
        }
    }
}

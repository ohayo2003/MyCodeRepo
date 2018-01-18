using SMSManagement.Web.Common;
using SMSManagement.Web.Model;
using SMSManagement.Web.Work;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Services;

namespace SMSManagement.Web
{
    /// <summary>
    /// SMSService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class SMSService : System.Web.Services.WebService
    {

        [WebMethod]
        //[Extension]
        public string SendVCode(byte[] param)
        {
            string result = string.Empty;
            try
            {
                if (param == null || param.Length < 1)
                    return "参数有误";

                string data = Common.AESHelper.AESDecryptString(param, SP.ep.AesKey);
                if(data.Length==0)
                    return "参数有误";

                ReceiveMsgStruct msg = JsonConvertEx.JsonToObject<ReceiveMsgStruct>(data);

                ////if(Cache) 需要其他的判断

                if (msg.RequestTime + 30 * 10000000 < DateTime.Now.Ticks)
                {
                    return "请求已过期";
                }

                //获取全局唯一标识
                msg.UniqueID = UniqueIDMaker.Make();
                msg.ReceiveTime = DateTime.Now.Ticks;

                bool RFlag = AsyncHelper.RunSync<bool>(() => Manager.Instance.WriteSendQueue(msg));

                if (RFlag)
                {
                    result = "已接收";
                    AsyncHelper.RunSync<bool>(() => Manager.Instance.WriteLogDB_SMSReceiveList(msg));
                }
                else
                {
                    result = "系统繁忙";
                    AsyncHelper.RunSync<bool>(() => Manager.Instance.WriteLogFile("插入发送队列出现繁忙"));
                }
                    
            }
            catch (Exception ex)
            {
                AsyncHelper.RunSync<bool>(() => Manager.Instance.WriteLogFile("接受ws请求出现异常", ex));
            }

            return result;
        }

    }
}

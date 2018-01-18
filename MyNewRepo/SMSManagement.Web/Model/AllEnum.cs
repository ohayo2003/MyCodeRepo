using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMSManagement.Web.Model
{
    public enum SMSHandlerType
    {
        /// <summary>
        /// 亿美
        /// </summary>
        Emay = 1,

        /// <summary>
        /// cnki
        /// </summary>
        Cnki = 2

    }

    public enum SendStateType
    {
        /// <summary>
        /// 成功发送
        /// </summary>
        SendOK = 1,

        /// <summary>
        /// 发送失败
        /// </summary>
        SendError = 2,

        /// <summary>
        /// 发送时已经超期30s了
        /// </summary>
        OutTime = 3

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMSManagement.Web.Common
{
    public class UniqueIDMaker
    {
        public static Guid Make()
        {
            return Guid.NewGuid();
        }
    }
}
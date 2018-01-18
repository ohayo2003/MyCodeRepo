using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSManagement.Web.Common
{
    public static class SysAppEventWriter
    {
        public static void WriteEvent(long EventId, string EventContent, EventLogEntryType EntryType)
        {
            try
            {
                System.Diagnostics.EventInstance theEvtInst = new System.Diagnostics.EventInstance(EventId, 0, EntryType);
                //System.Diagnostics.EventLog.WriteEvent(AppCfgs.CurrentAppCenterID + "KeDuoSysLogs", theEvtInst, EventContent, AppCfgs.ServiceBaseAddress);
            }
            catch
            {
            }

        }
    }
}


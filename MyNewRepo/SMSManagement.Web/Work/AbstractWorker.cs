using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SMSManagement.Web.Work
{
    public abstract class AbstractWorker
    {
        public abstract Task<bool> PushAsync(object logContent);
        public abstract void CloseLogThread();

        private DateTime _LastExecTime = DateTime.Now.AddDays(-1);
        public DateTime LastExecTime
        {
            get
            {
                return _LastExecTime;
            }
            set
            {
                _LastExecTime = value;
            }
        }

    }
}
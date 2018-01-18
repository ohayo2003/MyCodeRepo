using SMSManagement.Web.Common;
using SMSManagement.Web.Model;
using SMSManagement.Web.SP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SMSManagement.Web.Work
{
    /// <summary>
    /// 文件日志处理类,利用队列机制，让写日志调用和日志写到文件分离，调用
    /// 方将要写的日志和目标文件插入到日志队列中去就返回，
    /// 然后由内置线程去写到文件里去。这里用了单例模式。
    /// </summary>
    public class Manager : IDisposable
    {

        public static Manager Instance { get; private set; }
        static Manager()
        {
            Instance = new Manager();
        }
        private Manager()
        {
            _Workers = new Dictionary<string, AbstractWorker>();
            _writerThread = new Thread(new ThreadStart(Execute));
            FStop = false;
            _writerThread.Start();
        }


        #region 私有成员

        private class LocalLockObject
        {
        }
        private bool FStop = false;
        private const int Timeout = 30;

        private Dictionary<string, AbstractWorker> _Workers = null;
        private Thread _writerThread = null;

        /// <summary>
        /// 使用list来保存活动logworker，并且使用另外的线程来close niut
        /// 以此来实现动态管理
        /// </summary>
        /// <param name="createParam"></param>
        /// <returns></returns>
        private AbstractWorker GetWorker(string type, string createParam)
        {
            try
            {
                return _Workers[createParam];
            }
            catch
            {
                lock (typeof(LocalLockObject))
                {
                    if (_Workers.ContainsKey(createParam) == false)
                    {
                        AbstractWorker theWorker = null;
                        if (type == "FileLogWorker")
                        {
                            theWorker = new FileLogWorker(createParam);
                        }
                        else if (type == "ReceiveDBLogWorker")
                        {
                            theWorker = new ReceiveDBLogWorker();
                        }
                        else if (type == "SendDBLogWorker")
                        {
                            theWorker = new SendDBLogWorker();
                        }
                        else if (type == "SMSOperationWorker")
                        {
                            theWorker = new SMSOperationWorker();
                        }
                        else
                        {
                            return null;
                        }

                        _Workers.Add(createParam, theWorker);
                        return theWorker;
                    }
                    else
                    {
                        return _Workers[createParam];
                    }
                }
            }
        }

        /// <summary>
        /// 清理30分钟没有发生读写的文件流。对于日志文件按天产生的非常有用。
        /// </summary>
        private void Execute()
        {
            while (FStop == false)
            {
                try
                {

                    lock (typeof(LocalLockObject))
                    {
                        foreach (var theItem in _Workers)
                        {
                            if (DateTime.Now.Subtract(theItem.Value.LastExecTime).Minutes > Timeout)
                            {
                                theItem.Value.CloseLogThread();
                                _Workers.Remove(theItem.Key);
                            }
                        }
                    }
                    Thread.Sleep(100000);
                }
                catch (Exception ex)
                {
                    SysAppEventWriter.WriteEvent(-1, ex.Message, System.Diagnostics.EventLogEntryType.Error);
                }
            }
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 向文件日志写日志内容
        /// </summary>
        /// <param name="fileName">日志文件名</param>
        /// <param name="logContent">日志内容</param>
        public async Task<bool> WriteLogFile(string logContent, Exception ex)
        {
            string filePath = "";
            if (string.IsNullOrEmpty(ep.ErrorPath))
            {
                filePath = System.AppDomain.CurrentDomain.BaseDirectory;
            }
            else
            {
                filePath = ep.ErrorPath;
            }

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("------------------- " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " BEGIN -------------------");
            builder.AppendLine(logContent);
            builder.AppendFormat("ex.Message：{0}", ex.Message).AppendLine();
            builder.AppendFormat("ex.Source：{0}", ex.Source).AppendLine();
            builder.AppendFormat("ex.StackTrace：{0}", ex.StackTrace).AppendLine();
            builder.AppendLine("\n----------------------------- END -------------------------------");


            AbstractWorker theWorker = GetWorker("FileLogWorker", filePath + DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
            var result = await theWorker.PushAsync(builder.ToString());
            return result;
        }

        public async Task<bool> WriteLogFile(string logContent)
        {
            string filePath = "";
            if (string.IsNullOrEmpty(ep.ErrorPath))
            {
                filePath = System.AppDomain.CurrentDomain.BaseDirectory;
            }
            else
            {
                filePath = ep.ErrorPath;
            }

            AbstractWorker theWorker = GetWorker("FileLogWorker", filePath + DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
            var result = await theWorker.PushAsync(logContent);
            return result;
        }

        public async Task<bool> WriteLogDB_SMSReceiveList(ReceiveMsgStruct logContent)
        {

            AbstractWorker theWorker = GetWorker("ReceiveDBLogWorker", "ReceiveDBLogWorker");
            var result = await theWorker.PushAsync(logContent);
            return result;
        }

        public async Task<bool> WriteLogDB_SMSSendList(SendMsgStruct logContent)
        {
            AbstractWorker theWorker = GetWorker("SendDBLogWorker", "SendDBLogWorker");
            var result = await theWorker.PushAsync(logContent);
            return result;
        }

        public async Task<bool> WriteSendQueue(ReceiveMsgStruct logContent)
        {
            AbstractWorker theWorker = GetWorker("SMSOperationWorker", "SMSOperationWorker");
            var result = await theWorker.PushAsync(logContent);
            return result;
        }

        #endregion

        /// <summary>
        /// 结束日志内置线程，并关闭所有文件流。程序正常退出时调用.
        /// </summary>
        public void CloseAll()
        {
            FStop = true;
            if (_writerThread != null)
            {
                _writerThread.Join();
            }
            try
            {
                lock (typeof(LocalLockObject))
                {
                    if (_Workers != null)
                    {
                        foreach (var theItem in _Workers)
                        {
                            theItem.Value.CloseLogThread();
                            _Workers.Remove(theItem.Key);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SysAppEventWriter.WriteEvent(-1, ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
        }

        /// <summary>
        /// 注意析构
        /// </summary>
        public void Dispose()
        {
            try
            {
                CloseAll();
            }
            catch
            {
            }
        }

        ~Manager()
        {
            try
            {
                CloseAll();
            }
            catch
            {
            }
        }


    }
}

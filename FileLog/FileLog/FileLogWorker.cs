using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileLog
{
    /// <summary>
    /// 文件日志处理类,利用队列机制，让写日志调用和日志写到文件分离，调用
    /// 方将要写的日志和目标文件插入到日志队列中去就返回，
    /// 然后由内置线程去写到文件里去。这里用了单例模式。
    /// </summary>
    public class FileLogWorker
    {
        private Queue<string> _logCaches = null;

        private StreamWriter _streamWriter;

        public string FilePath { get; private set; }

        private Thread _writerThread = null;
        public Thread LogWriteThread { get { return _writerThread; } }

        public FileLogWorker(string FilePath)
        {
            this.FilePath = FilePath;
            _logCaches = new Queue<string>();
            _writerThread = new Thread(new ThreadStart(WriteLogToFile));
            FStop = false;
            _streamWriter = new StreamWriter(FilePath, true);
            _writerThread.Start();
        }

        private bool FStop = false;

        private void WriteLogToFile()
        {
            Int64 theCount = 0;
            while (FStop == false)
            {
                try
                {
                    theCount++;
                    string theLogContent = null;
                    //队列操作时需要锁定，否则会报错.队列并不是线程安全的.
                    //但多个队列可以同时写.
                    lock (this)
                    {
                        theLogContent = _logCaches.Dequeue();
                    }
                    if (theLogContent != null && theLogContent != "")
                    {
                        _streamWriter.WriteLine(theLogContent);
                        _streamWriter.Flush();
                        LastExecTime = DateTime.Now;
                    }
                    if (theCount > 10000)
                    {
                        //GC.Collect();
                        theCount = 0;
                    }
                }
                catch (Exception ex)
                {
                    SysAppEventWriter.WriteEvent(-1, ex.Message, System.Diagnostics.EventLogEntryType.Error);
                }

            }
        }


        #region 公共方法
        /// <summary>
        /// 向文件日志写日志内容
        /// </summary>
        /// <param name="fileName">日志文件名</param>
        /// <param name="logContent">日志内容</param>
        public void WriteLogContent(string logContent)
        {
            lock (this)
            {
                _logCaches.Enqueue(logContent);
            }
            if (_logCaches.Count > 10000)
            {
                Thread.CurrentThread.Join(50);
            }
        }

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
        
        #endregion

        /// <summary>
        /// 结束日志内置线程，并关闭所有文件流。程序正常退出时调用.
        /// </summary>
        public void CloseLogThread()
        {
            FStop = true;
            if (_writerThread != null)
            {
                _writerThread.Join();
            }
            try
            {
                _streamWriter.Flush();
                _streamWriter.Close();
                _streamWriter.Dispose();
            }
            catch (Exception ex)
            {
                SysAppEventWriter.WriteEvent(-1, ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
        }
    }
}

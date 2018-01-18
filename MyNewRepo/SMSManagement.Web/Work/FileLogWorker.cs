using SMSManagement.Web.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace SMSManagement.Web.Work
{
    /// <summary>
    /// 文件日志处理类,利用队列机制，让写日志调用和日志写到文件分离，调用
    /// 方将要写的日志和目标文件插入到日志队列中去就返回，
    /// 然后由内置线程去写到文件里去。这里用了单例模式。
    /// </summary>
    public class FileLogWorker : AbstractWorker
    {
        private int BatchMaxNum = 1000;
        private int DueTime = 5000;


        private BatchBlock<string> _logCaches = null;

        private ActionBlock<string[]> ab = null;

        private FileStream fs;
        private StreamWriter _streamWriter;

        public string FilePath { get; private set; }

        private Timer triggerBatchTimer = null;

        public FileLogWorker(string FilePath)
        {
            try
            {
                if (string.IsNullOrEmpty(SP.ep.SendBlockDueTime))
                    DueTime = Convert.ToInt32(SP.ep.SendBlockDueTime);

                if (string.IsNullOrEmpty(SP.ep.SendBlockBatchMaxNum))
                    BatchMaxNum = Convert.ToInt32(SP.ep.SendBlockBatchMaxNum);

                this.FilePath = FilePath;
                //_streamWriter = new StreamWriter(FilePath, true);
                //为避免进程占用文件导致异常，使用FileShare.ReadWrite
                fs = new FileStream(FilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                _streamWriter = new StreamWriter(fs);

                _logCaches = new BatchBlock<string>(BatchMaxNum);

                ab = new ActionBlock<string[]>((stringArray) =>
                 {
                     if (stringArray != null && stringArray.Length > 0)
                     {
                         foreach (var item in stringArray)
                         {
                             _streamWriter.WriteLine(item);
                         }

                         _streamWriter.Flush();
                         LastExecTime = DateTime.Now;
                     }

                     //_streamWriter.WriteLine(string.Format(" cdcd current taskID:{0} ,current thread :{1} ", Task.CurrentId, Thread.CurrentThread.ManagedThreadId));
                     //_streamWriter.Flush();

                 },
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = 1
                });

                _logCaches.LinkTo(ab);

                triggerBatchTimer = new Timer((obj) => _logCaches.TriggerBatch());

                _logCaches.Completion.ContinueWith(delegate { ab.Complete(); });
            }
            catch (Exception ex)
            {

                SysAppEventWriter.WriteEvent(-1, ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
        }

        #region 公共方法
        /// <summary>
        /// 向文件日志写日志内容
        /// </summary>
        /// <param name="fileName">日志文件名</param>
        /// <param name="logContent">日志内容</param>
        public override async Task<bool> PushAsync(object logContent)
        {
            var result = await _logCaches.SendAsync<string>(logContent as string);
            if (result == true)
            {
                triggerBatchTimer.Change(DueTime, Timeout.Infinite);
            }
            return result;
        }

        /// <summary>
        /// 结束日志内置线程，并关闭所有文件流。程序正常退出时调用.
        /// </summary>
        public override void CloseLogThread()
        {

            try
            {
                _logCaches.Complete();
                ab.Completion.Wait();

                triggerBatchTimer.Dispose();
                _streamWriter.Flush();
                _streamWriter.Close();
                _streamWriter.Dispose();
                fs.Flush();
                fs.Close();
                fs.Dispose();
            }
            catch (Exception ex)
            {
                SysAppEventWriter.WriteEvent(-1, ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
        }

        #endregion


    }
}

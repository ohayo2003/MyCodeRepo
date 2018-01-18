using SMSManagement.Web.BLL;
using SMSManagement.Web.Common;
using SMSManagement.Web.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace SMSManagement.Web.Work
{
    /// <summary>
    /// </summary>
    public class SendDBLogWorker : AbstractWorker
    {
        private readonly int BatchMaxNum = 1000;
        private readonly int DueTime = 5000;
        

        private BatchBlock<SendMsgStruct> _logCaches = null;

        private ActionBlock<SendMsgStruct[]> ab = null;

        public string KeyParam { get; private set; }

        private Timer triggerBatchTimer = null;

        private void DoAction(SendMsgStruct[] array)
        {
            if (array != null && array.Length > 0)
            {

                SMSSendListModel model = new SMSSendListModel();
                DataTable dtWrite = model.Tables[0];

                foreach (SendMsgStruct item in array)
                {

                    DataRow dr = dtWrite.NewRow();

                    dr[SMSSendListModel.CSShortName] = item.CSShortName;
                    dr[SMSSendListModel.UserName] = item.UserName;
                    dr[SMSSendListModel.HName] = item.HName;
                    dr[SMSSendListModel.UniqueID] = item.UniqueID;
                    dr[SMSSendListModel.ReceiveTime] = new DateTime(item.ReceiveTime).ToString("yyyy-MM-dd HH:mm:ss");
                    dr[SMSSendListModel.TelNumber] = item.TelNumber;
                    dr[SMSSendListModel.Content] = item.Content;
                    dr[SMSSendListModel.BatchID] = item.BatchID;
                    dr[SMSSendListModel.SendTime] = new DateTime(item.SendTime).ToString("yyyy-MM-dd HH:mm:ss");
                    dr[SMSSendListModel.SendState] = item.SendState;
                    dr[SMSSendListModel.ErrorMsg] = item.ErrorMsg;


                    dtWrite.Rows.Add(dr);
                }

                CommonBll cBll = new CommonBll(SP.DataConnectType.CustomDBDataService);
                if (cBll.BulkInsert(model))
                {
                    LastExecTime = DateTime.Now;

                }
                else//error
                {
                    AsyncHelper.RunSync<bool>(() => Manager.Instance.WriteLogFile("SendDBLogWorker_BulkInsert未插入"));
                }

            }
        }

        public SendDBLogWorker(string TableName = "SMSSendList")
        {
            try
            {
                if (string.IsNullOrEmpty(SP.ep.DBBlockDueTime))
                    DueTime = Convert.ToInt32(SP.ep.DBBlockDueTime);

                if (string.IsNullOrEmpty(SP.ep.DBBlockBatchMaxNum))
                    BatchMaxNum = Convert.ToInt32(SP.ep.DBBlockBatchMaxNum);

                this.KeyParam = TableName;

                _logCaches = new BatchBlock<SendMsgStruct>(BatchMaxNum);

                ab = new ActionBlock<SendMsgStruct[]>((array) =>
                 {
                     try
                     {
                         DoAction(array);
                     }
                     catch (Exception ex)
                     {
                         AsyncHelper.RunSync<bool>(() => Manager.Instance.WriteLogFile("SendDBLogWorker_ActionBlock出现异常", ex));
                     }
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

                AsyncHelper.RunSync<bool>(() => Manager.Instance.WriteLogFile("SendDBLogWorker出现异常", ex));
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
            var result = await _logCaches.SendAsync<SendMsgStruct>(logContent as SendMsgStruct);
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

            }
            catch (Exception ex)
            {
                SysAppEventWriter.WriteEvent(-1, ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
        }

        #endregion


    }
}

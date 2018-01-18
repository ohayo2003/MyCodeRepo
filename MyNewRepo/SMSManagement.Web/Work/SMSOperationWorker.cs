using SMSManagement.Web.BLL;
using SMSManagement.Web.Common;
using SMSManagement.Web.Model;
using SMSManagement.Web.SMSHandler;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
    public class SMSOperationWorker : AbstractWorker
    {
        private readonly int BatchMaxNum = 100;
        private readonly int DueTime = 5000;

        private BatchBlock<ReceiveMsgStruct> _logCaches = null;

        private TransformBlock<ReceiveMsgStruct[], List<SendMsgStruct>> mainTB = null;
        private ActionBlock<List<SendMsgStruct>> spareTB = null;

        public string KeyParam { get; private set; }

        private Timer triggerBatchTimer = null;

        public SMSOperationWorker()
        {
            try
            {
                if (string.IsNullOrEmpty(SP.ep.SendBlockDueTime))
                    DueTime = Convert.ToInt32(SP.ep.SendBlockDueTime);

                if (string.IsNullOrEmpty(SP.ep.SendBlockBatchMaxNum))
                    BatchMaxNum = Convert.ToInt32(SP.ep.SendBlockBatchMaxNum);

                this.KeyParam = "";

                _logCaches = new BatchBlock<ReceiveMsgStruct>(BatchMaxNum);

                mainTB = new TransformBlock<ReceiveMsgStruct[], List<SendMsgStruct>>(array =>
                 {
                     try
                     {
                         return EmaySendAction(array);
                     }
                     catch (Exception ex)
                     {
                         AsyncHelper.RunSync<bool>(() => Manager.Instance.WriteLogFile("EmaySendAction出现异常", ex));
                         return new List<SendMsgStruct>();
                     }

                 },
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = 1
                });

                spareTB = new ActionBlock<List<SendMsgStruct>>(array =>
                {
                    try
                    {
                        if (array != null && array.Count > 0)
                            CnkiSendAction(array);
                    }
                    catch (Exception ex)
                    {
                        AsyncHelper.RunSync<bool>(() => Manager.Instance.WriteLogFile("CnkiSendAction出现异常", ex));
                    }

                },
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = 1
                });


                mainTB.LinkTo(spareTB);
                _logCaches.LinkTo(mainTB);

                triggerBatchTimer = new Timer((obj) => _logCaches.TriggerBatch());

                _logCaches.Completion.ContinueWith(delegate { mainTB.Complete(); });
                mainTB.Completion.ContinueWith(delegate { spareTB.Complete(); });
            }
            catch (Exception ex)
            {

                AsyncHelper.RunSync<bool>(() => Manager.Instance.WriteLogFile("SMSOperationWorker出现异常", ex));
            }
        }

        private List<SendMsgStruct> EmaySendAction(ReceiveMsgStruct[] array)
        {
            //if (array.Length>0 && array[0] == null)
            //{
            //    Debug.WriteLine("becalled");
            //    return new List<SendMsgStruct>();
            //}

            List<SendMsgStruct> ErrorResultList = new List<SendMsgStruct>();

            if (array != null && array.Length > 0)
            {
                List<ReceiveMsgStruct> copyList = new List<ReceiveMsgStruct>();

                Guid BatchID = UniqueIDMaker.Make();
                LastExecTime = DateTime.Now;

                SMSRequestBody body = new SMSRequestBody();

                foreach (ReceiveMsgStruct item in array)
                {
                    if (item.ReceiveTime + 30 * 10000000 < LastExecTime.Ticks)
                    {
                        //"请求已过期";
                        SendMsgStruct sendItem = new SendMsgStruct()
                        {
                            CSShortName = item.CSShortName,
                            UserName = item.UserName,
                            HName = SMSHandlerType.Emay.ToString(),
                            UniqueID = item.UniqueID,
                            ReceiveTime = item.ReceiveTime,
                            TelNumber = item.TelNumber,
                            Content = item.Content,
                            BatchID = BatchID,
                            SendTime = LastExecTime.Ticks,
                            SendState = (int)SendStateType.OutTime
                        };

                        AsyncHelper.RunSync<bool>(() => Manager.Instance.WriteLogDB_SMSSendList(SendMsgStruct.Copy(sendItem)));

                    }
                    else
                    {
                        copyList.Add(item);
                        body.smses.Add(new SMSRequestEntity()
                        {
                            mobile = item.TelNumber,
                            content = item.Content,
                            customSmsId = ""
                        });
                    }

                }

                if (copyList.Count == 0)
                {
                    return ErrorResultList;
                }

                //使用copyList来处理发送

                body.requestTime = DateTime.Now.Ticks;
                SMSResponseBody responseBody = EmaySender.Instance.BatchSend(body);

                //#region forTestCnki
                //SMSResponseBody responseBody = new SMSResponseBody();
                //responseBody.flag = false;
                //responseBody.resultCode = "forTestCnki";
                //#endregion

                //加入dblog
                foreach (ReceiveMsgStruct item in copyList)
                {
                    SendMsgStruct sendItem = new SendMsgStruct()
                    {
                        CSShortName = item.CSShortName,
                        UserName = item.UserName,
                        HName = SMSHandlerType.Emay.ToString(),
                        UniqueID = item.UniqueID,
                        ReceiveTime = item.ReceiveTime,
                        TelNumber = item.TelNumber,
                        Content = item.Content,
                        BatchID = BatchID,
                        SendTime = body.requestTime
                    };
                    if (responseBody != null && responseBody.flag == true)
                    {
                        sendItem.SendState = (int)SendStateType.SendOK;
                    }
                    else
                    {
                        sendItem.SendState = (int)SendStateType.SendError;
                        sendItem.ErrorMsg = (responseBody == null ? "SendException" : responseBody.resultCode);

                        ErrorResultList.Add(sendItem);
                    }

                    AsyncHelper.RunSync<bool>(() => Manager.Instance.WriteLogDB_SMSSendList(SendMsgStruct.Copy(sendItem)));


                }
            }
            //防止引用结果被篡改
            List<SendMsgStruct> resultList = new List<SendMsgStruct>();
            foreach (SendMsgStruct item in ErrorResultList)
            {
                resultList.Add(SendMsgStruct.Copy(item));
            }

            return ErrorResultList;
        }

        public void CnkiSendAction(List<SendMsgStruct> array)
        {
            foreach (SendMsgStruct item in array)
            {
                item.HName = SMSHandlerType.Cnki.ToString();
                item.SendTime = DateTime.Now.Ticks;

                Task.Factory.StartNew(() =>
                {

                    try
                    {
                        int result = CnkiSender.Instance.SingleSend(item);

                        if (result == 1)
                        {
                            item.SendState = (int)SendStateType.SendOK;
                        }
                        else
                        {
                            item.SendState = (int)SendStateType.SendError;

                            if (result == -1)
                            {
                                item.ErrorMsg = "对不起，IP不在范围内！";
                            }
                            else if (result == -2)
                            {
                                item.ErrorMsg = "对不起，IP不在范围内！";
                            }
                            else if (result == -100)
                            {
                                item.ErrorMsg = "用户的手机号不正确！";
                            }
                            else
                            {
                                item.ErrorMsg = result.ToString();
                            }
                        }

                        Manager.Instance.WriteLogDB_SMSSendList(SendMsgStruct.Copy(item));

                    }
                    catch (Exception ex)
                    {

                        AsyncHelper.RunSync<bool>(() => Manager.Instance.WriteLogFile("SingleSend出现异常", ex));
                    }
                });

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
            var result = await _logCaches.SendAsync<ReceiveMsgStruct>(logContent as ReceiveMsgStruct);
            if (result == true)
            {
                //Debug.WriteLine((logContent as ReceiveMsgStruct).UniqueID);
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
                mainTB.Completion.Wait();
                spareTB.Completion.Wait();

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

using System;
using System.Data;
using System.Runtime.Serialization;
using System.Collections.Generic;
using SMSManagement.Web.DBUtility;

namespace SMSManagement.Web.Model
{
    [System.ComponentModel.DesignerCategory("+Code")]
    [SerializableAttribute]
    public class SMSSendListModel : DataSet
    {
        public const string TableName = "SMSSendList";

        public const string SID = "SID";        
        public const string CSShortName = "CSShortName";
        public const string UserName = "UserName";
        public const string HName = "HName";
        public const string UniqueID = "UniqueID";

        public const string ReceiveTime = "ReceiveTime";
        public const string TelNumber = "TelNumber";
        public const string Content = "Content";

        public const string BatchID = "BatchID";
        public const string SendTime = "SendTime";
        public const string SendState = "SendState";
        public const string ErrorMsg = "ErrorMsg";

        public List<CustomSqlParam> paramList;

        public SMSSendListModel(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public SMSSendListModel()
        {
            BuildeDataInfo();

            BuildDataCSP();
        }

        private void BuildeDataInfo()
        {

            DataTable table = new DataTable(TableName);
            DataColumnCollection columns = table.Columns;

            columns.Add(SID, typeof(System.Int64));            
            columns.Add(CSShortName, typeof(System.String));
            columns.Add(UserName, typeof(System.String));
            columns.Add(HName, typeof(System.String));
            columns.Add(UniqueID, typeof(System.Guid));

            columns.Add(ReceiveTime, typeof(System.DateTime));
            columns.Add(TelNumber, typeof(System.String));
            columns.Add(Content, typeof(System.String));

            columns.Add(BatchID, typeof(System.Guid));
            columns.Add(SendTime, typeof(System.DateTime));
            columns.Add(SendState, typeof(System.Int32));
            columns.Add(ErrorMsg, typeof(System.String));

            this.Tables.Add(table);
        }

        private void BuildDataCSP()
        {
            paramList = new List<CustomSqlParam>();

            paramList.Add(new CustomSqlParam(SID, SqlDbType.BigInt, System.DBNull.Value));
            paramList.Add(new CustomSqlParam(CSShortName, SqlDbType.VarChar, System.DBNull.Value));
            paramList.Add(new CustomSqlParam(UserName, SqlDbType.VarChar, System.DBNull.Value));
            paramList.Add(new CustomSqlParam(HName, SqlDbType.VarChar, System.DBNull.Value));
            paramList.Add(new CustomSqlParam(UniqueID, SqlDbType.UniqueIdentifier, System.DBNull.Value));

            paramList.Add(new CustomSqlParam(ReceiveTime, SqlDbType.DateTime, System.DBNull.Value));
            paramList.Add(new CustomSqlParam(TelNumber, SqlDbType.VarChar, System.DBNull.Value));
            paramList.Add(new CustomSqlParam(Content, SqlDbType.NVarChar, System.DBNull.Value));

            paramList.Add(new CustomSqlParam(BatchID, SqlDbType.UniqueIdentifier, System.DBNull.Value));
            paramList.Add(new CustomSqlParam(SendTime, SqlDbType.DateTime, System.DBNull.Value));
            paramList.Add(new CustomSqlParam(SendState, SqlDbType.Int, System.DBNull.Value));
            paramList.Add(new CustomSqlParam(ErrorMsg, SqlDbType.NVarChar, System.DBNull.Value));

        }

        public CustomSqlParam GetParam(string CName)
        {
            for (int i = 0; i < paramList.Count; i++)
            {
                if (CName.Trim().ToLower() == paramList[i].Name.Trim().ToLower())
                {
                    return paramList[i];
                }
            }
            return null;
        }
    }
}
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Collections.Generic;
using SMSManagement.Web.DBUtility;

namespace SMSManagement.Web.Model
{
    [System.ComponentModel.DesignerCategory("+Code")]
    [SerializableAttribute]
    public class SMSReceiveListModel : DataSet
    {
        public const string TableName = "SMSReceiveList";

        public const string RID = "RID";
        public const string UserName = "UserName";
        public const string CSShortName = "CSShortName";

        public const string UniqueID = "UniqueID";
        public const string RequestTime = "RequestTime";
        public const string ReceiveTime = "ReceiveTime";
        public const string TelNumber = "TelNumber";
        public const string Content = "Content";

        public List<CustomSqlParam> paramList;

        public SMSReceiveListModel(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public SMSReceiveListModel()
        {
            BuildeDataInfo();

            BuildDataCSP();
        }

        private void BuildeDataInfo()
        {

            DataTable table = new DataTable(TableName);
            DataColumnCollection columns = table.Columns;

            columns.Add(RID, typeof(System.Int64));
            columns.Add(UserName, typeof(System.String));
            columns.Add(CSShortName, typeof(System.String));

            columns.Add(UniqueID, typeof(System.Guid));
            columns.Add(RequestTime, typeof(System.DateTime));
            columns.Add(ReceiveTime, typeof(System.DateTime));
            columns.Add(TelNumber, typeof(System.String));
            columns.Add(Content, typeof(System.String));

            this.Tables.Add(table);
        }

        private void BuildDataCSP()
        {
            paramList = new List<CustomSqlParam>();

            paramList.Add(new CustomSqlParam(RID, SqlDbType.BigInt, System.DBNull.Value));
            paramList.Add(new CustomSqlParam(UserName, SqlDbType.VarChar, System.DBNull.Value));
            paramList.Add(new CustomSqlParam(CSShortName, SqlDbType.VarChar, System.DBNull.Value));

            paramList.Add(new CustomSqlParam(UniqueID, SqlDbType.UniqueIdentifier, System.DBNull.Value));
            paramList.Add(new CustomSqlParam(RequestTime, SqlDbType.DateTime, System.DBNull.Value));
            paramList.Add(new CustomSqlParam(ReceiveTime, SqlDbType.DateTime, System.DBNull.Value));
            paramList.Add(new CustomSqlParam(TelNumber, SqlDbType.VarChar, System.DBNull.Value));
            paramList.Add(new CustomSqlParam(Content, SqlDbType.NVarChar, System.DBNull.Value));

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
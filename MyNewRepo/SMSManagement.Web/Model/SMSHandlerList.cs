using System;
using System.Data;
using System.Runtime.Serialization;
using System.Collections.Generic;
using SMSManagement.Web.DBUtility;

namespace SMSManagement.Web.Model
{
    [System.ComponentModel.DesignerCategory("+Code")]
    [SerializableAttribute]
    public class SMSHandlerListModel : DataSet
    {
        public const string TableName = "SMSHandlerList";

        public const string AutoID = "AutoID";
        public const string HComment = "HComment";
        public const string HName = "HName";
        public const string HState = "HState";
        public const string HPriority = "HPriority";


        public List<CustomSqlParam> paramList;

        public SMSHandlerListModel(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public SMSHandlerListModel()
        {
            BuildeDataInfo();

            BuildDataCSP();
        }

        private void BuildeDataInfo()
        {

            DataTable table = new DataTable(TableName);
            DataColumnCollection columns = table.Columns;

            columns.Add(AutoID, typeof(System.Int32));
            columns.Add(HName, typeof(System.String));
            columns.Add(HComment, typeof(System.String));
            columns.Add(HState, typeof(System.Int32));
            columns.Add(HPriority, typeof(System.Int32));


            this.Tables.Add(table);
        }

        private void BuildDataCSP()
        {
            paramList = new List<CustomSqlParam>();

            paramList.Add(new CustomSqlParam(AutoID, SqlDbType.Int, System.DBNull.Value));
            paramList.Add(new CustomSqlParam(HName, SqlDbType.NVarChar, System.DBNull.Value));
            paramList.Add(new CustomSqlParam(HComment, SqlDbType.NVarChar, System.DBNull.Value));
            paramList.Add(new CustomSqlParam(HState, SqlDbType.Int, System.DBNull.Value));
            paramList.Add(new CustomSqlParam(HPriority, SqlDbType.Int, System.DBNull.Value));


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
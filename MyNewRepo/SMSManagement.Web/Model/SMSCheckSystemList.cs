using System;
using System.Data;
using System.Runtime.Serialization;
using System.Collections.Generic;
using SMSManagement.Web.DBUtility;

namespace SMSManagement.Web.Model
{
    [System.ComponentModel.DesignerCategory("+Code")]
    [SerializableAttribute]
    public class SMSCheckSystemListModel : DataSet
    {
        public const string TableName = "SMSCheckSystemList";

        public const string CSID = "CSID";        
        public const string CSShortName = "CSShortName";
        public const string CSName = "CSName";

        public const string LeaderName = "LeaderName";
        public const string LeaderTelNumber = "LeaderTelNumber";
        public const string Comment = "Comment";

        public List<CustomSqlParam> paramList;

        public SMSCheckSystemListModel(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public SMSCheckSystemListModel()
        {
            BuildeDataInfo();

            BuildDataCSP();
        }

        private void BuildeDataInfo()
        {

            DataTable table = new DataTable(TableName);
            DataColumnCollection columns = table.Columns;

            columns.Add(CSID, typeof(System.Int32));            
            columns.Add(CSShortName, typeof(System.String));
            columns.Add(CSName, typeof(System.String));

            columns.Add(LeaderName, typeof(System.String));
            columns.Add(LeaderTelNumber, typeof(System.String));
            columns.Add(Comment, typeof(System.String));

            this.Tables.Add(table);
        }

        private void BuildDataCSP()
        {
            paramList = new List<CustomSqlParam>();

            paramList.Add(new CustomSqlParam(CSID, SqlDbType.Int, System.DBNull.Value));            
            paramList.Add(new CustomSqlParam(CSShortName, SqlDbType.VarChar, System.DBNull.Value));
            paramList.Add(new CustomSqlParam(CSName, SqlDbType.NVarChar, System.DBNull.Value));

            paramList.Add(new CustomSqlParam(LeaderName, SqlDbType.NVarChar, System.DBNull.Value));
            paramList.Add(new CustomSqlParam(LeaderTelNumber, SqlDbType.VarChar, System.DBNull.Value));
            paramList.Add(new CustomSqlParam(Comment, SqlDbType.NVarChar, System.DBNull.Value));

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
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Collections.Generic;
using SMSManagement.Web.DBUtility;

namespace SMSManagement.Web.Model
{
    [System.ComponentModel.DesignerCategory("+Code")]
    [SerializableAttribute]
    public class SMSUserInfoModel : DataSet
    {
        public const string TableName = "SMSUserInfo";

        public const string AutoID = "AutoID";
        public const string UserName = "UserName";
        public const string CSShortName = "CSShortName";

        public const string PWD = "PWD";
        public const string UserState = "UserState";

        public List<CustomSqlParam> paramList;

        public SMSUserInfoModel(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public SMSUserInfoModel()
        {
            BuildeDataInfo();

            BuildDataCSP();
        }

        private void BuildeDataInfo()
        {

            DataTable table = new DataTable(TableName);
            DataColumnCollection columns = table.Columns;

            columns.Add(AutoID, typeof(System.Int32));
            columns.Add(UserName, typeof(System.String));
            columns.Add(CSShortName, typeof(System.String));

            columns.Add(PWD, typeof(System.String));
            columns.Add(UserState, typeof(System.Int32));

            this.Tables.Add(table);
        }

        private void BuildDataCSP()
        {
            paramList = new List<CustomSqlParam>();

            paramList.Add(new CustomSqlParam(AutoID, SqlDbType.Int, System.DBNull.Value));
            paramList.Add(new CustomSqlParam(UserName, SqlDbType.VarChar, System.DBNull.Value));
            paramList.Add(new CustomSqlParam(CSShortName, SqlDbType.VarChar, System.DBNull.Value));

            paramList.Add(new CustomSqlParam(PWD, SqlDbType.VarChar, System.DBNull.Value));
            paramList.Add(new CustomSqlParam(UserState, SqlDbType.Int, System.DBNull.Value));

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
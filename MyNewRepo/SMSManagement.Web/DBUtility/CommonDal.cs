using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using SMSManagement.Web.SP;


namespace SMSManagement.Web.DBUtility
{
    public class CommonDal : IDisposable
    {
        private Customerlink _clk;

        public CommonDal(DataConnectType dct)
        {
            _clk = ep.GetSQLLink(dct);
        }

        public CommonDal(Customerlink clk)
        {
            _clk = clk;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;
        }

        /// <summary>
        /// 根据sql语句获取结果
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public DataSet GetDataBySql(string sql)
        {

            SqlCommand command = new SqlCommand(sql);
            command.CommandType = CommandType.Text;
            command.CommandTimeout = 200;

            DataSet ds = new DataSet();
            using (DataAccess Access = new DataAccess())
            {
                Access.SetSQLLinkParam(_clk.m_ServerIP, _clk.m_DbName, _clk.m_ServerID, _clk.m_PassWord);
                return Access.Fill(command, ds);
            }
        }

        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public int ExecuteSql(string sql)
        {

            SqlCommand command = new SqlCommand(sql);
            command.CommandType = CommandType.Text;
            command.CommandTimeout = 200;
            DataSet ds = new DataSet();
            using (DataAccess Access = new DataAccess())
            {
                Access.SetSQLLinkParam(_clk.m_ServerIP, _clk.m_DbName, _clk.m_ServerID, _clk.m_PassWord);
                return Access.ExecuteNonQuery(command);
            }
        }

        /// <summary>   
        /// 根据参数化sql语句获取结果
        /// </summary>
        /// <param name="sql">参数化sql语句</param>
        /// <param name="paramList">参数对象列表</param>
        /// <returns></returns>
        public DataSet GetDataByParameterizedSql(string sql, List<CustomSqlParam> paramList)
        {

            SqlCommand command = new SqlCommand(sql);
            command.CommandType = CommandType.Text;
            command.CommandTimeout = 200;
            SqlParameterCollection sqlParams = command.Parameters;
            foreach (var sqlParam in paramList)
            {
                sqlParams.Add(sqlParam.Name, sqlParam.Type);
                sqlParams[sqlParam.Name].Value = sqlParam.Value;
            }

            DataSet ds = new DataSet();
            using (DataAccess Access = new DataAccess())
            {
                Access.SetSQLLinkParam(_clk.m_ServerIP, _clk.m_DbName, _clk.m_ServerID, _clk.m_PassWord);
                return Access.Fill(command, ds);
            }
        }

        public DataSet GetPageDataByParameterizedSql(List<CustomSqlParam> paramList,
            string TableName, string FieldList, string Condition,
             string OrderField, int PageIndex, int PageSize)
        {

            string strSQL =
                "  BEGIN  "
                + " select " + FieldList + " from (select *,ROW_NUMBER() over(" + OrderField
                + ") as num from " + TableName + " " + Condition + ")"
                + " as t where num between cast(((" + PageIndex.ToString() + "-1)*" + PageSize.ToString()
                + " + 1) as varchar(20)) and cast(" + PageIndex + "*" + PageSize + " as varchar(20))"
                + ";"
                + "SELECT COUNT(1) as TotalNumber FROM " + TableName + " " + Condition
                + ";"
                + "END;";

            SqlCommand command = new SqlCommand(strSQL);
            command.CommandType = CommandType.Text;
            command.CommandTimeout = 200;
            SqlParameterCollection sqlParams = command.Parameters;

            foreach (var sqlParam in paramList)
            {
                sqlParams.Add(sqlParam.Name, sqlParam.Type);
                sqlParams[sqlParam.Name].Value = sqlParam.Value;
            }

            DataSet ds = new DataSet();
            using (DataAccess Access = new DataAccess())
            {
                Access.SetSQLLinkParam(_clk.m_ServerIP, _clk.m_DbName, _clk.m_ServerID, _clk.m_PassWord);
                return Access.Fill(command, ds);
            }
        }

        /// <summary>
        /// 多表连接分页查询，多个表用重复的字段，不能用SELECT *,所以重载上面的方法，传递要查询的字段
        /// </summary>
        /// <param name="paramList"></param>
        /// <param name="TableName"></param>
        /// <param name="FieldList"></param>
        /// <param name="Condition"></param>
        /// <param name="OrderField"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public DataSet GetPageDataByParameterizedSql(List<CustomSqlParam> paramList,
          string TableName, string FieldList, string TableFiled, string Condition,
           string OrderField, int PageIndex, int PageSize)
        {

            string strSQL =
                "  BEGIN  "
                + " select " + FieldList + " from (select " + TableFiled + ",ROW_NUMBER() over(" + OrderField
                + ") as num from " + TableName + " " + Condition + ")"
                + " as t where num between cast(((" + PageIndex.ToString() + "-1)*" + PageSize.ToString()
                + " + 1) as varchar(20)) and cast(" + PageIndex + "*" + PageSize + " as varchar(20))"
                + ";"
                + "SELECT COUNT(1) as TotalNumber FROM " + TableName + " " + Condition
                + ";"
                + "END;";

            SqlCommand command = new SqlCommand(strSQL);
            command.CommandType = CommandType.Text;
            command.CommandTimeout = 200;
            SqlParameterCollection sqlParams = command.Parameters;

            foreach (var sqlParam in paramList)
            {
                sqlParams.Add(sqlParam.Name, sqlParam.Type);
                sqlParams[sqlParam.Name].Value = sqlParam.Value;
            }

            DataSet ds = new DataSet();
            using (DataAccess Access = new DataAccess())
            {
                Access.SetSQLLinkParam(_clk.m_ServerIP, _clk.m_DbName, _clk.m_ServerID, _clk.m_PassWord);
                return Access.Fill(command, ds);
            }
        }
        /// <summary>
        /// 根据文字复制比算出排名等信息
        /// </summary>
        /// <returns></returns>
        public DataSet GetPageDataByParameterizedSql(string SampleID,
           string OperatorID, string ParentUserName, string Result)
        {

            //string strSQL =
            //    "  BEGIN  "
            //    + " select " + FieldList + " from (select *,ROW_NUMBER() over(" + OrderField
            //    + ") as num from " + TableName + " " + Condition + ")"
            //    + " as t where num between cast(((" + PageIndex.ToString() + "-1)*" + PageSize.ToString()
            //    + " + 1) as varchar(20)) and cast(" + PageIndex + "*" + PageSize + " as varchar(20))"
            //    + ";"
            //    + "SELECT COUNT(1) as TotalNumber FROM " + TableName + " " + Condition
            //    + ";"
            //    + "  END;";
            string strSQL =
                " select * from ( "
 + "   SELECT 1 typeName, COUNT(*) countAll,"
   + "  sum(case when(ArticleResult>=" + Result + ")then 1 else 0 end)as conditionCount,"
   + "  Round(Convert(float,sum(case when ArticleResult>=" + Result + " then 1 else 0 end))*100/Convert(float,count(*)),4)  as PercentCount,"
   + "  T.OperatorID,"
   + "  ROW_NUMBER() over(order by Round(Convert(float,sum(case when ArticleResult>=" + Result + " then 1 else 0 end))/Convert(float,count(*)),4))as num "
   + "   FROM TaskFlowInfo T,StudentList S  where T.CurrentCheckID=S.CurrentCheckID and ParentUserName='" + ParentUserName + "' and T.SampleID=" + SampleID + " and S.Available=1 and T.CheckStatus=3 group by T.OperatorID"
   + "   Union ALL"

   + "  SELECT 2 typeName,COUNT(*) countAll,"
   + "  sum(case when(ArticleResultBenren>=" + Result + ")then 1 else 0 end)as conditionCount,"
   + "  Round(Convert(float,sum(case when ArticleResultBenren>=" + Result + " then 1 else 0 end))*100/Convert(float,count(*)),4)  as PercentCount,"
   + "  T.OperatorID,"
   + "   ROW_NUMBER() over(order by Round(Convert(float,sum(case when ArticleResultBenren>=" + Result + " then 1 else 0 end))/Convert(float,count(*)),4))as num "
  + "   FROM TaskFlowInfo T,StudentList S  where T.CurrentCheckID=S.CurrentCheckID and ParentUserName='" + ParentUserName + "' and T.SampleID=" + SampleID + " and S.Available=1 and T.CheckStatus=3  group by T.OperatorID"
   + "   Union ALL"
    + "   SELECT 3 typeName,COUNT(*) countAll,"
   + "  sum(case when( ArticleResultQuote>=" + Result + ")then 1 else 0 end)as conditionCount,"
   + "  Round(Convert(float,sum(case when ArticleResultQuote>=" + Result + " then 1 else 0 end))*100/Convert(float,count(*)),4)  as PercentCount,"
   + "  T.OperatorID,"
  + "   ROW_NUMBER() over(order by Round(Convert(float,sum(case when ArticleResultQuote>=" + Result + " then 1 else 0 end))/Convert(float,count(*)),4))as num "
 + "   FROM TaskFlowInfo T,StudentList S  where T.CurrentCheckID=S.CurrentCheckID and ParentUserName='" + ParentUserName + "' and T.SampleID=" + SampleID + " and S.Available=1 and T.CheckStatus=3  group by T.OperatorID"

   + "  ) NEWt where operatorID='" + OperatorID + "'";

            SqlCommand command = new SqlCommand(strSQL);
            command.CommandType = CommandType.Text;
            command.CommandTimeout = 200;
            DataSet ds = new DataSet();
            using (DataAccess Access = new DataAccess())
            {
                Access.SetSQLLinkParam(_clk.m_ServerIP, _clk.m_DbName, _clk.m_ServerID, _clk.m_PassWord);
                return Access.Fill(command, ds);
            }
        }
        /// <summary>
        /// 分页存储过程,不用了
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="FieldList"></param>
        /// <param name="Condition"></param>
        /// <param name="KeyField"></param>
        /// <param name="OrderField"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public DataSet GetPageData(string TableName, string FieldList, string Condition,
          string KeyField, string OrderField, int PageIndex, int PageSize)
        {
            SqlCommand command = new SqlCommand("MyPage");
            command.CommandType = CommandType.StoredProcedure;
            SqlParameterCollection sqlParams = command.Parameters;

            List<CustomSqlParam> paramList = new List<CustomSqlParam>();
            paramList.Add(new CustomSqlParam("FieldList", SqlDbType.VarChar, FieldList));
            paramList.Add(new CustomSqlParam("PageIndex", SqlDbType.Int, PageIndex));
            paramList.Add(new CustomSqlParam("PageSize", SqlDbType.Int, PageSize));
            paramList.Add(new CustomSqlParam("TableName", SqlDbType.VarChar, TableName));
            paramList.Add(new CustomSqlParam("KeyField", SqlDbType.VarChar, KeyField));
            paramList.Add(new CustomSqlParam("Condition", SqlDbType.VarChar, Condition));
            paramList.Add(new CustomSqlParam("OrderField", SqlDbType.VarChar, OrderField));

            foreach (var sqlParam in paramList)
            {
                sqlParams.Add(sqlParam.Name, sqlParam.Type);
                sqlParams[sqlParam.Name].Value = sqlParam.Value;
            }

            DataSet ds = new DataSet();
            using (DataAccess Access = new DataAccess())
            {
                Access.SetSQLLinkParam(_clk.m_ServerIP, _clk.m_DbName, _clk.m_ServerID, _clk.m_PassWord);
                return Access.Fill(command, ds);
            }
        }

        /// <summary>
        /// 执行参数化sql语句
        /// </summary>
        /// <param name="sql">参数化sql语句</param>
        /// <param name="paramList">参数对象列表</param>
        /// <returns></returns>
        public int ExecuteParameterizedSql(string sql, List<CustomSqlParam> paramList)
        {

            SqlCommand command = new SqlCommand(sql);
            command.CommandType = CommandType.Text;
            command.CommandTimeout = 200;

            SqlParameterCollection sqlParams = command.Parameters;
            foreach (var sqlParam in paramList)
            {
                sqlParams.Add(sqlParam.Name, sqlParam.Type);
                sqlParams[sqlParam.Name].Value = sqlParam.Value;
            }

            DataSet ds = new DataSet();
            using (DataAccess Access = new DataAccess())
            {
                Access.SetSQLLinkParam(_clk.m_ServerIP, _clk.m_DbName, _clk.m_ServerID, _clk.m_PassWord);
                return Access.ExecuteNonQuery(command);
            }
        }


        /// <summary>
        /// 执行参数化事务语句
        /// </summary>
        /// <param name="sqlDirectionary"></param>
        /// <returns></returns>
        public int ExecuteTransSql(Dictionary<String, List<CustomSqlParam>> sqlDirectionary)
        {

            SqlConnection conn = null;

            SqlTransaction trans = null;

            int status = 1;

            try
            {
                DateTime dtnow = DateTime.Now;

                using (DataAccess Access = new DataAccess())
                {
                    Access.SetSQLLinkParam(_clk.m_ServerIP, _clk.m_DbName, _clk.m_ServerID, _clk.m_PassWord);

                    conn = Access.Connection(true);

                    trans = conn.BeginTransaction();

                    SqlCommand command = new SqlCommand();
                    command.Connection = conn;
                    command.Transaction = trans;
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 200;

                    foreach (var item in sqlDirectionary)
                    {

                        string sqlstr = item.Key;
                        command.CommandText = sqlstr;
                        SqlParameterCollection sqlParams = command.Parameters;
                        sqlParams.Clear();
                        foreach (var sqlParam in item.Value)
                        {

                            sqlParams.Add(sqlParam.Name, sqlParam.Type);
                            sqlParams[sqlParam.Name].Value = sqlParam.Value;
                        }
                        int count = command.ExecuteNonQuery();
                    }

                    trans.Commit();

                }
            }
            catch (Exception ex)
            {
                trans.Rollback();
                status = 0;
            }
            trans.Dispose();

            conn.Close();
            return status;
        }


        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="mappingname"></param>
        /// <param name="OnRowsCopied"></param>
        public bool BulkInsert(DataSet model)
        {
            SqlConnection conn = null;
            SqlTransaction sqlTran = null;
            bool result = false;
            try
            {
                using (DataAccess Access = new DataAccess())
                {
                    Access.SetSQLLinkParam(_clk.m_ServerIP, _clk.m_DbName, _clk.m_ServerID, _clk.m_PassWord);

                    conn = Access.Connection(true);
                    sqlTran = conn.BeginTransaction(); //开始事务

                    using (SqlBulkCopy sqlBulk = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, sqlTran))
                    {
                        sqlBulk.BulkCopyTimeout = 5000;
                        sqlBulk.DestinationTableName = model.Tables[0].TableName;
                        sqlBulk.WriteToServer(model.Tables[0]);
                        sqlTran.Commit();
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                sqlTran.Rollback();

            }
            finally
            {
                sqlTran.Dispose();
                conn.Close();

            }
            return result;
        }

        /// <summary>
        /// 事务执行sql后批量插入
        /// </summary>
        /// <param name="sqlDirectionary"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public int ExecuteThenInsertTrans(Dictionary<String, List<CustomSqlParam>> sqlDirectionary, DataSet model)
        {

            SqlConnection conn = null;

            SqlTransaction trans = null;

            SqlBulkCopy sqlBulk = null;

            int status = 1;

            try
            {
                DateTime dtnow = DateTime.Now;

                using (DataAccess Access = new DataAccess())
                {
                    Access.SetSQLLinkParam(_clk.m_ServerIP, _clk.m_DbName, _clk.m_ServerID, _clk.m_PassWord);

                    conn = Access.Connection(true);

                    trans = conn.BeginTransaction();

                    SqlCommand command = new SqlCommand();
                    command.Connection = conn;
                    command.Transaction = trans;
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 200;

                    foreach (var item in sqlDirectionary)
                    {

                        string sqlstr = item.Key;
                        command.CommandText = sqlstr;
                        SqlParameterCollection sqlParams = command.Parameters;
                        sqlParams.Clear();
                        foreach (var sqlParam in item.Value)
                        {

                            sqlParams.Add(sqlParam.Name, sqlParam.Type);
                            sqlParams[sqlParam.Name].Value = sqlParam.Value;
                        }
                        int count = command.ExecuteNonQuery();
                    }

                    ///最终执行插入
                    sqlBulk = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, trans);
                    sqlBulk.BulkCopyTimeout = 5000;
                    sqlBulk.DestinationTableName = model.Tables[0].TableName;
                    sqlBulk.WriteToServer(model.Tables[0]);

                    trans.Commit();

                }
            }
            catch (Exception ex)
            {
                trans.Rollback();
                status = 0;
            }
            finally
            {
                trans.Dispose();
                sqlBulk.Close();
                conn.Close();
            }
            return status;
        }
    }
}

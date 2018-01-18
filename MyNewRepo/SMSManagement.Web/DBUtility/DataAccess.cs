using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using SMSManagement.Web.Common;

namespace SMSManagement.Web.DBUtility
{
    /// <summary>
    /// SqlServer数据库操作类
    /// </summary>
    public class DataAccess : IDisposable
    {
        private SqlConnection Conn;

        /// <summary>
        /// SQL服务器IP
        /// </summary>
        private string m_Serverip = string.Empty;
        /// <summary>
        /// SQL数据库名称
        /// </summary>
        private string m_DBname = string.Empty;
        /// <summary>
        /// SQL连接用户名
        /// </summary>
        private string m_ServerName = string.Empty;
        /// <summary>
        /// SQL连接密码
        /// </summary>
        private string m_Password = string.Empty;

        #region IDisposable 成员
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(true);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;
            if (Conn != null)
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
            }
        }
        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="ismaster">true连接用户信息服务器</param>
        public DataAccess()
        {

        }


        /// <summary>
        /// 设置用户指定连接参数
        /// </summary>
        /// <param name="Serverip">服务器IP</param>
        /// <param name="DBname">数据库名称</param>
        /// <param name="ServerName">用户名</param>
        /// <param name="Password">密码</param>
        public void SetSQLLinkParam(string Serverip, string DBname, string ServerName, string Password)
        {
            m_Serverip = Serverip;
            m_DBname = DBname;
            m_ServerName = ServerName;
            m_Password = Password;

        }

        /// <summary>
        /// 得到数据连接串
        /// </summary>
        /// <param name="v_ismaster">是否为Master</param>
        /// <returns>数据连接</returns>
        public string ConnString()
        {
            return "server=" + m_Serverip + ";Initial Catalog=" + m_DBname + "; User ID= " + m_ServerName + ";Password=" + m_Password + ";Connect Timeout=200";
        }

        /// <summary>
        /// 打开数据库链接
        /// </summary>
        /// <param name="Opened"></param>
        /// <returns></returns>
        public SqlConnection Connection(bool Opened)
        {
            SqlConnection v_conn = new SqlConnection(ConnString());
            try
            {
                if (Opened)
                {
                    if (v_conn.State == ConnectionState.Closed)
                        v_conn.Open();
                }
            }
            catch (Exception Ex)
            {
                throw new ConnectionException("数据库连接失败,可能由以下原因引起:\n" + Ex.Message, Ex);
            }
            return v_conn;
        }

        /// <summary>
        /// 执行SQL命令
        /// </summary>
        /// <param name="command">SQL命令</param>
        /// <returns>影响记录数量</returns>
        public int ExecuteNonQuery(SqlCommand command)
        {
            int Iresult = 0;
            try
            {
                this.Conn = this.Connection(true);
                command.Connection = Conn;
                Iresult = command.ExecuteNonQuery();

                //SQLLog(command);
            }
            catch (ConnectionException Ex)
            {
                throw new ConnectionException(Ex.Message);
            }
            catch (Exception Ex)
            {
                throw new DataAccessExcption("数据访问失败,可能由以下原因引起:\n" + Ex.Message, Ex);
            }
            finally
            {
                command.Dispose();
                if (this.Conn != null)
                {
                    this.Conn.Dispose();
                }
            }
            return Iresult;
        }



        /// <summary>
        /// 执行SQL命令
        /// </summary>
        /// <param name="command">SQL命令</param>
        /// <returns>操作结果对象，第一行第一列</returns>
        public object ExecuteScalar(SqlCommand command)
        {
            object Iresult = null;
            try
            {
                this.Conn = this.Connection(true);
                command.Connection = Conn;
                Iresult = command.ExecuteScalar();

                //SQLLog(command);
            }
            catch (ConnectionException Ex)
            {
                throw new ConnectionException(Ex.Message);
            }
            catch (Exception Ex)
            {
                throw new DataAccessExcption("数据访问失败,可能由以下原因引起:\n" + Ex.Message, Ex);
            }
            finally
            {
                command.Dispose();
                if (this.Conn != null)
                {
                    this.Conn.Dispose();
                }
            }
            return Iresult;
        }

        /// <summary>
        /// 查询数据并填入DataSet中映射的数据表中
        /// </summary>
        /// <param name="command">SQL查询命名</param>
        /// <param name="ds">数据实体</param>
        /// <param name="srcTable">映射表</param>
        /// <returns>数据实体</returns>
        public DataSet Fill(SqlCommand command, DataSet ds, string srcTable)
        {
            SqlDataAdapter dap = new SqlDataAdapter();
            try
            {
                this.Conn = this.Connection(true);
                command.Connection = this.Conn;
                dap.SelectCommand = command;
                dap.Fill(ds, srcTable);

                //SQLLog(command);
            }
            catch (ConnectionException Ex)
            {
                throw new ConnectionException(Ex.Message);
            }
            catch (Exception Ex)
            {
                throw new DataAccessExcption("[" + srcTable + "]表获取数据失败;" + "数据访问失败,可能由以下原因引起:\n" + Ex.Message, Ex);
            }
            finally
            {
                command.Dispose();
                if (this.Conn != null)
                {
                    this.Conn.Dispose();
                }
            }
            return ds;
        }

        /// <summary>
        /// 查询数据并填入DataSet中
        /// </summary>
        /// <param name="command">SQL查询命名</param>
        /// <param name="ds">数据实体</param>
        /// <param name="srcTable">映射表</param>
        /// <returns>数据实体</returns>
        public DataSet Fill(SqlCommand command, DataSet ds)
        {
            SqlDataAdapter dap = new SqlDataAdapter();
            try
            {
                this.Conn = this.Connection(true);
                command.Connection = this.Conn;
                dap.SelectCommand = command;
                dap.Fill(ds);

                //SQLLog(command);
            }
            catch (ConnectionException Ex)
            {
                throw new ConnectionException(Ex.Message);
            }
            catch (Exception Ex)
            {
                throw new DataAccessExcption("数据访问失败,可能由以下原因引起:\n" + Ex.Message, Ex);
            }
            finally
            {
                command.Dispose();
                if (this.Conn != null)
                {
                    this.Conn.Dispose();
                }
            }
            return ds;
        }
        /// <summary>
        /// 查询指定数据,并填入数据表中
        /// </summary>
        /// <param name="command">SQL命令</param>
        /// <param name="ds">数据实体</param>
        /// <param name="startRecord">开始记录</param>
        /// <param name="maxRecord">数据量</param>
        /// <param name="srcTable">映射表</param>
        /// <returns>数据实体</returns>
        public DataSet Fill(SqlCommand command, DataSet ds, int startRecord, int maxRecord, string srcTable)
        {
            SqlDataAdapter dap = new SqlDataAdapter();
            try
            {
                this.Conn = this.Connection(true);
                command.Connection = this.Conn;
                dap.SelectCommand = command;
                dap.Fill(ds, startRecord, maxRecord, srcTable);

                //SQLLog(command);
            }
            catch (ConnectionException Ex)
            {
                throw new ConnectionException(Ex.Message);
            }
            catch (Exception Ex)
            {
                throw new DataAccessExcption("[" + srcTable + "]表获取数据失败;" + "数据访问失败,可能由以下原因引起:\n" + Ex.Message, Ex);
            }
            finally
            {
                command.Dispose();
                this.Conn.Dispose();
            }
            return ds;
        }
        /// <summary>
        /// 查询数据,直接填充数据表
        /// </summary>
        /// <param name="command">SQL命令</param>
        /// <returns>数据表</returns>
        public DataTable Fill(SqlCommand command)
        {
            SqlDataAdapter dap = new SqlDataAdapter();
            DataTable srcTable = new DataTable();
            try
            {
                this.Conn = this.Connection(true);
                command.Connection = this.Conn;
                dap.SelectCommand = command;
                dap.Fill(srcTable);

                //SQLLog(command);
            }
            catch (ConnectionException Ex)
            {
                throw new ConnectionException(Ex.Message);
            }
            catch (Exception Ex)
            {
                throw new DataAccessExcption("数据访问失败,可能由以下原因引起:\n" + Ex.Message, Ex);
            }
            finally
            {
                command.Dispose();
                if (this.Conn != null)
                {
                    this.Conn.Dispose();
                }
            }
            return srcTable;
        }
        /// <summary>
        /// 执行新增数据SQL命令,支持数据批量增加
        /// </summary>
        /// <param name="command">SQL命令</param>
        /// <param name="ds">数据实体</param>
        /// <param name="mappingname">数据表映射名</param>
        /// <returns>影响数据数量</returns>
        public int Insert(SqlCommand command, DataSet ds, string mappingname)
        {
            int Iresult = 0;
            SqlDataAdapter dap = new SqlDataAdapter();
            try
            {
                this.Conn = this.Connection(true);
                command.Connection = this.Conn;
                dap.InsertCommand = command;
                Iresult = dap.Update(ds, mappingname);
                ds.Tables[mappingname].AcceptChanges();

                //SQLLog(command);
            }
            catch (ConnectionException Ex)
            {
                throw new ConnectionException(Ex.Message);
            }
            catch (Exception Ex)
            {
                throw new DataAccessExcption("[" + mappingname + "]表创建失败;" + "数据访问失败,可能由以下原因引起:\n" + Ex.Message, Ex);
            }
            finally
            {
                command.Dispose();
                if (this.Conn != null)
                {
                    this.Conn.Dispose();
                }
            }
            return Iresult;
        }
        /// <summary>
        /// 功能描述：执行sql命令，增加了事务的处理情况
        /// </summary>
        /// <param name="command">SQL命令</param>
        /// <param name="ds">数据实体</param>
        /// <param name="mappingname">影射名称</param>
        /// <returns></returns>
        public int InsertTrans(SqlCommand command, DataSet ds, string mappingname)
        {
            int Iresult = 0;
            SqlDataAdapter dap = new SqlDataAdapter();
            this.Conn = this.Connection(true);
            SqlTransaction trans = this.Conn.BeginTransaction();
            try
            {
                command.Connection = this.Conn;
                dap.InsertCommand = command;
                Iresult = dap.Update(ds, mappingname);
                trans.Commit();
                ds.Tables[mappingname].AcceptChanges();

                //SQLLog(command);
            }
            catch (ConnectionException Ex)
            {
                trans.Rollback();
                throw new ConnectionException(Ex.Message);
            }
            catch (Exception Ex)
            {
                trans.Rollback();
                throw new DataAccessExcption("[" + mappingname + "]表创建失败;" + "数据访问失败,可能由以下原因引起:\n" + Ex.Message, Ex);
            }
            finally
            {
                trans.Dispose();
                command.Dispose();
                this.Conn.Dispose();
            }
            return Iresult;
        }
        /// <summary>
        /// 事务处理
        /// </summary>
        /// <param name="command">sql命令</param>
        /// <returns>影响行数</returns>
        public int InsertTrans(SqlCommand command)
        {
            int Iresult = 0;
            this.Conn = this.Connection(true);
            SqlTransaction trans = this.Conn.BeginTransaction();
            try
            {
                command.Connection = this.Conn;
                command.Transaction = trans;
                SqlParameterCollection sqlParams = command.Parameters;
                Iresult = command.ExecuteNonQuery();
                trans.Commit();

                //SQLLog(command);
            }
            catch (ConnectionException Ex)
            {
                trans.Rollback();
                throw new ConnectionException(Ex.Message);
            }
            catch (Exception Ex)
            {
                trans.Rollback();
                throw new DataAccessExcption("数据访问失败,可能由以下原因引起:\n" + Ex.Message, Ex);
            }
            finally
            {
                trans.Dispose();
                command.Dispose();
                this.Conn.Dispose();
            }
            return Iresult;
        }
        /// <summary>
        /// 执行更新SQL命令,支持批量数据更新
        /// </summary>
        /// <param name="command">SQL命令</param>
        /// <param name="ds">数据实体</param>
        /// <param name="mappingname">数据表映射名</param>
        /// <returns>影响数据数量</returns>
        public int Update(SqlCommand command, DataSet ds, string mappingname)
        {
            int Iresult = 0;
            SqlDataAdapter dap = new SqlDataAdapter();
            try
            {
                this.Conn = this.Connection(true);
                command.Connection = this.Conn;
                dap.UpdateCommand = command;
                Iresult = dap.Update(ds, mappingname);
                ds.Tables[mappingname].AcceptChanges();

                //SQLLog(command);
            }
            catch (ConnectionException Ex)
            {
                throw new ConnectionException(Ex.Message);
            }
            catch (Exception Ex)
            {
                throw new DataAccessExcption("[" + mappingname + "]表更新失败;" + "数据访问失败,可能由以下原因引起:\n" + Ex.Message, Ex);
            }
            finally
            {
                command.Dispose();
                if (this.Conn != null)
                {
                    this.Conn.Dispose();
                }
            }
            return Iresult;
        }
        /// <summary>
        /// 执行更行数据命令
        /// </summary>
        /// <param name="command">SQL命令</param>
        /// <param name="dt">数据表</param>
        /// <returns>影响数据数量</returns>
        public int Update(SqlCommand command, DataTable dt)
        {
            int Iresult = 0;
            SqlDataAdapter dap = new SqlDataAdapter();
            try
            {
                this.Conn = this.Connection(true);
                command.Connection = this.Conn;
                dap.UpdateCommand = command;
                Iresult = dap.Update(dt);
                dt.AcceptChanges();

                //SQLLog(command);
            }
            catch (ConnectionException Ex)
            {
                throw new ConnectionException(Ex.Message);
            }
            catch (Exception Ex)
            {
                throw new DataAccessExcption("[" + dt.TableName + "]表更新失败;" + "数据访问失败,可能由以下原因引起:\n" + Ex.Message, Ex);
            }
            finally
            {
                command.Dispose();
                if (this.Conn != null)
                {
                    this.Conn.Dispose();
                }
            }
            return Iresult;
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcedureName">存储过程名称</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>执行结果</returns>
        //public int ExecuteStoredProcedure(string storedProcedureName, SqlParameter[] parameters)
        //{
        //    int Iresult = 0;
        //    SqlDataAdapter dap = new SqlDataAdapter();
        //    SqlCommand command = new SqlCommand();
        //    try
        //    {
        //        this.Conn = this.Connection(true);
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.CommandText = storedProcedureName;
        //        foreach (SqlParameter param in parameters)
        //        {
        //            command.Parameters.Add(param);
        //        }
        //        Iresult = command.ExecuteNonQuery();

        //        SQLLog(command);
        //    }
        //    catch (ConnectionException Ex)
        //    {
        //        throw new ConnectionException(Ex.Message);
        //    }
        //    catch (Exception Ex)
        //    {
        //        throw new DataAccessExcption("[" + storedProcedureName + "]数据库存储过程执行错误;" + "数据访问失败,可能由以下原因引起:\n" + Ex.Message, Ex);
        //    }
        //    finally
        //    {
        //        command.Dispose();
        //        if (this.Conn != null)
        //        {
        //            this.Conn.Dispose();
        //        }
        //    }
        //    return Iresult;
        //}

        //private void SQLLog(SqlCommand command)
        //{
        //    SqlCommand logcommand = null;
        //    SqlConnection logcon = null;

        //    try
        //    {
        //        if (Plib.ep.SQLLOG_STATE == "open")
        //        {
        //            string sqlstr = "INSERT INTO SQLLOG (系统,用户名,IP地址,SQL字符串,SQL参数) VALUES (@系统,@用户名,@IP地址,@SQL字符串,@SQL参数);";

        //            logcommand = new SqlCommand(sqlstr);
        //            logcommand.CommandType = CommandType.Text;

        //            SqlParameterCollection sqlParams = logcommand.Parameters;

        //            sqlParams.Add("@系统", SqlDbType.VarChar);
        //            sqlParams.Add("@用户名", SqlDbType.VarChar);
        //            sqlParams.Add("@IP地址", SqlDbType.VarChar);
        //            sqlParams.Add("@SQL字符串", SqlDbType.NVarChar);
        //            sqlParams.Add("@SQL参数", SqlDbType.NVarChar);

        //            sqlParams[0].Value = Plib.ep.CurSystemType.ToString();
        //            sqlParams[1].Value = Plib.ep.UserInfo.UserName;
        //            sqlParams[2].Value = "";
        //            sqlParams[3].Value = command.CommandText;

        //            if (command.Parameters == null)
        //            {
        //                sqlParams[4].Value = "无";
        //            }
        //            else
        //            {
        //                string temp_p = "";

        //                for (int i = 0; i < command.Parameters.Count; i++)
        //                {
        //                    if (command.Parameters[i].Value != null && command.Parameters[i].ToString().Trim() != "")
        //                    {
        //                        temp_p += command.Parameters[i].Value.ToString() + "|分隔|";
        //                    }
        //                }

        //                sqlParams[4].Value = temp_p;
        //            }

        //            logcon = new SqlConnection("server=" + Plib.ep.SQLLOG_IP + ";Initial Catalog=" + Plib.ep.SQLLOG_DB + "; User ID= " + Plib.ep.ServerSQLName + ";Password=" + Plib.ep.ServerSQLPassword + ";Connect Timeout=200");

        //            if (logcon.State == ConnectionState.Closed)
        //            {
        //                logcon.Open();
        //            }

        //            logcommand.Connection = logcon;

        //            logcommand.ExecuteNonQuery();
        //        }
        //    }
        //    catch
        //    {
        //    }
        //    finally
        //    {
        //        if (logcommand != null)
        //        {
        //            logcommand.Dispose();

        //            if (logcon != null)
        //            {
        //                logcon.Dispose();
        //            }
        //        }
        //    }
        //}
                
    }
}

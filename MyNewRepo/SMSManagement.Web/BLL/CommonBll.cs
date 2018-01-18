using SMSManagement.Web.DBUtility;
using SMSManagement.Web.SP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SMSManagement.Web.BLL
{
    public class CommonBll
    {
        public string _infomation;

        private DataConnectType _dct;

        public CommonBll()
        {
            _infomation = string.Empty;

            _dct = DataConnectType.ServerDBDataService;

        }

        public CommonBll(DataConnectType dct)
        {
            _infomation = string.Empty;

            _dct = dct;

        }

        public DataSet GetDataSet(string strSQL, List<CustomSqlParam> paramList)
        {
            DataSet ds = null;

            try
            {
                using (CommonDal Access = new CommonDal(_dct))
                {
                    ds = Access.GetDataByParameterizedSql(strSQL, paramList);
                }

            }
            catch (Exception ex)
            {
                ds = null;
                this._infomation = ex.Message;
            }

            return ds;


        }

        public int ExecuteTransSql(Dictionary<String, List<CustomSqlParam>> sqlDirectionary)
        {
            using (CommonDal Access = new CommonDal(_dct))
            {
                return Access.ExecuteTransSql(sqlDirectionary);
            }
        }

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="mappingname"></param>
        /// <param name="OnRowsCopied"></param>
        public bool BulkInsert(DataSet model)
        {
            using (CommonDal Access = new CommonDal(_dct))
            {
                return Access.BulkInsert(model);
            }
        }

        public int ExecuteThenInsertTrans(Dictionary<String, List<CustomSqlParam>> sqlDirectionary, DataSet model)
        {
            using (CommonDal Access = new CommonDal(_dct))
            {
                return Access.ExecuteThenInsertTrans(sqlDirectionary, model);
            }
        }

        public DataSet GetDataSetByPage(string TableName, string FieldList, string SearchCondition, string orderStr, int PageIndex, int PageSize)
        {
            DataSet ds = null;

            try
            {
                if (SearchCondition.Length > 0)
                {
                    SearchCondition = " WHERE " + SearchCondition;
                }

                using (CommonDal Access = new CommonDal(_dct))
                {
                    ds = Access.GetPageDataByParameterizedSql(new List<CustomSqlParam>(), TableName, FieldList, SearchCondition,
                        orderStr, PageIndex, PageSize);
                }

            }
            catch (Exception ex)
            {
                ds = null;
                this._infomation = ex.Message;
            }

            return ds;


        }
    }
}

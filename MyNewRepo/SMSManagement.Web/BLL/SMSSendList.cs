﻿using SMSManagement.Web.DBUtility;
using SMSManagement.Web.Model;
using SMSManagement.Web.SP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;


namespace SMSManagement.Web.BLL
{
    public class SMSSendListBLL
    {
        public string _infomation;

        private DataConnectType _dct;

        public SMSSendListModel model;

        public SMSSendListBLL()
        {
            _infomation = string.Empty;

            _dct = DataConnectType.ServerDBDataService;

            model = new SMSSendListModel();
        }

        public SMSSendListBLL(DataConnectType dct)
        {
            _infomation = string.Empty;

            _dct = dct;

            model = new SMSSendListModel();
        }

        public int UpdateData(string strSQL)
        {
            int i = 0;
            try
            {
                using (CommonDal Access = new CommonDal(_dct))
                {
                    i = Access.ExecuteParameterizedSql(strSQL, model.paramList);
                }

            }
            catch (Exception ex)
            {
                i = 0;
                this._infomation = ex.Message;
            }
            return i;

        }

        public DataSet GetDataSet(string SearchField, string SearchCondition)
        {
            DataSet ds = null;

            try
            {
                string strSQL = "  SELECT  "
                + " " + SearchField
                + " FROM " + SMSSendListModel.TableName;

                if (SearchCondition.Length > 0)
                {
                    strSQL += " WHERE " + SearchCondition;
                }



                using (CommonDal Access = new CommonDal(_dct))
                {
                    ds = Access.GetDataByParameterizedSql(strSQL, model.paramList);
                }

            }
            catch (Exception ex)
            {
                ds = null;
                this._infomation = ex.Message;
            }

            return ds;


        }

        public DataSet GetDataSetByPage(string FieldList, string SearchCondition, string orderStr, int PageIndex, int PageSize)
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
                    ds = Access.GetPageDataByParameterizedSql(model.paramList, SMSSendListModel.TableName, FieldList, SearchCondition,
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

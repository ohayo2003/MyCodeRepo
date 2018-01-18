using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic;
using System.Web;
using System.Data;
using System.Text.RegularExpressions;

namespace SMSManagement.Web.Common
{
    /// <summary>
    /// 转换方法集合
    /// </summary>
    public class MyTransform
    {
        /// <summary>
        /// 四舍五入
        /// </summary>
        /// <param name="value">源数据</param>
        /// <param name="decimals">保留小数位数</param>
        /// <returns></returns>
        public static double ChinaRound(double value, int decimals)
        {
            if (value < 0)
            {
                return Math.Round(value + 5 / Math.Pow(10, decimals + 1), decimals, MidpointRounding.AwayFromZero);
            }
            else
            {
                return Math.Round(value, decimals, MidpointRounding.AwayFromZero);
            }
        }
        /// <summary>
        /// 百分比
        /// </summary>
        /// <param name="a">被除数</param>
        /// <param name="b">除数</param>
        /// <param name="leave">小数点后保留位数</param>
        /// <returns></returns>
        public static decimal percentage(int a, int b, int leave)
        {
            if (b > 0)
            {
                //string temp = Math.Round((decimal)(a * 100.0 / b), leave, MidpointRounding.AwayFromZero).ToString();
                decimal temp = Math.Round((decimal)((decimal)(a) * 100 / b), leave, MidpointRounding.AwayFromZero);

                if (temp == 100)
                {
                    return 100;
                }
                else
                {
                    return temp;
                }
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 字符串转换成时间
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="f">格式，如yyyy-MM-dd</param>
        /// <returns></returns>
        public static string StringToDateTime(string s, string f)
        {
            DateTime temp = new DateTime();

            if (DateTime.TryParse(s, out temp))
            {
                return temp.ToString(f);
            }

            return "";
        }
        /// <summary>
        /// 字符串转换日期
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="defaulttime">转换失败返回的默认值</param>
        /// <returns></returns>
        public static DateTime StringToDateTime(string source, DateTime defaulttime)
        {
            DateTime temp = new DateTime();

            if (!DateTime.TryParse(source, out temp))
            {
                return defaulttime;
            }

            return temp;
        }
        /// <summary>
        /// 字符串转换浮点型
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="defaultnumber">转换失败返回的默认值</param>
        /// <returns></returns>
        public static float StringToFloat(string source, float defaultnumber)
        {
            float temp = 0;

            if (!float.TryParse(source, out temp))
            {
                return defaultnumber;
            }

            return temp;
        }
        /// <summary>
        /// 字符串转换成double
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="defaultnumber">转换失败返回的默认值</param>
        /// <returns></returns>
        public static double StringToDouble(string source, double defaultnumber)
        {
            double temp = 0d;

            if (!double.TryParse(source, out temp))
            {
                return defaultnumber;
            }

            return temp;
        }

        /// <summary>
        /// 字符串转换整数
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="defaultnumber">转换失败返回的默认值</param>
        /// <returns></returns>
        public static int StringToInt(string source, int defaultnumber)
        {
            int temp = 0;

            if (!int.TryParse(source, out temp))
            {
                return defaultnumber;
            }

            return temp;
        }
        /// <summary>
        /// 字符串转decimal
        /// </summary>
        /// <param name="source"></param>
        /// <param name="defaultnumber"></param>
        /// <returns></returns>
        public static decimal StringToDecimal(string source, decimal defaultnumber)
        {
            decimal temp = 0M;

            if (!decimal.TryParse(source, out temp))
            {
                return defaultnumber;
            }

            return temp;
        }
        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static string ObjectToString(object obj, string def)
        {
            string ret = def;
            if (obj != null)
            {
                try
                {
                    ret = obj.ToString();
                }
                catch
                {                	
                }
            }
            return ret;
        }
        /// <summary>
        /// 转换为Int64
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static long ObjectToInt64(object obj, long def)
        {
            long ret = def;
            if (obj != null)
            {
                try
                {
                    ret = Convert.ToInt64(obj);
                }
                catch
                {
                }
            }
            return ret;
        }
        /// <summary>
        /// 过滤字符串的特殊字符（“'”、“*”）
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Clear(string s)
        {
            return s.Replace("'", "").Replace("*", "");
        }
        /// <summary>
        /// IP地址转换成Int64整数
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static Int64 IPToInt64(string ip)
        {
            try
            {
                string[] temp = ip.Split('.');

                if (temp.Length != 4)
                {
                    return 0;
                }

                Int64 count = 0;

                for (int i = 0; i < temp.Length; i++)
                {
                    Int64 temp_count = 1;

                    for (int j = 3 - i; j > 0; j--)
                    {
                        temp_count = temp_count * 256;
                    }

                    count += temp_count * Int64.Parse(temp[i]);
                }

                return count;
            }
            catch
            {
                return 0;
            }
        }
        /// <summary>
        /// Int64整数字符串转换成IP地址
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string Int64ToIP(string num)
        {
            try
            {
                string temp = Int64.Parse(num).ToString("x");

                temp = temp.PadLeft(8, '0');

                string ip = "";

                for (int i = 0; i < 8; i += 2)
                {
                    ip += Int32.Parse(temp.Substring(i, 2), System.Globalization.NumberStyles.AllowHexSpecifier).ToString() + ".";
                }

                return ip.Substring(0, ip.Length - 1);
            }
            catch
            {
                return "";
            }
        }


        /// <summary>
        /// 获得字节数
        /// </summary>
        /// <param name="strTmp"></param>
        /// <returns></returns>
        public static int GetByteCount(string strTmp)
        {
            int intCharCount = 0;
            for (int i = 0; i < strTmp.Length; i++)
            {
                if (System.Text.UTF8Encoding.UTF8.GetByteCount(strTmp.Substring(i, 1)) == 3)
                {
                    intCharCount = intCharCount + 2;
                }
                else
                {
                    intCharCount = intCharCount + 1;
                }
            }
            return intCharCount;
        }
        /// <summary>
        /// 字符串转换成日期格式
        /// </summary>
        /// <param name="str_in"></param>
        /// <returns></returns>
        public static string Get_S_E_TimeFromString(string str_in)
        {
            string str_out = string.Empty;
            if (!str_in.Trim().Equals(""))
            {

                string tmp = str_in.Trim();

                DateTime dt_Tdate = DateTime.Now;
                if (DateTime.TryParse(tmp, out dt_Tdate))
                {
                    str_out = dt_Tdate.ToString("yyyy-MM-dd");
                }

            }

            return str_out;
        }

        /// <summary>
        /// 将俩个字段转成JASONString格式
        /// </summary>
        /// <param name="dtSourse"></param>
        /// <returns></returns>
        public static string GetDataTableToJasonString(DataTable dtSourse)
        {
            string JasonString = "";
            string graphs = "";

            if (dtSourse.Columns.Count == 2)
            {
                JasonString += "<chart>";
                JasonString += "<series>";

                graphs += "<graphs>";
                graphs += "<graph gid='0'>";
                for (int j = 0; j < dtSourse.Rows.Count; j++)
                {
                    JasonString += "<value xid='" + j + "'>" + dtSourse.Rows[j][1].ToString() + " </value>";
                    graphs += "<value xid='" + j + "'>" + dtSourse.Rows[j][0].ToString() + "</value>";
                }
                graphs += "</graph>";
                graphs += "</graphs>";
                JasonString += "</series>";
                JasonString += graphs;
                JasonString += "</chart>";
            }
            return JasonString;
        }

        /// <summary>
        /// 将俩个字段转成柱状多日期JASONString格式
        /// </summary>
        /// <param name="dtSourse"></param>
        /// <returns></returns>
        public static string GetDataTableToJasonStringCloumn(DataTable dtSourse, string[] year, string[] pNum)
        {
            string JasonString = "";
            string graphs = "";
            string series = "";
            for (int n = 0; n < pNum.Length; n++)
            {
                string value = "";
                for (int m = 0; m < year.Length; m++)
                {
                    if (dtSourse.Columns.Count == 3)
                    {
                        for (int i = 0; i < dtSourse.Rows.Count; i++)
                        {
                            if (dtSourse.Rows[i][1].ToString() == pNum[n].ToString() && dtSourse.Rows[i][2].ToString() == year[m].ToString())
                            {
                                value += "<value xid='" + m + "'>" + dtSourse.Rows[i][0].ToString() + "</value>";
                            }
                        }

                    }
                    if (!series.Contains(year[m].ToString()))
                    {
                        series += "<value xid='" + m + "'>" + year[m].ToString() + "</value>";
                    }
                }

                graphs += "<graph gid='" + n + "'>";
                graphs += value;
                graphs += " </graph>";
            }
            JasonString += "<chart><series>";
            JasonString += series;
            JasonString += "</series>";
            JasonString += "<graphs>";
            JasonString += graphs;
            JasonString += "</graphs></chart>";

            return JasonString;
        }

        /// <summary>
        /// 将俩个字段转成柱状多日期JASONString格式
        /// </summary>
        /// <param name="dtSourse"></param>
        /// <returns></returns>
        public static string GetDataTableToCloumnChart_settings(string[] pNum)
        {
            string[] colors = new string[] { "69102173", "1694259", "07660", "1853113", "244203207", "2313833", "2331945", "2451840", "0161124", "23169144", "10677157", "2159178", "070118", "2286844", "236183201", "170154167", "147123175", "18115689", "154182186", "25523672", "2389958", "183214144", "2156218", "12765150", "1871541", "2532190", "3179161", "104190151", "0166166", "250229228" };
            string graphs = "";
            graphs += " <graphs>";
            for (int i = 0; i < pNum.Length; i++)
            {
                if (!graphs.Contains(pNum[i].ToString()))
                {
                    graphs += "<graph gid='" + i + "'>";
                    graphs += "<title>" + pNum[i].ToString() + "</title>";
                    if (i <= 29)
                    {
                        graphs += "<color>" + colors[i].ToString() + "</color>";

                    }
                    else
                    {
                        graphs += "<color>0489e6</color>";
                    }
                    graphs += "</graph>";
                }
            }
            graphs += " </graphs>";

            return graphs;

        }

        /// <summary>  
        /// Json特符字符过滤，参见http://www.json.org/  
        /// </summary>  
        /// <param name="sourceStr">要过滤的源字符串</param> 
        /// <returns>返回过滤的字符串</returns>  
        public static string JsonCharFilter(string sourceStr)
        {

            sourceStr = sourceStr.Replace("\\", "\\\\");

            sourceStr = sourceStr.Replace("\b", "\\\b");

            sourceStr = sourceStr.Replace("\t", "\\\t");

            sourceStr = sourceStr.Replace("\n", "\\\n");

            sourceStr = sourceStr.Replace("\n", "\\\n");

            sourceStr = sourceStr.Replace("\f", "\\\f");

            sourceStr = sourceStr.Replace("\r", "\\\r");

            return sourceStr.Replace("\"", "\\\"");

        }
        /// <summary>
        /// 过滤来源字符
        /// </summary>
        /// <param name="WholeContent"></param>
        /// <returns></returns>
        public static string GetLaiyuan(string WholeContent)
        {
            string sOUT = "";

            //string Patten_ly = @"来源[:：\s]\s*(<a.*?>)*(.*?)[\s+<]";
            string Patten_ly = @"来源[:：]\s*(<.*?>)*(.*?)[\s+<]";

            WholeContent = Regex.Replace(WholeContent, @"<!--.*?-->", "", RegexOptions.Singleline);

            WholeContent = WholeContent.Replace("&nbsp;", " ");

            MatchCollection matches = Regex.Matches(WholeContent, Patten_ly, RegexOptions.IgnoreCase);

            if (matches.Count > 0)
            {
                sOUT = matches[0].Groups[2].Value.ToString().Trim().Replace("）", "").Replace("（", "").Replace("]", "").Replace("[", "");
                sOUT = sOUT.Replace("(", "").Replace(")", "");
                sOUT = sOUT.Replace("【", "").Replace("】", "");

                if (sOUT.Length > 20)
                {
                    sOUT = "";
                }

            }

            return sOUT;
        }

        #region DataTable


        /// <summary>
        /// 行列转换，只支持三列，可以参考SQL语句中关于pivot的介绍
        /// <para>第一列去重，不变</para>
        /// <para>第二列去重，各值转变为列名</para>
        /// <para>第三列为填充数值，各数值填充到转变后的相应位置</para>
        /// </summary>
        /// <param name="source">源数据集</param>
        /// <returns></returns>
        private static DataTable DataTablePivot_Three(DataTable source)
        {
            if (source == null)
            {
                return null;
            }
            if (source.Columns.Count < 3 || source.Rows.Count < 1)
            {
                return source;
            }
            DataTable dt = new DataTable();
            dt.Columns.Add(source.Columns[0].ColumnName);
            var columns = (from x in source.Rows.Cast<DataRow>() select x[1].ToString()).Distinct();
            foreach (var item in columns) dt.Columns.Add(item);
            var data = from x in source.Rows.Cast<DataRow>()
                       group x by x[0] into g
                       select new { Key = g.Key.ToString(), Items = g };
            data.ToList().ForEach(x =>
            {
                object[] array = new object[dt.Columns.Count];
                array[0] = x.Key;
                for (int i = 1; i < dt.Columns.Count; i++)
                    array[i] = (from y in x.Items
                                where y[1].ToString() == dt.Columns[i].ToString()
                                select y[2].ToString())
                               .SingleOrDefault();
                dt.Rows.Add(array);
            });
            return dt;
        }

        /// <summary>
        /// 行列转换，可以参考SQL语句中关于pivot的介绍
        /// <para>第一列去重，不变</para>
        /// <para>第二列去重，各值转变为列名</para>
        /// <para>其余列为填充数值，各数值填充到转变后的相应位置</para>
        /// </summary>
        /// <param name="source">源数据集</param>
        /// <returns></returns>
        public static DataTable DataTablePivot(DataTable source)
        {
            if (source == null)
            {
                return null;
            }
            if (source.Columns.Count < 3 || source.Rows.Count < 1)
            {
                return source;
            }

            DataTable dt = new DataTable();
            dt.Columns.Add(source.Columns[0].ColumnName);
            var columnNames = (from x in source.Rows.Cast<DataRow>() select x[1].ToString().Trim()).Distinct().ToList();
            foreach (var colName in columnNames)
            {
                for (int i = 2; i < source.Columns.Count; i++)
                {
                    dt.Columns.Add(string.Format("{0}({1})", colName, source.Columns[i].ColumnName));
                }
            }

            var data = from x in source.Rows.Cast<DataRow>()
                       group x by x[0] into g
                       select new { Key = g.Key.ToString(), Items = g };
            int oldColCount = source.Columns.Count - 2; //原数据列的个数
            data.ToList().ForEach(x =>
            {
                object[] array = new object[dt.Columns.Count];
                array[0] = x.Key;
                for (int i = 0; i < columnNames.Count(); i++)
                {
                    var rows = x.Items.Cast<DataRow>().ToList();
                    string newColName = columnNames[i];
                    var row = rows.SingleOrDefault(r => r[1].ToString().Trim() == newColName.Trim());
                    if (row != null)
                    {
                        for (int j = 0; j < oldColCount; j++)
                        {
                            array[i * oldColCount + j + 1] = row[j + 2];
                        }
                    }

                }
                dt.Rows.Add(array);
            });
            return dt;
        }

        /// <summary>
        /// 行列转换，将列转换为行
        /// </summary>
        /// <param name="dtSource">源数据集</param>
        /// <param name="isWirteColumnName">是否要写入源DataTable的列名</param>
        /// <returns></returns>
        public static DataTable DataTablePivot2(DataTable dtSource, bool isWirteColumnName)
        {
            if (dtSource == null)
            {
                return null;
            }
            if (dtSource.Columns.Count <= 0 || dtSource.Rows.Count <= 0)
            {
                return dtSource;
            }
            DataTable dt = new DataTable();
            int offset = 0;
            if (isWirteColumnName)
            {
                dt.Columns.Add("OldColumnName", typeof(string));
                offset = 1;
            }
            string firstColName = dtSource.Columns[0].ColumnName;
            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                dt.Columns.Add(firstColName + (i + offset).ToString(), typeof(string));
            }
            for (int c = 0; c < dtSource.Columns.Count; c++)
            {
                DataRow newRow = dt.NewRow();
                if (isWirteColumnName)
                {
                    newRow[0] = dtSource.Columns[c].ColumnName;
                }
                for (int r = 0; r < dtSource.Rows.Count; r++)
                {
                    newRow[r + offset] = dtSource.Rows[r][c];
                }
                dt.Rows.Add(newRow);
            }

            return dt;
        }

        /// <summary>
        /// 二维数组转换为DataTable
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DataTable ArrayToDataTable(string[,] source)
        {
            DataTable dt = new DataTable();
            if (source == null || source.GetLength(0) <= 0 || source.GetLength(1) <= 0)
            {
                return dt;
            }
            int rowCount = source.GetLength(0);
            int colCount = source.GetLength(1);
            for (int c = 0; c < colCount; c++)
            {
                dt.Columns.Add("column" + c, typeof(string));
            }
            for (int r = 0; r < rowCount; r++)
            {
                DataRow row = dt.NewRow();
                for (int c = 0; c < colCount; c++)
                {
                    row[c] = source[r, c];
                }
                dt.Rows.Add(row);
            }
            return dt;
        }

        /// <summary>
        /// 上下颠倒指定的行的顺序
        /// </summary>
        /// <param name="dtSource">源数据集</param>
        /// <param name="beginRow">开始行，从零开始</param>
        /// <param name="endRow">结束行，从零开始</param>
        /// <returns></returns>
        public static DataTable InvertDataRows(DataTable dtSource, int? beginRow = null, int? endRow = null)
        {
            if (dtSource == null || dtSource.Rows.Count <= 0)
            {
                return dtSource;
            }

            if (beginRow == null || beginRow <= 0)
            {
                beginRow = 0;
            }
            if (endRow == null || endRow >= dtSource.Rows.Count)
            {
                endRow = dtSource.Rows.Count - 1;
            }
            DataTable dt = dtSource.Clone();
            System.Collections.Generic.List<DataRow> rows = new System.Collections.Generic.List<DataRow>(); //需要颠倒的行
            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                DataRow row = dt.NewRow();
                row.ItemArray = dtSource.Rows[i].ItemArray;
                if (i >= beginRow && i <= endRow)
                {
                    rows.Add(row);
                }
                else
                {
                    dt.Rows.Add(row);
                }
            }
            if (rows.Count > 0)
            {
                foreach (var row in rows)
                {
                    dt.Rows.InsertAt(row, (int)beginRow);
                }
            }
            return dt;
        }

        /// <summary>
        /// 左右颠倒指定列的顺序
        /// </summary>
        /// <param name="dtSource">源数据集</param>
        /// <param name="beginCol">开始列，从零开始</param>
        /// <param name="endCol">结束列，从零开始</param>
        /// <returns></returns>
        public static DataTable InvertDataColumns(DataTable dtSource, int? beginCol = null, int? endCol = null)
        {
            if (dtSource == null)
            {
                throw new ArgumentNullException("dtSource");
            }

            if (beginCol == null || beginCol <= 0)
            {
                beginCol = 0;
            }
            if (endCol == null || endCol >= dtSource.Columns.Count)
            {
                endCol = dtSource.Columns.Count - 1;
            }

            DataTable dtNew = dtSource.Copy();
            for (int i = (int)endCol; i > beginCol; i--)
            {
                dtNew.Columns[(int)beginCol].SetOrdinal(i);
            }

            return dtNew;
        }

        /// <summary>
        /// 设置指定列的类型
        /// </summary>
        /// <param name="dtSource">源数据</param>
        /// <param name="columnName">列名</param>
        /// <param name="newType">新类型</param>
        /// <returns></returns>
        public static DataTable ResetColumnDataType(DataTable dtSource, string columnName, Type newType)
        {
            if (dtSource == null)
            {
                throw new ArgumentNullException("dtSource");
            }
            if (String.IsNullOrEmpty(columnName))
            {
                throw new ArgumentNullException("columnName");
            }
            if (!dtSource.Columns.Contains(columnName))
            {
                throw new ArgumentOutOfRangeException("columnName", "指定的DataTable中不存在列\"" + columnName + "\"");
            }
            if (newType == null)
            {
                throw new ArgumentNullException("newType");
            }

            DataTable dtNew = dtSource.Copy();
            if (dtNew.Rows.Count <= 0)
            {
                dtNew.Columns[columnName].DataType = newType;
            }
            else
            {
                int colIndex = dtNew.Columns.IndexOf(columnName);
                dtNew.Columns.RemoveAt(colIndex); //移除原来的列
                dtNew.Columns.Add(columnName, newType); //添加新列
                for (int i = 0; i < dtNew.Rows.Count; i++)
                {
                    dtNew.Rows[i][columnName] = dtSource.Rows[i][columnName];
                }
            }
            return dtNew;
        }

        #endregion
    }
}

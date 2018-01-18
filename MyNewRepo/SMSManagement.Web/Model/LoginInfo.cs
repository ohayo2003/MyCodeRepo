using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMSManagement.Web.Model
{
    /// <summary>
    /// 登录信息
    /// </summary>
    [Serializable]
    public class LoginInfo
    {
        /// <summary>
        /// 用户编号，如果是子帐号，此编号用父账号的ID，作为个人比对库查询范围的限定
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 服务器编号
        /// </summary>
        public int ServerID { get; set; }
        /// <summary>
        /// 上传文章的最大字数
        /// </summary>
        public int MaxNumber { get; set; }
        /// <summary>
        /// 上传文章的最小字数
        /// </summary>
        public int MinNumber { get; set; }
        /// <summary>
        /// 用户级别
        /// </summary>
        public int UserLevel { get; set; }
        /// <summary>
        /// 是否为一级管理用户
        /// </summary>
        public bool Root { get; set; }
        /// <summary>
        /// 当前账号所属的省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 当前账号所属的批次ID
        /// </summary>
        public string SampleID { get; set; }
        /// <summary>
        /// 当前账号所属的批次Name
        /// </summary>
        public string SampleName { get; set; }
        /// <summary>
        /// 当前账号所属的权限
        /// </summary>
        public string SchoolPrivilege { get; set; }
        /// <summary>
        /// 当前账号上传论文所属的权限
        /// </summary>
        public string UploadListPrivilege { get; set; }
        /// <summary>
        /// 当前账号所属的批次CheckCondition
        /// </summary>
        public string CheckCondition { get; set; }
        /// <summary>
        /// 当前账号所属的批次开始时间
        /// </summary>
        public string SampleBeginTime { get; set; }
        /// <summary>
        /// 当前账号所属的批次结束时间
        /// </summary>
        public string SampleEndTime { get; set; }
        /// <summary>
        /// 当前账号所属的批次的备注
        /// </summary>
        public string SampleComments { get; set; }
        /// <summary>
        /// 当前账号部分还是全部上传论文，0全部，1部分
        /// </summary>
        public string ThesisUploadRange { get; set; }
        /// <summary>
        /// <summary>
        /// 
        /// </summary>
        public string LimitValue { get; set; }

        //public string ProcessStatus { get; set; }

        /// <summary>
        /// 父账号的用户名
        /// </summary>
        public string RootUserName { get; set; }
        /// <summary>
        /// 父账号的单位名称
        /// </summary>
        public string RootUnitName { get; set; }
        /// <summary>
        /// 是否为子帐号
        /// </summary>
        public bool Child { get; set; }
        /// <summary>
        /// 子帐号类型
        /// </summary>

        public int AccountType { get; set; }
        /// <summary>
        /// 目前只有期刊系统用这个属性
        /// 是否加入投稿记录跟踪平台
        /// </summary>
        public bool IsManuscript { get; set; }

        #region 子帐号属性
        /// <summary>
        /// 子帐号权限，是否可以删除操作
        /// </summary>
        public bool CanDelete { get; set; }
        /// <summary>
        /// 子帐号权限，是否可以修改密码
        /// </summary>
        public bool CanPassword { get; set; }
        /// <summary>
        /// 子帐号权限，是否可以修改检测结果
        /// </summary>
        public bool ModifyResult { get; set; }
        /// <summary>
        /// 子帐号权限，是否可以查看详细检测结果
        /// </summary>
        public bool DetailInfo { get; set; }
        /// <summary>
        /// 子帐号权限，是否可以查看简洁报告单
        /// </summary>
        public bool SimpleReport { get; set; }
        /// <summary>
        /// 子帐号权限，是否可以查看全文报告单
        /// </summary>
        public bool ContentReport { get; set; }
        /// <summary>
        /// 子帐号权限，是否可以查看去除本人报告单
        /// </summary>
        public bool SameReport { get; set; }
        /// <summary>
        /// 子帐号权限，是否可以查看全文对照报告单
        /// </summary>
        public bool CompareReport { get; set; }
        /// <summary>
        /// 子帐号权限，是否可以查看跨语言报告单
        /// </summary>
        public bool EngReport { get; set; }
        /// <summary>
        /// 子帐号权限，是否可以查看全文概览报告单
        /// </summary>
        public bool ContentReport_Only { get; set; }
        #endregion

        #region 查询帐号属性
        /// <summary>
        /// 要查看的账号
        /// </summary>
        public string ViewUserName { get; set; }
        /// <summary>
        /// 要查看的文件夹编号，0表示查看全部
        /// </summary>
        public string ViewFolderID { get; set; }
        /// <summary>
        /// 是否查看管理员
        /// </summary>
        public bool ViewRoot { get; set; }
        #endregion

        /// <summary>
        /// 登录唯一标识
        /// </summary>
        public string LoginID { get; set; }
        /// <summary>
        /// 帐户到期时间
        /// </summary>
        public string ExpTime { get; set; }
        /// <summary>
        /// 上传剩余篇数
        /// </summary>
        public string UpFileLeftNumber { get; set; }
        /// <summary>
        /// 上次登陆地址
        /// </summary>
        public string LastLoginIP { get; set; }
        /// <summary>
        /// 本次登录IP
        /// </summary>
        public string CurLoginIP { get; set; }
        /// <summary>
        /// IP是否异常
        /// </summary>
        public bool IsIPUnusual { get; set; }
        /// <summary>
        /// 登录IP异常，第一次显示提示信息，true显示信息，false不显示信息
        /// </summary>
        public bool IsIPUnusualShow { get; set; }
        /// <summary>
        /// 上次登陆时间
        /// </summary>
        public string LastLoginTime { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 学号
        /// </summary>
        public string StuNum { get; set; }
        /// <summary>
        /// 院系
        /// </summary>
        public string Department { get; set; }
        /// <summary>
        /// 专业
        /// </summary>
        public string Professional { get; set; }
        /// <summary>
        /// 班级
        /// </summary>
        public string StuClass { get; set; }
        /// <summary>
        /// 学校编号
        /// </summary>
        public string UniversityNum { get; set; }
        /// <summary>
        /// 学校名称
        /// </summary>
        public string UniversityName { get; set; }
        /// <summary>
        /// 学生文件夹编号
        /// </summary>
        public string StuFolderNum { get; set; }
        /// <summary>
        /// 学生的指导老师编号
        /// </summary>
        public string StuTeacherNum { get; set; }
        /// <summary>
        /// 学生的上传次数
        /// </summary>
        public int StuUpLoadCount { get; set; }
        /// <summary>
        /// 学生上传时间限制（只试用大学生系统）
        /// </summary>
        public DateTime StuLimitTime { get; set; }
        /// <summary>
        /// 学生的报告单权限
        /// </summary>
        public string StuReportRule { get; set; }
        /// <summary>
        /// 教师审阅之后学生才能查看检测结果
        /// </summary>
        public string StuSchoolSet { get; set; }
        /// <summary>
        /// 用户类型（本科还是研究生）
        /// </summary>
        public int UserType { get; set; }
        /// <summary>
        /// 用户所属省厅的LogoUrl
        /// </summary>
        public string LogoUrl { get; set; }
        /// <summary>
        /// 每日上传数量
        /// </summary>
        public int UploadLimite { get; set; }
        /// <summary>
        /// 人事版后台采集起始时间
        /// </summary>
        public string CollectionBeginDate { get; set; }
        /// <summary>
        /// 人事版后台采集结束时间
        /// </summary>
        public string CollectionEndDate { get; set; }

        public bool IsReduceUploadLeftNumber { get; set; }

        public LoginInfo()
        {
            DefaultSetting();
        }
        /// <summary>
        /// 设置子帐号权限，s的格式是1|0|
        /// 第一个位置表示“删除上传文献或文件夹”，1表示允许，0表示不允许
        /// 第二个位置表示“修改密码”，1表示允许，0表示不允许
        /// 第二个位置表示“修改检测结果”，1表示允许，0表示不允许
        /// </summary>
        /// <param name="s"></param>
        public void SetChildLimit(object s)
        {
            this.SameReport = true;
            this.SimpleReport = true;
            this.ContentReport = true;
            this.CompareReport = true;
            this.DetailInfo = true;

            if (s == null)
            {
                this.CanDelete = false;
                this.CanPassword = false;
                this.ModifyResult = false;
            }
            else
            {
                string[] temp = s.ToString().Trim('|').Split('|');

                if (temp.Length == 2)
                {
                    this.CanDelete = temp[0].Equals("1") ? true : false;
                    this.CanPassword = temp[1].Equals("1") ? true : false;
                    this.ModifyResult = true;
                }
                else if (temp.Length == 3)
                {
                    this.CanDelete = temp[0].Equals("1") ? true : false;
                    this.CanPassword = temp[1].Equals("1") ? true : false;
                    this.ModifyResult = temp[2].Equals("1") ? true : false;
                }
                else
                {
                    this.CanDelete = false;
                    this.CanPassword = false;
                    this.ModifyResult = false;
                }
            }
        }

        public void DefaultSetting()
        {
            UserType = 0;
            UniversityNum = "";
            UserName = "";
            RootUserName = "";
            MaxNumber = 500000;
            MinNumber = 200;
            UploadLimite = 0;
            CollectionBeginDate = "";
            CollectionEndDate = "";
            IsReduceUploadLeftNumber = true;
        }

        #region 期刊监测信息
        /// <summary>
        /// 登录账号级别
        /// </summary>
        public string UserGrade { get; set; }
        /// <summary>
        /// 登录账号地区编号
        /// </summary>
        public string UserAid { get; set; }
        #endregion

        #region 原创监测系统
        /// <summary>
        /// 单位名称(原创监测系统)
        /// </summary>
        public string UnitName { get; set; }
        /// <summary>
        /// 单位编号(原创监测系统)
        /// </summary>
        public string UnitID { get; set; }
        #endregion

    }
}

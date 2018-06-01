using log4net;
using Microsoft.ApplicationBlocks.Data;
using Quartz;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using Senparc.Weixin.MP.Containers;
/* 
 * =====================================================================
 * 
 *  本页面版权归  雷好  及其所属公司所有
 *  作者：        雷好
 *  文件创建时间：2018/6/1 10:15:12
 *        版本号：V1.0
 *          用途：
 *          
 * =====================================================================
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatJob.lib;
using WeChatJob.model;

namespace WeChatJob
{
    public sealed class BillInform : IJob
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(BillInform));

        public void Execute(IJobExecutionContext context)
        {
            _logger.Info("开始推送账单通知");
            GetMsgData();
        }

        public Boolean SendTemplateMsg(BillDataModel billData)
        {
            var accessToken = AccessTokenContainer.GetAccessToken(Config.APPID);
            //由于源代码采用分支管理，这里将不再编写兼容所有模板消息的代码
            var templateId = "KpddG3wzOIVa5WOqFIYO4rBQL7knr0UJltcUWUqokAc";

            var data = new
            {
                first = new TemplateDataItem(""),
                keyword1 = new TemplateDataItem(billData.ACC_MONEY),
                keyword2 = new TemplateDataItem(billData.ACC_WATER),
                remark = new TemplateDataItem(billData.BILLING_MONTH.ToString())
            };

            string url = "";

            SendTemplateMessageResult result = TemplateApi.SendTemplateMessage(Config.APPID, billData.openID, templateId, url, data);
            if (result.errmsg == "ok")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region 获取账单信息并推送账单
        /// <summary>
        /// 获取账单信息并推送账单
        /// </summary>
        public void GetMsgData()
        {
            Boolean isContinue = true;
            while (isContinue)
            {
                String sql = @"select top 100
                                                    ID,
                                                    openID ,
                                                    CARD_ID ,
                                                    CARD_ADDRESS ,
                                                    ACC_MONEY ,
                                                    ACC_WATER ,
                                                    BILLING_MONTH
                                            from    dbo.PushBill
                                            where   State = 0
                                                    and SendNum < 4;";

                try
                {
                    DataTable dt = SqlHelper.ExecuteDataset(Config.WeChatConnString, CommandType.Text, sql).Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            BillDataModel model = new BillDataModel();
                            foreach (var item in model.GetType().GetProperties())
                            {
                                if (dr.Table.Columns.Contains(item.Name))
                                {
                                    if (DBNull.Value != dr[item.Name])
                                    {
                                        item.SetValue(model, Convert.ChangeType(dr[item.Name], item.PropertyType), null);
                                    }
                                }
                            }

                            if (SendTemplateMsg(model))
                            {
                                SetState(dr["ID"].ToString());
                            }
                            else  //修改推送次数
                            {
                                UpdateSendNum(dr["ID"].ToString());
                            }
                        }
                    }
                    else
                    {
                        isContinue = false;
                    }
                }
                catch (Exception ex)
                {
                    isContinue = false;
                    _logger.Error("推送账单出错，错误信息：" + ex.Message + "堆栈信息：" + ex.StackTrace);
                }
            }
        }
        #endregion

        #region 更新推送状态
        /// <summary>
        /// 更新推送状态
        /// </summary>
        /// <param name="ID">账单推送编号</param>
        public void SetState(String ID)
        {
            String sql = @"update  dbo.PushBill
                            set     State = 1 ,
                                    SendNum = SendNum + 1
                            where   ID = @ID;";
            SqlParameter[] pars = { new SqlParameter("@ID", ID) };
            SqlHelper.ExecuteNonQuery(Config.WeChatConnString, CommandType.Text, sql, pars);
        }
        #endregion

        #region 更新推送次数
        /// <summary>
        /// 更新推送次数
        /// </summary>
        /// <param name="ID">账单推送编号</param>
        public void UpdateSendNum(String ID)
        {
            String sql = @"update  dbo.PushBill
                            set     SendNum = SendNum + 1
                            where   ID = @ID;";
            SqlParameter[] pars = { new SqlParameter("@ID", ID) };
            SqlHelper.ExecuteNonQuery(Config.WeChatConnString, CommandType.Text, sql, pars);
        }
        #endregion
    }
}

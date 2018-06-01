/* 
 * =====================================================================
 * 
 *  本页面版权归  雷好  及其所属公司所有
 *  作者：        雷好
 *  文件创建时间：2018/6/1 14:05:57
 *        版本号：V1.0
 *          用途：
 *          
 * =====================================================================
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatJob.model
{
    public class BillDataModel
    {
        /// <summary>
        /// 用户身份标识
        /// </summary>
        public String openID { get; set; }

        /// <summary>
        /// 表卡编号
        /// </summary>
        public String CARD_ID { get; set; }

        /// <summary>
        /// 用水地址
        /// </summary>
        public String CARD_ADDRESS { get; set; }

        /// <summary>
        /// 本期水费
        /// </summary>
        public String ACC_MONEY { get; set; }

        /// <summary>
        /// 本期水量
        /// </summary>
        public String ACC_WATER { get; set; }

        /// <summary>
        /// 账务年月
        /// </summary>
        public Int32 BILLING_MONTH { get; set; }
    }
}

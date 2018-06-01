/* 
 * =====================================================================
 * 
 *  本页面版权归  雷好  及其所属公司所有
 *  作者：        雷好
 *  文件创建时间：2018/6/1 13:54:41
 *        版本号：V1.0
 *          用途：
 *          
 * =====================================================================
 */
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatJob.lib
{
    public class Config
    {
        public static string APPID = "";  //根据配置文件的 user 自动获取

        /// <summary>
        /// 微信数据库连接字符串
        /// </summary>
        public static String WeChatConnString = ConfigurationManager.ConnectionStrings["WeChatConnString"].ConnectionString;
    }
}

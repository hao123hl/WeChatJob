using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;
using Senparc.Weixin;
using Senparc.Weixin.MP;
using Senparc.Weixin.RegisterServices;
using System.Configuration;

namespace WeChatJob
{
    class Program
    {
        static void Main(string[] args)
        {
            RegisterConfiguration();

            XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"));
            HostFactory.Run(x =>
            {
                x.UseLog4Net();

                x.Service<ServiceRunner>();

                x.SetDescription("微信调度服务：负责消息推送");
                x.SetDisplayName("微信调度服务");
                x.SetServiceName("WeChat.Job");

                x.EnablePauseAndContinue();
            });
        }

        private static void RegisterConfiguration()
        {
            String user = ConfigurationManager.AppSettings["user"];
            String appId = "";
            String appSecret = "";
            String name = "";

            switch (user)
            {
                case "用户1":
                    appId = "";
                    appSecret = "";
                    name = "公众号1";
                    break;
                case "用户2":
                    appId = "";
                    appSecret = "";
                    name = "公众号2";
                    break;
            }

            RegisterService.Start()
                        .RegisterTraceLog(ConfigWeixinTraceLog)  //配置TraceLog
                        .RegisterMpAccount(appId, appSecret, name);  //注册公众号

            new WeChatJob.lib.Config();  //初始化配置
        }

        /// <summary>
        /// 配置微信跟踪日志
        /// </summary>
        private static void ConfigWeixinTraceLog()
        {
            //这里设为Debug状态时，/App_Data/WeixinTraceLog/目录下会生成日志文件记录所有的API请求日志，正式发布版本建议关闭
            Senparc.Weixin.Config.IsDebug = false;
            Senparc.Weixin.WeixinTrace.SendCustomLog("系统日志", "系统启动");//只在Senparc.Weixin.Config.IsDebug = true的情况下生效

            //自定义日志记录回调
            Senparc.Weixin.WeixinTrace.OnLogFunc = () =>
            {
                //加入每次触发Log后需要执行的代码
            };

            //当发生基于WeixinException的异常时触发
            Senparc.Weixin.WeixinTrace.OnWeixinExceptionFunc = ex =>
            {
                //加入每次触发WeixinExceptionLog后需要执行的代码

                //发送模板消息给管理员
                //var eventService = new EventService();
                //eventService.ConfigOnWeixinExceptionFunc(ex);
            };
        }
    }
}

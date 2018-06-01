using Quartz;
using Quartz.Impl;
/* 
 * =====================================================================
 * 
 *  本页面版权归  雷好  及其所属公司所有
 *  作者：        雷好
 *  文件创建时间：2018/6/1 10:33:00
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
using Topshelf;

namespace WeChatJob
{
    public sealed class ServiceRunner : ServiceControl, ServiceSuspend
    {
        private readonly IScheduler scheduler;

        public ServiceRunner()
        {
            scheduler = StdSchedulerFactory.GetDefaultScheduler();
        }

        public bool Start(HostControl hostControl)
        {
            scheduler.Start();
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            scheduler.Shutdown();
            return true;
        }

        public bool Continue(HostControl hostControl)
        {
            scheduler.ResumeAll();
            return true;
        }

        public bool Pause(HostControl hostControl)
        {
            scheduler.PauseAll();
            return true;
        }
    }
}

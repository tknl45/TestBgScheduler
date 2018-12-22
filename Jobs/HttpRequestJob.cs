using System;
using System.Threading.Tasks;
using Quartz;

namespace TestBgScheduler.Jobs
{
    public class HttpRequestJob:IJob
    {
         public async Task Execute(IJobExecutionContext context)
            {
                JobKey key = context.JobDetail.Key;

                //傳入參數
                JobDataMap dataMap = context.JobDetail.JobDataMap;
                string jobStr = dataMap.GetString("jobStr");
                string ItemID=dataMap.GetString("ItemID");
                string Method= dataMap.GetString("Method");
                string Type= dataMap.GetString("Type");
                string Url= dataMap.GetString("Url");
                string Headers= dataMap.GetString("Headers");
                string Parameters= dataMap.GetString("Parameters");

                //現在時間
                DateTime dt = DateTime.Now;
                string dateStr = dt.ToString();//2005-11-5 13:21:25

                await Console.Out.WriteLineAsync($"[{dateStr}]{ItemID} - {Url}");
            }
    }
}
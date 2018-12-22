namespace TestBgScheduler
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Quartz;
    using Quartz.Impl;
    using TestBgScheduler.Jobs;
    using TestBgScheduler.Models;

    public class BgScheduler
    {
        public static IConfiguration Configuration { get; set; }
        public static async Task RunProgram()
        {
            try
            {
                // 設定排程參數
                NameValueCollection props = new NameValueCollection
                {
                    { "quartz.serializer.type", "binary" }
                };
                StdSchedulerFactory factory = new StdSchedulerFactory(props);
                IScheduler scheduler = await factory.GetScheduler();

                // 啟用排程
                await scheduler.Start();

               

                //讀取設定檔
                var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
                Configuration = builder.Build();


                //Jobs
                var jobs = Configuration.GetSection("Jobs").Get<List<Job>>();
                System.Console.WriteLine("任務一覽 :"+jobs);


                // 設定job
                foreach (var job in jobs)
                {

                    IJobDetail jobDetail = JobBuilder.Create<HttpRequestJob>()
                    .WithIdentity(job.ItemID, "httpRequest") 
                    .UsingJobData("ItemID",job.ItemID) 
                    .UsingJobData("Method", job.Method) 
                    .UsingJobData("Type", job.Type) 
                    .UsingJobData("Url", job.Url) 
                    .UsingJobData("Headers", job.Headers) 
                    .UsingJobData("Parameters", job.Parameters) 
                    .Build();    


                    // 設定觸發原則: 每隔2分鐘,  時間區間：8am ~ 5pm, 每天
                    ITrigger trigger = TriggerBuilder.Create()
                        .WithIdentity(job.ItemID, "httpRequest")
                        .WithCronSchedule(job.CronTab) //每隔2分鐘,  時間區間：8am ~ 5pm, 每天
                        .ForJob(job.ItemID, "httpRequest")
                    .Build();

                    //寫進排程
                    await scheduler.ScheduleJob(jobDetail, trigger);


                }
                
               
               
              
            }
            catch (SchedulerException se)
            {
                await Console.Error.WriteLineAsync(se.ToString());
            }
        }


        
    }
}
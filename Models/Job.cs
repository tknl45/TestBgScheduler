using System;

namespace TestBgScheduler.Models
{
    public class Job
    {
        public String CronTab { get; set; }
        public String ItemID { get; set; }
        public String ItemName { get; set; }
        public String ItemNote { get; set; }
        public String Type { get; set; }
        public String Url { get; set; }
        public String Method { get; set; }
        public String Headers { get; set; }
        public String Parameters { get; set; }

    }
}